using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATM.Simulates.API.Models
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }

        [ForeignKey("TransactionTypeId")]
        public int TransactionTypeId { get; set; }
        public Guid WalletDesId { get; set; }
        public Guid WalletSourceId { get; set; }
        public DateTime TransDate { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public string TransType { get; set; }
        public string Status { get; set; }
    }

    public class TransactionType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionTypeId { get; set; }
        public string TransactionTypeName { get; set; }
        public decimal Fee { get; set; }
    }
}
