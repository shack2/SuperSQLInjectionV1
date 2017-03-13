using System;
using System.Collections.Generic;
using System.Text;

namespace SuperSQLInjection.model
{
    public static class ErrorMessage
    {
        public static String mysql4_no_error_inject_info = "抱歉MySQL4数据库，不支持错误显示注入！";
        public static String access_no_error_inject_info = "抱歉Access数据库，不支持错误显示注入！";
        public static String access_no_key = "Access数据库需要关键字协助盲猜表明，所以大侠请你填写好关键字！";
    }
}
