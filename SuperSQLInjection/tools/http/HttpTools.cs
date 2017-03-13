using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using model;
using System.Threading;

namespace tools
{
    
    class HttpTools
    {
         public static String getHTMLEncoding(String header){

             Match m=Regex.Match(header, "charset=\\S{0,8}\"");
             if (m.Success) {
                return m.Groups[0].Value.Replace("charset=","").Replace("\"","");
             }
             return "";
         }
         public static String getHtml(String url, int timeout)
         {
             String html = "";
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
                 request.Timeout = timeout * 1000;
                 request.AllowAutoRedirect = false;
                 response = (HttpWebResponse)request.GetResponse();
                 sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                
                 //读取服务器端返回的消息 
                 html = sr.ReadToEnd();

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
             return html;
         }
      
    }
}
