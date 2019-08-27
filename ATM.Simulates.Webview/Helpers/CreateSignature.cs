using NETCore.Encrypt;
using System;
using System.Linq;

namespace ATM.Simulates.Webview.Helpers
{
    public static class CreateSignature
    {
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static (string, string) CreateSign(object data, string publicKey)
        {
            string key = RandomString(10);
            string bodyraw = string.Empty;
            var _types = data.GetType();
            var _lsort = _types.GetProperties().OrderBy(x => x.Name).ToList();
            foreach (var prop in _lsort)
            {
                var value = prop.GetValue(data, null);
                if (value != null && prop.Name != "keyHash")
                    bodyraw += value.ToString();
            }
            string keyHash = EncryptProvider.RSAEncrypt(publicKey, key);
            string signature = EncryptProvider.HMACSHA256(bodyraw, key);
            return (signature, keyHash);
        }
    }
}

