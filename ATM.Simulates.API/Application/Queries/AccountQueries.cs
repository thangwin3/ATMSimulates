using ATM.Simulates.API.Application.Queries.Request;
using ATM.Simulates.API.Models;
using ATM.Simulates.API.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATM.Simulates.API.Application.Queries
{
    public class AccountQueries : IAccountQueries
    {

        private ATMContext _context;
        public AccountQueries(ATMContext context)
        {
            _context = context;

        }
        public Task<GetBalanceResponse> GetBalance(GetBalanceRequest request)
        {
            var response = new GetBalanceResponse();
            try
            {
                var wallet = (from p in _context.WalletType
                              join c in _context.Wallets
                              on p.WalletTypeId equals c.WalletTypeId
                              where p.WalletTypeName == request.AccountType
                              select new
                              {
                                  Balance = c.Balance,
                              }).FirstOrDefault();

                response.Data.Balance = wallet.Balance;
            }
            catch (Exception ex)
            {
                response.Code = ErrorCode.GetError(ErrorCode.SystemError).Key;
                response.Message = ErrorCode.GetError(ErrorCode.SystemError).Value;
                Logger.Error(ex);
            }
            return Task.FromResult(response);
        }

        public Task<GetListAccountResponse> GetListAccount()
        {
            var response = new GetListAccountResponse();
            try
            {
                var listAccount = (from p in _context.WalletType
                                   join c in _context.Wallets
                                   on p.WalletTypeId equals c.WalletTypeId
                                   select new
                                   {
                                       AccountType = p.WalletTypeName,
                                       Balance = c.Balance,
                                   }).ToListAsync().Result;
                foreach (var account in listAccount)
                {
                    var acountItem = new AcountItem()
                    {
                        AccountType = account.AccountType,
                        Balance = account.Balance
                    };

                    response.Data.ListAccount.Add(acountItem);
                }
            }
            catch (Exception ex)
            {
                response.Code = ErrorCode.GetError(ErrorCode.SystemError).Key;
                response.Message = ErrorCode.GetError(ErrorCode.SystemError).Value;
                Logger.Error(ex);
            }
            return Task.FromResult(response);
        }

        public Task<GetTransactionDetailResponse> GetTransactionDetail(GetTransactionDetailRequest request)
        {
            var response = new GetTransactionDetailResponse();
            try
            {
                var transactionDetail = (from p in _context.Transactions
                                         join c in _context.TransactionType
                                         on p.TransactionTypeId equals c.TransactionTypeId
                                         where p.TransactionId == request.TransactionId
                                         select new
                                         {
                                             TransactionType = c.TransactionTypeName,
                                             Amount = p.Amount,
                                             TransactionDate = p.TransDate,
                                             Status = p.Status,
                                             TransactionId = p.TransactionId,

                                         }).FirstOrDefault();

                response.Data.Amount = transactionDetail.Amount;
                response.Data.TransactionType = transactionDetail.TransactionType;

                response.Data.TransactionDate = transactionDetail.TransactionDate;
                response.Data.Status = transactionDetail.Status;

                response.Data.TransactionId = transactionDetail.TransactionId;
            }

            catch (Exception ex)
            {
                response.Code = ErrorCode.GetError(ErrorCode.SystemError).Key;
                response.Message = ErrorCode.GetError(ErrorCode.SystemError).Value;
                Logger.Error(ex);
            }
            return Task.FromResult(response);
        }


        public async Task<GetListTransactionResponse> GetListTransactionAsync()
        {
            var response = new GetListTransactionResponse();
            try
            {
                var listTransaction = await _context.Transactions.Take(20).ToListAsync();
                foreach (var transaction in listTransaction)
                {
                    var acountItem = new ItemTransaction()
                    {
                        TransType = transaction.TransType,
                        Amount = transaction.Amount,
                        TransactionDate = transaction.TransDate,
                        Status = transaction.Status,
                        Fee = transaction.Fee
                    };
                    response.Data.ListTransaction.Add(acountItem);
                }
            }
            catch (Exception ex)
            {
                response.Code = ErrorCode.GetError(ErrorCode.SystemError).Key;
                response.Message = ErrorCode.GetError(ErrorCode.SystemError).Value;
                Logger.Error(ex);
            }
            return await Task.FromResult(response);
        }
    }
}
