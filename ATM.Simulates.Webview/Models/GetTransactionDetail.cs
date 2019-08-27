using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATM.Simulates.Webview.Models
{
    public class GetTransactionDetail: BaseModel
    {
        public long TransactionId { get; set; }
    }
}
