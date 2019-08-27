using ATM.Simulates.API.Models;
using ATM.Simulates.API.Response;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ATM.Simulates.API.Application.Commands
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        ATMContext _context;
        public LoginCommandHandler(ATMContext context)
        {
            _context = context;
        }


        public Task<LoginResponse> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var response = new LoginResponse();
            try
            {
                var account = _context.Accounts
                    .Where(s => s.PinCode == command.PinCode)
                    .FirstOrDefault();
                if (account != null)
                {
                   
                    
                    var traceLogin = new TraceLogin
                    {
                        AccountId = account.AccountId,
                        CountLoginFail = 0,
                        IsLoginSuccess = true,
                        TimeLogin = DateTime.Now
                    };
                    _context.TraceLogin.Add(traceLogin);
                    _context.SaveChangesAsync();
                    response.Data.AccountName = account.AccountName;
                }
                else
                {
                    var traceLogin = _context.TraceLogin.LastOrDefault();
                    if (traceLogin != null)
                    {
                        traceLogin.CountLoginFail = traceLogin.CountLoginFail + 1;
                        if (traceLogin.CountLoginFail == 3)
                        {
                            account = _context.Accounts.FirstOrDefault();
                            account.isLock = true;
                        }
                        _context.SaveChangesAsync();
                    }

                    response.Code = ErrorCode.GetError(ErrorCode.PinWrong).Key;
                    response.Message = ErrorCode.GetError(ErrorCode.PinWrong).Value;
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
    }

}