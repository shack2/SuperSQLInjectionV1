using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace model
{
    public class ServerInfo
    {
        public String host = "";//host主机头
        public String url = "";//pathAndQuery
        public int port = 80;
        public String request = "";
        public String encoding = "";
        public String header = "";
        public String body = "";
        public String reuqestBody = "";
        public String reuqestHeader = "";
        public Dictionary<String, String> headers = new Dictionary<String, String>();
        public String response = "";
        public String gzip = "";
        public int length = 0;
        public int code = 0;
        public int location = 0;
        public int runTime = 0;//获取网页消耗时间，毫秒
        public int sleepTime = 0;//休息时间
        public String cookies = "";
        public Boolean timeout = false;
    }
}
