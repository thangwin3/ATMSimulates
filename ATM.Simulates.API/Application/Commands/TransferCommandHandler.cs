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
    public class TransferCommandHandler : IRequestHandler<TransferCommand, TransferResponse>
    {
        private ATMContext _context;
        public TransferCommandHandler(ATMContext context)
        {
            _context = context;

        }

        public async Task<TransferResponse> Handle(TransferCommand command, CancellationToken cancellationToken)
        {
            var response = new TransferResponse();
            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    DateTime TransDate = DateTime.Now;
                    var walletSource = _context.Wallets.Where
                    (s => s.WalletId == command.WalletSourceId)
                    .FirstOrDefault();


                    if (walletSource.Balance  < command.Amount - 50000 - command.Fee)
                    {
                        response.Code = ErrorCode.GetError(ErrorCode.AmountNotEnough).Key;
                        response.Message = ErrorCode.GetError(ErrorCode.AmountNotEnough).Value;
                        return await Task.FromResult(response);
                    }

                    decimal BeforeBalanceSource = walletSource.Balance;
                    if (walletSource != null)
                    {
                        walletSource.Balance = walletSource.Balance - command.Amount - command.Fee;
                    }
                    await _context.SaveChangesAsync();

                    var walletDes = _context.Wallets.Where
                    (s => s.WalletId == command.WalletDesId)
                    .FirstOrDefault();
                    decimal BeforeBalanceDes = walletDes.Balance;
                    if (walletDes != null)
                    {
                        walletDes.Balance = walletDes.Balance + command.Amount;
                    }
                    await _context.SaveChangesAsync();

                    Transaction transaction = new Transaction
                    {
                        Amount = command.Amount,
                        Status = TransactionStatusEnum.SUCCESS.ToString(),
                        TransDate = TransDate,
                        TransactionTypeId = command.TransactionTypeId,
                        WalletDesId = walletDes.WalletId,
                        WalletSourceId = walletSource.WalletId,
                        TransType = TransactionTypeEnum.TRANSFER.ToString(),
                        Fee = command.Fee
                    };
                    _context.Transactions.Add(transaction);
                    await _context.SaveChangesAsync();

                    TransactionLog transactionLogSource = new TransactionLog()
                    {
                        AccountId = walletSource.AccountId,
                        BalanceAfter = walletSource.Balance,
                        BalanceBefore = BeforeBalanceSource,
                        Status = TransactionStatusEnum.SUCCESS.ToString(),
                        TransactionDate = TransDate,
                        TransactionId = transaction.TransactionId,
                        TransactionTypeId = transaction.TransactionTypeId
                    };
                    _context.TransactionLogs.Add(transactionLogSource);
                    await _context.SaveChangesAsync();

                    TransactionLog transactionLogDes = new TransactionLog()
                    {
                        AccountId = walletSource.AccountId,
                        BalanceAfter = walletDes.Balance,
                        BalanceBefore = BeforeBalanceDes,
                        Status = TransactionStatusEnum.SUCCESS.ToString(),
                        TransactionDate = TransDate,
                        TransactionId = transaction.TransactionId,
                        TransactionTypeId = transaction.TransactionTypeId
                    };
                    _context.TransactionLogs.Add(transactionLogDes);
                    await _context.SaveChangesAsync();

                    response.Data.TransactionId = transaction.TransactionId;
                    trans.Commit();
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