using model;
using SuperSQLInjection.model;
using SuperSQLInjection.scan;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using tools;

namespace SuperSQLInjection.tools
{
    class InjectionTools
    {
        //需要忽略的扫描参数
        public static List<String> jumpkeyList = FileTool.readFileToList("config/injection/jumpkey.txt");
        //错误注入关键字目录
        public static List<String> errorDBList = FileTool.readAllDic("config/injection/error/");
        //盲注payload
        public static List<String> bool_payloads = FileTool.readFileToList("config/injection/injection.txt");
        public static List<String> errer_code = new List<String>();

        public static void addErrorCode()
        {

            errer_code.Add("501");
            errer_code.Add("503");
            errer_code.Add("403");
            errer_code.Add("502");
            errer_code.Add("400");
            errer_code.Add("401");
            errer_code.Add("0");
        }

        public static Injection testInjection(String url, Config config, Boolean justScanError)
        {

            Injection injection = new Injection();
            try
            {

                int index = url.IndexOf('?');
                if (index == -1)
                {

                    return injection;
                }
                injection.url = url;
                String testUrl = Uri.EscapeUriString(url);

                Uri uri = new Uri(url);
                bool isSSL = url.StartsWith("https", StringComparison.OrdinalIgnoreCase);
                string queryString = (uri.Query != null && uri.Query.Length > 0) ? uri.Query.Substring(1, uri.Query.Length - 1) : "";

                String[] strparams = queryString.Split('&');
                int timeout = 0;//超时3次，认为此URL为坏死URL
                //对参数进行注入测试
                foreach (String param in strparams)
                {
                    if (timeout >= 3)
                    {
                        break;//超时3次，认为此URL为坏死URL
                    }
                    if (param.IndexOf("=") == -1) {
                        continue;
                    }
                    String[] sprarm = param.Split('=');

                    String pramName = sprarm[0];
                    if (jumpkeyList.Contains(pramName))
                    {
                        continue;//忽略扫描参数
                    }
                    if (sprarm.Length <= 1)
                    {
                        continue;
                    }
                    String pramValue = sprarm[1];
                    String payload = param + "%27";
                    String curl = uri.PathAndQuery.Replace(param, payload);
                    injection.testUrl = testUrl.Replace(param, payload);
                    injection.paramName = sprarm[0];
                    String oldrequest = Spider.reqestGetTemplate.Replace("{url}", uri.PathAndQuery).Replace("{host}", uri.Host);
                    String request = Spider.reqestGetTemplate.Replace("{url}", curl).Replace("{host}", uri.Host);
                    //通过错误显示判断
                    if (timeout >= 3)
                    {
                        break;//超时3次，认为此URL为坏死URL
                    }
                    ServerInfo errorDBServer = HTTP.sendRequestRetry(isSSL, config.reTry, uri.Host, uri.Port, payload, request, config.timeOut, HTTP.AutoGetEncoding, config.is_foward_302, config.redirectDoGet);
                    if (errorDBServer.runTime >= config.timeOut * 1000) timeout++;
 

                    if (errorDBServer.body.Length == 0 | errorDBServer.code == 404)
                    {
                        continue;
                    }

                    foreach (String eop in errorDBList)
                    {
                        List<String> errorKeys = FileTool.readFileToList("config/injection/error/" + eop);
                        foreach (String key in errorKeys)
                        {
                            bool find = Regex.IsMatch(errorDBServer.body, key, RegexOptions.IgnoreCase);
                            if (find)
                            {
                                injection.isInjection = true;
                                injection.dbType = (eop.Replace(".txt", ""));
                                injection.payload = "'";
                                injection.remark = "错误显示信息判断";
                                injection.injectType = "错误显示";
                                injection.dbType = eop;
                                return injection;
                            }
                        }
                    }
                    if (!injection.isInjection && justScanError == false)
                    {
                        if (timeout >= 3)
                        {
                            break;//超时3次，认为此URL为坏死URL
                        }
                        //读取bool payload
                        ServerInfo oserver = HTTP.sendRequestRetry(isSSL, config.reTry, uri.Host, uri.Port, "获取原始页面内容", oldrequest, config.timeOut, HTTP.AutoGetEncoding, config.is_foward_302, config.redirectDoGet);
                        if (oserver.runTime >= config.timeOut * 1000) timeout++;

                        if (bool_payloads.Count > 0)
                        {

                            foreach (String bool_payload in bool_payloads)
                            {
                                String[] bool_ps = bool_payload.Split('：');

                                String flasePayload = pramName + "=" + URLEncode.UrlEncode(pramValue + bool_ps[1]);
                                String falseURL = uri.PathAndQuery.Replace(param, flasePayload);
                                injection.paramName = sprarm[0];
                                injection.testUrl = testUrl.Replace(param, flasePayload);
                                if (timeout >= 3)
                                {
                                    break;//超时3次，认为此URL为坏死URL
                                }
                                String falserequest = Spider.reqestGetTemplate.Replace("{url}", falseURL).Replace("{host}", uri.Host);
                                ServerInfo falseServer = HTTP.sendRequestRetry(isSSL, config.reTry, uri.Host, uri.Port, flasePayload, falserequest, config.timeOut, HTTP.AutoGetEncoding, false, config.redirectDoGet);
                                if (falseServer.runTime > config.timeOut * 1000) timeout++;
                                decimal pfalse = Tools.getLike(oserver.body, falseServer.body);
                                //静态参数
                                if (pfalse > 99)
                                {
                                    continue;
                                }

                                String truePayload = pramName + "=" + URLEncode.UrlEncode(pramValue + bool_ps[0]);
                                String trueURL = uri.PathAndQuery.Replace(param, truePayload);
                                String truerequest = Spider.reqestGetTemplate.Replace("{url}", trueURL).Replace("{host}", uri.Host);
                                if (timeout >= 3)
                                {
                                    break;//超时3次，认为此URL为坏死URL
                                }
                                ServerInfo trueServer = HTTP.sendRequestRetry(isSSL, config.reTry, uri.Host, uri.Port, truePayload, truerequest, config.timeOut, HTTP.AutoGetEncoding, false, config.redirectDoGet);
                                if (trueServer.runTime > config.timeOut*1000) timeout++;
                                //计算相似度
                                decimal ptrue = Tools.getLike(oserver.body, trueServer.body);
                                if (ptrue < 85)
                                {
                                    continue;
                                }
                                if (oserver.runTime > config.timeOut) timeout++;
                                injection.payload = bool_ps[1];
                                injection.injectType = bool_ps[2];
                                injection.dbType = "未知";

                                if (oserver.code != 404 && !errer_code.Contains(oserver.code.ToString()) && !errer_code.Contains(trueServer.code.ToString()) && !errer_code.Contains(falseServer.code.ToString()) && trueServer.body.Length > 0 && falseServer.body.Length > 0)
                                {

                                    //判断存在bool盲注
                                    //根据状态码判断
                                    if (oserver.code == trueServer.code && trueServer.code != falseServer.code)
                                    {
                                        injection.isInjection = true;
                                        injection.remark = "状态码判断----" + oserver.code + "|" + trueServer.code + "|" + falseServer.code;
                                        return injection;
                                    }

                                    if (falseServer.body.Length < trueServer.body.Length)
                                    {  
                                        /*
                                        if (ptrue > pfalse)
                                        {

                                            injection.isInjection = true;
                                            injection.remark = "动态响应长度判断,相似度----" + ptrue + "|" + pfalse + "|" + p + "%";
                                            return injection;
                                        }*/


                                        if (ptrue == 100)
                                        {

                                            if (ptrue > pfalse)
                                            {
                                                injection.isInjection = true;
                                                injection.remark = "固定长度,相似度--false|true--" + pfalse + "|" + ptrue + "%";
                                                return injection;
                                            }
                                        }
                                        else {
                                            if (timeout >= 3)
                                            {
                                                break;//超时3次，认为此URL为坏死URL
                                            }
                                            ServerInfo true1Server = HTTP.sendRequestRetry(isSSL, config.reTry, uri.Host, uri.Port, truePayload, truerequest.Replace("1%3d1", "2%3d2"), config.timeOut, HTTP.AutoGetEncoding, false, config.redirectDoGet);
                                            if (true1Server.runTime > config.timeOut * 1000) timeout++;
                                            decimal p = Tools.getLike(oserver.body, true1Server.body);
                                            if (ptrue-pfalse>= 2 && Math.Abs(p - pfalse) >= 2)
                                            {
                                                injection.isInjection = true;
                                                injection.remark = "动态长度,相似度--false|true1|true2--" + pfalse + "|" + ptrue + "|" + p + "%";
                                                return injection;
                                            }


                                        }


                                    }


                                }

                            }
                        }

                    }


                }

            }
            catch (Exception e)
            {
                Tools.SysLog("判断注入发生异常！" + e.Message);
            }
            return injection;
        }

    }
}