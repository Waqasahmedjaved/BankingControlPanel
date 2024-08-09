using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BankingControlPanel.Core
{
    public static class Validation
    {
        public static bool IsEmailValid(string email)
        {
            string regex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$";

            return Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);
        }

        public static bool IsValisMobileNumber(string number)
        {
            return Regex.Match(number, @"^(\+[0-9]{10})$").Success;
        }
    }
}
