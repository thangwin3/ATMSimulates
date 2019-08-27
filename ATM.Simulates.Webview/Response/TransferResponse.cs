namespace ATM.Simulates.Webview.Response
{
    public class TransferResponse : BaseResponse
    {
        public TransferData Data { get; set; }
        public TransferResponse()
        {
            this.Data = new TransferData();
        }
    }
    public class TransferData
    {
        public decimal Balance { get; set; }
        public long TransactionId { get; set; }
    }
}
