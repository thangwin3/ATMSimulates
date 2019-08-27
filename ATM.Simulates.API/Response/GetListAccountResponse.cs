using System.Collections.Generic;

namespace ATM.Simulates.API.Response
{
    public class GetListAccountResponse : BaseResponse
    {
        public ListAcountData Data { get; set; }
        public GetListAccountResponse()
        {
            this.Data = new ListAcountData();
        }
    }
    public class AcountItem
    {
        public decimal Balance { get; set; }
        public string AccountType { get; set; }
    }

    public class ListAcountData
    {
       public  List<AcountItem> ListAccount { get; set; }

        public ListAcountData()
        {
            this.ListAccount = new List<AcountItem>();
        }

    }
}
