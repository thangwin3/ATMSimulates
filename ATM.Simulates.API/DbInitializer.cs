using ATM.Simulates.API.Models;
using NETCore.Encrypt;
using System;
using System.Linq;

namespace ATM.Simulates.API
{
    public static class DbInitializer
    {
        public static void Initialize(ATMContext context)
        {
            context.Database.EnsureCreated();

            // Tạo data mẫu
            var checkData = context.Accounts.LastOrDefault();
            if (checkData == null)
            {

                Account account = new Account
                {
                    AccountName = "Nguyen Trong Thang",
                    PinCode = EncryptProvider.Sha256("123456")
                };
                context.Accounts.Add(account);
                context.SaveChanges();

                var WalletType1 = new WalletType()
                {
                    WalletTypeName = "CHECKING"
                };
                var WalletType2 = new WalletType()
                {
                    WalletTypeName = "SAVING"
                };
                context.WalletType.Add(WalletType1);
                context.WalletType.Add(WalletType2);
                context.SaveChanges();

                Guid g2 = Guid.NewGuid();
                Wallet wallet2 = new Wallet()
                {
                    AccountId = account.AccountId,
                    Balance = 0,
                    WalletId = g2,
                    WalletTypeId = WalletType2.WalletTypeId,
                };
                context.Wallets.Add(wallet2);
                context.SaveChanges();

                Guid g1 = Guid.NewGuid();
                Wallet wallet1 = new Wallet()
                {
                    AccountId = account.AccountId,
                    Balance = 5000000,
                    WalletId = g1,
                    WalletTypeId = WalletType1.WalletTypeId,
                };
                context.Wallets.Add(wallet1);
                context.SaveChanges();

                var transactiontype1 = new TransactionType()
                {
                    TransactionTypeName = "DEPOSIT",
                    Fee = 0
                };
                var transactiontype2 = new TransactionType()
                {
                    TransactionTypeName = "WITHDRAW",
                    Fee = 3000
                };
                var transactiontype3 = new TransactionType()
                {
                    TransactionTypeName = "TRANSFER",
                    Fee = 0
                };
                context.TransactionType.Add(transactiontype1);
                context.TransactionType.Add(transactiontype2);
                context.TransactionType.Add(transactiontype3);
                context.SaveChanges();
            }
        }
    }
}
