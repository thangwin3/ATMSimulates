using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATM.Simulates.API.Models
{
    public class ATMContext : DbContext
    {
        public ATMContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<WalletType> WalletType { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionType> TransactionType { get; set; }
        public DbSet<TransactionLog> TransactionLogs { get; set; }
        public DbSet<TraceLogin> TraceLogin { get; set; }

    }

}
