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
    public class WithdrawCommandHandler : IRequestHandler<WithdrawCommand, WithdrawResponse>
    {
        private ATMContext _context;
        public WithdrawCommandHandler(ATMContext context)
        {
            _context = context;

        }

        public async Task<WithdrawResponse> Handle(WithdrawCommand command, CancellationToken cancellationToken)
        {
            var response = new WithdrawResponse();
            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    // hạn mức tối đa 5.000.000
                    if (command.Amount > 5000000)
                    {
                        response.Code = ErrorCode.GetError(ErrorCode.OutRange).Key;
                        response.Message = ErrorCode.GetError(ErrorCode.OutRange).Value;
                        return await Task.FromResult(response);
                    }


                    var wallet = _context.Wallets.Where
                    (s => s.WalletTypeId == command.WalletTypeId)
                    .FirstOrDefault();
                    // trong tk còn ít nhất 50.000
                    if (wallet.Balance < command.Amount - 50000 - command.Fee)
                    {
                        response.Code = ErrorCode.GetError(ErrorCode.AmountNotEnough).Key;
                        response.Message = ErrorCode.GetError(ErrorCode.AmountNotEnough).Value;
                        return await Task.FromResult(response);
                    }
                    decimal BalanceBefore = wallet.Balance;
                    DateTime TransDate = DateTime.Now;
                    if (wallet != null)
                    {
                        wallet.Balance = wallet.Balance - command.Amount - command.Fee;
                    }
                    await _context.SaveChangesAsync();

                    Transaction transaction = new Transaction
                    {
                        Amount = command.Amount,
                        Status = TransactionStatusEnum.SUCCESS.ToString(),
                        TransDate = TransDate,
                        TransactionTypeId = command.TransactionTypeId,
                        WalletDesId = wallet.WalletId,
                        TransType = TransactionTypeEnum.WITHDRAW.ToString(),
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
                        TransactionTypeId = transaction.TransactionTypeId
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