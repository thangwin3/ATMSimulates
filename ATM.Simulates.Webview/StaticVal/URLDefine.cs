using Microsoft.Extensions.Configuration;

namespace ATM.Simulates.Webview.StaticVal
{
    public static class URLDefine
    {
        static string ATM_API_URL = Startup.StaticConfig.GetValue<string>("ATM_API_URL");
        public static string login = ATM_API_URL + "/login";
        public static string deposit = ATM_API_URL + "/deposit";
        public static string withdraw = ATM_API_URL + "/withdraw";
        public static string transfer = ATM_API_URL + "/transfer";
        public static string getlistaccount = ATM_API_URL +"/getlistaccount";
        public static string getbalance = ATM_API_URL + "/getbalance";
        public static string gettransactiondetail = ATM_API_URL + "/gettransactiondetail";
        public static string getListtransaction = ATM_API_URL + "/getListtransaction";
        

    }
}
