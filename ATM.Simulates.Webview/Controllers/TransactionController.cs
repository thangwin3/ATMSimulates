using ATM.Simulates.Webview.Helpers;
using ATM.Simulates.Webview.Models;
using ATM.Simulates.Webview.Response;
using ATM.Simulates.Webview.StaticVal;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ATM.Simulates.Webview.Controllers
{
    public class TransactionController : Controller
    {

        ClientService _clientService;
        public TransactionController(ClientService clientService)
        {
            _clientService = clientService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/TransactionDetail/{TransactionId}")]
        public async Task<IActionResult> TransactionDetail(long TransactionId)
        {
            try
            {
                var request = new GetTransactionDetail()
                {
                    TransactionId = TransactionId
                };

                var postResult = await _clientService.PostAsync<GetTransactionDetailResponse>(URLDefine.gettransactiondetail, request);
                if (postResult.Code == 0)
                {

                    return View(postResult.Data);
                }
                return View();
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception: {ex} , Method:TransactionDetail");
                return RedirectToAction("Index", "StatusCode", new { statusCode = 999 }); ;
            }
        }


        public async Task<IActionResult> History()
        {
            try
            {
                var listTransaction = await _clientService.GetAsync<GetListTransactionResponse>(URLDefine.getListtransaction);
                if (listTransaction.Code == 0)
                {
                    return View(listTransaction.Data);
                }
                return View();
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception: {ex} , Method:getListtransaction");
                return RedirectToAction("Index", "StatusCode", new { statusCode = 999 }); ;
            }
        }
    }
}