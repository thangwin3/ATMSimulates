#pragma checksum "D:\MyProject\ATMSimulates\ATM.Simulates.Webview\Views\Transaction\TransactionDetail.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0080af38c639c43b6f8b4e3ae2626a6eb9aedcf9"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Transaction_TransactionDetail), @"mvc.1.0.view", @"/Views/Transaction/TransactionDetail.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Transaction/TransactionDetail.cshtml", typeof(AspNetCore.Views_Transaction_TransactionDetail))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "D:\MyProject\ATMSimulates\ATM.Simulates.Webview\Views\_ViewImports.cshtml"
using ATM.Simulates.Webview;

#line default
#line hidden
#line 2 "D:\MyProject\ATMSimulates\ATM.Simulates.Webview\Views\_ViewImports.cshtml"
using ATM.Simulates.Webview.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0080af38c639c43b6f8b4e3ae2626a6eb9aedcf9", @"/Views/Transaction/TransactionDetail.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"72410c8819ca2d32c887f4828445f834fd60ee22", @"/Views/_ViewImports.cshtml")]
    public class Views_Transaction_TransactionDetail : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ATM.Simulates.Webview.Response.TransactionDetailData>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 1 "D:\MyProject\ATMSimulates\ATM.Simulates.Webview\Views\Transaction\TransactionDetail.cshtml"
  
    Layout = "~/Views/Shared/_Layout.cshtml";
    ViewData["Title"] = "TransactionDetail Page";

#line default
#line hidden
            BeginContext(166, 85, true);
            WriteLiteral("<div class=\"text-center\">\r\n    <div> Loại giao dịch:  <span class=\"label label-info\">");
            EndContext();
            BeginContext(252, 21, false);
#line 7 "D:\MyProject\ATMSimulates\ATM.Simulates.Webview\Views\Transaction\TransactionDetail.cshtml"
                                                     Write(Model.TransactionType);

#line default
#line hidden
            EndContext();
            BeginContext(273, 67, true);
            WriteLiteral("</span> </div>\r\n    <div> Số tiền:  <span class=\"label label-info\">");
            EndContext();
            BeginContext(341, 12, false);
#line 8 "D:\MyProject\ATMSimulates\ATM.Simulates.Webview\Views\Transaction\TransactionDetail.cshtml"
                                              Write(Model.Amount);

#line default
#line hidden
            EndContext();
            BeginContext(353, 69, true);
            WriteLiteral("</span> </div>\r\n    <div> Thời gian:  <span class=\"label label-info\">");
            EndContext();
            BeginContext(423, 52, false);
#line 9 "D:\MyProject\ATMSimulates\ATM.Simulates.Webview\Views\Transaction\TransactionDetail.cshtml"
                                                Write(Model.TransactionDate.ToString("hh:mm:ss dd/MM/yyy"));

#line default
#line hidden
            EndContext();
            BeginContext(475, 82, true);
            WriteLiteral("</span> </div>\r\n    <div> Trạng Thái Giao dịch  <span class=\"label label-success\">");
            EndContext();
            BeginContext(558, 12, false);
#line 10 "D:\MyProject\ATMSimulates\ATM.Simulates.Webview\Views\Transaction\TransactionDetail.cshtml"
                                                             Write(Model.Status);

#line default
#line hidden
            EndContext();
            BeginContext(570, 25, true);
            WriteLiteral("</span></div>\r\n</div>\r\n\r\n");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ATM.Simulates.Webview.Response.TransactionDetailData> Html { get; private set; }
    }
}
#pragma warning restore 1591