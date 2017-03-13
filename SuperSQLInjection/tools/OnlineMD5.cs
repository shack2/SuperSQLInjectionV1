using model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace SuperSQLInjection.tools
{
    class OnlineMD5
    {

        public static String decodeMD5_cmd5(String md5){
            ServerInfo server_index=HTTPRequest.getHtml("http://www.cmd5.com/",null,null);
            String VIEWSTATE = Regex.Match(server_index.body, "VIEWSTATE\" value=\"(?<result>\\S+)\"").Groups["result"].Value;

            String data = "__VIEWSTATE=" + VIEWSTATE + "&ctl00%24ContentPlaceHolder1%24TextBoxInput=" + md5 + "&ctl00%24ContentPlaceHolder1%24InputHashType=md5&ctl00%24ContentPlaceHolder1%24Button1=%E8%A7%A3%E5%AF%86";
            ServerInfo server_result = HTTPRequest.getHtmlByPost("http://www.cmd5.com", data, "http://www.cmd5.com/", server_index.cookies);
            String result = Regex.Match(server_result.body, "Answer\">(?<result>\\S+)<br").Groups["result"].Value;
            return result;
        
        }

        public static String decodeMD5_md5_com_cn(String md5)
        {

            ServerInfo server_index=HTTPRequest.getHtml("http://www.md5.com.cn/",null,null);
            String token = Regex.Match(server_index.body, "token\" value=\"(?<result>\\S+)\"").Groups["result"].Value;
            String sand = Regex.Match(server_index.body, "sand\" value=\"(?<result>\\S+)\"").Groups["result"].Value;
            if (token.Length > 1) {

                ServerInfo server_result = HTTPRequest.getHtmlByPost("http://www.md5.com.cn/md5reverse", "md=" + md5 + "&sand=" + sand + "&token=" + token + "&submit=MD5+Crack", "http://www.md5.com.cn/", server_index.cookies);
                String result = Regex.Match(server_result.body, "green\">(?<result>\\S+)</span>").Groups["result"].Value;
                return result;
            }
            return "接口异常";

        }
        public static String decodeMD5_xmd5_org(String md5)
        {

            ServerInfo server_index = HTTPRequest.getHtml("http://www.xmd5.org", null, null);

            ServerInfo server_result = HTTPRequest.getHtml("http://www.xmd5.org/md5/search.asp?hash="+md5+"&xmd5=MD5+%BD%E2%C3%DC", "http://www.xmd5.org/", server_index.cookies);
            String result = Regex.Match(server_result.body, "ff\" size=\"3\">(?<result>\\S+) ").Groups["result"].Value;
            return result;
        }

        public static String decodeMD5_somd5_com(String md5)
        {

            ServerInfo server_result = HTTPRequest.getHtmlByPost("http://www.somd5.com/somd5-index-md5.html", "isajax=sJUVsBd1XOzFDPynHEfSnSt&md5=" + md5, "http://www.somd5.com/", null);
            String result = Regex.Match(server_result.body, "inline;\">(?<result>\\S+)</h1>").Groups["result"].Value;
            return result;
        }
        public static String decodeMD5_md5_cc(String md5)
        {

            ServerInfo server_result = HTTPRequest.getHtml("http://www.md5.cc/ShowMD5Info.asp?GetType=ShowInfo&md5_str="+md5, "http://www.md5.cc/", null);
            String result = Regex.Match(server_result.body, "px\">(?<result>\\S+)</span>").Groups["result"].Value;
            return result;
        }

        public static String decodeMD5_pmd5_com(String md5)
        {
            ServerInfo server_index = HTTPRequest.getHtml("http://pmd5.com/", null, null);
            String VIEWSTATE = Regex.Match(server_index.body, "VIEWSTATE\" value=\"(?<result>\\S+)\"").Groups["result"].Value;
            String EVENTVALIDATION = Regex.Match(server_index.body, "EVENTVALIDATION\" value=\"(?<result>\\S+)\"").Groups["result"].Value;

            String data = "__VIEWSTATE=" + VIEWSTATE + "&__EVENTVALIDATION=" + EVENTVALIDATION + "&key=" + md5 + "&jiemi=MD5%E8%A7%A3%E5%AF%86";
            ServerInfo server_result = HTTPRequest.getHtmlByPost("http://pmd5.com/?action=getpwd", data, "http://pmd5.com/", server_index.cookies);
            String result = Regex.Match(server_result.body, "为“<em>(?<result>\\S+)</em>").Groups["result"].Value;
            return result;

        }
    }
}
