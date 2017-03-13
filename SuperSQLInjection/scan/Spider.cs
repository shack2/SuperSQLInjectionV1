using System;
using System.Collections.Generic;
using System.Text;
using SuperSQLInjection.model;
using SuperSQLInjection.tools;
using tools;
using model;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections;

namespace SuperSQLInjection.scan
{
    class Spider
    {
        
        public List<String> AllURL = new List<String>();
        public List<String> AllNoParamaValURL = new List<String>();//用于去掉相似URL
        public static Config config=null;
        public static String reqestGetTemplate = "GET {url} HTTP/1.1\r\nUser-Agent: BaiduSpider\r\nAccept-Encoding: gzip, deflate\r\nHost: {host}";
        public static String reqestPOSTTemplate = "POST {url} HTTP/1.1\r\nUser-Agent: BaiduSpider\r\nAccept-Encoding: gzip, deflate\r\nContent-Type: application/x-www-form-urlencoded\r\nContent-Length: 5\r\nHost: {host}\r\n\r\n{data}";
        public void findLinks(String url)
        {
            try
            {
                if (url.IndexOf("https") != -1)
                {
                    config.useSSL = true;
                }
                else {
                    config.useSSL = false;
                }
                ServerInfo sever = URLTools.getHostAndPathQueryByURL(url);
                Uri uri = new Uri(url);
                String crequest = reqestGetTemplate.Replace("{url}", uri.PathAndQuery).Replace("{host}", uri.Host + ":" + uri.Port);

                String rootPath = "";
                if (("http".Equals(uri.Scheme) && uri.Port == 80) || ("https".Equals(uri.Scheme) && uri.Port == 443))
                {

                    rootPath = uri.Scheme + "://" + uri.Host;
                }
                else
                {
                    rootPath = uri.Scheme + "://" + uri.Host + ":" + uri.Port;
                }
                ServerInfo urlServer = sendHTTP(url.StartsWith("https",StringComparison.OrdinalIgnoreCase),sever.host, sever.port, crequest);
                
                String rootHost = Tools.getRootDomain(uri.Host);
                //当前URL目录
                String cpath = rootPath + Tools.getCurrentPath(uri.AbsolutePath);
                int count = 0;
                if (urlServer != null)
                {
                    //抓取连接+*
                    //Thread.Sleep(200);
                    Match m;
                    Regex reg = new Regex("href=(['\"\\S]?)(?<href>[^'\"]*)", RegexOptions.IgnoreCase);
                    if (urlServer.code == 200 && urlServer.body.Length > 10)
                    {
                        for (m = reg.Match(urlServer.body); m.Success; m = m.NextMatch())
                        {
                            String curl = m.Groups["href"].Value;
                            if (!String.IsNullOrEmpty(curl))
                            {
                                if (!curl.Contains("?") || !curl.Contains("="))
                                {
                                    continue;
                                }

                                if (curl.ToLower().Contains("javascript:"))
                                {
                                    continue;
                                }
                                if (!curl.Contains(".") && !curl.Contains("/"))
                                {
                                    continue;
                                }
                                if (curl.Contains(".css") || curl.Contains(".js") || curl.Contains(".jpg") || curl.Contains(".png") || curl.Contains(".ico") || curl.Contains(".gif"))
                                {
                                    continue;
                                }
                                curl = curl.Replace("&amp;", "&");
                                if (curl.StartsWith("//"))
                                {

                                    curl = "http:" + curl;

                                }
                                else if (curl.StartsWith("/"))
                                {

                                    curl = rootPath + curl;

                                }
                                else if (curl.IndexOf("http://") == -1 && curl.IndexOf("www.") == -1 && curl.IndexOf(".com") == -1 && curl.IndexOf(".cn") == -1 && curl.IndexOf(".tw") == -1 && curl.IndexOf(".jp") == -1)
                                {
                                    //相对路径

                                    curl = cpath + curl;
                                }

                                if (curl.IndexOf(">")!=-1) {
                                    curl = curl.Substring(0, curl.IndexOf(">"));
                                }

                                if (curl.Contains(rootHost))
                                {
                                    //过滤相似URL
                                    String noValURL = Tools.clearURLParams(curl);
                                    try
                                    {
                                        Uri cu = new Uri(curl);
                                        if (!AllURL.Contains(curl) && !AllNoParamaValURL.Contains(noValURL)&&AllURL.Count<config.maxSpiderCount)
                                        {
                                            AllURL.Add(curl);
                                            AllNoParamaValURL.Add(noValURL);
                                            //findLinks(curl, clevel);
                                            count++;
                                        }
                                    }
                                    catch (Exception e) {
                                        Tools.SysLog(curl+"----不是一个URL----"+e.Message);
                                    }   
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {

                Tools.SysLog("爬行异常："+e.Message);

            }
        
        }
        public static ServerInfo sendHTTP(bool isSSL,String host, int port, String request)
        {
            return HTTP.sendRequestRetry(isSSL, config.reTry, host, port, "SQLInjection Spider", request, config.timeOut, HTTP.AutoGetEncoding, true, config.redirectDoGet);
        }
     
    }
}
