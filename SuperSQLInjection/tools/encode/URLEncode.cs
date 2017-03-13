using System;
using System.Collections.Generic;
using System.Text;

namespace SuperSQLInjection.tools
{
    class URLEncode
    {
       
        public static string UrlEncode(string sInput)
        {
            return UrlEncodeChars(sInput, Encoding.UTF8);
        }
        public static string UrlEncode(string sInput, Encoding oEnc)
        {
            return UrlEncodeChars(sInput, oEnc);
        }
        private static string UrlEncodeChars(string str, Encoding oEnc)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9') || c == '-' || c == '.' || c == '(' || c == ')' || c == '*' || c == '\'' || c == '_' || c == '!')
                {
                    stringBuilder.Append(c);
                }
                else
                {
                        byte[] bytes = oEnc.GetBytes(new char[]{c});
                        byte[] array = bytes;
                        for (int j = 0; j < array.Length; j++)
                        {
                            byte b = array[j];
                            stringBuilder.Append("%");
                            stringBuilder.Append(b.ToString("x2"));
                        } 
                }
            }
            return stringBuilder.ToString();
        }
        // Fiddler.Utilities
        public static string UrlPathEncode(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            int num = str.IndexOf('?');
            if (num >= 0)
            {
                return UrlPathEncode(str.Substring(0, num)) + str.Substring(num);
            }
            return UrlPathEncodeChars(str);
        }

        private static string UrlPathEncodeChars(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                if (c > ' ' && c < '\u007f')
                {
                    stringBuilder.Append(c);
                }
                else
                {
                    if (c < '!')
                    {
                        stringBuilder.Append("%");
                        stringBuilder.Append(((byte)c).ToString("X2"));
                    }
                    else
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(new char[]
				{
					c
				});
                        byte[] array = bytes;
                        for (int j = 0; j < array.Length; j++)
                        {
                            byte b = array[j];
                            stringBuilder.Append("%");
                            stringBuilder.Append(b.ToString("X2"));
                        }
                    }
                }
            }
            return stringBuilder.ToString();
        }
    }
}
