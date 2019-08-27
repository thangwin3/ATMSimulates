using System;

namespace ATM.Simulates.Webview.Response
{
    public class GetTransactionDetailResponse : BaseResponse
    {
        public TransactionDetailData Data { get; set; }
        public GetTransactionDetailResponse()
        {
            this.Data = new TransactionDetailData();
        }
    }
    public class TransactionDetailData
    {
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
