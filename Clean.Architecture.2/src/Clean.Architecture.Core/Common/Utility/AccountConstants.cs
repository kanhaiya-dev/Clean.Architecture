using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Architecture.Core.Common.Utility
{
    public class AccountConstants
    {
        public static string STATUS_201 = "201";
        public static string MESSAGE_201 = "Account created successfully";

        public static string STATUS_200 = "200";
        public static string MESSAGE_200 = "Request processed successfully";

        public static string STATUS_500 = "500";
        public static string MESSAGE_500 = "An error occurred. Please try again or contact dev team";
        
        public static string STATUS_404 = "404";
        public static string MESSAGE_404 = "Account not found";
        
        public static string STATUS_417 = "417";
        public static string MESSAGE_417 = "Update operation failed. Please try again or contact Dev team";
    }
}
