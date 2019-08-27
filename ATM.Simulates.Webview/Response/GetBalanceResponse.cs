namespace ATM.Simulates.Webview.Response
{
    public class GetBalanceResponse : BaseResponse
    {
        public GetBalanceData Data { get; set; }
        public GetBalanceResponse()
        {
            this.Data = new GetBalanceData();
        }
    }
    public class GetBalanceData
    {
        public decimal Balance { get; set; }
    }
}
