using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATM.Simulates.API.Models
{
    public class TransactionLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionLognId { get; set; }
        [ForeignKey("TransactionId")]
        public int TransactionId { get; set; }
        [ForeignKey("AccountId")]
        public long AccountId { get; set; }
        public decimal BalanceBefore { get; set; }
        public decimal BalanceAfter { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Status { get; set; }
        [ForeignKey("TransactionTypeId")]
        public int TransactionTypeId { get; set; }
    }


}
