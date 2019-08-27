using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ATM.Simulates.Webview.Models
{
    public class LoginModel:BaseModel
    {
        [Required(ErrorMessage = "Bạn chưa nhập mã Pin")]
        [StringLength(6, ErrorMessage = "Mã Pin 6 ký tự")]
        public string PinCode { get; set; }

    }
}
