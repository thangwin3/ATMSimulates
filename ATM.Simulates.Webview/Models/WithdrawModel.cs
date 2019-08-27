using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ATM.Simulates.Webview.Models
{
    public class WithdrawModel: BaseModel
    {
        [Required(ErrorMessage = "Bạn chưa nhập số tiền")]
        public decimal Amount { get; set; }
    }
}
