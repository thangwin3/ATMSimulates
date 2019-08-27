using ATM.Simulates.API.Application.Commands;
using ATM.Simulates.API.Application.Queries;
using ATM.Simulates.API.Application.Queries.Request;
using ATM.Simulates.API.Enum;
using ATM.Simulates.API.Models;
using ATM.Simulates.API.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Simulates.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ATMServicesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ATMContext _context;
        private readonly IAccountQueries _accountQueries;
        private readonly IConfiguration _configuration;
        public ATMServicesController(IConfiguration configuration, IMediator mediator, ATMContext context, IAccountQueries accountQueries)
        {
            _mediator = mediator;
            _context = context;
            _accountQueries = accountQueries;
            _configuration = configuration;
        }
        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> login([FromBody]LoginCommand command)
        {
            var response = new LoginResponse();
            try
            {
                var traceLogin = _context.TraceLogin.LastOrDefault();
                if (traceLogin != null)
                {
                    if (traceLogin.CountLoginFail == 3)
                    {
                        response.Code = ErrorCode.GetError(ErrorCode.AccountLocked).Key;
                        response.Message = ErrorCode.GetError(ErrorCode.AccountLocked).Value;
                        return Ok(response);
                    }

                }

                response = await _mediator.Send(command);
                if (response.Code == 0)
                {
                    var claims = new[]
                    {
                   new Claim(JwtRegisteredClaimNames.Sub, command.PinCode),
                   new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                   new Claim(JwtRegisteredClaimNames.Sub, command.PinCode)
               };
                    var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityToken:Key"]));
                    var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

                    var jwtSecurityToken = new JwtSecurityToken(
                        issuer: _configuration["JwtSecurityToken:Issuer"],
                        audience: _configuration["JwtSecurityToken:Audience"],
                        claims: claims,
                        expires: DateTime.UtcNow.AddMinutes(30),
                        signingCredentials: signingCredentials
                    );

                    var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                    var expiration = jwtSecurityToken.ValidTo;
                    response.Data.AccessToken = token;
                }
            }
            catch (Exception ex)
            {
                response.Code = ErrorCode.GetError(ErrorCode.SystemError).Key;
                response.Message = ErrorCode.GetError(ErrorCode.SystemError).Value;
                Logger.Error($"Exception: {ex} , Method:login");
            }

            return Ok(response);
        }
        [Route("deposit")]
        [HttpPost]
        public async Task<IActionResult> deposit([FromBody]DepositCommand command)
        {
            var response = new DepositResponse();

            try
            {
                var walletType = _context.WalletType.Where(s => s.WalletTypeName == WalletTypeEnum.CHECKING.ToString()).FirstOrDefault();
                var transactionType = _context.TransactionType.Where(s => s.TransactionTypeName == TransactionTypeEnum.WITHDRAW.ToString()).FirstOrDefault();
                command.Fee = transactionType.Fee;
                command.TransactionTypeId = transactionType.TransactionTypeId;


                command.WalletTypeId = walletType.WalletTypeId;
                response = await _mediator.Send(command);
            }
            catch (Exception ex)
            {
                response.Code = ErrorCode.GetError(ErrorCode.SystemError).Key;
                response.Message = ErrorCode.GetError(ErrorCode.SystemError).Value;
                Logger.Error($"Exception: {ex} , Method:deposit");
            }

            return Ok(response);
        }


        [Route("withdraw")]
        [HttpPost]
        public async Task<IActionResult> withdraw([FromBody]WithdrawCommand command)
        {
            var response = new DepositResponse();
            try
            {
                if(command.Amount < 50000 || command.Amount%50000!=0)
                {
                    response.Code = ErrorCode.GetError(ErrorCode.AmountInvalid).Key;
                    response.Message = ErrorCode.GetError(ErrorCode.AmountInvalid).Value;
                    return Ok(response);
                }

                var walletType = _context.WalletType.Where(s => s.WalletTypeName == WalletTypeEnum.CHECKING.ToString()).FirstOrDefault();
                var transactionType = _context.TransactionType.Where(s => s.TransactionTypeName == TransactionTypeEnum.WITHDRAW.ToString()).FirstOrDefault();
                command.Fee = transactionType.Fee;
                command.TransactionTypeId = transactionType.TransactionTypeId;
                command.WalletTypeId = walletType.WalletTypeId;
                response = await _mediator.Send(command);
            }
            catch (Exception ex)
            {
                response.Code = ErrorCode.GetError(ErrorCode.SystemError).Key;
                response.Message = ErrorCode.GetError(ErrorCode.SystemError).Value;
                Logger.Error($"Exception: {ex} , Method:withdraw");
            }

            return Ok(response);
        }


        [Route("transfer")]
        [HttpPost]
        public async Task<IActionResult> transfer([FromBody]TransferCommand command)
        {
            var response = new BaseResponse();
            try
            {

                var walletDes = (from p in _context.WalletType
                                 join c in _context.Wallets
                                 on p.WalletTypeId equals c.WalletTypeId
                                 where p.WalletTypeName == command.AccountDes
                                 select new
                                 {
                                     WalletId = c.WalletId,
                                     WalletType = p.WalletTypeName
                                 }).FirstOrDefault();



                var walletSource = (from p in _context.WalletType
                                    join c in _context.Wallets
                                    on p.WalletTypeId equals c.WalletTypeId
                                    where p.WalletTypeName == command.AccountSource
                                    select new
                                    {
                                        WalletId = c.WalletId,
                                        WalletType = p.WalletTypeName,
                                        Balance = c.Balance
                                    }).FirstOrDefault();
                if (walletSource == null || walletDes == null)
                {
                    response.Code = ErrorCode.GetError(ErrorCode.AccountNotFound).Key;
                    response.Message = ErrorCode.GetError(ErrorCode.AccountNotFound).Value;
                    return Ok(response);
                }

                var transactionType = _context.TransactionType.Where(s => s.TransactionTypeName == TransactionTypeEnum.TRANSFER.ToString()).FirstOrDefault();
                if (command.Amount + transactionType.Fee > walletSource.Balance)
                {

                    response.Code = ErrorCode.GetError(ErrorCode.AmountNotEnough).Key;
                    response.Message = ErrorCode.GetError(ErrorCode.AmountNotEnough).Value;
                    return Ok(response);
                }

                command.Fee = transactionType.Fee;
                command.TransactionTypeId = transactionType.TransactionTypeId;
                command.WalletSourceId = walletSource.WalletId;
                command.WalletDesId = walletDes.WalletId;

                response = await _mediator.Send(command);
            }
            catch (Exception ex)
            {
                response.Code = ErrorCode.GetError(ErrorCode.SystemError).Key;
                response.Message = ErrorCode.GetError(ErrorCode.SystemError).Value;
                Logger.Error($"Exception: {ex} , Method:transfer");
            }

            return Ok(response);
        }



        [Route("getlistaccount")]
        [HttpGet]
        public async Task<IActionResult> GetListAccount()
        {
            var response = new GetListAccountResponse();
            try
            {
                response = await _accountQueries.GetListAccount();
            }
            catch (Exception ex)
            {
                response.Code = ErrorCode.GetError(ErrorCode.SystemError).Key;
                response.Message = ErrorCode.GetError(ErrorCode.SystemError).Value;
                Logger.Error($"Exception: {ex} , Method:GetListAccount");
            }
            return Ok(response);
        }


        [Route("getbalance")]
        [HttpPost]
        public async Task<IActionResult> GetBalance(GetBalanceRequest request)
        {
            var response = new GetBalanceResponse();
            try
            {
                response = await _accountQueries.GetBalance(request);
            }
            catch (Exception ex)
            {
                response.Code = ErrorCode.GetError(ErrorCode.SystemError).Key;
                response.Message = ErrorCode.GetError(ErrorCode.SystemError).Value;
                Logger.Error($"Exception: {ex} , Method:GetBalance");
            }
            return Ok(response);
        }

        [Route("getListtransaction")]
        [HttpGet]
        public async Task<IActionResult> GetListTransaction()
        {
            var response = new GetListTransactionResponse();
            try
            {
                response = await _accountQueries.GetListTransactionAsync();
            }
            catch (Exception ex)
            {
                response.Code = ErrorCode.GetError(ErrorCode.SystemError).Key;
                response.Message = ErrorCode.GetError(ErrorCode.SystemError).Value;

                Logger.Error($"Exception: {ex} , Method:getListtransaction");
            }
            return Ok(response);
        }

        [Route("gettransactiondetail")]
        [HttpPost]
        public async Task<IActionResult> GetTransactinDetail(GetTransactionDetailRequest request)
        {
            var response = new GetTransactionDetailResponse();
            try
            {
                response = await _accountQueries.GetTransactionDetail(request);
            }
            catch (Exception ex)
            {
                response.Code = ErrorCode.GetError(ErrorCode.SystemError).Key;
                response.Message = ErrorCode.GetError(ErrorCode.SystemError).Value;

                Logger.Error($"Exception: {ex} , Method:GetTransactinDetail");
            }
            return Ok(response);
        }
    }
}