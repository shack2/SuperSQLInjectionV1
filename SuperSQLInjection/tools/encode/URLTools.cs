using System;
using System.Collections.Generic;
using System.Text;
using model;

namespace SuperSQLInjection.tools
{
    class URLTools
    {
        public static ServerInfo getHostAndPathQueryByURL(String url){

            try
            {
                ServerInfo server = new ServerInfo();
                Uri uri = new Uri(url);
                server.host = uri.Host;
                server.url = uri.PathAndQuery;
                server.port = uri.Port;
                return server;
            }
            catch (Exception e) {

                throw e;
            }
        }
    }
}
