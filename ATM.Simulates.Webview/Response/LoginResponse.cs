namespace ATM.Simulates.Webview.Response
{
    public class LoginResponse:BaseResponse
    {
        public DataLoginResponse Data { get; set; }
        public LoginResponse()
        {
            this.Data = new DataLoginResponse();
        }
    }
    public class DataLoginResponse
    {
        public string AccountName { get; set; }
        public string AccessToken { get; set; }
        
    }
}
