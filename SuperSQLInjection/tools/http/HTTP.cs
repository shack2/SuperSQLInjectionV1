using System;
using System.Text;
using tools;
using System.Net.Sockets;
using model;
using System.IO.Compression;
using System.IO;
using System.Net.Security;
using System.Text.RegularExpressions;
using System.Threading;
using System.Diagnostics;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using SuperSQLInjection.bypass;
using SuperSQLInjection.tools.http;
using System.Net;
using SuperSQLInjection.model;
using System.Collections.Generic;
using System.Collections;

namespace SuperSQLInjection.tools
{
    public class HTTP
    {
        
        public const char T = '\n';
        public const String ST = "\n";
        public const String CT = "\r\n";
        public const String AutoGetEncoding = "自动识别";
        public const String DefaultEncoding = "UTF-8";
        public const String CTRL = "\r\n\r\n";
        public const String Content_Length_Str = "content-length: ";
        public const String Content_Length_Str_M = "Content-Length: ";
        public const String Content_Length = "content-length";
        public const String Content_Encoding = "content-encoding";
        public const String Transfer_Encoding = "transfer-encoding";
        public const String Connection = "connection";

        public const String Content_Length_Zero= "Content-Length: 0";

        public const String ConnectionClose = "connection: close";
        public const int WaitTime =5;
        public static Main main = null;
        public static long index = 0;

        public const String Socks5ProxyType = "Socks5";

        public static String getTemplate = "GET /mysql.jsp?id=1 HTTP/1.1\r\nUser-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.135 Safari/537.36 Edge/12.10240\r\nAccept-Encoding: gzip, deflate\r\nHost: 127.0.0.1:8090\r\nConnection: Close\r\nCookie: JSESSIONID=2F6D5F1AC8C376FF0AB48A08282A6CED";
        public static String postTemplate = "POST /search/index.htm HTTP/1.1\r\nReferer: http://www.shack2.org/\r\nUser-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.135 Safari/537.36 Edge/12.10240\r\nContent-Type: application/x-www-form-urlencoded\r\nAccept-Encoding: gzip, deflate\r\nContent-Length: 5\r\nHost: www.shack2.org\r\nConnection: Keep-Alive\r\nPragma: no-cache\r\nCookie: CNZZDATA4159773=cnzz_eid%3D217492251-1446476958-%26ntime%3D1447834260; bdshare_firstime=1446476958863\r\n\r\nkey=s";
        public static void initMain(Main m)
        {
            main = m;
        }

        /**
         * 
         发生异常尝试重连  
         *
         */
        public static ServerInfo sendRequestRetry(Boolean isSSL, int tryCount, String host, int port, String payload, String request, int timeout, String encoding, Boolean foward_302,Boolean redirectDoGet)
        {
            if (request.IndexOf("<Token>") != -1) {
                String token = "";

                if (!"".Equals(main.config.token_request) &&!"".Equals(main.config.token_startStr)&& !"".Equals(main.config.token_endStr))
                {
                    ServerInfo tserver = HTTP.sendRequestRetryNoToken(isSSL, tryCount, host, port, "获取Token", main.config.token_request, timeout, encoding, foward_302, redirectDoGet);
                    token = Tools.substr(tserver.body, main.config.token_startStr, main.config.token_endStr);
                }
                request = Regex.Replace(request, "(\\<Token\\>[.\\s\\S]*?\\<\\/Token\\>)", token);
            }

            int count = 0;
            
            ServerInfo server = new ServerInfo();
            timeout = timeout * 1000;
            while (true)
            {
                if (count >tryCount) break;

                try
                {
                    if (!isSSL)
                    {
                        server = sendHTTPRequest(count, host, port, payload, request, timeout, encoding, foward_302,redirectDoGet);
                        if (!String.IsNullOrEmpty(main.config.sencondRequest) && main.config.sencondInject)
                        {
                            server = sendHTTPRequest(count, host, port, "请求二次注入页面", main.config.sencondRequest, timeout, encoding, foward_302, redirectDoGet);
                        }
                        return server;
                    }
                    else
                    {

                        server = sendHTTPSRequest(count, host, port, payload, request, timeout, encoding, foward_302, redirectDoGet);
                        if (!String.IsNullOrEmpty(main.config.sencondRequest)&& main.config.sencondInject)
                        {
                            server = sendHTTPSRequest(count, host, port, "请求二次注入页面", main.config.sencondRequest, timeout, encoding, foward_302, redirectDoGet);
                        }
                        return server;

                    }
                    
                }
                catch (Exception e)
                {
                    Tools.SysLog(e.Message);
                    main.showLog(e.Message, LogLevel.waring);
                    server.timeout = true;
                    continue;
                }
                finally
                {
                    count++;
                }

            }
            return server;

        }

