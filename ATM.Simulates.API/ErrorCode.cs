using System.Collections.Generic;
using System.Linq;

namespace ATM.Simulates.API
{
    public static class ErrorCode
    {
        private static readonly object Fag = new object();
        public static int SystemError = 999;
        public static int AccountLocked = 101;
        public static int OutRange = 102;
        public static int AmountNotEnough = 103;
        public static int PinWrong = 104;
        public static int AccountNotFound = 105;
        public static int AmountInvalid = 106;
        private static Dictionary<int, string> listError = new Dictionary<int, string>(){
            //GE
            {   OutRange, "Số tiền vượt hạn mức"  },
            {   AmountNotEnough, "Không đủ số dư"  },
            {   AccountLocked, "Tài khoản bí khóa"  },
            {   PinWrong, "Sai mãi Pin"  },
            {   SystemError, "Lỗi hệ thống"  },
             {   AccountNotFound, "Không tìn thấy tài khoản"  },
             {   AmountInvalid, "Số tiền không hợp lệ, Số tiền phải là bội của 50000"  },
        };

        public static KeyValuePair<int, string> GetError(int key)
        {
            lock (Fag)
            {
                return listError.FirstOrDefault(item => item.Key == key);
            }
        }

    }
}

