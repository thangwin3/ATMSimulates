namespace ATM.Simulates.API.Response
{
    public class LoginResponse:BaseResponse
    {
        public DataResponse Data { get; set; }
        public LoginResponse()
        {
            this.Data = new DataResponse();
        }
    }
    public class DataResponse
    {
        public string AccountName { get; set; }
        public string AccessToken { get; set; }
    }
}