        public static ServerInfo sendRequestRetryNoToken(Boolean isSSL, int tryCount, String host, int port, String payload, String request, int timeout, String encoding, Boolean foward_302, Boolean redirectDoGet)
        {
            int count = 0;

            ServerInfo server = new ServerInfo();
            timeout = timeout * 1000;
            while (true)
            {
                if (count > tryCount) break;

                try
                {
                    if (!isSSL)
                    {
                        server = sendHTTPRequest(count, host, port, payload, request, timeout, encoding, foward_302, redirectDoGet);
                        return server;
                    }
                    else
                    {

                        server = sendHTTPSRequest(count, host, port, payload, request, timeout, encoding, foward_302, redirectDoGet);
                        return server;

                    }
                }
                catch (Exception e)
                {
                    Tools.SysLog(e.Message);
                    main.showLog(e.Message, LogLevel.waring);
                    server.timeout = true;
                    continue;
                }
                finally
                {
                    count++;
                }

            }
            return server;

        }

        private static void checkContentLength(ref ServerInfo server,ref String request) {

            //重新计算并设置Content-length
            int sindex = request.IndexOf(CTRL);
            server.request = request;
            if (sindex != -1)
            {
               
                server.reuqestHeader = request.Substring(0, sindex);
                //chunked发送数据不修正Content-Length
                if (server.reuqestHeader.IndexOf("Transfer-Encoding: chunked")!=-1) {
                    return;
                }
                server.reuqestBody = request.Substring(sindex + 4, request.Length - sindex - 4);
                int contentLength = Encoding.UTF8.GetBytes(server.reuqestBody).Length;
                String newContentLength = Content_Length_Str_M + contentLength;
                //产生随机ip头
                if (!String.IsNullOrEmpty(main.config.randIPToHeader))
                {
                    request = request.Insert(sindex, "\r\n" + main.config.randIPToHeader + ": " + Tools.randIP());
                }

                if (request.IndexOf(Content_Length_Str_M) != -1)
                {
                    request = Regex.Replace(request, Content_Length_Str_M + "\\d+", newContentLength);
                }
                else
                {
                    if (request.StartsWith("POST")|| contentLength!=0||request.StartsWith("PUT")) {
                        request = request.Insert(sindex, "\r\n" + newContentLength);
                    }
                }
            }
            else
            {
                //产生随机ip头
                if (!String.IsNullOrEmpty(main.config.randIPToHeader))
                {
                    request = request + "\r\n" + main.config.randIPToHeader + ": " + Tools.randIP();
                }
                request = Regex.Replace(request, Content_Length_Str + "\\d+", Content_Length_Str_M + "0");
                request += CTRL;
            }


        }

        private static void doHeader(ref ServerInfo server, ref String[] headers, ref String encoding)
        {
            try
            {
                for (int i = 0; i < headers.Length; i++)
                {
                    if (i == 0)
                    {
                        String[] codesplit = headers[i].Split(' ');
                        if (codesplit.Length > 0)
                        {
                            String[] sh = headers[i].Split(' ');
                            if (sh.Length > 1) {
                                server.code = Tools.convertToInt(sh[1]);
                            } 
                        }
                        else {
                            server.code = 0;
                        }
                    }
                    else
                    {
                        String[] kv = Regex.Split(headers[i], ": ");
                        String key = kv[0].ToLower();
                        if (!server.headers.ContainsKey(key))
                        {
                            //自动识别编码
                            if ("自动识别".Equals(encoding)) {
                                if ("content-type".Equals(key))
                                {
                                    String hecnode = getHTMLEncoding(kv[1], "");
                                    if (!String.IsNullOrEmpty(hecnode))
                                    {
                                        server.encoding = hecnode;
                                    }
                                }
                            }
                            
                            if (kv.Length > 1)
                            {
                                server.headers.Add(key, kv[1]);
                            }
                            else
                            {
                                server.headers.Add(key, "");
                            }
                        }
                    }
                }
            }
            catch (Exception e) {
                throw e;
            }
            

        }

