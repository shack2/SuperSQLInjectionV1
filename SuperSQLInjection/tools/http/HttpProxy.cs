using Amib.Threading.Internal;
using model;
using SuperSQLInjection.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using tools;

namespace SuperSQLInjection.tools.http
{
    class HttpProxy
    {
        private static String request = "GET http://{host}:{port}/ HTTP/1.1\r\nHost: {host}:{port}\r\nConnection: close\r\nUser-Agent: Mozilla/5.0\r\nAccept: */*\r\nAccept-Encoding: gzip, deflate\r\n\r\n";
        public static int ConectProxyUseTime = 0;
        public static bool checkConnection(Config config,Proxy proxy) {
            String crequest = request.Replace("{host}", config.proxy_check_host).Replace("{port}", config.proxy_check_port.ToString());
            ServerInfo server=HTTP.sendRequestRetry(false,config.reTry,proxy.host, proxy.port, "", request, config.timeOut, config.encoding, true, false);
            
            if (!String.IsNullOrEmpty(server.body)&& server.body.IndexOf(config.proxy_check_Keys)!=-1)
            {
                ConectProxyUseTime = (int)server.runTime;
                return true;
            }
            else {
                return false;
            }
        }

    }
}
