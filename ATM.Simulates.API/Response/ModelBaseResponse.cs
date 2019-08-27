using Newtonsoft.Json;

namespace ATM.Simulates.API.Response
{
    public class ModelBaseResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Signature { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public dynamic Data { get; set; }
    }
}