        private static ServerInfo sendHTTPRequest(int count, String host, int port, String payload, String request, int timeout, String encoding, Boolean foward_302,Boolean redirectDoGet)
        {
            request = request.Replace(CT, ST).Replace(ST, CT);
            Interlocked.Increment(ref HTTP.index);
            String index = Thread.CurrentThread.Name+ Interlocked.Read(ref HTTP.index);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            ServerInfo server = new ServerInfo();
            TcpClient clientSocket = null;
            int sum = 0;
            Proxy cproxy = null;//当前使用代理
            Boolean isupdateEncoding = false;
            MemoryStream body_data = new MemoryStream();
            try
            {
                if (port > 0 && port <= 65556)
                {
                    request = request.Replace(Main.setInjectStr, payload);
                    request = StringReplace.strReplaceCenter(main.config, request, main.replaceList);
                    //编码处理
                    server.request = request;

                    TimeOutSocket tos = new TimeOutSocket();
                    if (main.config.proxy_mode == 1 || main.config.proxy_mode == 2)
                    {
                        if (main.config.proxy_mode == 1)
                        {
                            //随机代理
                            cproxy = getRandProxy();
                        }
                        else
                        {
                            cproxy = main.proxy;
                        }
                        //为空，没有代理资源
                        if (cproxy == null)
                        {
                            //不使用代理
                            try
                            {
                                clientSocket = tos.Connect(host, port, timeout);
                            }
                            catch (Exception)
                            {
                                Tools.SysLog(host + ":" + port + "无法连接！");
                            }
                        }
                        else {
                            if (Socks5ProxyType.Equals(cproxy.proxyType))
                            {
                                SocketProxy sp = new SocketProxy();
                                bool isok = false;
                                clientSocket = sp.creatProxySocket(cproxy.host, cproxy.port, timeout);
                                if (clientSocket != null)
                                {
                                    isok = sp.ConnectProxyServer(host, port, clientSocket, cproxy.username, cproxy.password, timeout);
                                }
                                if (!isok)
                                {
                                    throw new Exception("代理连接失败！");
                                }
                                tos.useTime = sp.ConectProxyUseTime;
                            }
                            else
                            {
                                //直接替换IP和端口即可
                                clientSocket = tos.Connect(cproxy.host, cproxy.port, timeout);
                            }
                        } 
                    }

                    else
                    {
                        //不使用代理
                        try
                        {
                            clientSocket = tos.Connect(host, port, timeout);
                        }
                        catch (Exception) {
                            Tools.SysLog(host+":"+ port+"无法连接！");
                        }
                       
                    }
                 
                    if (sw.ElapsedMilliseconds > timeout)
                    {
                        return server;
                    }
                    
                    if (clientSocket!=null&&clientSocket.Connected)
                    {
                        clientSocket.SendTimeout = timeout - tos.useTime;
                        checkContentLength(ref server, ref request);
                        server.request = request;
                        //分开发送header和body，可以绕过某些情况下的安全防护Connection: close,不能使用这种方式
                        if (!server.reuqestHeader.ToLower().Contains(ConnectionClose))
                        {
                            String[] reqs = Regex.Split(request, "\r\n\r\n");
                            server.reuqestHeader = reqs[0];
                            server.reuqestBody = reqs[1];
                            clientSocket.Client.Send(Encoding.UTF8.GetBytes(server.reuqestHeader + "\r\n\r\n"));
                            clientSocket.Client.Send(Encoding.UTF8.GetBytes(server.reuqestBody));
                        }
                        else
                        {
                            clientSocket.Client.Send(Encoding.UTF8.GetBytes(request));
                        }
                       
                        int len = 0;
                        //获取header头
                        String tmp = "";
                        StringBuilder sb = new StringBuilder();
                        clientSocket.ReceiveTimeout = timeout - (int)sw.ElapsedMilliseconds;
                        do
                        {
                            byte[] responseHeader = new byte[1];
                            len = clientSocket.Client.Receive(responseHeader, 1, SocketFlags.None);
                            if (len <= 0) {
                                Thread.Sleep(WaitTime);
                            }
                            if (len == 1)
                            {

                                char c = (char)responseHeader[0];
                                sb.Append(c);
                                if (c.Equals(T))
                                {
                                    tmp = String.Concat(sb[sb.Length - 4], sb[sb.Length - 3], sb[sb.Length - 2], c);
                                }
                            }
                        } while (!tmp.Equals(CTRL) && sw.ElapsedMilliseconds <= timeout);

                        server.header = sb.ToString().Replace(CTRL, "");
                        String[] headers = Regex.Split(server.header, CT);
                        if (headers != null && headers.Length > 0)
                        {
                            //处理header
                            doHeader(ref server, ref headers, ref encoding);
                            //302 301跳转
                            if ((server.code == 302 || server.code == 301) && foward_302)
                            {
                                StringBuilder rsb = new StringBuilder(server.request);
                                int urlStart = server.request.IndexOf(" ") + 1;
                                int urlEnd = server.request.IndexOf(" HTTP");
                                if (urlStart != -1 && urlEnd != -1)
                                {
                                    String url = server.request.Substring(urlStart, urlEnd - urlStart);
                                    rsb.Remove(urlStart, url.Length);
                                    String location = server.headers["location"];
                                    if (!server.headers["location"].StartsWith("/") && !server.headers["location"].StartsWith("http"))
                                    {
                                        location = Tools.getCurrentPath(url) + location;
                                    }
                                    location = location.Replace(" ", "%20");
                                    rsb.Insert(urlStart, location);
                                    String newReuqest = rsb.ToString();
                                    if (server.request.StartsWith("POST") && redirectDoGet) {
                                        rsb.Remove(0, 4);
                                        rsb.Insert(0, "GET");
                                    }
                                    return sendHTTPRequest(count, host, port, payload, rsb.ToString(), timeout, encoding, false, redirectDoGet);
                                }

                            }


                            //根据请求头解析
                            if (server.headers.ContainsKey(Content_Length)&& server.header.IndexOf(Content_Length_Zero) ==-1)
                            {
                                int length = int.Parse(server.headers[Content_Length]);
                                while (sum < length && sw.ElapsedMilliseconds <= timeout)
                                {
                                    int read = (length-sum);
                                    if (read > 1024) {
                                        read = 1024;
                                    }
                                    byte[] response_data = new byte[read];
                                    len = clientSocket.Client.Receive(response_data, 0, read, SocketFlags.None);
                                    if (len > 0)
                                    {
                                        body_data.Write(response_data, 0,len);
                                        sum += len;
                                    }
                                    if(len<=0&& sum < length) {
                                       Thread.Sleep(WaitTime);
                                    }
                                }
                            }
                            //解析chunked传输
                            else if (server.headers.ContainsKey(Transfer_Encoding))
                            {
                                //读取长度
                                int chunkedSize = 0;
                                byte[] chunkedByte = new byte[1];
                                //读取总长度
                                sum = 0;
                                do
                                {
                                    String ctmp = "";
                                    do
                                    {
                                        len = clientSocket.Client.Receive(chunkedByte, 1, SocketFlags.None);
                                        if(len<=0) {
                                            Thread.Sleep(WaitTime);
                                        }
                                        ctmp += Encoding.UTF8.GetString(chunkedByte);

                                    } while ((ctmp.IndexOf(CT) == -1) && (sw.ElapsedMilliseconds <= timeout));

                                    chunkedSize = Tools.convertToIntBy16(ctmp.Replace(CT, ""));

                                    //chunked的结束0\r\n\r\n是结束标志，单个chunked块\r\n结束
                                    if (ctmp.Equals(CT))
                                    {
                                        continue;
                                    }
                                    if (chunkedSize == 0)
                                    {
                                        //结束了
                                        break;
                                    }
                                    int onechunkLen = 0;
                                    while (onechunkLen < chunkedSize && sw.ElapsedMilliseconds <= timeout)
                                    {
                                      
                                        int read = chunkedSize - onechunkLen;
                                        if (read > 1024)
                                        {
                                            read = 1024;
                                        }
                                        byte[] response_data = new byte[read];
                                        len = clientSocket.Client.Receive(response_data, 0, read, SocketFlags.None);
                                        if (len > 0)
                                        {
                                            body_data.Write(response_data,0, len);
                                            onechunkLen += len;
                                            sum += len;
                                        }
                                        if(len<=0&& onechunkLen < chunkedSize)
                                        {
                                            Thread.Sleep(WaitTime);
                                        }
                                    }

                                    //判断
                                } while (sw.ElapsedMilliseconds <= timeout);
                            }
                            //connection close方式或未知body长度
                            else
                            {
                                //等待数据可读
                                while (sw.ElapsedMilliseconds <= timeout&&!clientSocket.Client.Poll(timeout, SelectMode.SelectRead))
                                {

                                }
                                while (sw.ElapsedMilliseconds <= timeout)
                                {
                                    if (clientSocket.Available <= 0)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        int read = clientSocket.Available;
                                        if (read > 0)
                                        {
                                            byte[] response_data = new byte[read];
                                            len = clientSocket.Client.Receive(response_data, 0, read,SocketFlags.None);
                                            if (len > 0)
                                            {
                                                sum += len;
                                                body_data.Write(response_data, 0, len);
                                            }
                                        }
                                        else
                                        {
                                            Thread.Sleep(WaitTime);
                                        }
                                    }

                                }
                            }

                            //自动识别编码
                            if (AutoGetEncoding.Equals(encoding))
                            {
                                if (!String.IsNullOrEmpty(server.encoding))
                                {
                                    encoding = server.encoding;//header找到编码
                                }
                                else {
                                    encoding = DefaultEncoding;//默认一个编码
                                    isupdateEncoding = true;//body找编码
                                }
                                Encoding encod = Encoding.GetEncoding(encoding);
                               
                                getBody(ref server, ref body_data, ref encod, ref index);
                                //修正编码
                                if (isupdateEncoding)
                                {
                                    String cEncoding = getHTMLEncoding("", server.body);
                                    if (!String.IsNullOrEmpty(cEncoding))
                                    {
                                        server.encoding = cEncoding;//body找到编码
                                        getBody(ref server, ref body_data, ref encod, ref index);
                                    }

                                }
                            }
                            else {
                                //指定编码
                                Encoding encod = Encoding.GetEncoding(encoding);
                                getBody(ref server, ref body_data, ref encod, ref index);
                            }
                            
                        }
                    }

                }
            }
            catch (Exception e)
            {
                Exception ee = new Exception("HTTP发包错误,错误消息：" + e.Message + e.TargetSite.Name + "----发包编号：" + index);
                throw ee;
            }
            finally
            {
                sw.Stop();
                server.length = sum;
                server.runTime = sw.ElapsedMilliseconds;
                body_data.Close();
                if (clientSocket != null)
                {
                    clientSocket.Close();
                }

                if (main.config.isOpenHTTPLog)
                {
                    server.sleepTime = main.config.sendHTTPSleepTime;
                    Tools.sysHTTPLog(index, server);
                    String proxyInfo = "";
                    if (cproxy != null) {
                        proxyInfo = cproxy.host + ":" + cproxy.port;
                    }
                    main.showHTTPLog(index, server, payload, proxyInfo);
                   
                }
                if (main.config.sendHTTPSleepTime > 0)
                {
                    Thread.Sleep(main.config.sendHTTPSleepTime);
                }
            }
            return server;

        }
        
