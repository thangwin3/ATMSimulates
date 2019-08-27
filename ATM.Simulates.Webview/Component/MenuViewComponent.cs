using ATM.Simulates.Webview.Helpers;
using ATM.Simulates.Webview.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATM.Simulates.Webview.Component
{
    public class MenuViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var account = SessionHelper.GetObjectFromJson<DataLoginResponse>(HttpContext.Session, "Account");
            if (account != null)
            {
                ViewBag.account = account.AccountName;
            }
            return View();
        }
    }
}
