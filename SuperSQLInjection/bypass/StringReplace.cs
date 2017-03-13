using System;
using System.Collections.Generic;
using System.Text;
using SuperSQLInjection.model;
using System.Text.RegularExpressions;
using SuperSQLInjection.tools;
using System.Collections;
using tools;

namespace SuperSQLInjection.bypass
{
    class StringReplace
    {
        public static String strReplaceCenter(Config config, String request, Hashtable replaceList)
        {
            
            //找到需要处理的字符
            MatchCollection mc = Regex.Matches(request, "(?<=(\\<sEncode\\>))[.\\s\\S]*?(?=(\\<eEncode\\>))");
            String str="";
            foreach (Match m in mc)
            {
                 str = m.Value;
                 
                if (config.reaplaceBeforURLEncode || config.isOpenURLEncoding==false)
                {
                    //替换字符
                    str = ReplaceString(replaceList, str);
                    if (config.inculdeStr)
                    {
                        String split = " ";
                        ///*!包含分隔符*/
                        String val=getValue(replaceList," ");
                        if (!"".Equals(val)) {
                            split = val;
                        }
                        str = ReplaceString(str, split);
                    }
                    if (config.isOpenURLEncoding)
                    {
                        //URL编码
                        str = urlEncoding(str, config.urlencodeCount);
                    }
                }
                else {

                   if (config.inculdeStr)
                    {
                        ///*!包含*/
                        str = ReplaceString(str, "%20");
                    }
                   if (config.isOpenURLEncoding)
                   {
                       //URL编码
                       str = urlEncoding(str,config.urlencodeCount);
                       
                    }
                    //替换字符
                    str = ReplaceString(replaceList, str);
                }
                //随机大小写
                if (config.keyReplace>0)
                {
                    String splitstr = " ";
                    if (config.isOpenURLEncoding) {
                        splitstr = "%20";
                    }
                    str = toLowerOrUpperCase(str, splitstr, config.keyReplace);
                }
                //base64处理
                if (config.base64) {
                    str = base64Encoding(str,config.base64Count);
                }
                //替换request
                request = request.Replace("<sEncode>" + m.Value + "<eEncode>", str);
            }
            return request;
        }

        public static String urlEncoding(String str,int index)
        {

            for (int i = 1; i <= index; i++)
            {
                str=URLEncode.UrlEncode(str);
            }
            return str;

        }


        public static String base64Encoding(String str,int index) {

            for (int i = 1; i <= index; i++) {
                str = Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
            }
            return str;
            
        }
        public static String ReplaceString(Hashtable repalceList,String str) {
            if (repalceList != null && repalceList.Count > 0) {
                try
                {
                    IDictionaryEnumerator ite = repalceList.GetEnumerator();
                    while (ite.MoveNext())
                    {
                        String key = ite.Key.ToString();
                        if (!String.IsNullOrEmpty(key)) {
                            str = str.Replace(key, ite.Value + "");
                        }
                        
                    }
                }
                catch (Exception e)
                {
                    Tools.SysLog("替换字符发生错误！----" + e.Message);
                }
               
            }
            return str;

        }

        public static String ReplaceString(String oldStr,String splitStr)
        {
            StringBuilder sb = new StringBuilder();
            try
            {

                String[] strs = Regex.Split(oldStr, splitStr.Replace("/**/","/\\*\\*/"));
                for (int i = 0; i < strs.Length; i++) {
                    sb.Append("/*!" + strs[i] + "*/");
                    sb.Append(splitStr);
                }
            }
            catch (Exception e)
            {
                Tools.SysLog("使用/*!*/包含关键字发生错误！----" + e.Message);
            }
            return sb.ToString().Remove(sb.Length-splitStr.Length,splitStr.Length);

        }

        public static String getValue(Hashtable table, String key)
        {
            try
            {
                IDictionaryEnumerator ite = table.GetEnumerator();
                while (ite.MoveNext())
                {
                    if (key.Equals(ite.Key)) {
                        return ite.Value+"";
                    }
                }
            }
            catch (Exception e)
            {
                Tools.SysLog("获取对应键值对发生错误！----" + e.Message);
            }
            return "";
        }


        public static String randStr(String key) {
            StringBuilder sb = new StringBuilder();
            Char[] cs = new Char[key.Length];
            cs = key.ToCharArray(0, key.Length);
            for (int j = 0; j < cs.Length; j++)
            {
                string c = cs[j] + "";
                if (j % 2 == 0)
                {
                    c = c.ToUpper();
                }
                else
                {
                    c = c.ToLower();
                }
                sb.Append(c);
            }
            return sb.ToString();
        }

        public static String toLowerOrUpperCase(String oldStr, String split,int changeType)
        {

            StringBuilder sb = new StringBuilder();
            try
            {
                MatchCollection mc = Regex.Matches(oldStr, "([a-zA-Z_\\.]+"+split+ "|[a-zA-Z_\\.]+\\()");
                foreach (Match m in mc) {

                    String keyStr =m.Groups[0].Value;
                    //库名.表不处理防止部分情况出现错误
                    if (keyStr.IndexOf(".") != -1) {
                        continue;
                    }
                    if (changeType == 1) {
                        oldStr = oldStr.Replace(keyStr, randStr(keyStr));
                    }
                    if (changeType == 2)
                    {
                        oldStr = oldStr.Replace(keyStr, keyStr.ToUpper());
                    }
                    if (changeType == 3)
                    {
                        oldStr = oldStr.Replace(keyStr, keyStr.ToLower());
                    } 
                    m.NextMatch();

                }
                /*
                String[] strs = Regex.Split(oldStr, split);
                for (int i = 0; i < strs.Length; i++)
                {
                    String s = strs[i];
                    if (s.IndexOf(".") != -1||s.IndexOf("@") != -1||s.IndexOf("(") != -1)
                    {
                        sb.Append(s);
                    }
                    else {
                    Char[] cs=new Char[s.Length];
                    cs = s.ToCharArray(0, s.Length);
                    for (int j = 0; j < cs.Length; j++)
                    {
                        string c = cs[j]+"";
                        if (j % 2 == 0)
                        {
                            c = c.ToUpper();
                        }
                        else {
                            c = c.ToLower();
                        }
                        sb.Append(c);
                    }
                    }
                    if (i+1 != strs.Length) { 
                        sb.Append(split);
                    }
                }*/
            }
            catch (Exception e)
            {
                Tools.SysLog("生成随机大小写字母发生错误！----" + e.Message);
            }
            return oldStr.Replace("0X","0x");
        }
    }
}