        private static Random rd = new Random();
        private static Proxy getRandProxy() {
            //复制一个，如果有未验证或验证失败的去掉。
            Dictionary<String, Proxy> ok_porxyList = new Dictionary<String, Proxy>(main.proxy_List);

            while (ok_porxyList.Count>0) {

                int rand = rd.Next(0,ok_porxyList.Count);
                lock (ok_porxyList)
                {
                    int i = 0;

                    foreach (Proxy proxy in ok_porxyList.Values)
                    {
                        if (i == rand)
                        {
                            if ("是".Equals(proxy.isOk))
                            {
                                return proxy;
                            }
                            else
                            {
                                ok_porxyList.Remove(proxy.host+ proxy.port);
                                break;
                            }
                        }

                        i++;
                    }

                }
            }
            return null;
        }

        private static void getBody(ref ServerInfo server, ref MemoryStream body_data, ref Encoding encod, ref String index) {
            if (server.headers.ContainsKey(Content_Encoding))
            {
                if (server.headers[Content_Encoding].IndexOf("gzip") != -1)
                {
                    server.body = unGzip(ref body_data, encod, index);
                }
                else if (server.headers[Content_Encoding].IndexOf("deflate") != -1)
                {
                    server.body = unDeflate(ref body_data, encod, index);
                }
                else {
                    server.body = encod.GetString(body_data.ToArray());
                }

            }
            else
            {
                server.body = encod.GetString(body_data.ToArray());
            }
        }

