using ATM.Simulates.API.Extentions;
using ATM.Simulates.API.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Configuration;
using NETCore.Encrypt;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Simulates.API
{
    public class RequestMiddleware
    {
        private readonly RequestDelegate _next;
        private string PrivateKey = "";
        public RequestMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            PrivateKey = configuration.GetValue<string>("PrivateKey");
        }

        public async Task Invoke(HttpContext context)
        {
            context.Request.EnableRewind();
            //if (!context.Request.Headers.Keys.Contains("Sign"))
            //{
            //    context.Response.StatusCode = 400; // Unauthorized
            //    return;
            //}
            var signature = context.Request.Headers["Signature"];

            var buffer = new byte[Convert.ToInt32(context.Request.ContentLength)];
            var json = Encoding.UTF8.GetString(buffer);
            object data = JsonConvert.DeserializeObject<object>(json);
            var _types = data.GetType();
            var _lsort = _types.GetProperties().OrderBy(x => x.Name).ToList();
            string bodyraw = "";
            string keyraw = "";
            foreach (var prop in _lsort)
            {
                var value = prop.GetValue(data, null);
                if (prop.Name == "keyHash")
                {
                    keyraw = value.ToString();
                    keyraw = EncryptProvider.RSADecrypt(PrivateKey, keyraw);
                }
                if (value != null)
                {
                    bodyraw += value.ToString();
                }
            }

            string signatureCompare = EncryptProvider.HMACSHA256(bodyraw, keyraw);
            if (signatureCompare != signature)
            {
                //if (!context.Request.Headers.Keys.Contains("Sign"))
                //{
                //    context.Response.StatusCode = 400; // Unauthorized
                //    return;
                //}
            }
            await _next.Invoke(context);
        }

    }


    public class ResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public ResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var originBody = context.Response.Body;
            var newBody = new MemoryStream();
            context.Response.Body = newBody;
            await _next(context);
            newBody.Seek(0, SeekOrigin.Begin);
            string json = new StreamReader(newBody).ReadToEnd();
            context.Response.Body = originBody;
            //  await context.Response.WriteAsync(json);

            var downStreamData = JsonConvert.DeserializeObject<ModelBaseResponse>(json);
            string datacontent = JsonConvert.SerializeObject(downStreamData.Data);
            var jsonToken = JToken.Parse(datacontent);
            jsonToken.OrderByProperties();

            string signature = EncryptProvider.HMACSHA256(jsonToken.ToString(Formatting.None), "123");
            downStreamData.Signature = signature;
            downStreamData.Data = jsonToken;
            var responseData = JsonConvert.SerializeObject(downStreamData);
            byte[] buffer = Encoding.UTF8.GetBytes(responseData);
            await context.Response.Body.WriteAsync(buffer, 0, buffer.Length);
        }


    }

}