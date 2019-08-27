namespace ATM.Simulates.Webview.Response
{
    public class BaseResponse
    {
        public string Message { get; set; }
        public int Code { get; set; }

        public BaseResponse ()
        {
            this.Message = "success";
            this.Code = 0;
        }
    }
}