        private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        

        private static ServerInfo sendHTTPSRequest(int count, String host, int port, String payload, String request, int timeout, String encoding, Boolean foward_302,Boolean redirectDoGet)
        {
            request = request.Replace(CT, ST).Replace(ST, CT);
            Interlocked.Increment(ref HTTP.index);
            String index = Thread.CurrentThread.Name + Interlocked.Read(ref HTTP.index);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            ServerInfo server = new ServerInfo();
            Boolean isupdateEncoding = false;
            int sum = 0;
            Proxy cproxy = null;//当前使用代理
            TcpClient clientSocket = null; ;
            MemoryStream body_data = new MemoryStream();
            try
            {

                if (port > 0 && port <= 65556)
                {
                    request = request.Replace(Main.setInjectStr, payload);

                    //编码处理
                    request = StringReplace.strReplaceCenter(main.config, request, main.replaceList);
                    TimeOutSocket tos = new TimeOutSocket();
                    if (main.config.proxy_mode == 1 || main.config.proxy_mode == 2)
                    {
                        if (main.config.proxy_mode == 1)
                        {
                            //随机代理
                            cproxy = getRandProxy();
                        }
                        else
                        {
                            cproxy = main.proxy;
                        }
                        //为空，没有代理资源
                        if (cproxy == null)
                        {
                            //不使用代理
                            try
                            {
                                clientSocket = tos.Connect(host, port, timeout);
                            }
                            catch (Exception)
                            {
                                Tools.SysLog(host + ":" + port + "无法连接！");
                            }
                        }
                        else
                        {
                            if (Socks5ProxyType.Equals(cproxy.proxyType))
                            {
                                SocketProxy sp = new SocketProxy();
                                bool isok = false;
                                clientSocket = sp.creatProxySocket(cproxy.host, cproxy.port, timeout);
                                if (clientSocket != null)
                                {
                                    isok = sp.ConnectProxyServer(host, port, clientSocket, cproxy.username, cproxy.password, timeout);
                                }
                                if (!isok)
                                {
                                    throw new Exception("代理连接失败！");
                                }
                                tos.useTime = sp.ConectProxyUseTime;
                            }
                            else
                            {
                                //直接替换IP和端口即可
                                clientSocket = tos.Connect(cproxy.host, cproxy.port, timeout);
                            }
                        }
                    }

                    else
                    {
                        try
                        {
                            clientSocket = tos.Connect(host, port, timeout);
                        }
                        catch (Exception)
                        {
                            Tools.SysLog(host + ":" + port + "无法连接！");
                        }
                    }

                    if (sw.ElapsedMilliseconds >= timeout)
                    {
                        return server;
                    }
                   

                    SslStream ssl = null;
                    if (clientSocket!=null&&clientSocket.Connected)
                    {
                        clientSocket.SendTimeout = timeout - tos.useTime;
                        ssl = new SslStream(clientSocket.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificate));
                       
                        //增加支持TLS1.1和TLS1.2支持3072，768
                        SslProtocols protocol = (SslProtocols)3072|(SslProtocols)768|SslProtocols.Tls|SslProtocols.Ssl3;
                        ssl.AuthenticateAsClient(host, null, protocol, false);
                        if (ssl.IsAuthenticated)
                        {
                            checkContentLength(ref server, ref request);
                            server.request = request;
                            //分开发送header和body，可以绕过某些情况下的安全防护Connection: close,不能使用这种方式
                            if (!server.reuqestHeader.ToLower().Contains(ConnectionClose))
                            {
                                String[] reqs = Regex.Split(request, "\r\n\r\n");
                                server.reuqestHeader = reqs[0];
                                server.reuqestBody = reqs[1];
                                ssl.Write(Encoding.UTF8.GetBytes(server.reuqestHeader + "\r\n\r\n"));
                                ssl.Write(Encoding.UTF8.GetBytes(server.reuqestBody));
                            }
                            else {
                                ssl.Write(Encoding.UTF8.GetBytes(request));
                            }
                            ssl.Flush();
                        }
                    }
                    server.request = request;
                    int len = 0;
                    //获取header头
                    String tmp = "";

                    StringBuilder sb = new StringBuilder();
                   
                    clientSocket.ReceiveTimeout = timeout - (int)sw.ElapsedMilliseconds;
                    do
                    {
                        byte[] responseHeader = new byte[1];
                        int read = ssl.ReadByte();
                        if (read <= 0)
                        {
                            Thread.Sleep(WaitTime);
                        }
                        char c = (char)read;
                        sb.Append(c);
                        if (c.Equals(T))
                        {
                            tmp = String.Concat(sb[sb.Length - 4], sb[sb.Length - 3], sb[sb.Length - 2], c);
                        }

                    } while (!tmp.Equals(CTRL) && sw.ElapsedMilliseconds <= timeout);

                    server.header = sb.ToString().Replace(CTRL, "");
                    String[] headers = Regex.Split(server.header, CT);
                    //处理header
                    doHeader(ref server, ref headers, ref encoding);
                    //302 301跳转
                    if ((server.code == 302 || server.code == 301) && foward_302)
                    {

                        StringBuilder rsb = new StringBuilder(server.request);
                        int urlStart = server.request.IndexOf(" ") + 1;
                        int urlEnd = server.request.IndexOf(" HTTP");
                        if (urlStart != -1 && urlEnd != -1)
                        {
                            String url = server.request.Substring(urlStart, urlEnd - urlStart);
                            rsb.Remove(urlStart, url.Length);
                            String location = server.headers["location"];
                            if (!server.headers["location"].StartsWith("/") && !server.headers["location"].StartsWith("http"))
                            {
                                location = Tools.getCurrentPath(url) + location;
                            }
                            location = location.Replace(" ", "%20");
                            rsb.Insert(urlStart, location);
                            String newReuqest = rsb.ToString();
                            if (rsb.ToString().StartsWith("POST") && redirectDoGet)
                            {
                                rsb.Remove(0, 4);
                                rsb.Insert(0, "GET");
                            }

                            return sendHTTPSRequest(count, host, port, payload, rsb.ToString(), timeout, encoding, false, redirectDoGet);
                        }


                    }


                    //根据请求头解析
                    if (server.headers.ContainsKey(Content_Length) && server.header.IndexOf(Content_Length_Zero) == -1)
                    {
                        int length = int.Parse(server.headers[Content_Length]);
                        
                        while (sum < length && sw.ElapsedMilliseconds <= timeout)
                        {
                            int read = length - sum;
                            if (read > 1024)
                            {
                                read = 1024;
                            }
                            byte[] response_data = new byte[read];
                            
                            len = ssl.Read(response_data, 0, read);
                           
                            if (len > 0)
                            {
                                sum += len;
                                body_data.Write(response_data, 0, len);
                            }
                            if (len <= 0 && sum < length)
                            {
                                Thread.Sleep(WaitTime);
                            }
                        }
                    }
                    //解析chunked传输
                    else if (server.headers.ContainsKey(Transfer_Encoding))
                    {
                        //读取长度
                        int chunkedSize = 0;
                        byte[] chunkedByte = new byte[1];
                        //读取总长度
                        sum = 0;
                        do
                        {
                            String ctmp = "";
                            do
                            {
                                len = ssl.Read(chunkedByte, 0, 1);
                                if (len <= 0)
                                {
                                    Thread.Sleep(WaitTime);
                                }
                                ctmp += Encoding.UTF8.GetString(chunkedByte);

                            } while (ctmp.IndexOf(CT) == -1 && sw.ElapsedMilliseconds <= timeout);

                            chunkedSize = Tools.convertToIntBy16(ctmp.Replace(CT, ""));

                            //chunked的结束0\r\n\r\n是结束标志，单个chunked块\r\n结束
                            if (ctmp.Equals(CT))
                            {
                                continue;
                            }
                            if (chunkedSize == 0)
                            {
                                //结束了
                                break;
                            }
                            int onechunkLen = 0;

                            while (onechunkLen < chunkedSize && sw.ElapsedMilliseconds <= timeout)
                            {
                               
                                int read = chunkedSize - onechunkLen;
                                if (read > 1024)
                                {
                                    read = 1024;
                                }
                                byte[] response_data = new byte[read];
                                
                                len = ssl.Read(response_data, 0, read);
             
                                if (len > 0)
                                {
                                    onechunkLen += len;
                                    sum += len;
                                    body_data.Write(response_data, 0, len);
                                }
                                if (len <= 0 && onechunkLen < chunkedSize)
                                {
                                    Thread.Sleep(WaitTime);
                                }
                            }

                            //判断
                        } while (sw.ElapsedMilliseconds <= timeout);
                    }
                    //connection close方式或未知body长度
                    else
                    {
                        while (sw.ElapsedMilliseconds <= timeout)
                        {
                            bool isok = clientSocket.Client.Poll(timeout, SelectMode.SelectRead);
                            if (!isok || clientSocket.Available <= 0)
                            {
                                break;
                            }
                            else {
                                int read = clientSocket.Available;
                                if (read > 0)
                                {
                                    byte[] response_data = new byte[read];
                                    len = ssl.Read(response_data, 0, read);
                                    if (len > 0)
                                    {
                                        sum += len;
                                        body_data.Write(response_data, 0, len);
                                    }
                                }
                                else
                                {
                                    Thread.Sleep(WaitTime);
                                }
                            }
                            
                        }
                    }
                    //自动识别编码
                    if (AutoGetEncoding.Equals(encoding))
                    {
                        if (!String.IsNullOrEmpty(server.encoding))
                        {
                            encoding = server.encoding;//header找到编码
                        }
                        else {
                            encoding = DefaultEncoding;//默认一个编码
                            isupdateEncoding = true;//body找编码
                        }
                        Encoding encod = Encoding.GetEncoding(encoding);
                        getBody(ref server, ref body_data, ref encod, ref index);
                        //修正编码
                        if (isupdateEncoding)
                        {
                            String cEncoding = getHTMLEncoding("", server.body);
                            if (!String.IsNullOrEmpty(cEncoding))
                            {
                                server.encoding = cEncoding;//body找到编码
                                getBody(ref server, ref body_data, ref encod, ref index);
                            }

                        }
                    }
                    else {
                        //指定编码
                        Encoding encod = Encoding.GetEncoding(encoding);
                        getBody(ref server, ref body_data, ref encod, ref index);
                    }
                }

            }catch (Exception e)
            {
                Exception ee = new Exception("HTTPS发包错误！错误消息：" + e.Message + "----发包编号：" + index);
                throw ee;
            }
            finally
            {
                sw.Stop();
                server.length = sum;
                server.runTime = sw.ElapsedMilliseconds;
                body_data.Close();
                if (clientSocket != null)
                {
                    clientSocket.Close();
                }

                if (main.config.isOpenHTTPLog)
                {
                    server.sleepTime = main.config.sendHTTPSleepTime;
                    Tools.sysHTTPLog(index, server);
                    String proxyInfo = "";
                    if (cproxy != null)
                    {
                        proxyInfo = cproxy.host + ":" + cproxy.port;
                    }
                    main.showHTTPLog(index, server, payload, proxyInfo);
                }
                if (main.config.sendHTTPSleepTime > 0)
                {
                    Thread.Sleep(main.config.sendHTTPSleepTime);
                }
            }
            return server;

        }

        public static String unGzip(ref MemoryStream ms, Encoding encoding,String index)
        {

            String str = "";
            MemoryStream input = new MemoryStream(ms.ToArray());
            GZipStream gs = new GZipStream(input, CompressionMode.Decompress);
            MemoryStream outbuf = new MemoryStream();
            byte[] block = new byte[1024];
            
            try
            {
                while (true)
                {
                    int bytesRead = gs.Read(block, 0, block.Length);
                    if (bytesRead <= 0)
                    {
                        break;
                    }
                    else
                    {
                        outbuf.Write(block, 0, bytesRead);
                    }
                }
                str = encoding.GetString(outbuf.ToArray());
            }
            catch (Exception e)
            {
                Tools.SysLog("解压Gzip发生异常----" + e.Message); 
            }
            finally
            {
                outbuf.Close();
                gs.Close();
                input.Close();
                ms.Close();
            }
            return str;

        }

        public static String unDeflate(ref MemoryStream ms, Encoding encoding, String index)
        {
            String str = "";
            MemoryStream input = new MemoryStream(ms.ToArray());
            DeflateStream ds = new DeflateStream(input, CompressionMode.Decompress);
            MemoryStream outbuf = new MemoryStream();
            byte[] block = new byte[1024];
            try
            {
                while (true)
                {
                    int bytesRead = ds.Read(block, 0, block.Length);
                    if (bytesRead <= 0)
                    {
                        break;
                    }
                    else
                    {
                        outbuf.Write(block, 0, bytesRead);
                    }
                }
                str = encoding.GetString(outbuf.ToArray());
            }
            catch (Exception e)
            {
                Tools.SysLog("解压deflate发生异常----" + e.Message + "----" + index);

            }
            finally
            {
                outbuf.Close();
                ds.Close();
                input.Close();
                ms.Close();

            }
            return str;

        }

        public static String getHTMLEncoding(String header, String body)
        {
            if (String.IsNullOrEmpty(header)&& String.IsNullOrEmpty(body))
            {
                return "";
            }
            String encode = "";
            Match m = Regex.Match(header, @"charset=(?<charset>[\w\-]+)", RegexOptions.IgnoreCase);
            if (m.Success)
            {
                encode=m.Groups["charset"].Value.ToUpper();
            }
            else
            {
                if (String.IsNullOrEmpty(body))
                {
                    return "";
                }
                m = Regex.Match(body, @"charset=['""]{0,1}(?<charset>[\w\-]+)['""]{0,1}", RegexOptions.IgnoreCase);
                if (m.Success)
                {
                    encode=m.Groups["charset"].Value.ToUpper();
                }
            }
            if ("UTF8".Equals(encode)) {
                encode = "UTF-8";
            }
            return encode.ToUpper();


        }
    }
}