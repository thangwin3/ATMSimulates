using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ATM.Simulates.Webview.Models
{
    public class TransferModel: BaseModel
    {
        [Required(ErrorMessage = "Bạn chưa số tiền")]
        public decimal Amount { get; set; }
        [Required(ErrorMessage = "Bạn chưa chọn tài khoản đích")]
        public string AccountDes { get; set; }
        [Required(ErrorMessage = "Bạn chưa chọn tài khoản nguồn")]
        public string AccountSource { get; set; }
    }
}
