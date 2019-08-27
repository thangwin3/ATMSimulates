using ATM.Simulates.API.Enum;
using ATM.Simulates.API.Models;
using ATM.Simulates.API.Response;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ATM.Simulates.API.Application.Commands
{
    public class DepositCommandHandler : IRequestHandler<DepositCommand, DepositResponse>
    {
        private ATMContext _context;
        public DepositCommandHandler(ATMContext context)
        {
            _context = context;

        }

        public async Task<DepositResponse> Handle(DepositCommand command, CancellationToken cancellationToken)
        {
            var response = new DepositResponse();
            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    var wallet = _context.Wallets.Where
                    (s => s.WalletTypeId == command.WalletTypeId)
                    .FirstOrDefault();
                    decimal BalanceBefore = wallet.Balance;
                    DateTime TransDate = DateTime.Now;
                    if (wallet != null)
                    {
                        wallet.Balance = wallet.Balance + command.Amount;
                    }
                    await _context.SaveChangesAsync();

                    Transaction transaction = new Transaction
                    {
                        Amount = command.Amount,
                        Status = TransactionStatusEnum.SUCCESS.ToString(),
                        TransDate = TransDate,
                        TransactionTypeId = command.TransactionTypeId,
                        WalletDesId = wallet.WalletId,
                        TransType = TransactionTypeEnum.DEPOSIT.ToString(),
                        Fee = command.Fee
                    };
                    _context.Transactions.Add(transaction);
                    await _context.SaveChangesAsync();

                    TransactionLog transactionLog = new TransactionLog()
                    {
                        AccountId = wallet.AccountId,
                        BalanceAfter = wallet.Balance,
                        BalanceBefore = BalanceBefore,
                        Status = TransactionStatusEnum.SUCCESS.ToString(),
                        TransactionDate = TransDate,
                        TransactionId = transaction.TransactionId,
                        TransactionTypeId = transaction.TransactionTypeId,
                    };
                    _context.TransactionLogs.Add(transactionLog);
                    await _context.SaveChangesAsync();
                    trans.Commit();
                    response.Data.Balance = wallet.Balance;
                    response.Data.TransactionId = transaction.TransactionId;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    response.Code = ErrorCode.GetError(ErrorCode.SystemError).Key;
                    response.Message = ErrorCode.GetError(ErrorCode.SystemError).Value;
                    Logger.Error(ex);
                }
            }
            return await Task.FromResult(response);
        }
    }

}