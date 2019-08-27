using ATM.Simulates.Webview.Helpers;
using ATM.Simulates.Webview.Models;
using ATM.Simulates.Webview.Response;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ATM.Simulates.Webview.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var account = SessionHelper.GetObjectFromJson<DataLoginResponse>(HttpContext.Session, "Account");
            if (account != null)
            {
                ViewBag.account = account.AccountName;

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
