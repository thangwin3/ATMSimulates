using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATM.Simulates.API.Models
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AccountId { get; set; }
        [MaxLength(50)]
        public string AccountName { get; set; }
        [Required]
        [MaxLength(100)]
        public string PinCode { get; set; }
        public string AccessToken { get; set; }
        public bool isLock { get; set; } = false;
        public virtual ICollection<Wallet> Wallet { get; set; }
    }

    public class TraceLogin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TraceLoginId { get; set; }
        [ForeignKey("AccountId")]
        public long AccountId { get; set; }
        public bool IsLoginSuccess { get; set; }
        public int CountLoginFail { get; set; }
        public DateTime TimeLogin { get; set; }
    }
}
