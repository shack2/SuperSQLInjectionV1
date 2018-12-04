using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperSQLInjection.model
{
    class InjectLog
    {
        public int id=0;
        public String ip= "";
        public int port = 0;
        public String url = "";
        public InjectType injectType = new InjectType();
        public DBType dbType = new DBType();
        public String usePayload = "";
        public String testPayload = "";
        public String request = "";

    }
}
