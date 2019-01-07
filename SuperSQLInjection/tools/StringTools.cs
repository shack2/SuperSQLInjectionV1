using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SuperSQLInjection.tools
{
    class StringTools
    {

        public static bool CheckIsIP(String ipStr) {
            return Regex.IsMatch(ipStr, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
        }

        public static bool CheckIsDomain(String ipStr)
        {
            return Regex.IsMatch(ipStr, "[\\w\\-\\.]{1,100}[a-zA-Z]{1,8}");
        }

        public static bool IsNumber(String ipStr)
        {
            return Regex.IsMatch(ipStr, @"[\d]{1,5}");
        }
        
        public static bool CheckIsDomainOrIP(String str)
        {
            return (CheckIsDomain(str)|| CheckIsIP(str));
        }
    }
}
