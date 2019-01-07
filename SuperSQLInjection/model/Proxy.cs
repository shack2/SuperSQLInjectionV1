using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace SuperSQLInjection.model
{
    [Serializable]
    public class Proxy
    {
        public Proxy() {
        
        }
        public String host = "";
        public int port = 0;
        public String proxyType = "";//socks5，或HTTP/HTTPS
        public String username = "";//代理账户
        public String password = "";//代理密码
        public String isOk ="未验证";//未验证,是，否
        public int useTime = 0;//连接使用时间
        public String checkTime = "";//验证时间
    }
}
