using ATM.Simulates.Webview.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Simulates.Webview.Helpers
{
    public class ClientService
    {
        public HttpClient Client { get; }
        public string _token { get; set; }
        string PublicKey = "";
        public ClientService(HttpClient client, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, ILogger<ClientService> logger)
        {
            PublicKey = configuration.GetValue<string>("PublicKey");
            if (httpContextAccessor.HttpContext != null)
            {

                var account = SessionHelper.GetObjectFromJson<DataLoginResponse>(httpContextAccessor.HttpContext.Session, "Account");
                if (account != null)
                {
                    client.DefaultRequestHeaders.Add("Authorization", "bearer " + account.AccessToken);
                }
            }

            client.BaseAddress = new Uri(configuration.GetValue<string>("ATM_API_URL"));
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("UTF-8"));
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue(System.Reflection.Assembly.GetEntryAssembly().GetName().Name)));
            client.Timeout = TimeSpan.FromSeconds(5);
            Client = client;
        }

        public async Task<T> PostAsync<T>(string Path, dynamic data) where T : class, new()
        {
            try
            {
                //(string, string) getSign = CreateSignature.CreateSign(data, PublicKey);
                //data.KeyHash = getSign.Item2;
                //Client.DefaultRequestHeaders.Add("Signature", getSign.Item1);

                var content = JsonConvert.SerializeObject(data);
                var buffer = Encoding.UTF8.GetBytes(content);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await Client.PostAsync(Path, byteContent).ConfigureAwait(false);
                var result = await response.Content.ReadAsStringAsync();

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Logger.Error($"PostAsync End,  request: {content}, url: {Path}, HttpStatusCode: {response.StatusCode}, result: {result}");
                }
                // if not success status code it exception
                response.EnsureSuccessStatusCode();

                Logger.Info($"PostAsync End, request: {content}, url: {Path}, result: {result}");
                return JsonConvert.DeserializeObject<T>(result);

            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    Logger.Error($"Exception: {ex} ,PostAsync End, url:{Path}, result:{responseContent}");
                    throw new System.Exception($"response :{responseContent}", ex);
                }
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception: {ex} ,PostAsync End, url:{Path}");
                throw ex;
            }
        }


        public async Task<T> GetAsync<T>(string Path) where T : class, new()
        {
            try
            {
                var response = await Client.GetAsync(Path).ConfigureAwait(false);
                var result = await response.Content.ReadAsStringAsync();

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Logger.Error($"GetAsync End,  url: {Path}, HttpStatusCode: {response.StatusCode}, result: {result}");
                }

                // if not success status code it exception
                response.EnsureSuccessStatusCode();

                Logger.Info($"GetAsync End, url: {Path}, result: {result}");
                return JsonConvert.DeserializeObject<T>(result);

            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    Logger.Error($"Exception: {ex} ,GetAsync End, url:{Path}, result:{responseContent}");
                    throw new System.Exception($"response :{responseContent}", ex);
                }
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception: {ex} ,GetAsync End, url:{Path}");
                throw ex;
            }
        }

    }
}
