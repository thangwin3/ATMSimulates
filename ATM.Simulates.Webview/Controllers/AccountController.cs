using ATM.Simulates.Webview.Helpers;
using ATM.Simulates.Webview.Models;
using ATM.Simulates.Webview.Response;
using ATM.Simulates.Webview.StaticVal;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NETCore.Encrypt;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ATM.Simulates.Webview.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        ClientService _clientService;
        private readonly IConfiguration _configuration;
        public AccountController(ClientService clientService, IConfiguration configuration)
        {
            _clientService = clientService;
            _configuration = configuration;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.PinCode = EncryptProvider.Sha256(model.PinCode);
                    var loginResponse = await _clientService.PostAsync<LoginResponse>(URLDefine.login, model);
                    if (loginResponse.Code == 0)
                    {

                        SessionHelper.SetObjectAsJson(HttpContext.Session, "Account", loginResponse.Data);

                        var identity = new ClaimsIdentity(new[] {
                       new Claim(ClaimTypes.Name, loginResponse.Data.AccountName)
                        }, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.errmess = loginResponse.Message;
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception: {ex} , Method:Login");
                ErrorViewModel err = new ErrorViewModel();
                err.Message = "Lỗi hệ thống";
                return PartialView("Error", err);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Transfer(TransferModel model)
        {
            ErrorViewModel err = new ErrorViewModel();
            try
            {
                if (ModelState.IsValid)
                {
                    var transferResponse = await _clientService.PostAsync<TransferResponse>(URLDefine.transfer, model);
                    if (transferResponse.Code == 0)
                    {
                        return RedirectToAction("TransactionDetail", "Transaction", new { TransactionId = transferResponse.Data.TransactionId });
                    }
                    else
                    {
                        err.Message = transferResponse.Message;
                    }
                }
                else
                {
                    var listAccount = await _clientService.GetAsync<GetListAccountResponse>(URLDefine.getlistaccount);
                    ViewBag.listAccount = listAccount.Data.ListAccount;
                    return View();
                }
            }

            catch (Exception ex)
            {
                Logger.Error($"Exception: {ex} , Method:Transfer");

            }
            return PartialView("Error", err);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deposit(DepositModel model)
        {
            ErrorViewModel err = new ErrorViewModel();
            try
            {
                if (ModelState.IsValid)
                {
                    var transferResponse = await _clientService.PostAsync<TransferResponse>(URLDefine.deposit, model);
                    if (transferResponse.Code == 0)
                    {
                        return RedirectToAction("TransactionDetail", "Transaction", new { TransactionId = transferResponse.Data.TransactionId });
                    }
                    else
                    {
                        err.Message = transferResponse.Message;
                    }
                }
                else
                {
                    return View();
                }
            }

            catch (Exception ex)
            {
                Logger.Error($"Exception: {ex} , Method:Deposit");
            }
            return PartialView("Error", err);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WithDraw(WithdrawModel model)
        {
            ErrorViewModel err = new ErrorViewModel();
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.Amount < 50000 || model.Amount % 50000 != 0)
                    {
                        err.Message = ErrorCode.GetError(ErrorCode.AmountInvalid).Value;
                    }
                    else
                    {
                        var transferResponse = await _clientService.PostAsync<TransferResponse>(URLDefine.withdraw, model);
                        if (transferResponse.Code == 0)
                        {
                            return RedirectToAction("TransactionDetail", "Transaction", new { TransactionId = transferResponse.Data.TransactionId });
                        }
                        else
                        {
                            err.Message = transferResponse.Message;
                        }
                    }
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception: {ex} , Method:WithDraw");
                err.Message = "Lỗi hệ thống";
            }
            return PartialView("Error", err);
        }

        public async Task<ActionResult> Transfer()
        {
            try
            {

                var listAccount = await _clientService.GetAsync<GetListAccountResponse>(URLDefine.getlistaccount);
                ViewBag.listAccount = listAccount.Data.ListAccount;
                return View();
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception: {ex} , Method:GetTransactinDetail");
                ErrorViewModel err = new ErrorViewModel();
                err.Message = "Lỗi hệ thống";
                return PartialView("Error", err);
            }

        }

        [Authorize]
        public ActionResult Deposit()
        {
            return View();
        }
        [Authorize]
        public ActionResult Withdraw()
        {
            return View();
        }

        [Authorize]
        public async Task<ActionResult> GetBalance()
        {
            try
            {
                var listAccount = await _clientService.GetAsync<GetListAccountResponse>(URLDefine.getlistaccount);
                return View(listAccount.Data);
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception: {ex} , Method:GetBalance");
                ErrorViewModel err = new ErrorViewModel();
                err.Message = "Lỗi hệ thống";
                return PartialView("Error", err);
            }
        }

        [Authorize]
        public IActionResult Logout()
        {
            try
            {
                var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                SessionHelper.Clear(HttpContext.Session, "Account");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception: {ex} , Method:Logout");
                ErrorViewModel err = new ErrorViewModel();
                err.Message = "Lỗi hệ thống";
                return PartialView("Error", err);
            }
        }
    }
}