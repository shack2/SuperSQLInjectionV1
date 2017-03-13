using model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using tools;

namespace SuperSQLInjection.tools
{
    class HTTPRequest
    {
        public static ServerInfo getHtmlByPost(String url, String data,String referer,String cookies)
        {
            ServerInfo server = new ServerInfo();
            HttpWebResponse response = null;
            StreamReader sr = null;
            HttpWebRequest request = null;
            
            try
            {
                //设置模拟http访问参数
                Uri uri = new Uri(url);
                request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.UserAgent = "Mozilla/5.0";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Timeout = 30000;
                request.KeepAlive = true;
                if (referer != null) {
                    request.Referer = referer;
                }
                request.AllowAutoRedirect = false;
                if (!"".Equals(cookies))
                {
                    request.Headers.Add("Cookie", cookies);
                }
                byte[] bydata = Encoding.ASCII.GetBytes(data);
                request.ContentLength = bydata.Length;
                Stream reqStream = request.GetRequestStream();
                reqStream.Write(bydata, 0, bydata.Length);
                reqStream.Close();
                response = (HttpWebResponse)request.GetResponse();
                CookieCollection cc  = response.Cookies;
                StreamReader str = new StreamReader(response.GetResponseStream());
                server.body= str.ReadToEnd();
                server.cookies = response.Headers["Set-Cookie"];
            }
            catch (Exception e)
            {
                Tools.SysLog(e.Message);
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return server;
        }
        public static String getHTMLEncoding(String header)
        {

            Match m = Regex.Match(header, "charset=\\S{0,8}\"");
            if (m.Success)
            {
                return m.Groups[0].Value.Replace("charset=", "").Replace("\"", "");
            }
            return "";
        }
        public static ServerInfo getHtml(String url,String referer,String cookies)
        {
            ServerInfo server = new ServerInfo();
            HttpWebResponse response = null;
            StreamReader sr = null;
            HttpWebRequest request = null;
            try
            {

                //设置模拟http访问参数
                Uri uri = new Uri(url);
                request = (HttpWebRequest)WebRequest.Create(uri);
                request.Accept = "*/*";
                request.Method = "GET";
                request.Timeout = 30000;
                request.AllowAutoRedirect = false;
                if (referer != null)
                {
                    request.Referer = referer;
                }
                if (!"".Equals(cookies))
                {
                    request.Headers.Add("Cookie", cookies);
                }
                response = (HttpWebResponse)request.GetResponse();
                sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);

                //读取服务器端返回的消息 
                server.body = sr.ReadToEnd();
                server.cookies = response.Headers["Set-Cookie"];

            }
            catch (Exception e)
            {
                Tools.SysLog(e.Message);
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return server;
        }
    }
}
