using System;
using System.Collections.Generic;

namespace ATM.Simulates.Webview.Response
{
    public class GetListTransactionResponse : BaseResponse
    {
        public ListTransactionData Data { get; set; }
        public GetListTransactionResponse()
        {
            this.Data = new ListTransactionData();
        }
    }
    public class ItemTransaction
    {
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public string TransType { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Status { get; set; }
    }

    public class ListTransactionData
    {
       public  List<ItemTransaction> ListTransaction { get; set; }

        public ListTransactionData()
        {
            this.ListTransaction = new List<ItemTransaction>();
        }

    }
}
