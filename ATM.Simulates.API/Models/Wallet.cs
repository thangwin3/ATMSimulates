using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATM.Simulates.API.Models
{
    public class Wallet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid WalletId { get; set; }
        public decimal Balance { get; set; }

        [ForeignKey("WalletTypeId")]
        public int WalletTypeId { get; set; }
        
        [ForeignKey("AccountId")]
        public long AccountId { get; set; }
        public virtual Account Account { get; set; }
        public virtual WalletType WalletType { get; set; }

    }


    public class WalletType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WalletTypeId { get; set; }
        public string WalletTypeName { get; set; }
    }
}
