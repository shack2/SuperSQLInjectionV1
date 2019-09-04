namespace SuperSQLInjection
{
    partial class Main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
             {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.gb_basic = new System.Windows.Forms.GroupBox();
            this.chk_useSSL = new System.Windows.Forms.CheckBox();
            this.btn_stop = new System.Windows.Forms.Button();
            this.cbox_basic_threadSize = new System.Windows.Forms.ComboBox();
            this.cbox_basic_reTryCount = new System.Windows.Forms.ComboBox();
            this.cbox_basic_encoding = new System.Windows.Forms.ComboBox();
            this.cbox_basic_timeOut = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btn_autoInject = new System.Windows.Forms.Button();
            this.cbox_basic_dbType = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbox_basic_injectType = new System.Windows.Forms.ComboBox();
            this.txt_basic_port = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_basic_host = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.gb_logo = new System.Windows.Forms.GroupBox();
            this.txt_log = new System.Windows.Forms.RichTextBox();
            this.tab_logCenter = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.log_lvw_httpLog = new System.Windows.Forms.ListView();
            this.log_col_index = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.log_col_payload = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_runtime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.log_col_code = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.log_col_bodyLength = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.log_col_sleepTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_proxy = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.log_cms_dataifo = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.data_cms_clearLog = new System.Windows.Forms.ToolStripMenuItem();
            this.data_cms_copyPaylaod = new System.Windows.Forms.ToolStripMenuItem();
            this.img_line = new System.Windows.Forms.ImageList(this.components);
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.log_txt_request = new System.Windows.Forms.RichTextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.log_txt_response = new System.Windows.Forms.RichTextBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.webBro_log = new System.Windows.Forms.WebBrowser();
            this.tab_file = new System.Windows.Forms.TabPage();
            this.file_txt_result = new System.Windows.Forms.TextBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.file_btn_stop = new System.Windows.Forms.Button();
            this.file_btn_start = new System.Windows.Forms.Button();
            this.file_cbox_readWrite = new System.Windows.Forms.ComboBox();
            this.file_txt_filePath = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.cbox_file_readFileEncoding = new System.Windows.Forms.ComboBox();
            this.label24 = new System.Windows.Forms.Label();
            this.tab_dataCenter = new System.Windows.Forms.TabPage();
            this.tabC_dataCenter = new System.Windows.Forms.TabControl();
            this.tab_vers = new System.Windows.Forms.TabPage();
            this.toolStrip_getVers = new System.Windows.Forms.ToolStrip();
            this.toolStrip_vers_btn_selectAll = new System.Windows.Forms.ToolStripButton();
            this.toolStrip_vers_btn_selectReverse = new System.Windows.Forms.ToolStripButton();
            this.toolStrip_vers_btn_getVariable = new System.Windows.Forms.ToolStripButton();
            this.toolStrip_vers_btn_stopGetVariable = new System.Windows.Forms.ToolStripButton();
            this.data_lvw_ver = new System.Windows.Forms.ListView();
            this.data_lvw_ver_verName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.data_lvw_ver_val = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.data_cms_vers = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.data_cms_tsmi_getVariable = new System.Windows.Forms.ToolStripMenuItem();
            this.data_cms_tsmi_selectAllVers = new System.Windows.Forms.ToolStripMenuItem();
            this.data_cms_tsmi_selectReversVers = new System.Windows.Forms.ToolStripMenuItem();
            this.data_cms_tsmi_copyVerValue = new System.Windows.Forms.ToolStripMenuItem();
            this.data_cms_tsmi_stopGetVariable = new System.Windows.Forms.ToolStripMenuItem();
            this.tab_dbs = new System.Windows.Forms.TabPage();
            this.spc_dbs = new System.Windows.Forms.SplitContainer();
            this.data_dbs_ts = new System.Windows.Forms.ToolStrip();
            this.data_dbs_tsl_getDBS = new System.Windows.Forms.ToolStripButton();
            this.data_dbs_tsl_getTables = new System.Windows.Forms.ToolStripButton();
            this.data_dbs_tsl_getColumns = new System.Windows.Forms.ToolStripButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.data_tvw_dbs = new System.Windows.Forms.TreeView();
            this.data_cms_dbs = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.data_dbs_tsmi_addDBS = new System.Windows.Forms.ToolStripMenuItem();
            this.data_dbs_tsmi_addTableOrColumn = new System.Windows.Forms.ToolStripMenuItem();
            this.data_dbs_tsmi_getTableNames = new System.Windows.Forms.ToolStripMenuItem();
            this.data_dbs_tsmi_deleteNode = new System.Windows.Forms.ToolStripMenuItem();
            this.data_dbs_tsmi_stopGetInfos = new System.Windows.Forms.ToolStripMenuItem();
            this.data_dbs_tsmi_saveDTCStruct = new System.Windows.Forms.ToolStripMenuItem();
            this.data_dbs_tsmi_loadDTCStruct = new System.Windows.Forms.ToolStripMenuItem();
            this.data_dbs_tsmi_clearDTCStruct = new System.Windows.Forms.ToolStripMenuItem();
            this.data_dbs_tsmi_selectAllSubNode = new System.Windows.Forms.ToolStripMenuItem();
            this.data_dbs_tsmi_selectReversSubNode = new System.Windows.Forms.ToolStripMenuItem();
            this.imglist_database = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.data_dbs_txt_start = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.data_dbs_txt_count = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.data_dbs_cob_db_encoding = new System.Windows.Forms.ToolStripComboBox();
            this.data_dbs_tsl_getDatas = new System.Windows.Forms.ToolStripButton();
            this.data_dbs_tsl_exportDatas = new System.Windows.Forms.ToolStripButton();
            this.data_dbs_tsl_stopGetDatas = new System.Windows.Forms.ToolStripLabel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.data_dbs_lvw_data = new System.Windows.Forms.ListView();
            this.cms_data_dbs_lvw_data = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.data_dbs_lvw_tsmi_copyLineData = new System.Windows.Forms.ToolStripMenuItem();
            this.data_dbs_lvw_tsmi_stop = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_clearColumns = new System.Windows.Forms.ToolStripMenuItem();
            this.myicon_list = new System.Windows.Forms.ImageList(this.components);
            this.tab_injectCenter = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tab_datapack = new System.Windows.Forms.TabPage();
            this.txt_inject_request = new System.Windows.Forms.RichTextBox();
            this.cms_dataPacks = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmi_createGetTemplate = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_createPOSTTemplate = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_changeRequestMethod = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_createPackByURL = new System.Windows.Forms.ToolStripMenuItem();
            this.tab_tokenset = new System.Windows.Forms.TabPage();
            this.groupBox17 = new System.Windows.Forms.GroupBox();
            this.token_txt_http_request = new System.Windows.Forms.RichTextBox();
            this.groupBox19 = new System.Windows.Forms.GroupBox();
            this.label36 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.token_txt_endStr = new System.Windows.Forms.TextBox();
            this.token_txt_startStr = new System.Windows.Forms.TextBox();
            this.token_btn_testGetToken = new System.Windows.Forms.Button();
            this.tab_sencond_inject = new System.Windows.Forms.TabPage();
            this.groupBox20 = new System.Windows.Forms.GroupBox();
            this.txt_sencond_request = new System.Windows.Forms.RichTextBox();
            this.groupBox21 = new System.Windows.Forms.GroupBox();
            this.label28 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox16 = new System.Windows.Forms.GroupBox();
            this.label34 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_inject_showIndex = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txt_inject_unionTemplate = new System.Windows.Forms.TextBox();
            this.txt_inject_unionColumnsCount = new System.Windows.Forms.TextBox();
            this.btn_inject_sendData = new System.Windows.Forms.Button();
            this.btn_inject_clearRequest = new System.Windows.Forms.Button();
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.cbox_inject_type = new System.Windows.Forms.ComboBox();
            this.label29 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.inject_btn_autoFindKey = new System.Windows.Forms.Button();
            this.injectConfig_btn_checkKey = new System.Windows.Forms.Button();
            this.txt_inject_key = new System.Windows.Forms.TextBox();
            this.chk_inject_reverseKey = new System.Windows.Forms.CheckBox();
            this.chk_openURLEncoding = new System.Windows.Forms.CheckBox();
            this.btn_inject_setEncodingRange = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.btn_inject_randStr = new System.Windows.Forms.Button();
            this.btn_inject_setTokenLocation = new System.Windows.Forms.Button();
            this.chk_sencondInject = new System.Windows.Forms.CheckBox();
            this.chk_inject_foward_302 = new System.Windows.Forms.CheckBox();
            this.btn_inject_setInject = new System.Windows.Forms.Button();
            this.mytab = new System.Windows.Forms.TabControl();
            this.tab_proxy = new System.Windows.Forms.TabPage();
            this.groupBox25 = new System.Windows.Forms.GroupBox();
            this.label46 = new System.Windows.Forms.Label();
            this.proxy_lbl_proxyType = new System.Windows.Forms.Label();
            this.proxy_lbl_proxy_port = new System.Windows.Forms.Label();
            this.proxy_lbl_proxy_password = new System.Windows.Forms.Label();
            this.proxy_lbl_proxy_username = new System.Windows.Forms.Label();
            this.proxy_lbl_proxy_host = new System.Windows.Forms.Label();
            this.label45 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.proxy_cmb_proxyMode = new System.Windows.Forms.ComboBox();
            this.groupBox24 = new System.Windows.Forms.GroupBox();
            this.toolStrip_proxyList = new System.Windows.Forms.ToolStrip();
            this.proxy_ts_btn_clearAllFailedProxy = new System.Windows.Forms.ToolStripLabel();
            this.proxy_ts_btn_proxy_checkNoCheckProxy = new System.Windows.Forms.ToolStripLabel();
            this.groupBox26 = new System.Windows.Forms.GroupBox();
            this.label40 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.proxy_btn_importProxy = new System.Windows.Forms.Button();
            this.proxy_btn_addProxy = new System.Windows.Forms.Button();
            this.label44 = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.proxy_txt_addProxyPassword = new System.Windows.Forms.TextBox();
            this.label42 = new System.Windows.Forms.Label();
            this.proxy_txt_addProxyUserName = new System.Windows.Forms.TextBox();
            this.proxy_txt_addProxyHost = new System.Windows.Forms.TextBox();
            this.proxy_txt_addProxyPort = new System.Windows.Forms.TextBox();
            this.proxy_txt_addProxyType = new System.Windows.Forms.ComboBox();
            this.proxy_lvw_proxyList = new System.Windows.Forms.ListView();
            this.col_host = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_port = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_proxyType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_username = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_password = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_conectIsOK = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_useTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_checkTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.proxy_cms = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.proxy_tsmi_setCurrentProxy = new System.Windows.Forms.ToolStripMenuItem();
            this.proxy_checkNoCheckProxy = new System.Windows.Forms.ToolStripMenuItem();
            this.proxy_checkSelectedProxy = new System.Windows.Forms.ToolStripMenuItem();
            this.proxy_checkAllProxy = new System.Windows.Forms.ToolStripMenuItem();
            this.proxy_importProxy = new System.Windows.Forms.ToolStripMenuItem();
            this.proxy_exportProxy = new System.Windows.Forms.ToolStripMenuItem();
            this.proxy_delSelectedProxy = new System.Windows.Forms.ToolStripMenuItem();
            this.proxy_copySelectedProxy = new System.Windows.Forms.ToolStripMenuItem();
            this.proxy_clearAllProxy = new System.Windows.Forms.ToolStripMenuItem();
            this.proxy_clearAllFailedProxy = new System.Windows.Forms.ToolStripMenuItem();
            this.tab_cmd = new System.Windows.Forms.TabPage();
            this.cmd_txt_result = new System.Windows.Forms.TextBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.cmd_chk_showCmdResult = new System.Windows.Forms.CheckBox();
            this.cmd_btn_stop = new System.Windows.Forms.Button();
            this.cmd_btn_start = new System.Windows.Forms.Button();
            this.cmd_txt_cmd = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.tab_bypass = new System.Windows.Forms.TabPage();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.groupBox18 = new System.Windows.Forms.GroupBox();
            this.bypass_btn_saveTemplate = new System.Windows.Forms.Button();
            this.bypass_cbox_loadTemplate = new System.Windows.Forms.ComboBox();
            this.label33 = new System.Windows.Forms.Label();
            this.groupBox23 = new System.Windows.Forms.GroupBox();
            this.chk_reaplaceBeforURLEncode = new System.Windows.Forms.CheckBox();
            this.label19 = new System.Windows.Forms.Label();
            this.bypass_btn_addReplaceStr = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.bypass_lvw_replaceString = new System.Windows.Forms.ListView();
            this.col_replace = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_replaceTo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.bypass_lvw_replaceString_cms = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.bypass_tsmi_delselect = new System.Windows.Forms.ToolStripMenuItem();
            this.bypass_tsmi_copy = new System.Windows.Forms.ToolStripMenuItem();
            this.bypass_txt_replaceTo = new System.Windows.Forms.TextBox();
            this.bypass_txt_replace = new System.Windows.Forms.TextBox();
            this.groupBox22 = new System.Windows.Forms.GroupBox();
            this.cbox_bypass_urlencode_count = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.cbox_base64Count = new System.Windows.Forms.ComboBox();
            this.bypass_chk_inculdeStr = new System.Windows.Forms.CheckBox();
            this.bypass_chk_usebetween = new System.Windows.Forms.CheckBox();
            this.cob_keyRepalce = new System.Windows.Forms.ComboBox();
            this.bypass_cbox_sendHTTPSleepTime = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.bypass_cbox_randIPToHeader = new System.Windows.Forms.ComboBox();
            this.bypass_chk_use_unicode = new System.Windows.Forms.CheckBox();
            this.bypass_hex = new System.Windows.Forms.CheckBox();
            this.tab_encoding = new System.Windows.Forms.TabPage();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.encode_cbox_encode = new System.Windows.Forms.ComboBox();
            this.encode_cbox_decode = new System.Windows.Forms.ComboBox();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.encode_txt_result = new System.Windows.Forms.TextBox();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.encode_txt_input = new System.Windows.Forms.TextBox();
            this.tab_scanInjection = new System.Windows.Forms.TabPage();
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.scanInject_lsb_links = new System.Windows.Forms.ListBox();
            this.scanInjectionURL_cms = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmi_exportScanInjectionURL = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_clearScanInjectionURL = new System.Windows.Forms.ToolStripMenuItem();
            this.scanInjection_lvw_result = new System.Windows.Forms.ListView();
            this.col_index = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_url = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_testURL = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_param = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_injectionType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_injectionDB = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_mark = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.scanInjection_cms = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.scanInjection_cms_exportResult = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_tsmi_exortTestURL = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_tsmi_exortOldURL = new System.Windows.Forms.ToolStripMenuItem();
            this.scanInjection_cms_copyURL = new System.Windows.Forms.ToolStripMenuItem();
            this.scanInjection_cms_clearResult = new System.Windows.Forms.ToolStripMenuItem();
            this.scanInjection_cms_delThisLine = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_openURL = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_tsmi_opentestURL = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.scanInect_chk_isSpider = new System.Windows.Forms.CheckBox();
            this.scanInect_chk_scanError = new System.Windows.Forms.CheckBox();
            this.scanInjection_btn_spider = new System.Windows.Forms.Button();
            this.scanInjection_btn_scan = new System.Windows.Forms.Button();
            this.scanInjection_importDomains = new System.Windows.Forms.Button();
            this.scanInjection_scanedURLSCount = new System.Windows.Forms.Label();
            this.scanInjection_txt_domainsPath = new System.Windows.Forms.TextBox();
            this.scanInjection_findURLSCount = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.scanInjection_domainsCount = new System.Windows.Forms.Label();
            this.scanInjection_scanedDomainCount = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.tab_injectLog = new System.Windows.Forms.TabPage();
            this.lvw_injectLog = new System.Windows.Forms.ListView();
            this.injectlog_col_ip = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.injectlog_col_port = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.injectlog_col_uri = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.injectlog_col_pname = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.injectlog_col_injectType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.injectlog_col_dbType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.injectlog_col_payload = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.injectlog_col_time = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.injectLog_cm = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmi_injectLog_useCLog = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_injectLog_delSLog = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_injectLog_clearAllLog = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.status_lbl_time = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.status_lbl_threadStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.status_lbl_dbsCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.status_lbl_tableCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel6 = new System.Windows.Forms.ToolStripStatusLabel();
            this.status_lbl_dataCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel7 = new System.Windows.Forms.ToolStripStatusLabel();
            this.status_lbl_runStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel8 = new System.Windows.Forms.ToolStripStatusLabel();
            this.status_lbl_all_status = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel11 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbl_packsCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbl_info = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer_status = new System.Windows.Forms.Timer(this.components);
            this.timer_scanInjection = new System.Windows.Forms.Timer(this.components);
            this.menuStrip_main = new System.Windows.Forms.MenuStrip();
            this.tsmi_menu = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_openConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_saveConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_tools = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_seting = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_lang = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_zh_cn = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_en_us = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_help = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_readme = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_about = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_update = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_mustRead = new System.Windows.Forms.ToolStripMenuItem();
            this.版本ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_bugReport = new System.Windows.Forms.ToolStripMenuItem();
            this.gb_basic.SuspendLayout();
            this.gb_logo.SuspendLayout();
            this.tab_logCenter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.log_cms_dataifo.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tab_file.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.tab_dataCenter.SuspendLayout();
            this.tabC_dataCenter.SuspendLayout();
            this.tab_vers.SuspendLayout();
            this.toolStrip_getVers.SuspendLayout();
            this.data_cms_vers.SuspendLayout();
            this.tab_dbs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spc_dbs)).BeginInit();
            this.spc_dbs.Panel1.SuspendLayout();
            this.spc_dbs.Panel2.SuspendLayout();
            this.spc_dbs.SuspendLayout();
            this.data_dbs_ts.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.data_cms_dbs.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.cms_data_dbs_lvw_data.SuspendLayout();
            this.tab_injectCenter.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tab_datapack.SuspendLayout();
            this.cms_dataPacks.SuspendLayout();
            this.tab_tokenset.SuspendLayout();
            this.groupBox17.SuspendLayout();
            this.groupBox19.SuspendLayout();
            this.tab_sencond_inject.SuspendLayout();
            this.groupBox20.SuspendLayout();
            this.groupBox21.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox16.SuspendLayout();
            this.groupBox15.SuspendLayout();
            this.mytab.SuspendLayout();
            this.tab_proxy.SuspendLayout();
            this.groupBox25.SuspendLayout();
            this.groupBox24.SuspendLayout();
            this.toolStrip_proxyList.SuspendLayout();
            this.groupBox26.SuspendLayout();
            this.proxy_cms.SuspendLayout();
            this.tab_cmd.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.tab_bypass.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox18.SuspendLayout();
            this.groupBox23.SuspendLayout();
            this.bypass_lvw_replaceString_cms.SuspendLayout();
            this.groupBox22.SuspendLayout();
            this.tab_encoding.SuspendLayout();
            this.groupBox13.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.tab_scanInjection.SuspendLayout();
            this.groupBox14.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.scanInjectionURL_cms.SuspendLayout();
            this.scanInjection_cms.SuspendLayout();
            this.groupBox12.SuspendLayout();
            this.tab_injectLog.SuspendLayout();
            this.injectLog_cm.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.menuStrip_main.SuspendLayout();
            this.SuspendLayout();
            // 
            // gb_basic
            // 
            this.gb_basic.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_basic.Controls.Add(this.chk_useSSL);
            this.gb_basic.Controls.Add(this.btn_stop);
            this.gb_basic.Controls.Add(this.cbox_basic_threadSize);
            this.gb_basic.Controls.Add(this.cbox_basic_reTryCount);
            this.gb_basic.Controls.Add(this.cbox_basic_encoding);
            this.gb_basic.Controls.Add(this.cbox_basic_timeOut);
            this.gb_basic.Controls.Add(this.label9);
            this.gb_basic.Controls.Add(this.label11);
            this.gb_basic.Controls.Add(this.label5);
            this.gb_basic.Controls.Add(this.btn_autoInject);
            this.gb_basic.Controls.Add(this.cbox_basic_dbType);
            this.gb_basic.Controls.Add(this.label6);
            this.gb_basic.Controls.Add(this.cbox_basic_injectType);
            this.gb_basic.Controls.Add(this.txt_basic_port);
            this.gb_basic.Controls.Add(this.label2);
            this.gb_basic.Controls.Add(this.txt_basic_host);
            this.gb_basic.Controls.Add(this.label3);
            this.gb_basic.Controls.Add(this.label7);
            this.gb_basic.Controls.Add(this.label1);
            this.gb_basic.Location = new System.Drawing.Point(11, 30);
            this.gb_basic.Name = "gb_basic";
            this.gb_basic.Size = new System.Drawing.Size(837, 84);
            this.gb_basic.TabIndex = 0;
            this.gb_basic.TabStop = false;
            this.gb_basic.Text = "基础信息";
            // 
            // chk_useSSL
            // 
            this.chk_useSSL.AutoSize = true;
            this.chk_useSSL.Location = new System.Drawing.Point(189, 54);
            this.chk_useSSL.Name = "chk_useSSL";
            this.chk_useSSL.Size = new System.Drawing.Size(42, 16);
            this.chk_useSSL.TabIndex = 12;
            this.chk_useSSL.Text = "SSL";
            this.chk_useSSL.UseVisualStyleBackColor = true;
            this.chk_useSSL.CheckedChanged += new System.EventHandler(this.chk_useSSL_CheckedChanged);
            // 
            // btn_stop
            // 
            this.btn_stop.BackColor = System.Drawing.Color.Transparent;
            this.btn_stop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_stop.Location = new System.Drawing.Point(719, 50);
            this.btn_stop.Name = "btn_stop";
            this.btn_stop.Size = new System.Drawing.Size(99, 23);
            this.btn_stop.TabIndex = 11;
            this.btn_stop.Text = "停止线程";
            this.btn_stop.UseVisualStyleBackColor = false;
            this.btn_stop.Click += new System.EventHandler(this.btn_stop_Click);
            // 
            // cbox_basic_threadSize
            // 
            this.cbox_basic_threadSize.FormattingEnabled = true;
            this.cbox_basic_threadSize.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "15",
            "20",
            "30",
            "40",
            "50",
            "70",
            "100"});
            this.cbox_basic_threadSize.Location = new System.Drawing.Point(641, 18);
            this.cbox_basic_threadSize.Name = "cbox_basic_threadSize";
            this.cbox_basic_threadSize.Size = new System.Drawing.Size(61, 20);
            this.cbox_basic_threadSize.TabIndex = 10;
            this.cbox_basic_threadSize.TextChanged += new System.EventHandler(this.cbox_basic_threadSize_TextChanged);
            // 
            // cbox_basic_reTryCount
            // 
            this.cbox_basic_reTryCount.FormattingEnabled = true;
            this.cbox_basic_reTryCount.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3"});
            this.cbox_basic_reTryCount.Location = new System.Drawing.Point(641, 50);
            this.cbox_basic_reTryCount.Name = "cbox_basic_reTryCount";
            this.cbox_basic_reTryCount.Size = new System.Drawing.Size(61, 20);
            this.cbox_basic_reTryCount.TabIndex = 9;
            this.cbox_basic_reTryCount.TextChanged += new System.EventHandler(this.cbox_basic_reTryCount_TextChanged);
            // 
            // cbox_basic_encoding
            // 
            this.cbox_basic_encoding.FormattingEnabled = true;
            this.cbox_basic_encoding.Items.AddRange(new object[] {
            "自动识别",
            "UTF-8",
            "GB2312",
            "GBK",
            "ISO-8859-1",
            "EUC-KR"});
            this.cbox_basic_encoding.Location = new System.Drawing.Point(329, 52);
            this.cbox_basic_encoding.Name = "cbox_basic_encoding";
            this.cbox_basic_encoding.Size = new System.Drawing.Size(79, 20);
            this.cbox_basic_encoding.TabIndex = 8;
            this.cbox_basic_encoding.TextChanged += new System.EventHandler(this.cbox_basic_encoding_TextChanged);
            // 
            // cbox_basic_timeOut
            // 
            this.cbox_basic_timeOut.FormattingEnabled = true;
            this.cbox_basic_timeOut.Items.AddRange(new object[] {
            "5",
            "10",
            "15",
            "20",
            "25",
            "30",
            "40",
            "50",
            "60"});
            this.cbox_basic_timeOut.Location = new System.Drawing.Point(329, 20);
            this.cbox_basic_timeOut.Name = "cbox_basic_timeOut";
            this.cbox_basic_timeOut.Size = new System.Drawing.Size(79, 20);
            this.cbox_basic_timeOut.TabIndex = 7;
            this.cbox_basic_timeOut.TextChanged += new System.EventHandler(this.cbox_basic_timeOut_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(259, 56);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 2;
            this.label9.Text = "网页编码：";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(591, 54);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(47, 12);
            this.label11.TabIndex = 2;
            this.label11.Text = "重 试：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(591, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 12);
            this.label5.TabIndex = 2;
            this.label5.Text = "线 程：";
            // 
            // btn_autoInject
            // 
            this.btn_autoInject.BackColor = System.Drawing.Color.Transparent;
            this.btn_autoInject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_autoInject.ImageKey = "(无)";
            this.btn_autoInject.Location = new System.Drawing.Point(719, 18);
            this.btn_autoInject.Name = "btn_autoInject";
            this.btn_autoInject.Size = new System.Drawing.Size(99, 23);
            this.btn_autoInject.TabIndex = 5;
            this.btn_autoInject.Text = "识别注入";
            this.btn_autoInject.UseVisualStyleBackColor = false;
            this.btn_autoInject.Click += new System.EventHandler(this.btn_autoInject_Click);
            // 
            // cbox_basic_dbType
            // 
            this.cbox_basic_dbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbox_basic_dbType.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbox_basic_dbType.FormattingEnabled = true;
            this.cbox_basic_dbType.Location = new System.Drawing.Point(489, 50);
            this.cbox_basic_dbType.Name = "cbox_basic_dbType";
            this.cbox_basic_dbType.Size = new System.Drawing.Size(87, 20);
            this.cbox_basic_dbType.TabIndex = 3;
            this.cbox_basic_dbType.SelectedIndexChanged += new System.EventHandler(this.cbox_basic_dbType_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(419, 54);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 4;
            this.label6.Text = "数 据 库：";
            // 
            // cbox_basic_injectType
            // 
            this.cbox_basic_injectType.BackColor = System.Drawing.SystemColors.Window;
            this.cbox_basic_injectType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbox_basic_injectType.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbox_basic_injectType.FormattingEnabled = true;
            this.cbox_basic_injectType.Items.AddRange(new object[] {
            "UnKnow",
            "盲 注",
            "显错注入",
            "Union注入"});
            this.cbox_basic_injectType.Location = new System.Drawing.Point(489, 18);
            this.cbox_basic_injectType.Name = "cbox_basic_injectType";
            this.cbox_basic_injectType.Size = new System.Drawing.Size(87, 20);
            this.cbox_basic_injectType.TabIndex = 5;
            this.cbox_basic_injectType.SelectedIndexChanged += new System.EventHandler(this.cbox_basic_injectType_SelectedIndexChanged);
            // 
            // txt_basic_port
            // 
            this.txt_basic_port.Location = new System.Drawing.Point(87, 51);
            this.txt_basic_port.Name = "txt_basic_port";
            this.txt_basic_port.Size = new System.Drawing.Size(79, 21);
            this.txt_basic_port.TabIndex = 4;
            this.txt_basic_port.Text = "80";
            this.txt_basic_port.TextChanged += new System.EventHandler(this.txt_basic_port_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "目标端口：";
            // 
            // txt_basic_host
            // 
            this.txt_basic_host.Location = new System.Drawing.Point(87, 18);
            this.txt_basic_host.Name = "txt_basic_host";
            this.txt_basic_host.Size = new System.Drawing.Size(155, 21);
            this.txt_basic_host.TabIndex = 2;
            this.txt_basic_host.Text = "127.0.0.1";
            this.txt_basic_host.TextChanged += new System.EventHandler(this.txt_basic_host_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(261, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "超时时间：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(419, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 1;
            this.label7.Text = "注入类型：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "域名或IP：";
            // 
            // gb_logo
            // 
            this.gb_logo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_logo.Controls.Add(this.txt_log);
            this.gb_logo.Location = new System.Drawing.Point(9, 578);
            this.gb_logo.Name = "gb_logo";
            this.gb_logo.Size = new System.Drawing.Size(839, 125);
            this.gb_logo.TabIndex = 3;
            this.gb_logo.TabStop = false;
            this.gb_logo.Text = "日志";
            // 
            // txt_log
            // 
            this.txt_log.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_log.Location = new System.Drawing.Point(3, 17);
            this.txt_log.Name = "txt_log";
            this.txt_log.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.txt_log.Size = new System.Drawing.Size(833, 105);
            this.txt_log.TabIndex = 0;
            this.txt_log.Text = "";
            // 
            // tab_logCenter
            // 
            this.tab_logCenter.BackColor = System.Drawing.SystemColors.Window;
            this.tab_logCenter.Controls.Add(this.splitContainer1);
            this.tab_logCenter.ImageKey = "log.png";
            this.tab_logCenter.Location = new System.Drawing.Point(4, 32);
            this.tab_logCenter.Name = "tab_logCenter";
            this.tab_logCenter.Size = new System.Drawing.Size(832, 416);
            this.tab_logCenter.TabIndex = 3;
            this.tab_logCenter.Text = "日志中心";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox5);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox6);
            this.splitContainer1.Size = new System.Drawing.Size(832, 416);
            this.splitContainer1.SplitterDistance = 202;
            this.splitContainer1.TabIndex = 2;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.log_lvw_httpLog);
            this.groupBox5.Location = new System.Drawing.Point(0, 8);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(832, 186);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "数据包历史记录";
            // 
            // log_lvw_httpLog
            // 
            this.log_lvw_httpLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.log_col_index,
            this.log_col_payload,
            this.col_runtime,
            this.log_col_code,
            this.log_col_bodyLength,
            this.log_col_sleepTime,
            this.col_proxy});
            this.log_lvw_httpLog.ContextMenuStrip = this.log_cms_dataifo;
            this.log_lvw_httpLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.log_lvw_httpLog.FullRowSelect = true;
            this.log_lvw_httpLog.GridLines = true;
            this.log_lvw_httpLog.HideSelection = false;
            this.log_lvw_httpLog.Location = new System.Drawing.Point(3, 17);
            this.log_lvw_httpLog.Name = "log_lvw_httpLog";
            this.log_lvw_httpLog.Size = new System.Drawing.Size(826, 166);
            this.log_lvw_httpLog.SmallImageList = this.img_line;
            this.log_lvw_httpLog.TabIndex = 1;
            this.log_lvw_httpLog.UseCompatibleStateImageBehavior = false;
            this.log_lvw_httpLog.View = System.Windows.Forms.View.Details;
            this.log_lvw_httpLog.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.log_lvw_httpLog_ColumnClick);
            this.log_lvw_httpLog.SelectedIndexChanged += new System.EventHandler(this.log_lvw_httpLog_SelectedIndexChanged);
            // 
            // log_col_index
            // 
            this.log_col_index.Text = "发包序号";
            // 
            // log_col_payload
            // 
            this.log_col_payload.Text = "Payload";
            this.log_col_payload.Width = 360;
            // 
            // col_runtime
            // 
            this.col_runtime.Text = "用时[毫秒]";
            this.col_runtime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.col_runtime.Width = 72;
            // 
            // log_col_code
            // 
            this.log_col_code.Text = "状态码";
            this.log_col_code.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.log_col_code.Width = 50;
            // 
            // log_col_bodyLength
            // 
            this.log_col_bodyLength.Text = "body长度";
            this.log_col_bodyLength.Width = 80;
            // 
            // log_col_sleepTime
            // 
            this.log_col_sleepTime.Text = "延时[毫秒]";
            this.log_col_sleepTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.log_col_sleepTime.Width = 72;
            // 
            // col_proxy
            // 
            this.col_proxy.Text = "代理";
            this.col_proxy.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.col_proxy.Width = 120;
            // 
            // log_cms_dataifo
            // 
            this.log_cms_dataifo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.data_cms_clearLog,
            this.data_cms_copyPaylaod});
            this.log_cms_dataifo.Name = "log_cms_dataifo";
            this.log_cms_dataifo.Size = new System.Drawing.Size(147, 48);
            // 
            // data_cms_clearLog
            // 
            this.data_cms_clearLog.Name = "data_cms_clearLog";
            this.data_cms_clearLog.Size = new System.Drawing.Size(146, 22);
            this.data_cms_clearLog.Text = "清空记录";
            this.data_cms_clearLog.Click += new System.EventHandler(this.data_cms_clearLog_Click);
            // 
            // data_cms_copyPaylaod
            // 
            this.data_cms_copyPaylaod.Name = "data_cms_copyPaylaod";
            this.data_cms_copyPaylaod.Size = new System.Drawing.Size(146, 22);
            this.data_cms_copyPaylaod.Text = "复制Payload";
            this.data_cms_copyPaylaod.Click += new System.EventHandler(this.data_cms_copyPaylaod_Click);
            // 
            // img_line
            // 
            this.img_line.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("img_line.ImageStream")));
            this.img_line.TransparentColor = System.Drawing.Color.Transparent;
            this.img_line.Images.SetKeyName(0, "line.png");
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.tabControl2);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox6.Location = new System.Drawing.Point(0, 0);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(832, 210);
            this.groupBox6.TabIndex = 1;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "数据包详情";
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Controls.Add(this.tabPage1);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(3, 17);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(826, 190);
            this.tabControl2.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.log_txt_request);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(818, 164);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "请 求";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // log_txt_request
            // 
            this.log_txt_request.DetectUrls = false;
            this.log_txt_request.Dock = System.Windows.Forms.DockStyle.Fill;
            this.log_txt_request.Location = new System.Drawing.Point(3, 3);
            this.log_txt_request.Name = "log_txt_request";
            this.log_txt_request.Size = new System.Drawing.Size(812, 158);
            this.log_txt_request.TabIndex = 0;
            this.log_txt_request.Text = "";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.log_txt_response);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(818, 164);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "响 应";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // log_txt_response
            // 
            this.log_txt_response.DetectUrls = false;
            this.log_txt_response.Dock = System.Windows.Forms.DockStyle.Fill;
            this.log_txt_response.Location = new System.Drawing.Point(3, 3);
            this.log_txt_response.Name = "log_txt_response";
            this.log_txt_response.Size = new System.Drawing.Size(812, 158);
            this.log_txt_response.TabIndex = 0;
            this.log_txt_response.Text = "";
            this.log_txt_response.KeyDown += new System.Windows.Forms.KeyEventHandler(this.log_txt_response_KeyDown);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.webBro_log);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(818, 164);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "在浏览器中显示";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // webBro_log
            // 
            this.webBro_log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBro_log.Location = new System.Drawing.Point(0, 0);
            this.webBro_log.MinimumSize = new System.Drawing.Size(21, 20);
            this.webBro_log.Name = "webBro_log";
            this.webBro_log.Size = new System.Drawing.Size(818, 164);
            this.webBro_log.TabIndex = 1;
            // 
            // tab_file
            // 
            this.tab_file.BackColor = System.Drawing.SystemColors.Window;
            this.tab_file.Controls.Add(this.file_txt_result);
            this.tab_file.Controls.Add(this.groupBox7);
            this.tab_file.ImageKey = "editFile.png";
            this.tab_file.Location = new System.Drawing.Point(4, 32);
            this.tab_file.Name = "tab_file";
            this.tab_file.Size = new System.Drawing.Size(832, 416);
            this.tab_file.TabIndex = 4;
            this.tab_file.Text = "文件操作";
            // 
            // file_txt_result
            // 
            this.file_txt_result.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.file_txt_result.Location = new System.Drawing.Point(3, 82);
            this.file_txt_result.MaxLength = 3276700;
            this.file_txt_result.Multiline = true;
            this.file_txt_result.Name = "file_txt_result";
            this.file_txt_result.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.file_txt_result.Size = new System.Drawing.Size(826, 331);
            this.file_txt_result.TabIndex = 0;
            this.file_txt_result.TextChanged += new System.EventHandler(this.file_txt_result_TextChanged);
            this.file_txt_result.KeyDown += new System.Windows.Forms.KeyEventHandler(this.file_txt_result_KeyDown);
            // 
            // groupBox7
            // 
            this.groupBox7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox7.Controls.Add(this.file_btn_stop);
            this.groupBox7.Controls.Add(this.file_btn_start);
            this.groupBox7.Controls.Add(this.file_cbox_readWrite);
            this.groupBox7.Controls.Add(this.file_txt_filePath);
            this.groupBox7.Controls.Add(this.label14);
            this.groupBox7.Controls.Add(this.cbox_file_readFileEncoding);
            this.groupBox7.Controls.Add(this.label24);
            this.groupBox7.Location = new System.Drawing.Point(3, 10);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(826, 63);
            this.groupBox7.TabIndex = 0;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "文件操作";
            // 
            // file_btn_stop
            // 
            this.file_btn_stop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.file_btn_stop.Location = new System.Drawing.Point(742, 22);
            this.file_btn_stop.Name = "file_btn_stop";
            this.file_btn_stop.Size = new System.Drawing.Size(69, 23);
            this.file_btn_stop.TabIndex = 12;
            this.file_btn_stop.Text = "停止";
            this.file_btn_stop.UseVisualStyleBackColor = true;
            this.file_btn_stop.Click += new System.EventHandler(this.file_btn_stop_Click);
            // 
            // file_btn_start
            // 
            this.file_btn_start.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.file_btn_start.Location = new System.Drawing.Point(658, 22);
            this.file_btn_start.Name = "file_btn_start";
            this.file_btn_start.Size = new System.Drawing.Size(69, 23);
            this.file_btn_start.TabIndex = 12;
            this.file_btn_start.Text = "开始";
            this.file_btn_start.UseVisualStyleBackColor = true;
            this.file_btn_start.Click += new System.EventHandler(this.file_btn_start_Click);
            // 
            // file_cbox_readWrite
            // 
            this.file_cbox_readWrite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.file_cbox_readWrite.FormattingEnabled = true;
            this.file_cbox_readWrite.Location = new System.Drawing.Point(408, 24);
            this.file_cbox_readWrite.Name = "file_cbox_readWrite";
            this.file_cbox_readWrite.Size = new System.Drawing.Size(229, 20);
            this.file_cbox_readWrite.TabIndex = 2;
            this.file_cbox_readWrite.SelectedIndexChanged += new System.EventHandler(this.file_cbox_readWrite_SelectedIndexChanged);
            // 
            // file_txt_filePath
            // 
            this.file_txt_filePath.Location = new System.Drawing.Point(84, 24);
            this.file_txt_filePath.Name = "file_txt_filePath";
            this.file_txt_filePath.Size = new System.Drawing.Size(157, 21);
            this.file_txt_filePath.TabIndex = 1;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(32, 27);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(47, 12);
            this.label14.TabIndex = 0;
            this.label14.Text = "路 径：";
            // 
            // cbox_file_readFileEncoding
            // 
            this.cbox_file_readFileEncoding.FormattingEnabled = true;
            this.cbox_file_readFileEncoding.Items.AddRange(new object[] {
            "UTF-8",
            "GB2312",
            "GBK",
            "ISO-8859-1"});
            this.cbox_file_readFileEncoding.Location = new System.Drawing.Point(324, 24);
            this.cbox_file_readFileEncoding.Name = "cbox_file_readFileEncoding";
            this.cbox_file_readFileEncoding.Size = new System.Drawing.Size(79, 20);
            this.cbox_file_readFileEncoding.TabIndex = 8;
            this.cbox_file_readFileEncoding.SelectedValueChanged += new System.EventHandler(this.cbox_file_readFileEncoding_SelectedValueChanged);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(252, 27);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(65, 12);
            this.label24.TabIndex = 2;
            this.label24.Text = "文件编码：";
            // 
            // tab_dataCenter
            // 
            this.tab_dataCenter.BackColor = System.Drawing.SystemColors.Window;
            this.tab_dataCenter.Controls.Add(this.tabC_dataCenter);
            this.tab_dataCenter.ImageKey = "db.png";
            this.tab_dataCenter.Location = new System.Drawing.Point(4, 32);
            this.tab_dataCenter.Name = "tab_dataCenter";
            this.tab_dataCenter.Padding = new System.Windows.Forms.Padding(3);
            this.tab_dataCenter.Size = new System.Drawing.Size(832, 416);
            this.tab_dataCenter.TabIndex = 1;
            this.tab_dataCenter.Text = "数据中心";
            // 
            // tabC_dataCenter
            // 
            this.tabC_dataCenter.Controls.Add(this.tab_vers);
            this.tabC_dataCenter.Controls.Add(this.tab_dbs);
            this.tabC_dataCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabC_dataCenter.ImageList = this.myicon_list;
            this.tabC_dataCenter.Location = new System.Drawing.Point(3, 3);
            this.tabC_dataCenter.Name = "tabC_dataCenter";
            this.tabC_dataCenter.Padding = new System.Drawing.Point(6, 6);
            this.tabC_dataCenter.SelectedIndex = 0;
            this.tabC_dataCenter.Size = new System.Drawing.Size(826, 410);
            this.tabC_dataCenter.TabIndex = 2;
            // 
            // tab_vers
            // 
            this.tab_vers.Controls.Add(this.toolStrip_getVers);
            this.tab_vers.Controls.Add(this.data_lvw_ver);
            this.tab_vers.ImageIndex = 12;
            this.tab_vers.Location = new System.Drawing.Point(4, 29);
            this.tab_vers.Name = "tab_vers";
            this.tab_vers.Padding = new System.Windows.Forms.Padding(3);
            this.tab_vers.Size = new System.Drawing.Size(818, 377);
            this.tab_vers.TabIndex = 0;
            this.tab_vers.Text = "环境变量";
            this.tab_vers.UseVisualStyleBackColor = true;
            // 
            // toolStrip_getVers
            // 
            this.toolStrip_getVers.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip_getVers.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip_getVers.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStrip_vers_btn_selectAll,
            this.toolStrip_vers_btn_selectReverse,
            this.toolStrip_vers_btn_getVariable,
            this.toolStrip_vers_btn_stopGetVariable});
            this.toolStrip_getVers.Location = new System.Drawing.Point(3, 349);
            this.toolStrip_getVers.Name = "toolStrip_getVers";
            this.toolStrip_getVers.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip_getVers.Size = new System.Drawing.Size(812, 25);
            this.toolStrip_getVers.TabIndex = 1;
            // 
            // toolStrip_vers_btn_selectAll
            // 
            this.toolStrip_vers_btn_selectAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStrip_vers_btn_selectAll.Image = ((System.Drawing.Image)(resources.GetObject("toolStrip_vers_btn_selectAll.Image")));
            this.toolStrip_vers_btn_selectAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStrip_vers_btn_selectAll.Name = "toolStrip_vers_btn_selectAll";
            this.toolStrip_vers_btn_selectAll.Size = new System.Drawing.Size(40, 22);
            this.toolStrip_vers_btn_selectAll.Text = "全 选";
            this.toolStrip_vers_btn_selectAll.Click += new System.EventHandler(this.toolStrip_vers_btn_selectAll_Click);
            // 
            // toolStrip_vers_btn_selectReverse
            // 
            this.toolStrip_vers_btn_selectReverse.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStrip_vers_btn_selectReverse.Image = ((System.Drawing.Image)(resources.GetObject("toolStrip_vers_btn_selectReverse.Image")));
            this.toolStrip_vers_btn_selectReverse.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStrip_vers_btn_selectReverse.Name = "toolStrip_vers_btn_selectReverse";
            this.toolStrip_vers_btn_selectReverse.Size = new System.Drawing.Size(40, 22);
            this.toolStrip_vers_btn_selectReverse.Text = "反 选";
            this.toolStrip_vers_btn_selectReverse.Click += new System.EventHandler(this.toolStrip_vers_btn_selectReverse_Click);
            // 
            // toolStrip_vers_btn_getVariable
            // 
            this.toolStrip_vers_btn_getVariable.Image = global::SuperSQLInjection.Properties.Resources.getvers;
            this.toolStrip_vers_btn_getVariable.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStrip_vers_btn_getVariable.Name = "toolStrip_vers_btn_getVariable";
            this.toolStrip_vers_btn_getVariable.Size = new System.Drawing.Size(100, 22);
            this.toolStrip_vers_btn_getVariable.Text = "获取环境变量";
            this.toolStrip_vers_btn_getVariable.Click += new System.EventHandler(this.toolStrip_vers_btn_getVariable_Click);
            // 
            // toolStrip_vers_btn_stopGetVariable
            // 
            this.toolStrip_vers_btn_stopGetVariable.Image = global::SuperSQLInjection.Properties.Resources.stop;
            this.toolStrip_vers_btn_stopGetVariable.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStrip_vers_btn_stopGetVariable.Name = "toolStrip_vers_btn_stopGetVariable";
            this.toolStrip_vers_btn_stopGetVariable.Size = new System.Drawing.Size(76, 22);
            this.toolStrip_vers_btn_stopGetVariable.Text = "停止获取";
            this.toolStrip_vers_btn_stopGetVariable.Click += new System.EventHandler(this.toolStrip_vers_btn_stopGetVariable_Click);
            // 
            // data_lvw_ver
            // 
            this.data_lvw_ver.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.data_lvw_ver.CheckBoxes = true;
            this.data_lvw_ver.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.data_lvw_ver_verName,
            this.data_lvw_ver_val});
            this.data_lvw_ver.ContextMenuStrip = this.data_cms_vers;
            this.data_lvw_ver.FullRowSelect = true;
            this.data_lvw_ver.HideSelection = false;
            this.data_lvw_ver.Location = new System.Drawing.Point(3, 3);
            this.data_lvw_ver.Name = "data_lvw_ver";
            this.data_lvw_ver.Size = new System.Drawing.Size(810, 343);
            this.data_lvw_ver.TabIndex = 0;
            this.data_lvw_ver.UseCompatibleStateImageBehavior = false;
            this.data_lvw_ver.View = System.Windows.Forms.View.Details;
            // 
            // data_lvw_ver_verName
            // 
            this.data_lvw_ver_verName.Text = "变量名";
            this.data_lvw_ver_verName.Width = 250;
            // 
            // data_lvw_ver_val
            // 
            this.data_lvw_ver_val.Text = "变量值";
            this.data_lvw_ver_val.Width = 500;
            // 
            // data_cms_vers
            // 
            this.data_cms_vers.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.data_cms_tsmi_getVariable,
            this.data_cms_tsmi_selectAllVers,
            this.data_cms_tsmi_selectReversVers,
            this.data_cms_tsmi_copyVerValue,
            this.data_cms_tsmi_stopGetVariable});
            this.data_cms_vers.Name = "data_cms_getVariable";
            this.data_cms_vers.Size = new System.Drawing.Size(149, 114);
            // 
            // data_cms_tsmi_getVariable
            // 
            this.data_cms_tsmi_getVariable.Name = "data_cms_tsmi_getVariable";
            this.data_cms_tsmi_getVariable.Size = new System.Drawing.Size(148, 22);
            this.data_cms_tsmi_getVariable.Text = "获取环境变量";
            this.data_cms_tsmi_getVariable.Click += new System.EventHandler(this.data_cms_tsmi_getVariable_Click);
            // 
            // data_cms_tsmi_selectAllVers
            // 
            this.data_cms_tsmi_selectAllVers.Name = "data_cms_tsmi_selectAllVers";
            this.data_cms_tsmi_selectAllVers.Size = new System.Drawing.Size(148, 22);
            this.data_cms_tsmi_selectAllVers.Text = "全 选";
            this.data_cms_tsmi_selectAllVers.Click += new System.EventHandler(this.data_cms_tsmi_selectAllVers_Click);
            // 
            // data_cms_tsmi_selectReversVers
            // 
            this.data_cms_tsmi_selectReversVers.Name = "data_cms_tsmi_selectReversVers";
            this.data_cms_tsmi_selectReversVers.Size = new System.Drawing.Size(148, 22);
            this.data_cms_tsmi_selectReversVers.Text = "反 选";
            this.data_cms_tsmi_selectReversVers.Click += new System.EventHandler(this.data_cms_tsmi_selectReversVers_Click);
            // 
            // data_cms_tsmi_copyVerValue
            // 
            this.data_cms_tsmi_copyVerValue.Name = "data_cms_tsmi_copyVerValue";
            this.data_cms_tsmi_copyVerValue.Size = new System.Drawing.Size(148, 22);
            this.data_cms_tsmi_copyVerValue.Text = "复制变量值";
            this.data_cms_tsmi_copyVerValue.Click += new System.EventHandler(this.data_cms_tsmi_copyVerValue_Click);
            // 
            // data_cms_tsmi_stopGetVariable
            // 
            this.data_cms_tsmi_stopGetVariable.Name = "data_cms_tsmi_stopGetVariable";
            this.data_cms_tsmi_stopGetVariable.Size = new System.Drawing.Size(148, 22);
            this.data_cms_tsmi_stopGetVariable.Text = "立即停止";
            this.data_cms_tsmi_stopGetVariable.Click += new System.EventHandler(this.data_cms_tsmi_stopGetVariable_Click);
            // 
            // tab_dbs
            // 
            this.tab_dbs.Controls.Add(this.spc_dbs);
            this.tab_dbs.ImageIndex = 13;
            this.tab_dbs.Location = new System.Drawing.Point(4, 29);
            this.tab_dbs.Name = "tab_dbs";
            this.tab_dbs.Padding = new System.Windows.Forms.Padding(3);
            this.tab_dbs.Size = new System.Drawing.Size(818, 377);
            this.tab_dbs.TabIndex = 1;
            this.tab_dbs.Text = "数据库信息";
            this.tab_dbs.UseVisualStyleBackColor = true;
            // 
            // spc_dbs
            // 
            this.spc_dbs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spc_dbs.Location = new System.Drawing.Point(3, 3);
            this.spc_dbs.Name = "spc_dbs";
            // 
            // spc_dbs.Panel1
            // 
            this.spc_dbs.Panel1.Controls.Add(this.data_dbs_ts);
            this.spc_dbs.Panel1.Controls.Add(this.groupBox2);
            // 
            // spc_dbs.Panel2
            // 
            this.spc_dbs.Panel2.Controls.Add(this.toolStrip1);
            this.spc_dbs.Panel2.Controls.Add(this.groupBox4);
            this.spc_dbs.Size = new System.Drawing.Size(812, 371);
            this.spc_dbs.SplitterDistance = 240;
            this.spc_dbs.SplitterWidth = 3;
            this.spc_dbs.TabIndex = 5;
            // 
            // data_dbs_ts
            // 
            this.data_dbs_ts.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.data_dbs_ts.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.data_dbs_ts.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.data_dbs_tsl_getDBS,
            this.data_dbs_tsl_getTables,
            this.data_dbs_tsl_getColumns});
            this.data_dbs_ts.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.data_dbs_ts.Location = new System.Drawing.Point(0, 346);
            this.data_dbs_ts.Name = "data_dbs_ts";
            this.data_dbs_ts.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.data_dbs_ts.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.data_dbs_ts.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.data_dbs_ts.Size = new System.Drawing.Size(240, 25);
            this.data_dbs_ts.TabIndex = 4;
            // 
            // data_dbs_tsl_getDBS
            // 
            this.data_dbs_tsl_getDBS.Image = global::SuperSQLInjection.Properties.Resources.getvers;
            this.data_dbs_tsl_getDBS.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.data_dbs_tsl_getDBS.Name = "data_dbs_tsl_getDBS";
            this.data_dbs_tsl_getDBS.Size = new System.Drawing.Size(72, 22);
            this.data_dbs_tsl_getDBS.Text = "获 取 库";
            this.data_dbs_tsl_getDBS.Click += new System.EventHandler(this.data_dbs_tsl_getDBS_Click);
            // 
            // data_dbs_tsl_getTables
            // 
            this.data_dbs_tsl_getTables.Image = global::SuperSQLInjection.Properties.Resources.getvers;
            this.data_dbs_tsl_getTables.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.data_dbs_tsl_getTables.Name = "data_dbs_tsl_getTables";
            this.data_dbs_tsl_getTables.Size = new System.Drawing.Size(72, 22);
            this.data_dbs_tsl_getTables.Text = "获 取 表";
            this.data_dbs_tsl_getTables.Click += new System.EventHandler(this.data_dbs_tsl_getTables_Click);
            // 
            // data_dbs_tsl_getColumns
            // 
            this.data_dbs_tsl_getColumns.Image = global::SuperSQLInjection.Properties.Resources.getvers;
            this.data_dbs_tsl_getColumns.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.data_dbs_tsl_getColumns.Name = "data_dbs_tsl_getColumns";
            this.data_dbs_tsl_getColumns.Size = new System.Drawing.Size(72, 22);
            this.data_dbs_tsl_getColumns.Text = "获 取 列";
            this.data_dbs_tsl_getColumns.Click += new System.EventHandler(this.data_dbs_tsl_getColumns_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.data_tvw_dbs);
            this.groupBox2.Location = new System.Drawing.Point(5, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(235, 338);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "数据库信息";
            // 
            // data_tvw_dbs
            // 
            this.data_tvw_dbs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.data_tvw_dbs.CheckBoxes = true;
            this.data_tvw_dbs.ContextMenuStrip = this.data_cms_dbs;
            this.data_tvw_dbs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.data_tvw_dbs.ImageIndex = 0;
            this.data_tvw_dbs.ImageList = this.imglist_database;
            this.data_tvw_dbs.LabelEdit = true;
            this.data_tvw_dbs.Location = new System.Drawing.Point(3, 17);
            this.data_tvw_dbs.Name = "data_tvw_dbs";
            this.data_tvw_dbs.SelectedImageIndex = 6;
            this.data_tvw_dbs.Size = new System.Drawing.Size(229, 318);
            this.data_tvw_dbs.TabIndex = 0;
            this.data_tvw_dbs.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.data_tvw_dbs_AfterCheck);
            this.data_tvw_dbs.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.data_tvw_dbs_AfterSelect);
            // 
            // data_cms_dbs
            // 
            this.data_cms_dbs.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.data_dbs_tsmi_addDBS,
            this.data_dbs_tsmi_addTableOrColumn,
            this.data_dbs_tsmi_getTableNames,
            this.data_dbs_tsmi_deleteNode,
            this.data_dbs_tsmi_stopGetInfos,
            this.data_dbs_tsmi_saveDTCStruct,
            this.data_dbs_tsmi_loadDTCStruct,
            this.data_dbs_tsmi_clearDTCStruct,
            this.data_dbs_tsmi_selectAllSubNode,
            this.data_dbs_tsmi_selectReversSubNode});
            this.data_cms_dbs.Name = "data_cms_getVariable";
            this.data_cms_dbs.Size = new System.Drawing.Size(161, 224);
            // 
            // data_dbs_tsmi_addDBS
            // 
            this.data_dbs_tsmi_addDBS.Name = "data_dbs_tsmi_addDBS";
            this.data_dbs_tsmi_addDBS.Size = new System.Drawing.Size(160, 22);
            this.data_dbs_tsmi_addDBS.Text = "添加数据库";
            this.data_dbs_tsmi_addDBS.Click += new System.EventHandler(this.data_dbs_tsmi_addDBS_Click);
            // 
            // data_dbs_tsmi_addTableOrColumn
            // 
            this.data_dbs_tsmi_addTableOrColumn.Name = "data_dbs_tsmi_addTableOrColumn";
            this.data_dbs_tsmi_addTableOrColumn.Size = new System.Drawing.Size(160, 22);
            this.data_dbs_tsmi_addTableOrColumn.Text = "添加表或列";
            this.data_dbs_tsmi_addTableOrColumn.Click += new System.EventHandler(this.data_dbs_tsmi_addTableOrColumn_Click);
            // 
            // data_dbs_tsmi_getTableNames
            // 
            this.data_dbs_tsmi_getTableNames.Name = "data_dbs_tsmi_getTableNames";
            this.data_dbs_tsmi_getTableNames.Size = new System.Drawing.Size(160, 22);
            this.data_dbs_tsmi_getTableNames.Text = "修改节点";
            this.data_dbs_tsmi_getTableNames.Click += new System.EventHandler(this.data_dbs_tsmi_getTableNames_Click);
            // 
            // data_dbs_tsmi_deleteNode
            // 
            this.data_dbs_tsmi_deleteNode.Name = "data_dbs_tsmi_deleteNode";
            this.data_dbs_tsmi_deleteNode.Size = new System.Drawing.Size(160, 22);
            this.data_dbs_tsmi_deleteNode.Text = "删除节点";
            this.data_dbs_tsmi_deleteNode.Click += new System.EventHandler(this.data_dbs_tsmi_deleteNode_Click);
            // 
            // data_dbs_tsmi_stopGetInfos
            // 
            this.data_dbs_tsmi_stopGetInfos.Name = "data_dbs_tsmi_stopGetInfos";
            this.data_dbs_tsmi_stopGetInfos.Size = new System.Drawing.Size(160, 22);
            this.data_dbs_tsmi_stopGetInfos.Text = "立即停止";
            this.data_dbs_tsmi_stopGetInfos.Click += new System.EventHandler(this.data_dbs_tsmi_stopGetInfos_Click);
            // 
            // data_dbs_tsmi_saveDTCStruct
            // 
            this.data_dbs_tsmi_saveDTCStruct.Name = "data_dbs_tsmi_saveDTCStruct";
            this.data_dbs_tsmi_saveDTCStruct.Size = new System.Drawing.Size(160, 22);
            this.data_dbs_tsmi_saveDTCStruct.Text = "保存库表列结构";
            this.data_dbs_tsmi_saveDTCStruct.Click += new System.EventHandler(this.data_dbs_tsmi_saveDTCStruct_Click);
            // 
            // data_dbs_tsmi_loadDTCStruct
            // 
            this.data_dbs_tsmi_loadDTCStruct.Name = "data_dbs_tsmi_loadDTCStruct";
            this.data_dbs_tsmi_loadDTCStruct.Size = new System.Drawing.Size(160, 22);
            this.data_dbs_tsmi_loadDTCStruct.Text = "加载库表列结构";
            this.data_dbs_tsmi_loadDTCStruct.Click += new System.EventHandler(this.data_dbs_tsmi_loadDTCStruct_Click);
            // 
            // data_dbs_tsmi_clearDTCStruct
            // 
            this.data_dbs_tsmi_clearDTCStruct.Name = "data_dbs_tsmi_clearDTCStruct";
            this.data_dbs_tsmi_clearDTCStruct.Size = new System.Drawing.Size(160, 22);
            this.data_dbs_tsmi_clearDTCStruct.Text = "清空所有结构";
            this.data_dbs_tsmi_clearDTCStruct.Click += new System.EventHandler(this.data_dbs_tsmi_clearDTCStruct_Click);
            // 
            // data_dbs_tsmi_selectAllSubNode
            // 
            this.data_dbs_tsmi_selectAllSubNode.Name = "data_dbs_tsmi_selectAllSubNode";
            this.data_dbs_tsmi_selectAllSubNode.Size = new System.Drawing.Size(160, 22);
            this.data_dbs_tsmi_selectAllSubNode.Text = "全选子节点";
            this.data_dbs_tsmi_selectAllSubNode.Click += new System.EventHandler(this.data_dbs_tsmi_selectAllSubNode_Click);
            // 
            // data_dbs_tsmi_selectReversSubNode
            // 
            this.data_dbs_tsmi_selectReversSubNode.Name = "data_dbs_tsmi_selectReversSubNode";
            this.data_dbs_tsmi_selectReversSubNode.Size = new System.Drawing.Size(160, 22);
            this.data_dbs_tsmi_selectReversSubNode.Text = "反选子节点";
            this.data_dbs_tsmi_selectReversSubNode.Click += new System.EventHandler(this.data_dbs_tsmi_selectReversSubNode_Click);
            // 
            // imglist_database
            // 
            this.imglist_database.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imglist_database.ImageStream")));
            this.imglist_database.TransparentColor = System.Drawing.Color.Transparent;
            this.imglist_database.Images.SetKeyName(0, "db.png");
            this.imglist_database.Images.SetKeyName(1, "table2.png");
            this.imglist_database.Images.SetKeyName(2, "column2.png");
            this.imglist_database.Images.SetKeyName(3, "column.png");
            this.imglist_database.Images.SetKeyName(4, "column1.png");
            this.imglist_database.Images.SetKeyName(5, "table1.png");
            this.imglist_database.Images.SetKeyName(6, "Image_1.png");
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel3,
            this.data_dbs_txt_start,
            this.toolStripLabel1,
            this.data_dbs_txt_count,
            this.toolStripLabel2,
            this.data_dbs_cob_db_encoding,
            this.data_dbs_tsl_getDatas,
            this.data_dbs_tsl_exportDatas,
            this.data_dbs_tsl_stopGetDatas});
            this.toolStrip1.Location = new System.Drawing.Point(0, 346);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(569, 25);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.TextChanged += new System.EventHandler(this.toolStrip1_TextChanged);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(48, 22);
            this.toolStripLabel3.Text = "开 始：";
            // 
            // data_dbs_txt_start
            // 
            this.data_dbs_txt_start.Margin = new System.Windows.Forms.Padding(0);
            this.data_dbs_txt_start.MaxLength = 9;
            this.data_dbs_txt_start.Name = "data_dbs_txt_start";
            this.data_dbs_txt_start.Size = new System.Drawing.Size(49, 25);
            this.data_dbs_txt_start.Text = "0";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(48, 22);
            this.toolStripLabel1.Text = "条 数：";
            // 
            // data_dbs_txt_count
            // 
            this.data_dbs_txt_count.MaxLength = 9;
            this.data_dbs_txt_count.Name = "data_dbs_txt_count";
            this.data_dbs_txt_count.Size = new System.Drawing.Size(49, 25);
            this.data_dbs_txt_count.Text = "1";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(44, 22);
            this.toolStripLabel2.Text = "编码：";
            // 
            // data_dbs_cob_db_encoding
            // 
            this.data_dbs_cob_db_encoding.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.data_dbs_cob_db_encoding.Items.AddRange(new object[] {
            "UTF-8",
            "GB2312",
            "GBK",
            "ISO-8859-1",
            "EUC-KR"});
            this.data_dbs_cob_db_encoding.Name = "data_dbs_cob_db_encoding";
            this.data_dbs_cob_db_encoding.Size = new System.Drawing.Size(89, 25);
            this.data_dbs_cob_db_encoding.TextChanged += new System.EventHandler(this.data_dbs_cob_db_encoding_TextChanged);
            // 
            // data_dbs_tsl_getDatas
            // 
            this.data_dbs_tsl_getDatas.Image = global::SuperSQLInjection.Properties.Resources.下载;
            this.data_dbs_tsl_getDatas.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.data_dbs_tsl_getDatas.Name = "data_dbs_tsl_getDatas";
            this.data_dbs_tsl_getDatas.Size = new System.Drawing.Size(76, 22);
            this.data_dbs_tsl_getDatas.Text = "获取数据";
            this.data_dbs_tsl_getDatas.Click += new System.EventHandler(this.data_dbs_tsl_getDatas_Click);
            // 
            // data_dbs_tsl_exportDatas
            // 
            this.data_dbs_tsl_exportDatas.Image = global::SuperSQLInjection.Properties.Resources.导出;
            this.data_dbs_tsl_exportDatas.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.data_dbs_tsl_exportDatas.Name = "data_dbs_tsl_exportDatas";
            this.data_dbs_tsl_exportDatas.Size = new System.Drawing.Size(76, 22);
            this.data_dbs_tsl_exportDatas.Text = "导出数据";
            this.data_dbs_tsl_exportDatas.Click += new System.EventHandler(this.data_dbs_tsl_exportDatas_Click);
            // 
            // data_dbs_tsl_stopGetDatas
            // 
            this.data_dbs_tsl_stopGetDatas.Image = global::SuperSQLInjection.Properties.Resources.stop;
            this.data_dbs_tsl_stopGetDatas.Margin = new System.Windows.Forms.Padding(10, 0, 0, 2);
            this.data_dbs_tsl_stopGetDatas.Name = "data_dbs_tsl_stopGetDatas";
            this.data_dbs_tsl_stopGetDatas.Size = new System.Drawing.Size(72, 23);
            this.data_dbs_tsl_stopGetDatas.Text = "停止获取";
            this.data_dbs_tsl_stopGetDatas.Click += new System.EventHandler(this.data_dbs_tsl_stopGetDatas_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.data_dbs_lvw_data);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(569, 371);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "获取数据";
            // 
            // data_dbs_lvw_data
            // 
            this.data_dbs_lvw_data.BackColor = System.Drawing.SystemColors.Window;
            this.data_dbs_lvw_data.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.data_dbs_lvw_data.ContextMenuStrip = this.cms_data_dbs_lvw_data;
            this.data_dbs_lvw_data.Dock = System.Windows.Forms.DockStyle.Fill;
            this.data_dbs_lvw_data.FullRowSelect = true;
            this.data_dbs_lvw_data.GridLines = true;
            this.data_dbs_lvw_data.HideSelection = false;
            this.data_dbs_lvw_data.Location = new System.Drawing.Point(3, 17);
            this.data_dbs_lvw_data.Name = "data_dbs_lvw_data";
            this.data_dbs_lvw_data.Size = new System.Drawing.Size(563, 351);
            this.data_dbs_lvw_data.SmallImageList = this.img_line;
            this.data_dbs_lvw_data.TabIndex = 1;
            this.data_dbs_lvw_data.UseCompatibleStateImageBehavior = false;
            this.data_dbs_lvw_data.View = System.Windows.Forms.View.Details;
            this.data_dbs_lvw_data.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.data_dbs_lvw_data_ColumnClick);
            // 
            // cms_data_dbs_lvw_data
            // 
            this.cms_data_dbs_lvw_data.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.data_dbs_lvw_tsmi_copyLineData,
            this.data_dbs_lvw_tsmi_stop,
            this.tsmi_clearColumns});
            this.cms_data_dbs_lvw_data.Name = "data_cms_getVariable";
            this.cms_data_dbs_lvw_data.Size = new System.Drawing.Size(149, 70);
            // 
            // data_dbs_lvw_tsmi_copyLineData
            // 
            this.data_dbs_lvw_tsmi_copyLineData.Name = "data_dbs_lvw_tsmi_copyLineData";
            this.data_dbs_lvw_tsmi_copyLineData.Size = new System.Drawing.Size(148, 22);
            this.data_dbs_lvw_tsmi_copyLineData.Text = "复制此行数据";
            this.data_dbs_lvw_tsmi_copyLineData.Click += new System.EventHandler(this.data_dbs_lvw_tsmi_copyLineData_Click);
            // 
            // data_dbs_lvw_tsmi_stop
            // 
            this.data_dbs_lvw_tsmi_stop.Name = "data_dbs_lvw_tsmi_stop";
            this.data_dbs_lvw_tsmi_stop.Size = new System.Drawing.Size(148, 22);
            this.data_dbs_lvw_tsmi_stop.Text = "立即停止";
            this.data_dbs_lvw_tsmi_stop.Click += new System.EventHandler(this.data_dbs_lvw_tsmi_stop_Click);
            // 
            // tsmi_clearColumns
            // 
            this.tsmi_clearColumns.Name = "tsmi_clearColumns";
            this.tsmi_clearColumns.Size = new System.Drawing.Size(148, 22);
            this.tsmi_clearColumns.Text = "清 空";
            this.tsmi_clearColumns.Click += new System.EventHandler(this.tsmi_clearColumns_Click);
            // 
            // myicon_list
            // 
            this.myicon_list.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("myicon_list.ImageStream")));
            this.myicon_list.TransparentColor = System.Drawing.Color.Transparent;
            this.myicon_list.Images.SetKeyName(0, "config.png");
            this.myicon_list.Images.SetKeyName(1, "db.png");
            this.myicon_list.Images.SetKeyName(2, "proxy.png");
            this.myicon_list.Images.SetKeyName(3, "editFile.png");
            this.myicon_list.Images.SetKeyName(4, "cmd.png");
            this.myicon_list.Images.SetKeyName(5, "bypass.png");
            this.myicon_list.Images.SetKeyName(6, "convert.png");
            this.myicon_list.Images.SetKeyName(7, "Image_1.png");
            this.myicon_list.Images.SetKeyName(8, "scan.png");
            this.myicon_list.Images.SetKeyName(9, "Ilog.png");
            this.myicon_list.Images.SetKeyName(10, "log.png");
            this.myicon_list.Images.SetKeyName(11, "tools.png");
            this.myicon_list.Images.SetKeyName(12, "vers.png");
            this.myicon_list.Images.SetKeyName(13, "dbinfo.png");
            this.myicon_list.Images.SetKeyName(14, "HTTP.png");
            // 
            // tab_injectCenter
            // 
            this.tab_injectCenter.BackColor = System.Drawing.SystemColors.Window;
            this.tab_injectCenter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tab_injectCenter.Controls.Add(this.groupBox1);
            this.tab_injectCenter.ImageKey = "config.png";
            this.tab_injectCenter.Location = new System.Drawing.Point(4, 32);
            this.tab_injectCenter.Name = "tab_injectCenter";
            this.tab_injectCenter.Padding = new System.Windows.Forms.Padding(3);
            this.tab_injectCenter.Size = new System.Drawing.Size(832, 416);
            this.tab_injectCenter.TabIndex = 0;
            this.tab_injectCenter.Text = "注入中心";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tabControl1);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(826, 410);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tab_datapack);
            this.tabControl1.Controls.Add(this.tab_tokenset);
            this.tabControl1.Controls.Add(this.tab_sencond_inject);
            this.tabControl1.ImageList = this.myicon_list;
            this.tabControl1.ItemSize = new System.Drawing.Size(118, 25);
            this.tabControl1.Location = new System.Drawing.Point(6, 13);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(0, 0);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(565, 391);
            this.tabControl1.TabIndex = 14;
            // 
            // tab_datapack
            // 
            this.tab_datapack.BackColor = System.Drawing.Color.Transparent;
            this.tab_datapack.Controls.Add(this.txt_inject_request);
            this.tab_datapack.ImageKey = "(无)";
            this.tab_datapack.Location = new System.Drawing.Point(4, 29);
            this.tab_datapack.Name = "tab_datapack";
            this.tab_datapack.Padding = new System.Windows.Forms.Padding(3);
            this.tab_datapack.Size = new System.Drawing.Size(557, 358);
            this.tab_datapack.TabIndex = 0;
            this.tab_datapack.Text = "HTTP请求包";
            // 
            // txt_inject_request
            // 
            this.txt_inject_request.BackColor = System.Drawing.SystemColors.Window;
            this.txt_inject_request.ContextMenuStrip = this.cms_dataPacks;
            this.txt_inject_request.DetectUrls = false;
            this.txt_inject_request.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_inject_request.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_inject_request.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txt_inject_request.Location = new System.Drawing.Point(3, 3);
            this.txt_inject_request.Name = "txt_inject_request";
            this.txt_inject_request.Size = new System.Drawing.Size(551, 352);
            this.txt_inject_request.TabIndex = 14;
            this.txt_inject_request.Text = resources.GetString("txt_inject_request.Text");
            this.txt_inject_request.TextChanged += new System.EventHandler(this.txt_inject_request_TextChanged);
            this.txt_inject_request.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_inject_request_KeyDown);
            // 
            // cms_dataPacks
            // 
            this.cms_dataPacks.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_createGetTemplate,
            this.tsmi_createPOSTTemplate,
            this.tsmi_changeRequestMethod,
            this.tsmi_createPackByURL});
            this.cms_dataPacks.Name = "cms_dataPacks";
            this.cms_dataPacks.Size = new System.Drawing.Size(184, 92);
            this.cms_dataPacks.Text = "复制数据";
            // 
            // tsmi_createGetTemplate
            // 
            this.tsmi_createGetTemplate.Name = "tsmi_createGetTemplate";
            this.tsmi_createGetTemplate.Size = new System.Drawing.Size(183, 22);
            this.tsmi_createGetTemplate.Text = "生成GET模板";
            this.tsmi_createGetTemplate.Click += new System.EventHandler(this.tsmi_createGetTemplate_Click);
            // 
            // tsmi_createPOSTTemplate
            // 
            this.tsmi_createPOSTTemplate.Name = "tsmi_createPOSTTemplate";
            this.tsmi_createPOSTTemplate.Size = new System.Drawing.Size(183, 22);
            this.tsmi_createPOSTTemplate.Text = "生成POST模板";
            this.tsmi_createPOSTTemplate.Click += new System.EventHandler(this.tsmi_createPOSTTemplate_Click);
            // 
            // tsmi_changeRequestMethod
            // 
            this.tsmi_changeRequestMethod.Name = "tsmi_changeRequestMethod";
            this.tsmi_changeRequestMethod.Size = new System.Drawing.Size(183, 22);
            this.tsmi_changeRequestMethod.Text = "转换提交方式";
            this.tsmi_changeRequestMethod.Click += new System.EventHandler(this.tsmi_changeRequestMethod_Click);
            // 
            // tsmi_createPackByURL
            // 
            this.tsmi_createPackByURL.Name = "tsmi_createPackByURL";
            this.tsmi_createPackByURL.Size = new System.Drawing.Size(183, 22);
            this.tsmi_createPackByURL.Text = "根据URL生成数据包";
            this.tsmi_createPackByURL.Click += new System.EventHandler(this.tsmi_createPackByURL_Click);
            // 
            // tab_tokenset
            // 
            this.tab_tokenset.Controls.Add(this.groupBox17);
            this.tab_tokenset.Controls.Add(this.groupBox19);
            this.tab_tokenset.Location = new System.Drawing.Point(4, 29);
            this.tab_tokenset.Name = "tab_tokenset";
            this.tab_tokenset.Padding = new System.Windows.Forms.Padding(3);
            this.tab_tokenset.Size = new System.Drawing.Size(557, 358);
            this.tab_tokenset.TabIndex = 1;
            this.tab_tokenset.Text = "Token/随机值";
            this.tab_tokenset.UseVisualStyleBackColor = true;
            // 
            // groupBox17
            // 
            this.groupBox17.Controls.Add(this.token_txt_http_request);
            this.groupBox17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox17.Location = new System.Drawing.Point(3, 3);
            this.groupBox17.Name = "groupBox17";
            this.groupBox17.Size = new System.Drawing.Size(551, 288);
            this.groupBox17.TabIndex = 25;
            this.groupBox17.TabStop = false;
            this.groupBox17.Text = "获取Token的HTTP请求包";
            // 
            // token_txt_http_request
            // 
            this.token_txt_http_request.DetectUrls = false;
            this.token_txt_http_request.Dock = System.Windows.Forms.DockStyle.Fill;
            this.token_txt_http_request.Location = new System.Drawing.Point(3, 17);
            this.token_txt_http_request.Name = "token_txt_http_request";
            this.token_txt_http_request.Size = new System.Drawing.Size(545, 268);
            this.token_txt_http_request.TabIndex = 0;
            this.token_txt_http_request.Text = "";
            this.token_txt_http_request.TextChanged += new System.EventHandler(this.token_txt_http_request_TextChanged);
            this.token_txt_http_request.KeyDown += new System.Windows.Forms.KeyEventHandler(this.token_txt_http_request_KeyDown);
            // 
            // groupBox19
            // 
            this.groupBox19.Controls.Add(this.label36);
            this.groupBox19.Controls.Add(this.label37);
            this.groupBox19.Controls.Add(this.token_txt_endStr);
            this.groupBox19.Controls.Add(this.token_txt_startStr);
            this.groupBox19.Controls.Add(this.token_btn_testGetToken);
            this.groupBox19.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox19.Location = new System.Drawing.Point(3, 291);
            this.groupBox19.Name = "groupBox19";
            this.groupBox19.Size = new System.Drawing.Size(551, 64);
            this.groupBox19.TabIndex = 25;
            this.groupBox19.TabStop = false;
            this.groupBox19.Text = "随机Token抓取规则";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(229, 27);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(65, 12);
            this.label36.TabIndex = 16;
            this.label36.Text = "结束字符：";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(19, 27);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(65, 12);
            this.label37.TabIndex = 16;
            this.label37.Text = "开始字符：";
            // 
            // token_txt_endStr
            // 
            this.token_txt_endStr.AcceptsReturn = true;
            this.token_txt_endStr.Location = new System.Drawing.Point(301, 25);
            this.token_txt_endStr.MaxLength = 100;
            this.token_txt_endStr.Name = "token_txt_endStr";
            this.token_txt_endStr.Size = new System.Drawing.Size(121, 21);
            this.token_txt_endStr.TabIndex = 8;
            this.token_txt_endStr.TextChanged += new System.EventHandler(this.token_txt_endStr_TextChanged);
            // 
            // token_txt_startStr
            // 
            this.token_txt_startStr.AcceptsReturn = true;
            this.token_txt_startStr.Location = new System.Drawing.Point(90, 25);
            this.token_txt_startStr.MaxLength = 100;
            this.token_txt_startStr.Name = "token_txt_startStr";
            this.token_txt_startStr.Size = new System.Drawing.Size(121, 21);
            this.token_txt_startStr.TabIndex = 8;
            this.token_txt_startStr.TextChanged += new System.EventHandler(this.token_txt_startStr_TextChanged);
            // 
            // token_btn_testGetToken
            // 
            this.token_btn_testGetToken.Location = new System.Drawing.Point(439, 23);
            this.token_btn_testGetToken.Name = "token_btn_testGetToken";
            this.token_btn_testGetToken.Size = new System.Drawing.Size(93, 23);
            this.token_btn_testGetToken.TabIndex = 6;
            this.token_btn_testGetToken.Text = "测试抓取Token";
            this.token_btn_testGetToken.UseVisualStyleBackColor = true;
            this.token_btn_testGetToken.Click += new System.EventHandler(this.token_btn_testGetToken_Click);
            // 
            // tab_sencond_inject
            // 
            this.tab_sencond_inject.Controls.Add(this.groupBox20);
            this.tab_sencond_inject.Location = new System.Drawing.Point(4, 29);
            this.tab_sencond_inject.Name = "tab_sencond_inject";
            this.tab_sencond_inject.Size = new System.Drawing.Size(557, 358);
            this.tab_sencond_inject.TabIndex = 2;
            this.tab_sencond_inject.Text = "二次注入";
            this.tab_sencond_inject.UseVisualStyleBackColor = true;
            // 
            // groupBox20
            // 
            this.groupBox20.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox20.Controls.Add(this.txt_sencond_request);
            this.groupBox20.Controls.Add(this.groupBox21);
            this.groupBox20.Location = new System.Drawing.Point(0, 3);
            this.groupBox20.Name = "groupBox20";
            this.groupBox20.Size = new System.Drawing.Size(555, 349);
            this.groupBox20.TabIndex = 26;
            this.groupBox20.TabStop = false;
            this.groupBox20.Text = "二次注入请求包";
            // 
            // txt_sencond_request
            // 
            this.txt_sencond_request.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_sencond_request.DetectUrls = false;
            this.txt_sencond_request.Location = new System.Drawing.Point(6, 19);
            this.txt_sencond_request.Name = "txt_sencond_request";
            this.txt_sencond_request.Size = new System.Drawing.Size(541, 269);
            this.txt_sencond_request.TabIndex = 11;
            this.txt_sencond_request.Text = "";
            this.txt_sencond_request.TextChanged += new System.EventHandler(this.txt_sencond_request_TextChanged);
            this.txt_sencond_request.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_sencond_request_KeyDown);
            // 
            // groupBox21
            // 
            this.groupBox21.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox21.Controls.Add(this.label28);
            this.groupBox21.Location = new System.Drawing.Point(6, 294);
            this.groupBox21.Name = "groupBox21";
            this.groupBox21.Size = new System.Drawing.Size(541, 50);
            this.groupBox21.TabIndex = 10;
            this.groupBox21.TabStop = false;
            this.groupBox21.Text = "说明";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(19, 24);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(425, 12);
            this.label28.TabIndex = 9;
            this.label28.Text = "此处可以放二次注入时第二次请求获取注入结果的页面，用于对付一些二次注入";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.groupBox16);
            this.groupBox3.Controls.Add(this.btn_inject_sendData);
            this.groupBox3.Controls.Add(this.btn_inject_clearRequest);
            this.groupBox3.Controls.Add(this.groupBox15);
            this.groupBox3.Controls.Add(this.chk_openURLEncoding);
            this.groupBox3.Controls.Add(this.btn_inject_setEncodingRange);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.btn_inject_randStr);
            this.groupBox3.Controls.Add(this.btn_inject_setTokenLocation);
            this.groupBox3.Controls.Add(this.chk_sencondInject);
            this.groupBox3.Controls.Add(this.chk_inject_foward_302);
            this.groupBox3.Controls.Add(this.btn_inject_setInject);
            this.groupBox3.Location = new System.Drawing.Point(577, 13);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(247, 384);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "注入设置";
            // 
            // groupBox16
            // 
            this.groupBox16.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox16.Controls.Add(this.label34);
            this.groupBox16.Controls.Add(this.label4);
            this.groupBox16.Controls.Add(this.txt_inject_showIndex);
            this.groupBox16.Controls.Add(this.label8);
            this.groupBox16.Controls.Add(this.txt_inject_unionTemplate);
            this.groupBox16.Controls.Add(this.txt_inject_unionColumnsCount);
            this.groupBox16.Location = new System.Drawing.Point(0, 145);
            this.groupBox16.Name = "groupBox16";
            this.groupBox16.Size = new System.Drawing.Size(249, 88);
            this.groupBox16.TabIndex = 14;
            this.groupBox16.TabStop = false;
            this.groupBox16.Text = "Union注入取数据配置";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(11, 57);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(65, 12);
            this.label34.TabIndex = 1;
            this.label34.Text = "填充模板：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "总列数：";
            // 
            // txt_inject_showIndex
            // 
            this.txt_inject_showIndex.Location = new System.Drawing.Point(195, 21);
            this.txt_inject_showIndex.MaxLength = 3;
            this.txt_inject_showIndex.Name = "txt_inject_showIndex";
            this.txt_inject_showIndex.Size = new System.Drawing.Size(40, 21);
            this.txt_inject_showIndex.TabIndex = 9;
            this.txt_inject_showIndex.Text = "2";
            this.txt_inject_showIndex.TextChanged += new System.EventHandler(this.txt_inject_showColumn_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(135, 25);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 8;
            this.label8.Text = "显示列：";
            // 
            // txt_inject_unionTemplate
            // 
            this.txt_inject_unionTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_inject_unionTemplate.Location = new System.Drawing.Point(78, 54);
            this.txt_inject_unionTemplate.MaxLength = 1000;
            this.txt_inject_unionTemplate.Name = "txt_inject_unionTemplate";
            this.txt_inject_unionTemplate.Size = new System.Drawing.Size(158, 21);
            this.txt_inject_unionTemplate.TabIndex = 7;
            this.txt_inject_unionTemplate.TextChanged += new System.EventHandler(this.txt_inject_unionTemplate_TextChanged);
            // 
            // txt_inject_unionColumnsCount
            // 
            this.txt_inject_unionColumnsCount.Location = new System.Drawing.Point(71, 22);
            this.txt_inject_unionColumnsCount.MaxLength = 3;
            this.txt_inject_unionColumnsCount.Name = "txt_inject_unionColumnsCount";
            this.txt_inject_unionColumnsCount.Size = new System.Drawing.Size(40, 21);
            this.txt_inject_unionColumnsCount.TabIndex = 7;
            this.txt_inject_unionColumnsCount.Text = "3";
            this.txt_inject_unionColumnsCount.TextChanged += new System.EventHandler(this.txt_inject_unionColumnsCount_TextChanged);
            // 
            // btn_inject_sendData
            // 
            this.btn_inject_sendData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_inject_sendData.Location = new System.Drawing.Point(13, 113);
            this.btn_inject_sendData.Name = "btn_inject_sendData";
            this.btn_inject_sendData.Size = new System.Drawing.Size(99, 23);
            this.btn_inject_sendData.TabIndex = 14;
            this.btn_inject_sendData.Text = "发送数据";
            this.btn_inject_sendData.UseVisualStyleBackColor = true;
            this.btn_inject_sendData.Click += new System.EventHandler(this.btn_inject_sendData_Click);
            // 
            // btn_inject_clearRequest
            // 
            this.btn_inject_clearRequest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_inject_clearRequest.Location = new System.Drawing.Point(137, 113);
            this.btn_inject_clearRequest.Name = "btn_inject_clearRequest";
            this.btn_inject_clearRequest.Size = new System.Drawing.Size(99, 23);
            this.btn_inject_clearRequest.TabIndex = 10;
            this.btn_inject_clearRequest.Text = "获取数据";
            this.btn_inject_clearRequest.UseVisualStyleBackColor = true;
            this.btn_inject_clearRequest.Click += new System.EventHandler(this.btn_inject_clearRequest_Click);
            // 
            // groupBox15
            // 
            this.groupBox15.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox15.Controls.Add(this.cbox_inject_type);
            this.groupBox15.Controls.Add(this.label29);
            this.groupBox15.Controls.Add(this.label27);
            this.groupBox15.Controls.Add(this.inject_btn_autoFindKey);
            this.groupBox15.Controls.Add(this.injectConfig_btn_checkKey);
            this.groupBox15.Controls.Add(this.txt_inject_key);
            this.groupBox15.Controls.Add(this.chk_inject_reverseKey);
            this.groupBox15.Location = new System.Drawing.Point(0, 239);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Size = new System.Drawing.Size(249, 145);
            this.groupBox15.TabIndex = 14;
            this.groupBox15.TabStop = false;
            this.groupBox15.Text = "盲注取数据配置";
            // 
            // cbox_inject_type
            // 
            this.cbox_inject_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbox_inject_type.FormattingEnabled = true;
            this.cbox_inject_type.Items.AddRange(new object[] {
            "关键字判断-Bool",
            "正则匹配判断-Bool",
            "状态码判断-Bool",
            "时间判断-延时Bool",
            "响应长度等于-Bool",
            "响应长度大于-Bool",
            "响应长度小于-Bool"});
            this.cbox_inject_type.Location = new System.Drawing.Point(79, 21);
            this.cbox_inject_type.Name = "cbox_inject_type";
            this.cbox_inject_type.Size = new System.Drawing.Size(158, 20);
            this.cbox_inject_type.TabIndex = 27;
            this.cbox_inject_type.SelectedIndexChanged += new System.EventHandler(this.cbox_inject_type_SelectedIndexChanged);
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(9, 82);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(65, 12);
            this.label29.TabIndex = 26;
            this.label29.Text = "判 断 值：";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(9, 24);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(65, 12);
            this.label27.TabIndex = 26;
            this.label27.Text = "判断方式：";
            // 
            // inject_btn_autoFindKey
            // 
            this.inject_btn_autoFindKey.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.inject_btn_autoFindKey.Location = new System.Drawing.Point(11, 110);
            this.inject_btn_autoFindKey.Name = "inject_btn_autoFindKey";
            this.inject_btn_autoFindKey.Size = new System.Drawing.Size(99, 23);
            this.inject_btn_autoFindKey.TabIndex = 24;
            this.inject_btn_autoFindKey.Text = "查找判断值";
            this.inject_btn_autoFindKey.UseVisualStyleBackColor = true;
            this.inject_btn_autoFindKey.Click += new System.EventHandler(this.inject_btn_autoFindKey_Click);
            // 
            // injectConfig_btn_checkKey
            // 
            this.injectConfig_btn_checkKey.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.injectConfig_btn_checkKey.Location = new System.Drawing.Point(137, 110);
            this.injectConfig_btn_checkKey.Name = "injectConfig_btn_checkKey";
            this.injectConfig_btn_checkKey.Size = new System.Drawing.Size(99, 23);
            this.injectConfig_btn_checkKey.TabIndex = 23;
            this.injectConfig_btn_checkKey.Text = "验证判断值";
            this.injectConfig_btn_checkKey.UseVisualStyleBackColor = true;
            this.injectConfig_btn_checkKey.Click += new System.EventHandler(this.injectConfig_btn_checkKey_Click);
            // 
            // txt_inject_key
            // 
            this.txt_inject_key.Location = new System.Drawing.Point(79, 79);
            this.txt_inject_key.MaxLength = 50;
            this.txt_inject_key.Name = "txt_inject_key";
            this.txt_inject_key.Size = new System.Drawing.Size(158, 21);
            this.txt_inject_key.TabIndex = 7;
            this.txt_inject_key.Text = "个";
            this.txt_inject_key.TextChanged += new System.EventHandler(this.txt_inject_key_TextChanged);
            // 
            // chk_inject_reverseKey
            // 
            this.chk_inject_reverseKey.AutoSize = true;
            this.chk_inject_reverseKey.Location = new System.Drawing.Point(13, 53);
            this.chk_inject_reverseKey.Name = "chk_inject_reverseKey";
            this.chk_inject_reverseKey.Size = new System.Drawing.Size(192, 16);
            this.chk_inject_reverseKey.TabIndex = 22;
            this.chk_inject_reverseKey.Text = "是否反取判断值[盲注逻辑反转]";
            this.chk_inject_reverseKey.UseVisualStyleBackColor = true;
            this.chk_inject_reverseKey.CheckedChanged += new System.EventHandler(this.chk_inject_reverseKey_CheckedChanged);
            // 
            // chk_openURLEncoding
            // 
            this.chk_openURLEncoding.AutoSize = true;
            this.chk_openURLEncoding.Checked = true;
            this.chk_openURLEncoding.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_openURLEncoding.Location = new System.Drawing.Point(12, 22);
            this.chk_openURLEncoding.Name = "chk_openURLEncoding";
            this.chk_openURLEncoding.Size = new System.Drawing.Size(66, 16);
            this.chk_openURLEncoding.TabIndex = 21;
            this.chk_openURLEncoding.Text = "URL编码";
            this.chk_openURLEncoding.UseVisualStyleBackColor = true;
            this.chk_openURLEncoding.CheckedChanged += new System.EventHandler(this.chk_openURLEncoding_CheckedChanged);
            // 
            // btn_inject_setEncodingRange
            // 
            this.btn_inject_setEncodingRange.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_inject_setEncodingRange.Location = new System.Drawing.Point(137, 48);
            this.btn_inject_setEncodingRange.Name = "btn_inject_setEncodingRange";
            this.btn_inject_setEncodingRange.Size = new System.Drawing.Size(99, 23);
            this.btn_inject_setEncodingRange.TabIndex = 19;
            this.btn_inject_setEncodingRange.Text = "编码标记";
            this.btn_inject_setEncodingRange.UseVisualStyleBackColor = true;
            this.btn_inject_setEncodingRange.Click += new System.EventHandler(this.btn_inject_setEncodingRange_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(9, 227);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(0, 12);
            this.label13.TabIndex = 18;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(9, 126);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(0, 12);
            this.label12.TabIndex = 17;
            // 
            // btn_inject_randStr
            // 
            this.btn_inject_randStr.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_inject_randStr.Location = new System.Drawing.Point(137, 80);
            this.btn_inject_randStr.Name = "btn_inject_randStr";
            this.btn_inject_randStr.Size = new System.Drawing.Size(99, 23);
            this.btn_inject_randStr.TabIndex = 10;
            this.btn_inject_randStr.Text = "Rand随机值";
            this.btn_inject_randStr.UseVisualStyleBackColor = true;
            this.btn_inject_randStr.Click += new System.EventHandler(this.btn_inject_randStr_Click);
            // 
            // btn_inject_setTokenLocation
            // 
            this.btn_inject_setTokenLocation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_inject_setTokenLocation.Location = new System.Drawing.Point(12, 80);
            this.btn_inject_setTokenLocation.Name = "btn_inject_setTokenLocation";
            this.btn_inject_setTokenLocation.Size = new System.Drawing.Size(99, 23);
            this.btn_inject_setTokenLocation.TabIndex = 10;
            this.btn_inject_setTokenLocation.Text = "标记Token";
            this.btn_inject_setTokenLocation.UseVisualStyleBackColor = true;
            this.btn_inject_setTokenLocation.Click += new System.EventHandler(this.btn_inject_setTokenLocation_Click);
            // 
            // chk_sencondInject
            // 
            this.chk_sencondInject.AutoSize = true;
            this.chk_sencondInject.Location = new System.Drawing.Point(165, 22);
            this.chk_sencondInject.Name = "chk_sencondInject";
            this.chk_sencondInject.Size = new System.Drawing.Size(72, 16);
            this.chk_sencondInject.TabIndex = 16;
            this.chk_sencondInject.Text = "二次注入";
            this.chk_sencondInject.UseVisualStyleBackColor = true;
            this.chk_sencondInject.CheckedChanged += new System.EventHandler(this.chk_sencondInject_CheckedChanged);
            // 
            // chk_inject_foward_302
            // 
            this.chk_inject_foward_302.AutoSize = true;
            this.chk_inject_foward_302.Location = new System.Drawing.Point(90, 22);
            this.chk_inject_foward_302.Name = "chk_inject_foward_302";
            this.chk_inject_foward_302.Size = new System.Drawing.Size(66, 16);
            this.chk_inject_foward_302.TabIndex = 16;
            this.chk_inject_foward_302.Text = "302跟踪";
            this.chk_inject_foward_302.UseVisualStyleBackColor = true;
            this.chk_inject_foward_302.CheckedChanged += new System.EventHandler(this.chk_inject_foward_302_CheckedChanged);
            // 
            // btn_inject_setInject
            // 
            this.btn_inject_setInject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_inject_setInject.Location = new System.Drawing.Point(12, 48);
            this.btn_inject_setInject.Name = "btn_inject_setInject";
            this.btn_inject_setInject.Size = new System.Drawing.Size(99, 23);
            this.btn_inject_setInject.TabIndex = 13;
            this.btn_inject_setInject.Text = "注入标记";
            this.btn_inject_setInject.UseVisualStyleBackColor = true;
            this.btn_inject_setInject.Click += new System.EventHandler(this.btn_inject_setInject_Click);
            // 
            // mytab
            // 
            this.mytab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mytab.Controls.Add(this.tab_injectCenter);
            this.mytab.Controls.Add(this.tab_dataCenter);
            this.mytab.Controls.Add(this.tab_proxy);
            this.mytab.Controls.Add(this.tab_file);
            this.mytab.Controls.Add(this.tab_cmd);
            this.mytab.Controls.Add(this.tab_bypass);
            this.mytab.Controls.Add(this.tab_encoding);
            this.mytab.Controls.Add(this.tab_scanInjection);
            this.mytab.Controls.Add(this.tab_injectLog);
            this.mytab.Controls.Add(this.tab_logCenter);
            this.mytab.ImageList = this.myicon_list;
            this.mytab.ItemSize = new System.Drawing.Size(82, 28);
            this.mytab.Location = new System.Drawing.Point(9, 120);
            this.mytab.Name = "mytab";
            this.mytab.Padding = new System.Drawing.Point(0, 0);
            this.mytab.SelectedIndex = 0;
            this.mytab.Size = new System.Drawing.Size(840, 452);
            this.mytab.TabIndex = 1;
            // 
            // tab_proxy
            // 
            this.tab_proxy.BackColor = System.Drawing.SystemColors.Window;
            this.tab_proxy.Controls.Add(this.groupBox25);
            this.tab_proxy.Controls.Add(this.groupBox24);
            this.tab_proxy.ImageKey = "proxy.png";
            this.tab_proxy.Location = new System.Drawing.Point(4, 32);
            this.tab_proxy.Name = "tab_proxy";
            this.tab_proxy.Padding = new System.Windows.Forms.Padding(3);
            this.tab_proxy.Size = new System.Drawing.Size(832, 416);
            this.tab_proxy.TabIndex = 11;
            this.tab_proxy.Text = "代理设置";
            // 
            // groupBox25
            // 
            this.groupBox25.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox25.Controls.Add(this.label46);
            this.groupBox25.Controls.Add(this.proxy_lbl_proxyType);
            this.groupBox25.Controls.Add(this.proxy_lbl_proxy_port);
            this.groupBox25.Controls.Add(this.proxy_lbl_proxy_password);
            this.groupBox25.Controls.Add(this.proxy_lbl_proxy_username);
            this.groupBox25.Controls.Add(this.proxy_lbl_proxy_host);
            this.groupBox25.Controls.Add(this.label45);
            this.groupBox25.Controls.Add(this.label39);
            this.groupBox25.Controls.Add(this.label38);
            this.groupBox25.Controls.Add(this.label35);
            this.groupBox25.Controls.Add(this.proxy_cmb_proxyMode);
            this.groupBox25.Location = new System.Drawing.Point(6, 12);
            this.groupBox25.Name = "groupBox25";
            this.groupBox25.Size = new System.Drawing.Size(821, 53);
            this.groupBox25.TabIndex = 6;
            this.groupBox25.TabStop = false;
            this.groupBox25.Text = "设置代理";
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Location = new System.Drawing.Point(439, 25);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(41, 12);
            this.label46.TabIndex = 0;
            this.label46.Text = "类型：";
            // 
            // proxy_lbl_proxyType
            // 
            this.proxy_lbl_proxyType.AutoSize = true;
            this.proxy_lbl_proxyType.Location = new System.Drawing.Point(486, 25);
            this.proxy_lbl_proxyType.Name = "proxy_lbl_proxyType";
            this.proxy_lbl_proxyType.Size = new System.Drawing.Size(53, 12);
            this.proxy_lbl_proxyType.TabIndex = 0;
            this.proxy_lbl_proxyType.Text = "--------";
            // 
            // proxy_lbl_proxy_port
            // 
            this.proxy_lbl_proxy_port.AutoSize = true;
            this.proxy_lbl_proxy_port.Location = new System.Drawing.Point(385, 25);
            this.proxy_lbl_proxy_port.Name = "proxy_lbl_proxy_port";
            this.proxy_lbl_proxy_port.Size = new System.Drawing.Size(41, 12);
            this.proxy_lbl_proxy_port.TabIndex = 0;
            this.proxy_lbl_proxy_port.Text = "------";
            // 
            // proxy_lbl_proxy_password
            // 
            this.proxy_lbl_proxy_password.AutoSize = true;
            this.proxy_lbl_proxy_password.Location = new System.Drawing.Point(731, 25);
            this.proxy_lbl_proxy_password.Name = "proxy_lbl_proxy_password";
            this.proxy_lbl_proxy_password.Size = new System.Drawing.Size(77, 12);
            this.proxy_lbl_proxy_password.TabIndex = 0;
            this.proxy_lbl_proxy_password.Text = "------------";
            // 
            // proxy_lbl_proxy_username
            // 
            this.proxy_lbl_proxy_username.AutoSize = true;
            this.proxy_lbl_proxy_username.Location = new System.Drawing.Point(611, 25);
            this.proxy_lbl_proxy_username.Name = "proxy_lbl_proxy_username";
            this.proxy_lbl_proxy_username.Size = new System.Drawing.Size(71, 12);
            this.proxy_lbl_proxy_username.TabIndex = 0;
            this.proxy_lbl_proxy_username.Text = "-----------";
            // 
            // proxy_lbl_proxy_host
            // 
            this.proxy_lbl_proxy_host.AutoSize = true;
            this.proxy_lbl_proxy_host.Location = new System.Drawing.Point(233, 25);
            this.proxy_lbl_proxy_host.Name = "proxy_lbl_proxy_host";
            this.proxy_lbl_proxy_host.Size = new System.Drawing.Size(95, 12);
            this.proxy_lbl_proxy_host.TabIndex = 0;
            this.proxy_lbl_proxy_host.Text = "---------------";
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Location = new System.Drawing.Point(151, 25);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(77, 12);
            this.label45.TabIndex = 0;
            this.label45.Text = "固定代理IP：";
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(687, 25);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(41, 12);
            this.label39.TabIndex = 0;
            this.label39.Text = "密码：";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(551, 25);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(53, 12);
            this.label38.TabIndex = 0;
            this.label38.Text = "用户名：";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(337, 25);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(41, 12);
            this.label35.TabIndex = 0;
            this.label35.Text = "端口：";
            // 
            // proxy_cmb_proxyMode
            // 
            this.proxy_cmb_proxyMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.proxy_cmb_proxyMode.FormattingEnabled = true;
            this.proxy_cmb_proxyMode.Items.AddRange(new object[] {
            "不开启代理",
            "代理池随机选择",
            "固定代理方式"});
            this.proxy_cmb_proxyMode.Location = new System.Drawing.Point(13, 22);
            this.proxy_cmb_proxyMode.Name = "proxy_cmb_proxyMode";
            this.proxy_cmb_proxyMode.Size = new System.Drawing.Size(131, 20);
            this.proxy_cmb_proxyMode.TabIndex = 5;
            this.proxy_cmb_proxyMode.TextChanged += new System.EventHandler(this.proxy_cmb_proxyMode_TextChanged);
            // 
            // groupBox24
            // 
            this.groupBox24.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox24.Controls.Add(this.toolStrip_proxyList);
            this.groupBox24.Controls.Add(this.groupBox26);
            this.groupBox24.Controls.Add(this.proxy_lvw_proxyList);
            this.groupBox24.Location = new System.Drawing.Point(6, 76);
            this.groupBox24.Name = "groupBox24";
            this.groupBox24.Size = new System.Drawing.Size(821, 343);
            this.groupBox24.TabIndex = 6;
            this.groupBox24.TabStop = false;
            this.groupBox24.Text = "代理池列表";
            // 
            // toolStrip_proxyList
            // 
            this.toolStrip_proxyList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip_proxyList.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip_proxyList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.proxy_ts_btn_clearAllFailedProxy,
            this.proxy_ts_btn_proxy_checkNoCheckProxy});
            this.toolStrip_proxyList.Location = new System.Drawing.Point(3, 315);
            this.toolStrip_proxyList.Name = "toolStrip_proxyList";
            this.toolStrip_proxyList.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip_proxyList.Size = new System.Drawing.Size(815, 25);
            this.toolStrip_proxyList.TabIndex = 7;
            this.toolStrip_proxyList.Text = "toolStrip2";
            // 
            // proxy_ts_btn_clearAllFailedProxy
            // 
            this.proxy_ts_btn_clearAllFailedProxy.Name = "proxy_ts_btn_clearAllFailedProxy";
            this.proxy_ts_btn_clearAllFailedProxy.Size = new System.Drawing.Size(80, 22);
            this.proxy_ts_btn_clearAllFailedProxy.Text = "清除无效代理";
            this.proxy_ts_btn_clearAllFailedProxy.Click += new System.EventHandler(this.proxy_ts_btn_clearAllFailedProxy_Click);
            // 
            // proxy_ts_btn_proxy_checkNoCheckProxy
            // 
            this.proxy_ts_btn_proxy_checkNoCheckProxy.Name = "proxy_ts_btn_proxy_checkNoCheckProxy";
            this.proxy_ts_btn_proxy_checkNoCheckProxy.Size = new System.Drawing.Size(92, 22);
            this.proxy_ts_btn_proxy_checkNoCheckProxy.Text = "验证未验证代理";
            this.proxy_ts_btn_proxy_checkNoCheckProxy.Click += new System.EventHandler(this.proxy_ts_btn_proxy_checkNoCheckProxy_Click);
            // 
            // groupBox26
            // 
            this.groupBox26.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox26.Controls.Add(this.label40);
            this.groupBox26.Controls.Add(this.label41);
            this.groupBox26.Controls.Add(this.proxy_btn_importProxy);
            this.groupBox26.Controls.Add(this.proxy_btn_addProxy);
            this.groupBox26.Controls.Add(this.label44);
            this.groupBox26.Controls.Add(this.label43);
            this.groupBox26.Controls.Add(this.proxy_txt_addProxyPassword);
            this.groupBox26.Controls.Add(this.label42);
            this.groupBox26.Controls.Add(this.proxy_txt_addProxyUserName);
            this.groupBox26.Controls.Add(this.proxy_txt_addProxyHost);
            this.groupBox26.Controls.Add(this.proxy_txt_addProxyPort);
            this.groupBox26.Controls.Add(this.proxy_txt_addProxyType);
            this.groupBox26.Location = new System.Drawing.Point(6, 15);
            this.groupBox26.Name = "groupBox26";
            this.groupBox26.Size = new System.Drawing.Size(809, 57);
            this.groupBox26.TabIndex = 6;
            this.groupBox26.TabStop = false;
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(275, 26);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(41, 12);
            this.label40.TabIndex = 0;
            this.label40.Text = "类型：";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(6, 25);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(53, 12);
            this.label41.TabIndex = 0;
            this.label41.Text = "代理IP：";
            // 
            // proxy_btn_importProxy
            // 
            this.proxy_btn_importProxy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.proxy_btn_importProxy.ImageKey = "(无)";
            this.proxy_btn_importProxy.Location = new System.Drawing.Point(737, 21);
            this.proxy_btn_importProxy.Name = "proxy_btn_importProxy";
            this.proxy_btn_importProxy.Size = new System.Drawing.Size(65, 23);
            this.proxy_btn_importProxy.TabIndex = 5;
            this.proxy_btn_importProxy.Text = "导入代理";
            this.proxy_btn_importProxy.UseVisualStyleBackColor = true;
            this.proxy_btn_importProxy.Click += new System.EventHandler(this.proxy_btn_importProxy_Click);
            // 
            // proxy_btn_addProxy
            // 
            this.proxy_btn_addProxy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.proxy_btn_addProxy.ImageKey = "(无)";
            this.proxy_btn_addProxy.Location = new System.Drawing.Point(665, 21);
            this.proxy_btn_addProxy.Name = "proxy_btn_addProxy";
            this.proxy_btn_addProxy.Size = new System.Drawing.Size(65, 23);
            this.proxy_btn_addProxy.TabIndex = 5;
            this.proxy_btn_addProxy.Text = "添加代理";
            this.proxy_btn_addProxy.UseVisualStyleBackColor = true;
            this.proxy_btn_addProxy.Click += new System.EventHandler(this.proxy_btn_addProxy_Click);
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(545, 26);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(41, 12);
            this.label44.TabIndex = 0;
            this.label44.Text = "密码：";
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(415, 26);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(53, 12);
            this.label43.TabIndex = 0;
            this.label43.Text = "用户名：";
            // 
            // proxy_txt_addProxyPassword
            // 
            this.proxy_txt_addProxyPassword.Location = new System.Drawing.Point(591, 22);
            this.proxy_txt_addProxyPassword.Name = "proxy_txt_addProxyPassword";
            this.proxy_txt_addProxyPassword.Size = new System.Drawing.Size(65, 21);
            this.proxy_txt_addProxyPassword.TabIndex = 4;
            this.proxy_txt_addProxyPassword.TextChanged += new System.EventHandler(this.txt_basic_port_TextChanged);
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(187, 26);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(41, 12);
            this.label42.TabIndex = 0;
            this.label42.Text = "端口：";
            // 
            // proxy_txt_addProxyUserName
            // 
            this.proxy_txt_addProxyUserName.Location = new System.Drawing.Point(473, 23);
            this.proxy_txt_addProxyUserName.Name = "proxy_txt_addProxyUserName";
            this.proxy_txt_addProxyUserName.Size = new System.Drawing.Size(65, 21);
            this.proxy_txt_addProxyUserName.TabIndex = 4;
            this.proxy_txt_addProxyUserName.TextChanged += new System.EventHandler(this.txt_basic_port_TextChanged);
            // 
            // proxy_txt_addProxyHost
            // 
            this.proxy_txt_addProxyHost.Location = new System.Drawing.Point(65, 22);
            this.proxy_txt_addProxyHost.Name = "proxy_txt_addProxyHost";
            this.proxy_txt_addProxyHost.Size = new System.Drawing.Size(117, 21);
            this.proxy_txt_addProxyHost.TabIndex = 4;
            this.proxy_txt_addProxyHost.TextChanged += new System.EventHandler(this.txt_basic_port_TextChanged);
            // 
            // proxy_txt_addProxyPort
            // 
            this.proxy_txt_addProxyPort.Location = new System.Drawing.Point(229, 23);
            this.proxy_txt_addProxyPort.Name = "proxy_txt_addProxyPort";
            this.proxy_txt_addProxyPort.Size = new System.Drawing.Size(39, 21);
            this.proxy_txt_addProxyPort.TabIndex = 4;
            this.proxy_txt_addProxyPort.TextChanged += new System.EventHandler(this.txt_basic_port_TextChanged);
            // 
            // proxy_txt_addProxyType
            // 
            this.proxy_txt_addProxyType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.proxy_txt_addProxyType.FormattingEnabled = true;
            this.proxy_txt_addProxyType.Items.AddRange(new object[] {
            "Socks5",
            "HTTP"});
            this.proxy_txt_addProxyType.Location = new System.Drawing.Point(321, 23);
            this.proxy_txt_addProxyType.Name = "proxy_txt_addProxyType";
            this.proxy_txt_addProxyType.Size = new System.Drawing.Size(88, 20);
            this.proxy_txt_addProxyType.TabIndex = 5;
            this.proxy_txt_addProxyType.SelectedIndexChanged += new System.EventHandler(this.cbox_basic_injectType_SelectedIndexChanged);
            // 
            // proxy_lvw_proxyList
            // 
            this.proxy_lvw_proxyList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.proxy_lvw_proxyList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.col_host,
            this.col_port,
            this.col_proxyType,
            this.col_username,
            this.col_password,
            this.col_conectIsOK,
            this.col_useTime,
            this.col_checkTime});
            this.proxy_lvw_proxyList.ContextMenuStrip = this.proxy_cms;
            this.proxy_lvw_proxyList.FullRowSelect = true;
            this.proxy_lvw_proxyList.HideSelection = false;
            this.proxy_lvw_proxyList.Location = new System.Drawing.Point(6, 83);
            this.proxy_lvw_proxyList.Name = "proxy_lvw_proxyList";
            this.proxy_lvw_proxyList.Size = new System.Drawing.Size(808, 229);
            this.proxy_lvw_proxyList.SmallImageList = this.img_line;
            this.proxy_lvw_proxyList.TabIndex = 0;
            this.proxy_lvw_proxyList.UseCompatibleStateImageBehavior = false;
            this.proxy_lvw_proxyList.View = System.Windows.Forms.View.Details;
            // 
            // col_host
            // 
            this.col_host.Text = "域名或IP";
            this.col_host.Width = 120;
            // 
            // col_port
            // 
            this.col_port.Text = "代理端口";
            this.col_port.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.col_port.Width = 80;
            // 
            // col_proxyType
            // 
            this.col_proxyType.Text = "代理类型";
            this.col_proxyType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.col_proxyType.Width = 80;
            // 
            // col_username
            // 
            this.col_username.Text = "代理账号";
            this.col_username.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.col_username.Width = 120;
            // 
            // col_password
            // 
            this.col_password.Text = "代理密码";
            this.col_password.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.col_password.Width = 120;
            // 
            // col_conectIsOK
            // 
            this.col_conectIsOK.Text = "是否可用";
            this.col_conectIsOK.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.col_conectIsOK.Width = 70;
            // 
            // col_useTime
            // 
            this.col_useTime.Text = "用时[毫秒]";
            this.col_useTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.col_useTime.Width = 80;
            // 
            // col_checkTime
            // 
            this.col_checkTime.Text = "验证时间";
            this.col_checkTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.col_checkTime.Width = 130;
            // 
            // proxy_cms
            // 
            this.proxy_cms.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.proxy_tsmi_setCurrentProxy,
            this.proxy_checkNoCheckProxy,
            this.proxy_checkSelectedProxy,
            this.proxy_checkAllProxy,
            this.proxy_importProxy,
            this.proxy_exportProxy,
            this.proxy_delSelectedProxy,
            this.proxy_copySelectedProxy,
            this.proxy_clearAllProxy,
            this.proxy_clearAllFailedProxy});
            this.proxy_cms.Name = "proxy_cms";
            this.proxy_cms.Size = new System.Drawing.Size(161, 224);
            // 
            // proxy_tsmi_setCurrentProxy
            // 
            this.proxy_tsmi_setCurrentProxy.Name = "proxy_tsmi_setCurrentProxy";
            this.proxy_tsmi_setCurrentProxy.Size = new System.Drawing.Size(160, 22);
            this.proxy_tsmi_setCurrentProxy.Text = "设置固定代理";
            this.proxy_tsmi_setCurrentProxy.Click += new System.EventHandler(this.proxy_tsmi_setCurrentProxy_Click);
            // 
            // proxy_checkNoCheckProxy
            // 
            this.proxy_checkNoCheckProxy.Name = "proxy_checkNoCheckProxy";
            this.proxy_checkNoCheckProxy.Size = new System.Drawing.Size(160, 22);
            this.proxy_checkNoCheckProxy.Text = "验证未验证代理";
            this.proxy_checkNoCheckProxy.Click += new System.EventHandler(this.proxy_checkNoCheckProxy_Click);
            // 
            // proxy_checkSelectedProxy
            // 
            this.proxy_checkSelectedProxy.Name = "proxy_checkSelectedProxy";
            this.proxy_checkSelectedProxy.Size = new System.Drawing.Size(160, 22);
            this.proxy_checkSelectedProxy.Text = "验证选中代理";
            this.proxy_checkSelectedProxy.Click += new System.EventHandler(this.proxy_checkSelectedProxy_Click);
            // 
            // proxy_checkAllProxy
            // 
            this.proxy_checkAllProxy.Name = "proxy_checkAllProxy";
            this.proxy_checkAllProxy.Size = new System.Drawing.Size(160, 22);
            this.proxy_checkAllProxy.Text = "验证全部代理";
            this.proxy_checkAllProxy.Click += new System.EventHandler(this.proxy_checkAllProxy_Click);
            // 
            // proxy_importProxy
            // 
            this.proxy_importProxy.Name = "proxy_importProxy";
            this.proxy_importProxy.Size = new System.Drawing.Size(160, 22);
            this.proxy_importProxy.Text = "导入代理";
            this.proxy_importProxy.Click += new System.EventHandler(this.proxy_importProxy_Click);
            // 
            // proxy_exportProxy
            // 
            this.proxy_exportProxy.Name = "proxy_exportProxy";
            this.proxy_exportProxy.Size = new System.Drawing.Size(160, 22);
            this.proxy_exportProxy.Text = "导出代理";
            this.proxy_exportProxy.Click += new System.EventHandler(this.proxy_exportProxy_Click);
            // 
            // proxy_delSelectedProxy
            // 
            this.proxy_delSelectedProxy.Name = "proxy_delSelectedProxy";
            this.proxy_delSelectedProxy.Size = new System.Drawing.Size(160, 22);
            this.proxy_delSelectedProxy.Text = "删除此代理";
            this.proxy_delSelectedProxy.Click += new System.EventHandler(this.proxy_delSelectedProxy_Click);
            // 
            // proxy_copySelectedProxy
            // 
            this.proxy_copySelectedProxy.Name = "proxy_copySelectedProxy";
            this.proxy_copySelectedProxy.Size = new System.Drawing.Size(160, 22);
            this.proxy_copySelectedProxy.Text = "复制此代理";
            this.proxy_copySelectedProxy.Click += new System.EventHandler(this.proxy_copySelectedProxy_Click);
            // 
            // proxy_clearAllProxy
            // 
            this.proxy_clearAllProxy.Name = "proxy_clearAllProxy";
            this.proxy_clearAllProxy.Size = new System.Drawing.Size(160, 22);
            this.proxy_clearAllProxy.Text = "清空代理池";
            this.proxy_clearAllProxy.Click += new System.EventHandler(this.proxy_clearAllProxy_Click);
            // 
            // proxy_clearAllFailedProxy
            // 
            this.proxy_clearAllFailedProxy.Name = "proxy_clearAllFailedProxy";
            this.proxy_clearAllFailedProxy.Size = new System.Drawing.Size(160, 22);
            this.proxy_clearAllFailedProxy.Text = "清除无效代理";
            this.proxy_clearAllFailedProxy.Click += new System.EventHandler(this.proxy_clearAllFailedProxy_Click);
            // 
            // tab_cmd
            // 
            this.tab_cmd.BackColor = System.Drawing.SystemColors.Window;
            this.tab_cmd.Controls.Add(this.cmd_txt_result);
            this.tab_cmd.Controls.Add(this.groupBox8);
            this.tab_cmd.ImageKey = "cmd.png";
            this.tab_cmd.Location = new System.Drawing.Point(4, 32);
            this.tab_cmd.Name = "tab_cmd";
            this.tab_cmd.Size = new System.Drawing.Size(832, 416);
            this.tab_cmd.TabIndex = 6;
            this.tab_cmd.Text = "命令执行";
            // 
            // cmd_txt_result
            // 
            this.cmd_txt_result.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmd_txt_result.Location = new System.Drawing.Point(3, 92);
            this.cmd_txt_result.MaxLength = 3276700;
            this.cmd_txt_result.Multiline = true;
            this.cmd_txt_result.Name = "cmd_txt_result";
            this.cmd_txt_result.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.cmd_txt_result.Size = new System.Drawing.Size(826, 321);
            this.cmd_txt_result.TabIndex = 2;
            this.cmd_txt_result.TextChanged += new System.EventHandler(this.cmd_txt_result_TextChanged);
            this.cmd_txt_result.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmd_txt_result_KeyDown);
            // 
            // groupBox8
            // 
            this.groupBox8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox8.Controls.Add(this.cmd_chk_showCmdResult);
            this.groupBox8.Controls.Add(this.cmd_btn_stop);
            this.groupBox8.Controls.Add(this.cmd_btn_start);
            this.groupBox8.Controls.Add(this.cmd_txt_cmd);
            this.groupBox8.Controls.Add(this.label15);
            this.groupBox8.Location = new System.Drawing.Point(3, 10);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(826, 73);
            this.groupBox8.TabIndex = 1;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "命令执行";
            // 
            // cmd_chk_showCmdResult
            // 
            this.cmd_chk_showCmdResult.AutoSize = true;
            this.cmd_chk_showCmdResult.Checked = true;
            this.cmd_chk_showCmdResult.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cmd_chk_showCmdResult.Location = new System.Drawing.Point(502, 32);
            this.cmd_chk_showCmdResult.Name = "cmd_chk_showCmdResult";
            this.cmd_chk_showCmdResult.Size = new System.Drawing.Size(72, 16);
            this.cmd_chk_showCmdResult.TabIndex = 13;
            this.cmd_chk_showCmdResult.Text = "回显结果";
            this.cmd_chk_showCmdResult.UseVisualStyleBackColor = true;
            this.cmd_chk_showCmdResult.CheckedChanged += new System.EventHandler(this.cmd_chk_showCmdResult_CheckedChanged);
            // 
            // cmd_btn_stop
            // 
            this.cmd_btn_stop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmd_btn_stop.Location = new System.Drawing.Point(711, 28);
            this.cmd_btn_stop.Name = "cmd_btn_stop";
            this.cmd_btn_stop.Size = new System.Drawing.Size(100, 23);
            this.cmd_btn_stop.TabIndex = 12;
            this.cmd_btn_stop.Text = "停止";
            this.cmd_btn_stop.UseVisualStyleBackColor = true;
            this.cmd_btn_stop.Click += new System.EventHandler(this.cmd_btn_stop_Click);
            // 
            // cmd_btn_start
            // 
            this.cmd_btn_start.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmd_btn_start.Location = new System.Drawing.Point(580, 28);
            this.cmd_btn_start.Name = "cmd_btn_start";
            this.cmd_btn_start.Size = new System.Drawing.Size(100, 23);
            this.cmd_btn_start.TabIndex = 12;
            this.cmd_btn_start.Text = "执行";
            this.cmd_btn_start.UseVisualStyleBackColor = true;
            this.cmd_btn_start.Click += new System.EventHandler(this.cmd_btn_start_Click);
            // 
            // cmd_txt_cmd
            // 
            this.cmd_txt_cmd.Location = new System.Drawing.Point(81, 30);
            this.cmd_txt_cmd.MaxLength = 8000;
            this.cmd_txt_cmd.Name = "cmd_txt_cmd";
            this.cmd_txt_cmd.Size = new System.Drawing.Size(401, 21);
            this.cmd_txt_cmd.TabIndex = 1;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(29, 32);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(47, 12);
            this.label15.TabIndex = 0;
            this.label15.Text = "命 令：";
            // 
            // tab_bypass
            // 
            this.tab_bypass.BackColor = System.Drawing.SystemColors.Window;
            this.tab_bypass.Controls.Add(this.groupBox9);
            this.tab_bypass.ImageKey = "bypass.png";
            this.tab_bypass.Location = new System.Drawing.Point(4, 32);
            this.tab_bypass.Name = "tab_bypass";
            this.tab_bypass.Size = new System.Drawing.Size(832, 416);
            this.tab_bypass.TabIndex = 7;
            this.tab_bypass.Text = "注入绕过";
            // 
            // groupBox9
            // 
            this.groupBox9.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox9.Controls.Add(this.groupBox18);
            this.groupBox9.Controls.Add(this.groupBox23);
            this.groupBox9.Controls.Add(this.groupBox22);
            this.groupBox9.Location = new System.Drawing.Point(3, 8);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(826, 414);
            this.groupBox9.TabIndex = 1;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "注入绕过处理";
            // 
            // groupBox18
            // 
            this.groupBox18.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox18.Controls.Add(this.bypass_btn_saveTemplate);
            this.groupBox18.Controls.Add(this.bypass_cbox_loadTemplate);
            this.groupBox18.Controls.Add(this.label33);
            this.groupBox18.Location = new System.Drawing.Point(9, 353);
            this.groupBox18.Name = "groupBox18";
            this.groupBox18.Size = new System.Drawing.Size(811, 51);
            this.groupBox18.TabIndex = 25;
            this.groupBox18.TabStop = false;
            // 
            // bypass_btn_saveTemplate
            // 
            this.bypass_btn_saveTemplate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bypass_btn_saveTemplate.Location = new System.Drawing.Point(679, 15);
            this.bypass_btn_saveTemplate.Name = "bypass_btn_saveTemplate";
            this.bypass_btn_saveTemplate.Size = new System.Drawing.Size(125, 23);
            this.bypass_btn_saveTemplate.TabIndex = 21;
            this.bypass_btn_saveTemplate.Text = "保存绕过模板";
            this.bypass_btn_saveTemplate.UseVisualStyleBackColor = true;
            this.bypass_btn_saveTemplate.Click += new System.EventHandler(this.bypass_btn_saveTemplate_Click);
            // 
            // bypass_cbox_loadTemplate
            // 
            this.bypass_cbox_loadTemplate.FormattingEnabled = true;
            this.bypass_cbox_loadTemplate.Items.AddRange(new object[] {
            "我要自己选择"});
            this.bypass_cbox_loadTemplate.Location = new System.Drawing.Point(115, 18);
            this.bypass_cbox_loadTemplate.Name = "bypass_cbox_loadTemplate";
            this.bypass_cbox_loadTemplate.Size = new System.Drawing.Size(527, 20);
            this.bypass_cbox_loadTemplate.TabIndex = 20;
            this.bypass_cbox_loadTemplate.TextChanged += new System.EventHandler(this.bypass_cbox_loadTemplate_TextChanged);
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(19, 21);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(89, 12);
            this.label33.TabIndex = 13;
            this.label33.Text = "选择绕过模板：";
            // 
            // groupBox23
            // 
            this.groupBox23.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox23.Controls.Add(this.chk_reaplaceBeforURLEncode);
            this.groupBox23.Controls.Add(this.label19);
            this.groupBox23.Controls.Add(this.bypass_btn_addReplaceStr);
            this.groupBox23.Controls.Add(this.label16);
            this.groupBox23.Controls.Add(this.bypass_lvw_replaceString);
            this.groupBox23.Controls.Add(this.bypass_txt_replaceTo);
            this.groupBox23.Controls.Add(this.bypass_txt_replace);
            this.groupBox23.Location = new System.Drawing.Point(9, 124);
            this.groupBox23.Name = "groupBox23";
            this.groupBox23.Size = new System.Drawing.Size(811, 225);
            this.groupBox23.TabIndex = 27;
            this.groupBox23.TabStop = false;
            this.groupBox23.Text = "字符替换处理";
            // 
            // chk_reaplaceBeforURLEncode
            // 
            this.chk_reaplaceBeforURLEncode.AutoSize = true;
            this.chk_reaplaceBeforURLEncode.Location = new System.Drawing.Point(23, 28);
            this.chk_reaplaceBeforURLEncode.Name = "chk_reaplaceBeforURLEncode";
            this.chk_reaplaceBeforURLEncode.Size = new System.Drawing.Size(234, 16);
            this.chk_reaplaceBeforURLEncode.TabIndex = 18;
            this.chk_reaplaceBeforURLEncode.Text = "在URL或Unicoded等编码前处理绕过字符";
            this.chk_reaplaceBeforURLEncode.UseVisualStyleBackColor = true;
            this.chk_reaplaceBeforURLEncode.CheckedChanged += new System.EventHandler(this.chk_reaplaceBeforURLEncode_CheckedChanged);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(281, 29);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(41, 12);
            this.label19.TabIndex = 5;
            this.label19.Text = "将字符";
            // 
            // bypass_btn_addReplaceStr
            // 
            this.bypass_btn_addReplaceStr.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bypass_btn_addReplaceStr.Location = new System.Drawing.Point(727, 24);
            this.bypass_btn_addReplaceStr.Name = "bypass_btn_addReplaceStr";
            this.bypass_btn_addReplaceStr.Size = new System.Drawing.Size(78, 23);
            this.bypass_btn_addReplaceStr.TabIndex = 6;
            this.bypass_btn_addReplaceStr.Text = "添加";
            this.bypass_btn_addReplaceStr.UseVisualStyleBackColor = true;
            this.bypass_btn_addReplaceStr.Click += new System.EventHandler(this.bypass_btn_addReplaceStr_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(498, 30);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(41, 12);
            this.label16.TabIndex = 10;
            this.label16.Text = "替换成";
            // 
            // bypass_lvw_replaceString
            // 
            this.bypass_lvw_replaceString.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bypass_lvw_replaceString.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.col_replace,
            this.col_replaceTo});
            this.bypass_lvw_replaceString.ContextMenuStrip = this.bypass_lvw_replaceString_cms;
            this.bypass_lvw_replaceString.FullRowSelect = true;
            this.bypass_lvw_replaceString.HideSelection = false;
            this.bypass_lvw_replaceString.Location = new System.Drawing.Point(7, 64);
            this.bypass_lvw_replaceString.Name = "bypass_lvw_replaceString";
            this.bypass_lvw_replaceString.Size = new System.Drawing.Size(798, 154);
            this.bypass_lvw_replaceString.SmallImageList = this.img_line;
            this.bypass_lvw_replaceString.TabIndex = 7;
            this.bypass_lvw_replaceString.UseCompatibleStateImageBehavior = false;
            this.bypass_lvw_replaceString.View = System.Windows.Forms.View.Details;
            // 
            // col_replace
            // 
            this.col_replace.Text = "替换字符";
            this.col_replace.Width = 380;
            // 
            // col_replaceTo
            // 
            this.col_replaceTo.Text = "目标字符";
            this.col_replaceTo.Width = 380;
            // 
            // bypass_lvw_replaceString_cms
            // 
            this.bypass_lvw_replaceString_cms.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bypass_tsmi_delselect,
            this.bypass_tsmi_copy});
            this.bypass_lvw_replaceString_cms.Name = "data_cms_getVariable";
            this.bypass_lvw_replaceString_cms.Size = new System.Drawing.Size(125, 48);
            // 
            // bypass_tsmi_delselect
            // 
            this.bypass_tsmi_delselect.Name = "bypass_tsmi_delselect";
            this.bypass_tsmi_delselect.Size = new System.Drawing.Size(124, 22);
            this.bypass_tsmi_delselect.Text = "删除选中";
            this.bypass_tsmi_delselect.Click += new System.EventHandler(this.bypass_tsmi_delselect_Click);
            // 
            // bypass_tsmi_copy
            // 
            this.bypass_tsmi_copy.Name = "bypass_tsmi_copy";
            this.bypass_tsmi_copy.Size = new System.Drawing.Size(124, 22);
            this.bypass_tsmi_copy.Text = "复制";
            this.bypass_tsmi_copy.Click += new System.EventHandler(this.bypass_tsmi_copy_Click);
            // 
            // bypass_txt_replaceTo
            // 
            this.bypass_txt_replaceTo.Location = new System.Drawing.Point(547, 26);
            this.bypass_txt_replaceTo.MaxLength = 500;
            this.bypass_txt_replaceTo.Name = "bypass_txt_replaceTo";
            this.bypass_txt_replaceTo.Size = new System.Drawing.Size(160, 21);
            this.bypass_txt_replaceTo.TabIndex = 9;
            // 
            // bypass_txt_replace
            // 
            this.bypass_txt_replace.AcceptsReturn = true;
            this.bypass_txt_replace.Location = new System.Drawing.Point(329, 26);
            this.bypass_txt_replace.MaxLength = 500;
            this.bypass_txt_replace.Name = "bypass_txt_replace";
            this.bypass_txt_replace.Size = new System.Drawing.Size(160, 21);
            this.bypass_txt_replace.TabIndex = 8;
            // 
            // groupBox22
            // 
            this.groupBox22.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox22.Controls.Add(this.cbox_bypass_urlencode_count);
            this.groupBox22.Controls.Add(this.label17);
            this.groupBox22.Controls.Add(this.cbox_base64Count);
            this.groupBox22.Controls.Add(this.bypass_chk_inculdeStr);
            this.groupBox22.Controls.Add(this.bypass_chk_usebetween);
            this.groupBox22.Controls.Add(this.cob_keyRepalce);
            this.groupBox22.Controls.Add(this.bypass_cbox_sendHTTPSleepTime);
            this.groupBox22.Controls.Add(this.label10);
            this.groupBox22.Controls.Add(this.label18);
            this.groupBox22.Controls.Add(this.label32);
            this.groupBox22.Controls.Add(this.label31);
            this.groupBox22.Controls.Add(this.bypass_cbox_randIPToHeader);
            this.groupBox22.Controls.Add(this.bypass_chk_use_unicode);
            this.groupBox22.Controls.Add(this.bypass_hex);
            this.groupBox22.Location = new System.Drawing.Point(9, 20);
            this.groupBox22.Name = "groupBox22";
            this.groupBox22.Size = new System.Drawing.Size(811, 96);
            this.groupBox22.TabIndex = 26;
            this.groupBox22.TabStop = false;
            // 
            // cbox_bypass_urlencode_count
            // 
            this.cbox_bypass_urlencode_count.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbox_bypass_urlencode_count.FormattingEnabled = true;
            this.cbox_bypass_urlencode_count.Items.AddRange(new object[] {
            "一次",
            "二次"});
            this.cbox_bypass_urlencode_count.Location = new System.Drawing.Point(73, 20);
            this.cbox_bypass_urlencode_count.Name = "cbox_bypass_urlencode_count";
            this.cbox_bypass_urlencode_count.Size = new System.Drawing.Size(83, 20);
            this.cbox_bypass_urlencode_count.TabIndex = 23;
            this.cbox_bypass_urlencode_count.TextChanged += new System.EventHandler(this.cbox_bypass_urlencode_count_TextChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(363, 58);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(101, 12);
            this.label17.TabIndex = 12;
            this.label17.Text = "发包延时[毫秒]：";
            // 
            // cbox_base64Count
            // 
            this.cbox_base64Count.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbox_base64Count.FormattingEnabled = true;
            this.cbox_base64Count.Items.AddRange(new object[] {
            "选择",
            "一次",
            "二次",
            "三次"});
            this.cbox_base64Count.Location = new System.Drawing.Point(265, 54);
            this.cbox_base64Count.Name = "cbox_base64Count";
            this.cbox_base64Count.Size = new System.Drawing.Size(67, 20);
            this.cbox_base64Count.TabIndex = 22;
            this.cbox_base64Count.SelectedIndexChanged += new System.EventHandler(this.cbox_base64Count_SelectedIndexChanged);
            this.cbox_base64Count.TextChanged += new System.EventHandler(this.cbox_base64Count_TextChanged);
            // 
            // bypass_chk_inculdeStr
            // 
            this.bypass_chk_inculdeStr.AutoSize = true;
            this.bypass_chk_inculdeStr.Location = new System.Drawing.Point(649, 22);
            this.bypass_chk_inculdeStr.Name = "bypass_chk_inculdeStr";
            this.bypass_chk_inculdeStr.Size = new System.Drawing.Size(156, 16);
            this.bypass_chk_inculdeStr.TabIndex = 3;
            this.bypass_chk_inculdeStr.Text = "MySQL/*!xx*/包含关键字";
            this.bypass_chk_inculdeStr.UseVisualStyleBackColor = true;
            this.bypass_chk_inculdeStr.CheckedChanged += new System.EventHandler(this.bypass_chk_inculdeStr_CheckedChanged);
            // 
            // bypass_chk_usebetween
            // 
            this.bypass_chk_usebetween.AutoSize = true;
            this.bypass_chk_usebetween.Location = new System.Drawing.Point(469, 22);
            this.bypass_chk_usebetween.Name = "bypass_chk_usebetween";
            this.bypass_chk_usebetween.Size = new System.Drawing.Size(162, 16);
            this.bypass_chk_usebetween.TabIndex = 3;
            this.bypass_chk_usebetween.Text = "between and替换大于等于";
            this.bypass_chk_usebetween.UseVisualStyleBackColor = true;
            this.bypass_chk_usebetween.CheckedChanged += new System.EventHandler(this.bypass_chk_usebetween_CheckedChanged);
            // 
            // cob_keyRepalce
            // 
            this.cob_keyRepalce.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cob_keyRepalce.FormattingEnabled = true;
            this.cob_keyRepalce.Items.AddRange(new object[] {
            "不处理",
            "随机大小写",
            "关键字大写",
            "关键字小写"});
            this.cob_keyRepalce.Location = new System.Drawing.Point(73, 55);
            this.cob_keyRepalce.Name = "cob_keyRepalce";
            this.cob_keyRepalce.Size = new System.Drawing.Size(83, 20);
            this.cob_keyRepalce.TabIndex = 17;
            this.cob_keyRepalce.SelectedIndexChanged += new System.EventHandler(this.cob_keyRepalce_SelectedIndexChanged);
            // 
            // bypass_cbox_sendHTTPSleepTime
            // 
            this.bypass_cbox_sendHTTPSleepTime.FormattingEnabled = true;
            this.bypass_cbox_sendHTTPSleepTime.Items.AddRange(new object[] {
            "0",
            "100",
            "200",
            "300",
            "400",
            "500",
            "600",
            "700",
            "800",
            "900",
            "1000",
            "2000",
            "3000",
            "4000",
            "5000",
            "6000",
            "7000",
            "8000",
            "9000",
            "10000"});
            this.bypass_cbox_sendHTTPSleepTime.Location = new System.Drawing.Point(469, 54);
            this.bypass_cbox_sendHTTPSleepTime.Name = "bypass_cbox_sendHTTPSleepTime";
            this.bypass_cbox_sendHTTPSleepTime.Size = new System.Drawing.Size(70, 20);
            this.bypass_cbox_sendHTTPSleepTime.TabIndex = 11;
            this.bypass_cbox_sendHTTPSleepTime.TextChanged += new System.EventHandler(this.bypass_cbox_sendHTTPSleepTime_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(17, 58);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 16;
            this.label10.Text = "关键字：";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(577, 58);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(65, 12);
            this.label18.TabIndex = 13;
            this.label18.Text = "随机IP头：";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(11, 23);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(59, 12);
            this.label32.TabIndex = 13;
            this.label32.Text = "URL编码：";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(181, 58);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(77, 12);
            this.label31.TabIndex = 13;
            this.label31.Text = "Base64编码：";
            // 
            // bypass_cbox_randIPToHeader
            // 
            this.bypass_cbox_randIPToHeader.FormattingEnabled = true;
            this.bypass_cbox_randIPToHeader.Items.AddRange(new object[] {
            "",
            "X-Forwarded-For",
            "Remote-Addr",
            "Client-Ip"});
            this.bypass_cbox_randIPToHeader.Location = new System.Drawing.Point(649, 54);
            this.bypass_cbox_randIPToHeader.Name = "bypass_cbox_randIPToHeader";
            this.bypass_cbox_randIPToHeader.Size = new System.Drawing.Size(155, 20);
            this.bypass_cbox_randIPToHeader.TabIndex = 14;
            this.bypass_cbox_randIPToHeader.TextChanged += new System.EventHandler(this.bypass_cbox_randIPToHeader_TextChanged);
            // 
            // bypass_chk_use_unicode
            // 
            this.bypass_chk_use_unicode.AutoSize = true;
            this.bypass_chk_use_unicode.Location = new System.Drawing.Point(185, 22);
            this.bypass_chk_use_unicode.Name = "bypass_chk_use_unicode";
            this.bypass_chk_use_unicode.Size = new System.Drawing.Size(114, 16);
            this.bypass_chk_use_unicode.TabIndex = 3;
            this.bypass_chk_use_unicode.Text = "IIS Unicode编码";
            this.bypass_chk_use_unicode.UseVisualStyleBackColor = true;
            this.bypass_chk_use_unicode.CheckedChanged += new System.EventHandler(this.bypass_chk_use_unicode_CheckedChanged);
            // 
            // bypass_hex
            // 
            this.bypass_hex.AutoSize = true;
            this.bypass_hex.Location = new System.Drawing.Point(341, 22);
            this.bypass_hex.Name = "bypass_hex";
            this.bypass_hex.Size = new System.Drawing.Size(102, 16);
            this.bypass_hex.TabIndex = 3;
            this.bypass_hex.Text = "Hex16进制编码";
            this.bypass_hex.UseVisualStyleBackColor = true;
            this.bypass_hex.CheckedChanged += new System.EventHandler(this.bypass_hex_CheckedChanged);
            // 
            // tab_encoding
            // 
            this.tab_encoding.BackColor = System.Drawing.SystemColors.Window;
            this.tab_encoding.Controls.Add(this.groupBox13);
            this.tab_encoding.Controls.Add(this.groupBox11);
            this.tab_encoding.Controls.Add(this.groupBox10);
            this.tab_encoding.ImageKey = "convert.png";
            this.tab_encoding.Location = new System.Drawing.Point(4, 32);
            this.tab_encoding.Name = "tab_encoding";
            this.tab_encoding.Size = new System.Drawing.Size(832, 416);
            this.tab_encoding.TabIndex = 5;
            this.tab_encoding.Text = "编码转换";
            // 
            // groupBox13
            // 
            this.groupBox13.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox13.Controls.Add(this.label21);
            this.groupBox13.Controls.Add(this.label23);
            this.groupBox13.Controls.Add(this.encode_cbox_encode);
            this.groupBox13.Controls.Add(this.encode_cbox_decode);
            this.groupBox13.Location = new System.Drawing.Point(6, 364);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(819, 53);
            this.groupBox13.TabIndex = 8;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "操作：";
            // 
            // label21
            // 
            this.label21.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(33, 25);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(65, 12);
            this.label21.TabIndex = 8;
            this.label21.Text = "编码方式：";
            // 
            // label23
            // 
            this.label23.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(363, 25);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(65, 12);
            this.label23.TabIndex = 9;
            this.label23.Text = "解码方式：";
            // 
            // encode_cbox_encode
            // 
            this.encode_cbox_encode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.encode_cbox_encode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.encode_cbox_encode.FormattingEnabled = true;
            this.encode_cbox_encode.Items.AddRange(new object[] {
            "选择编码格式",
            "URLEncode",
            "Base64Encode",
            "字符转Unicode",
            "字符转16进制（UTF-8编码）",
            "MD5加密",
            "字符串转chr",
            "字符串转char"});
            this.encode_cbox_encode.Location = new System.Drawing.Point(103, 20);
            this.encode_cbox_encode.Name = "encode_cbox_encode";
            this.encode_cbox_encode.Size = new System.Drawing.Size(199, 20);
            this.encode_cbox_encode.TabIndex = 4;
            this.encode_cbox_encode.SelectedIndexChanged += new System.EventHandler(this.encode_cbox_encode_SelectedIndexChanged);
            // 
            // encode_cbox_decode
            // 
            this.encode_cbox_decode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.encode_cbox_decode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.encode_cbox_decode.FormattingEnabled = true;
            this.encode_cbox_decode.Items.AddRange(new object[] {
            "选择解码格式",
            "URLDecode",
            "Base64Decode",
            "Unicode转字符",
            "16进制（UTF-8编码）转字符串",
            "MD5解密",
            "chr转字符",
            "char转字符"});
            this.encode_cbox_decode.Location = new System.Drawing.Point(435, 20);
            this.encode_cbox_decode.Name = "encode_cbox_decode";
            this.encode_cbox_decode.Size = new System.Drawing.Size(199, 20);
            this.encode_cbox_decode.TabIndex = 5;
            this.encode_cbox_decode.SelectedIndexChanged += new System.EventHandler(this.encode_cbox_decode_SelectedIndexChanged);
            // 
            // groupBox11
            // 
            this.groupBox11.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox11.Controls.Add(this.encode_txt_result);
            this.groupBox11.Location = new System.Drawing.Point(6, 171);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(819, 182);
            this.groupBox11.TabIndex = 7;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "结果：";
            // 
            // encode_txt_result
            // 
            this.encode_txt_result.Dock = System.Windows.Forms.DockStyle.Fill;
            this.encode_txt_result.Location = new System.Drawing.Point(3, 17);
            this.encode_txt_result.MaxLength = 3276700;
            this.encode_txt_result.Multiline = true;
            this.encode_txt_result.Name = "encode_txt_result";
            this.encode_txt_result.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.encode_txt_result.Size = new System.Drawing.Size(813, 162);
            this.encode_txt_result.TabIndex = 2;
            this.encode_txt_result.KeyDown += new System.Windows.Forms.KeyEventHandler(this.encode_txt_result_KeyDown);
            // 
            // groupBox10
            // 
            this.groupBox10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox10.Controls.Add(this.encode_txt_input);
            this.groupBox10.Location = new System.Drawing.Point(6, 8);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(819, 157);
            this.groupBox10.TabIndex = 6;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "输入：";
            // 
            // encode_txt_input
            // 
            this.encode_txt_input.Dock = System.Windows.Forms.DockStyle.Fill;
            this.encode_txt_input.Location = new System.Drawing.Point(3, 17);
            this.encode_txt_input.MaxLength = 3276700;
            this.encode_txt_input.Multiline = true;
            this.encode_txt_input.Name = "encode_txt_input";
            this.encode_txt_input.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.encode_txt_input.Size = new System.Drawing.Size(813, 137);
            this.encode_txt_input.TabIndex = 1;
            this.encode_txt_input.TextChanged += new System.EventHandler(this.encode_txt_encode_TextChanged);
            this.encode_txt_input.KeyDown += new System.Windows.Forms.KeyEventHandler(this.encode_txt_input_KeyDown);
            // 
            // tab_scanInjection
            // 
            this.tab_scanInjection.BackColor = System.Drawing.SystemColors.Window;
            this.tab_scanInjection.Controls.Add(this.groupBox14);
            this.tab_scanInjection.Controls.Add(this.groupBox12);
            this.tab_scanInjection.ImageKey = "scan.png";
            this.tab_scanInjection.Location = new System.Drawing.Point(4, 32);
            this.tab_scanInjection.Name = "tab_scanInjection";
            this.tab_scanInjection.Size = new System.Drawing.Size(832, 416);
            this.tab_scanInjection.TabIndex = 8;
            this.tab_scanInjection.Text = "注入扫描";
            // 
            // groupBox14
            // 
            this.groupBox14.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox14.Controls.Add(this.splitContainer2);
            this.groupBox14.Location = new System.Drawing.Point(6, 105);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Size = new System.Drawing.Size(820, 320);
            this.groupBox14.TabIndex = 21;
            this.groupBox14.TabStop = false;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 17);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.scanInject_lsb_links);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.scanInjection_lvw_result);
            this.splitContainer2.Size = new System.Drawing.Size(814, 300);
            this.splitContainer2.SplitterDistance = 245;
            this.splitContainer2.SplitterWidth = 3;
            this.splitContainer2.TabIndex = 18;
            // 
            // scanInject_lsb_links
            // 
            this.scanInject_lsb_links.ContextMenuStrip = this.scanInjectionURL_cms;
            this.scanInject_lsb_links.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scanInject_lsb_links.FormattingEnabled = true;
            this.scanInject_lsb_links.ItemHeight = 12;
            this.scanInject_lsb_links.Location = new System.Drawing.Point(0, 0);
            this.scanInject_lsb_links.Name = "scanInject_lsb_links";
            this.scanInject_lsb_links.ScrollAlwaysVisible = true;
            this.scanInject_lsb_links.Size = new System.Drawing.Size(245, 300);
            this.scanInject_lsb_links.TabIndex = 0;
            // 
            // scanInjectionURL_cms
            // 
            this.scanInjectionURL_cms.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_exportScanInjectionURL,
            this.tsmi_clearScanInjectionURL});
            this.scanInjectionURL_cms.Name = "scanInjectionURL_cms";
            this.scanInjectionURL_cms.Size = new System.Drawing.Size(124, 48);
            // 
            // tsmi_exportScanInjectionURL
            // 
            this.tsmi_exportScanInjectionURL.Name = "tsmi_exportScanInjectionURL";
            this.tsmi_exportScanInjectionURL.Size = new System.Drawing.Size(123, 22);
            this.tsmi_exportScanInjectionURL.Text = "导出URL";
            this.tsmi_exportScanInjectionURL.Click += new System.EventHandler(this.tsmi_exportScanInjectionURL_Click);
            // 
            // tsmi_clearScanInjectionURL
            // 
            this.tsmi_clearScanInjectionURL.Name = "tsmi_clearScanInjectionURL";
            this.tsmi_clearScanInjectionURL.Size = new System.Drawing.Size(123, 22);
            this.tsmi_clearScanInjectionURL.Text = "清 空";
            this.tsmi_clearScanInjectionURL.Click += new System.EventHandler(this.tsmi_clearScanInjectionURL_Click);
            // 
            // scanInjection_lvw_result
            // 
            this.scanInjection_lvw_result.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.col_index,
            this.col_url,
            this.col_testURL,
            this.col_param,
            this.col_injectionType,
            this.col_injectionDB,
            this.col_mark});
            this.scanInjection_lvw_result.ContextMenuStrip = this.scanInjection_cms;
            this.scanInjection_lvw_result.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scanInjection_lvw_result.FullRowSelect = true;
            this.scanInjection_lvw_result.GridLines = true;
            this.scanInjection_lvw_result.HideSelection = false;
            this.scanInjection_lvw_result.Location = new System.Drawing.Point(0, 0);
            this.scanInjection_lvw_result.Name = "scanInjection_lvw_result";
            this.scanInjection_lvw_result.Size = new System.Drawing.Size(566, 300);
            this.scanInjection_lvw_result.SmallImageList = this.img_line;
            this.scanInjection_lvw_result.TabIndex = 5;
            this.scanInjection_lvw_result.UseCompatibleStateImageBehavior = false;
            this.scanInjection_lvw_result.View = System.Windows.Forms.View.Details;
            this.scanInjection_lvw_result.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.scanInjection_lvw_result_ColumnClick);
            this.scanInjection_lvw_result.DoubleClick += new System.EventHandler(this.scanInjection_lvw_result_DoubleClick);
            // 
            // col_index
            // 
            this.col_index.Text = "序号";
            this.col_index.Width = 46;
            // 
            // col_url
            // 
            this.col_url.Text = "原始URL";
            this.col_url.Width = 100;
            // 
            // col_testURL
            // 
            this.col_testURL.Text = "测试URL";
            this.col_testURL.Width = 150;
            // 
            // col_param
            // 
            this.col_param.Text = "注入参数";
            // 
            // col_injectionType
            // 
            this.col_injectionType.Text = "注入类型";
            this.col_injectionType.Width = 65;
            // 
            // col_injectionDB
            // 
            this.col_injectionDB.Text = "数据库类型";
            this.col_injectionDB.Width = 75;
            // 
            // col_mark
            // 
            this.col_mark.Text = "备注";
            // 
            // scanInjection_cms
            // 
            this.scanInjection_cms.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scanInjection_cms_exportResult,
            this.tsmi_tsmi_exortTestURL,
            this.tsmi_tsmi_exortOldURL,
            this.scanInjection_cms_copyURL,
            this.scanInjection_cms_clearResult,
            this.scanInjection_cms_delThisLine,
            this.tsmi_openURL,
            this.tsmi_tsmi_opentestURL});
            this.scanInjection_cms.Name = "scanInjection_cms";
            this.scanInjection_cms.Size = new System.Drawing.Size(149, 180);
            // 
            // scanInjection_cms_exportResult
            // 
            this.scanInjection_cms_exportResult.Name = "scanInjection_cms_exportResult";
            this.scanInjection_cms_exportResult.Size = new System.Drawing.Size(148, 22);
            this.scanInjection_cms_exportResult.Text = "导出扫描结果";
            this.scanInjection_cms_exportResult.Click += new System.EventHandler(this.scanInjection_cms_exportResult_Click);
            // 
            // tsmi_tsmi_exortTestURL
            // 
            this.tsmi_tsmi_exortTestURL.Name = "tsmi_tsmi_exortTestURL";
            this.tsmi_tsmi_exortTestURL.Size = new System.Drawing.Size(148, 22);
            this.tsmi_tsmi_exortTestURL.Text = "导出测试URL";
            this.tsmi_tsmi_exortTestURL.Click += new System.EventHandler(this.tsmi_tsmi_exortTestURL_Click);
            // 
            // tsmi_tsmi_exortOldURL
            // 
            this.tsmi_tsmi_exortOldURL.Name = "tsmi_tsmi_exortOldURL";
            this.tsmi_tsmi_exortOldURL.Size = new System.Drawing.Size(148, 22);
            this.tsmi_tsmi_exortOldURL.Text = "导出原始URL";
            this.tsmi_tsmi_exortOldURL.Click += new System.EventHandler(this.tsmi_tsmi_exortOldURL_Click);
            // 
            // scanInjection_cms_copyURL
            // 
            this.scanInjection_cms_copyURL.Name = "scanInjection_cms_copyURL";
            this.scanInjection_cms_copyURL.Size = new System.Drawing.Size(148, 22);
            this.scanInjection_cms_copyURL.Text = "复制URL";
            this.scanInjection_cms_copyURL.Click += new System.EventHandler(this.scanInjection_cms_copyURL_Click);
            // 
            // scanInjection_cms_clearResult
            // 
            this.scanInjection_cms_clearResult.Name = "scanInjection_cms_clearResult";
            this.scanInjection_cms_clearResult.Size = new System.Drawing.Size(148, 22);
            this.scanInjection_cms_clearResult.Text = "清空结果";
            this.scanInjection_cms_clearResult.Click += new System.EventHandler(this.scanInjection_cms_clearResult_Click);
            // 
            // scanInjection_cms_delThisLine
            // 
            this.scanInjection_cms_delThisLine.Name = "scanInjection_cms_delThisLine";
            this.scanInjection_cms_delThisLine.Size = new System.Drawing.Size(148, 22);
            this.scanInjection_cms_delThisLine.Text = "删除选中行";
            this.scanInjection_cms_delThisLine.Click += new System.EventHandler(this.scanInjection_cms_delThisLine_Click);
            // 
            // tsmi_openURL
            // 
            this.tsmi_openURL.Name = "tsmi_openURL";
            this.tsmi_openURL.Size = new System.Drawing.Size(148, 22);
            this.tsmi_openURL.Text = "打开原始URL";
            this.tsmi_openURL.Click += new System.EventHandler(this.tsmi_openURL_Click);
            // 
            // tsmi_tsmi_opentestURL
            // 
            this.tsmi_tsmi_opentestURL.Name = "tsmi_tsmi_opentestURL";
            this.tsmi_tsmi_opentestURL.Size = new System.Drawing.Size(148, 22);
            this.tsmi_tsmi_opentestURL.Text = "打开测试URL";
            this.tsmi_tsmi_opentestURL.Click += new System.EventHandler(this.tsmi_tsmi_opentestURL_Click);
            // 
            // groupBox12
            // 
            this.groupBox12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox12.Controls.Add(this.scanInect_chk_isSpider);
            this.groupBox12.Controls.Add(this.scanInect_chk_scanError);
            this.groupBox12.Controls.Add(this.scanInjection_btn_spider);
            this.groupBox12.Controls.Add(this.scanInjection_btn_scan);
            this.groupBox12.Controls.Add(this.scanInjection_importDomains);
            this.groupBox12.Controls.Add(this.scanInjection_scanedURLSCount);
            this.groupBox12.Controls.Add(this.scanInjection_txt_domainsPath);
            this.groupBox12.Controls.Add(this.scanInjection_findURLSCount);
            this.groupBox12.Controls.Add(this.label20);
            this.groupBox12.Controls.Add(this.label30);
            this.groupBox12.Controls.Add(this.label22);
            this.groupBox12.Controls.Add(this.label26);
            this.groupBox12.Controls.Add(this.scanInjection_domainsCount);
            this.groupBox12.Controls.Add(this.scanInjection_scanedDomainCount);
            this.groupBox12.Controls.Add(this.label25);
            this.groupBox12.Location = new System.Drawing.Point(6, 7);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(820, 92);
            this.groupBox12.TabIndex = 21;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "注入扫描";
            // 
            // scanInect_chk_isSpider
            // 
            this.scanInect_chk_isSpider.AutoSize = true;
            this.scanInect_chk_isSpider.Checked = true;
            this.scanInect_chk_isSpider.CheckState = System.Windows.Forms.CheckState.Checked;
            this.scanInect_chk_isSpider.Location = new System.Drawing.Point(329, 26);
            this.scanInect_chk_isSpider.Name = "scanInect_chk_isSpider";
            this.scanInect_chk_isSpider.Size = new System.Drawing.Size(96, 16);
            this.scanInect_chk_isSpider.TabIndex = 19;
            this.scanInect_chk_isSpider.Text = "是否爬行一次";
            this.scanInect_chk_isSpider.UseVisualStyleBackColor = true;
            // 
            // scanInect_chk_scanError
            // 
            this.scanInect_chk_scanError.AutoSize = true;
            this.scanInect_chk_scanError.Location = new System.Drawing.Point(489, 25);
            this.scanInect_chk_scanError.Name = "scanInect_chk_scanError";
            this.scanInect_chk_scanError.Size = new System.Drawing.Size(84, 16);
            this.scanInect_chk_scanError.TabIndex = 20;
            this.scanInect_chk_scanError.Text = "只扫错误型";
            this.scanInect_chk_scanError.UseVisualStyleBackColor = true;
            // 
            // scanInjection_btn_spider
            // 
            this.scanInjection_btn_spider.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.scanInjection_btn_spider.Location = new System.Drawing.Point(709, 16);
            this.scanInjection_btn_spider.Name = "scanInjection_btn_spider";
            this.scanInjection_btn_spider.Size = new System.Drawing.Size(99, 23);
            this.scanInjection_btn_spider.TabIndex = 1;
            this.scanInjection_btn_spider.Text = "爬行链接";
            this.scanInjection_btn_spider.UseVisualStyleBackColor = true;
            this.scanInjection_btn_spider.Click += new System.EventHandler(this.scanInjection_btn_spider_Click);
            // 
            // scanInjection_btn_scan
            // 
            this.scanInjection_btn_scan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.scanInjection_btn_scan.Location = new System.Drawing.Point(709, 57);
            this.scanInjection_btn_scan.Name = "scanInjection_btn_scan";
            this.scanInjection_btn_scan.Size = new System.Drawing.Size(99, 23);
            this.scanInjection_btn_scan.TabIndex = 1;
            this.scanInjection_btn_scan.Text = "扫描注入";
            this.scanInjection_btn_scan.UseVisualStyleBackColor = true;
            this.scanInjection_btn_scan.Click += new System.EventHandler(this.scanInjection_btn_scan_Click);
            // 
            // scanInjection_importDomains
            // 
            this.scanInjection_importDomains.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.scanInjection_importDomains.Location = new System.Drawing.Point(183, 22);
            this.scanInjection_importDomains.Name = "scanInjection_importDomains";
            this.scanInjection_importDomains.Size = new System.Drawing.Size(61, 23);
            this.scanInjection_importDomains.TabIndex = 2;
            this.scanInjection_importDomains.Text = "导入";
            this.scanInjection_importDomains.UseVisualStyleBackColor = true;
            this.scanInjection_importDomains.Click += new System.EventHandler(this.scanInjection_importDomains_Click);
            // 
            // scanInjection_scanedURLSCount
            // 
            this.scanInjection_scanedURLSCount.AutoSize = true;
            this.scanInjection_scanedURLSCount.Location = new System.Drawing.Point(561, 66);
            this.scanInjection_scanedURLSCount.Name = "scanInjection_scanedURLSCount";
            this.scanInjection_scanedURLSCount.Size = new System.Drawing.Size(11, 12);
            this.scanInjection_scanedURLSCount.TabIndex = 16;
            this.scanInjection_scanedURLSCount.Text = "0";
            // 
            // scanInjection_txt_domainsPath
            // 
            this.scanInjection_txt_domainsPath.Location = new System.Drawing.Point(66, 23);
            this.scanInjection_txt_domainsPath.Name = "scanInjection_txt_domainsPath";
            this.scanInjection_txt_domainsPath.Size = new System.Drawing.Size(111, 21);
            this.scanInjection_txt_domainsPath.TabIndex = 3;
            // 
            // scanInjection_findURLSCount
            // 
            this.scanInjection_findURLSCount.AutoSize = true;
            this.scanInjection_findURLSCount.Location = new System.Drawing.Point(407, 66);
            this.scanInjection_findURLSCount.Name = "scanInjection_findURLSCount";
            this.scanInjection_findURLSCount.Size = new System.Drawing.Size(11, 12);
            this.scanInjection_findURLSCount.TabIndex = 16;
            this.scanInjection_findURLSCount.Text = "0";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(27, 26);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(35, 12);
            this.label20.TabIndex = 4;
            this.label20.Text = "URL：";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(489, 66);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(71, 12);
            this.label30.TabIndex = 15;
            this.label30.Text = "已扫描URL：";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(27, 66);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(95, 12);
            this.label22.TabIndex = 11;
            this.label22.Text = "域名或URL总数：";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(327, 66);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(71, 12);
            this.label26.TabIndex = 15;
            this.label26.Text = "待扫描URL：";
            // 
            // scanInjection_domainsCount
            // 
            this.scanInjection_domainsCount.AutoSize = true;
            this.scanInjection_domainsCount.Location = new System.Drawing.Point(127, 66);
            this.scanInjection_domainsCount.Name = "scanInjection_domainsCount";
            this.scanInjection_domainsCount.Size = new System.Drawing.Size(11, 12);
            this.scanInjection_domainsCount.TabIndex = 12;
            this.scanInjection_domainsCount.Text = "0";
            // 
            // scanInjection_scanedDomainCount
            // 
            this.scanInjection_scanedDomainCount.AutoSize = true;
            this.scanInjection_scanedDomainCount.Location = new System.Drawing.Point(233, 66);
            this.scanInjection_scanedDomainCount.Name = "scanInjection_scanedDomainCount";
            this.scanInjection_scanedDomainCount.Size = new System.Drawing.Size(11, 12);
            this.scanInjection_scanedDomainCount.TabIndex = 14;
            this.scanInjection_scanedDomainCount.Text = "0";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(177, 66);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(53, 12);
            this.label25.TabIndex = 13;
            this.label25.Text = "已爬行：";
            // 
            // tab_injectLog
            // 
            this.tab_injectLog.BackColor = System.Drawing.SystemColors.Window;
            this.tab_injectLog.Controls.Add(this.lvw_injectLog);
            this.tab_injectLog.ImageKey = "Ilog.png";
            this.tab_injectLog.Location = new System.Drawing.Point(4, 32);
            this.tab_injectLog.Name = "tab_injectLog";
            this.tab_injectLog.Padding = new System.Windows.Forms.Padding(3);
            this.tab_injectLog.Size = new System.Drawing.Size(832, 416);
            this.tab_injectLog.TabIndex = 10;
            this.tab_injectLog.Text = "注入记录";
            // 
            // lvw_injectLog
            // 
            this.lvw_injectLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.injectlog_col_ip,
            this.injectlog_col_port,
            this.injectlog_col_uri,
            this.injectlog_col_pname,
            this.injectlog_col_injectType,
            this.injectlog_col_dbType,
            this.injectlog_col_payload,
            this.injectlog_col_time});
            this.lvw_injectLog.ContextMenuStrip = this.injectLog_cm;
            this.lvw_injectLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvw_injectLog.FullRowSelect = true;
            this.lvw_injectLog.GridLines = true;
            this.lvw_injectLog.HideSelection = false;
            this.lvw_injectLog.Location = new System.Drawing.Point(3, 3);
            this.lvw_injectLog.Name = "lvw_injectLog";
            this.lvw_injectLog.Size = new System.Drawing.Size(826, 410);
            this.lvw_injectLog.SmallImageList = this.img_line;
            this.lvw_injectLog.TabIndex = 1;
            this.lvw_injectLog.UseCompatibleStateImageBehavior = false;
            this.lvw_injectLog.View = System.Windows.Forms.View.Details;
            // 
            // injectlog_col_ip
            // 
            this.injectlog_col_ip.Text = "IP";
            // 
            // injectlog_col_port
            // 
            this.injectlog_col_port.Text = "端口";
            // 
            // injectlog_col_uri
            // 
            this.injectlog_col_uri.Text = "资源路径";
            // 
            // injectlog_col_pname
            // 
            this.injectlog_col_pname.Text = "参数名称";
            this.injectlog_col_pname.Width = 82;
            // 
            // injectlog_col_injectType
            // 
            this.injectlog_col_injectType.Text = "注入类型";
            this.injectlog_col_injectType.Width = 96;
            // 
            // injectlog_col_dbType
            // 
            this.injectlog_col_dbType.Text = "数据库类型";
            this.injectlog_col_dbType.Width = 86;
            // 
            // injectlog_col_payload
            // 
            this.injectlog_col_payload.Text = "测试Payload";
            this.injectlog_col_payload.Width = 259;
            // 
            // injectlog_col_time
            // 
            this.injectlog_col_time.Text = "记录时间";
            this.injectlog_col_time.Width = 109;
            // 
            // injectLog_cm
            // 
            this.injectLog_cm.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_injectLog_useCLog,
            this.tsmi_injectLog_delSLog,
            this.tsmi_injectLog_clearAllLog});
            this.injectLog_cm.Name = "contextMenuStrip1";
            this.injectLog_cm.Size = new System.Drawing.Size(185, 70);
            // 
            // tsmi_injectLog_useCLog
            // 
            this.tsmi_injectLog_useCLog.Name = "tsmi_injectLog_useCLog";
            this.tsmi_injectLog_useCLog.Size = new System.Drawing.Size(184, 22);
            this.tsmi_injectLog_useCLog.Text = "选择此记录进行注入";
            this.tsmi_injectLog_useCLog.Click += new System.EventHandler(this.tsmi_injectLog_useCLog_Click);
            // 
            // tsmi_injectLog_delSLog
            // 
            this.tsmi_injectLog_delSLog.Name = "tsmi_injectLog_delSLog";
            this.tsmi_injectLog_delSLog.Size = new System.Drawing.Size(184, 22);
            this.tsmi_injectLog_delSLog.Text = "删除选择记录";
            this.tsmi_injectLog_delSLog.Click += new System.EventHandler(this.tsmi_injectLog_delSLog_Click);
            // 
            // tsmi_injectLog_clearAllLog
            // 
            this.tsmi_injectLog_clearAllLog.Name = "tsmi_injectLog_clearAllLog";
            this.tsmi_injectLog_clearAllLog.Size = new System.Drawing.Size(184, 22);
            this.tsmi_injectLog_clearAllLog.Text = "清空记录";
            this.tsmi_injectLog_clearAllLog.Click += new System.EventHandler(this.tsmi_injectLog_clearAllLog_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.status_lbl_time,
            this.toolStripStatusLabel2,
            this.status_lbl_threadStatus,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel4,
            this.status_lbl_dbsCount,
            this.toolStripStatusLabel5,
            this.status_lbl_tableCount,
            this.toolStripStatusLabel6,
            this.status_lbl_dataCount,
            this.toolStripStatusLabel7,
            this.status_lbl_runStatus,
            this.toolStripStatusLabel8,
            this.status_lbl_all_status,
            this.toolStripStatusLabel11,
            this.lbl_packsCount,
            this.lbl_info});
            this.statusStrip1.Location = new System.Drawing.Point(0, 707);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 15, 0);
            this.statusStrip1.Size = new System.Drawing.Size(861, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(68, 17);
            this.toolStripStatusLabel1.Text = "运行时间：";
            // 
            // status_lbl_time
            // 
            this.status_lbl_time.Name = "status_lbl_time";
            this.status_lbl_time.Size = new System.Drawing.Size(15, 17);
            this.status_lbl_time.Text = "0";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(68, 17);
            this.toolStripStatusLabel2.Text = "当前线程：";
            // 
            // status_lbl_threadStatus
            // 
            this.status_lbl_threadStatus.Name = "status_lbl_threadStatus";
            this.status_lbl_threadStatus.Size = new System.Drawing.Size(27, 17);
            this.status_lbl_threadStatus.Text = "0/0";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(56, 17);
            this.toolStripStatusLabel4.Text = "数据库：";
            // 
            // status_lbl_dbsCount
            // 
            this.status_lbl_dbsCount.Name = "status_lbl_dbsCount";
            this.status_lbl_dbsCount.Size = new System.Drawing.Size(15, 17);
            this.status_lbl_dbsCount.Text = "0";
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            this.toolStripStatusLabel5.Size = new System.Drawing.Size(56, 17);
            this.toolStripStatusLabel5.Text = "表数量：";
            // 
            // status_lbl_tableCount
            // 
            this.status_lbl_tableCount.Name = "status_lbl_tableCount";
            this.status_lbl_tableCount.Size = new System.Drawing.Size(15, 17);
            this.status_lbl_tableCount.Text = "0";
            // 
            // toolStripStatusLabel6
            // 
            this.toolStripStatusLabel6.Name = "toolStripStatusLabel6";
            this.toolStripStatusLabel6.Size = new System.Drawing.Size(68, 17);
            this.toolStripStatusLabel6.Text = "数据获取：";
            // 
            // status_lbl_dataCount
            // 
            this.status_lbl_dataCount.Name = "status_lbl_dataCount";
            this.status_lbl_dataCount.Size = new System.Drawing.Size(15, 17);
            this.status_lbl_dataCount.Text = "0";
            // 
            // toolStripStatusLabel7
            // 
            this.toolStripStatusLabel7.Name = "toolStripStatusLabel7";
            this.toolStripStatusLabel7.Size = new System.Drawing.Size(44, 17);
            this.toolStripStatusLabel7.Text = "状态：";
            // 
            // status_lbl_runStatus
            // 
            this.status_lbl_runStatus.Name = "status_lbl_runStatus";
            this.status_lbl_runStatus.Size = new System.Drawing.Size(44, 17);
            this.status_lbl_runStatus.Text = "未开始";
            // 
            // toolStripStatusLabel8
            // 
            this.toolStripStatusLabel8.Name = "toolStripStatusLabel8";
            this.toolStripStatusLabel8.Size = new System.Drawing.Size(44, 17);
            this.toolStripStatusLabel8.Text = "进度：";
            // 
            // status_lbl_all_status
            // 
            this.status_lbl_all_status.Name = "status_lbl_all_status";
            this.status_lbl_all_status.Size = new System.Drawing.Size(27, 17);
            this.status_lbl_all_status.Text = "0/0";
            // 
            // toolStripStatusLabel11
            // 
            this.toolStripStatusLabel11.Name = "toolStripStatusLabel11";
            this.toolStripStatusLabel11.Size = new System.Drawing.Size(56, 17);
            this.toolStripStatusLabel11.Text = "已发包：";
            // 
            // lbl_packsCount
            // 
            this.lbl_packsCount.Name = "lbl_packsCount";
            this.lbl_packsCount.Size = new System.Drawing.Size(15, 17);
            this.lbl_packsCount.Text = "0";
            // 
            // lbl_info
            // 
            this.lbl_info.Name = "lbl_info";
            this.lbl_info.Size = new System.Drawing.Size(0, 17);
            // 
            // timer_status
            // 
            this.timer_status.Enabled = true;
            this.timer_status.Interval = 1000;
            this.timer_status.Tick += new System.EventHandler(this.timer_status_Tick);
            // 
            // timer_scanInjection
            // 
            this.timer_scanInjection.Enabled = true;
            this.timer_scanInjection.Interval = 1000;
            this.timer_scanInjection.Tick += new System.EventHandler(this.timer_scanInjection_Tick);
            // 
            // menuStrip_main
            // 
            this.menuStrip_main.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.menuStrip_main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_menu,
            this.tsmi_tools,
            this.toolStripMenuItem1,
            this.tsmi_help});
            this.menuStrip_main.Location = new System.Drawing.Point(0, 0);
            this.menuStrip_main.Name = "menuStrip_main";
            this.menuStrip_main.Size = new System.Drawing.Size(861, 25);
            this.menuStrip_main.TabIndex = 2;
            this.menuStrip_main.Text = "menuStrip1";
            // 
            // tsmi_menu
            // 
            this.tsmi_menu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_openConfig,
            this.tsmi_saveConfig});
            this.tsmi_menu.ForeColor = System.Drawing.SystemColors.WindowText;
            this.tsmi_menu.Image = global::SuperSQLInjection.Properties.Resources.menu;
            this.tsmi_menu.Name = "tsmi_menu";
            this.tsmi_menu.Size = new System.Drawing.Size(64, 21);
            this.tsmi_menu.Text = "菜 单";
            // 
            // tsmi_openConfig
            // 
            this.tsmi_openConfig.Image = global::SuperSQLInjection.Properties.Resources.导入;
            this.tsmi_openConfig.Name = "tsmi_openConfig";
            this.tsmi_openConfig.Size = new System.Drawing.Size(124, 22);
            this.tsmi_openConfig.Text = "导入配置";
            this.tsmi_openConfig.Click += new System.EventHandler(this.tsmi_openConfig_Click);
            // 
            // tsmi_saveConfig
            // 
            this.tsmi_saveConfig.Image = global::SuperSQLInjection.Properties.Resources.保存;
            this.tsmi_saveConfig.Name = "tsmi_saveConfig";
            this.tsmi_saveConfig.Size = new System.Drawing.Size(124, 22);
            this.tsmi_saveConfig.Text = "保存配置";
            this.tsmi_saveConfig.Click += new System.EventHandler(this.tsmi_saveConfig_Click);
            // 
            // tsmi_tools
            // 
            this.tsmi_tools.Image = global::SuperSQLInjection.Properties.Resources.tool;
            this.tsmi_tools.Name = "tsmi_tools";
            this.tsmi_tools.Size = new System.Drawing.Size(64, 21);
            this.tsmi_tools.Text = "工 具";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_seting,
            this.tsmi_lang});
            this.toolStripMenuItem1.Image = global::SuperSQLInjection.Properties.Resources.config;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(84, 21);
            this.toolStripMenuItem1.Text = "系统设置";
            // 
            // tsmi_seting
            // 
            this.tsmi_seting.Image = global::SuperSQLInjection.Properties.Resources.set;
            this.tsmi_seting.Name = "tsmi_seting";
            this.tsmi_seting.Size = new System.Drawing.Size(124, 22);
            this.tsmi_seting.Text = "系统设置";
            this.tsmi_seting.Click += new System.EventHandler(this.tsmi_seting_Click);
            // 
            // tsmi_lang
            // 
            this.tsmi_lang.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_zh_cn,
            this.tsmi_en_us});
            this.tsmi_lang.Image = global::SuperSQLInjection.Properties.Resources.lang;
            this.tsmi_lang.Name = "tsmi_lang";
            this.tsmi_lang.Size = new System.Drawing.Size(124, 22);
            this.tsmi_lang.Text = "语 言";
            // 
            // tsmi_zh_cn
            // 
            this.tsmi_zh_cn.Checked = true;
            this.tsmi_zh_cn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsmi_zh_cn.Name = "tsmi_zh_cn";
            this.tsmi_zh_cn.Size = new System.Drawing.Size(117, 22);
            this.tsmi_zh_cn.Text = "中文";
            // 
            // tsmi_en_us
            // 
            this.tsmi_en_us.Name = "tsmi_en_us";
            this.tsmi_en_us.Size = new System.Drawing.Size(117, 22);
            this.tsmi_en_us.Text = "English";
            // 
            // tsmi_help
            // 
            this.tsmi_help.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_readme,
            this.tsmi_about,
            this.tsmi_update,
            this.tsmi_mustRead,
            this.版本ToolStripMenuItem,
            this.tsmi_bugReport});
            this.tsmi_help.Image = global::SuperSQLInjection.Properties.Resources.help;
            this.tsmi_help.Name = "tsmi_help";
            this.tsmi_help.Size = new System.Drawing.Size(64, 21);
            this.tsmi_help.Text = "帮 助";
            // 
            // tsmi_readme
            // 
            this.tsmi_readme.Image = global::SuperSQLInjection.Properties.Resources.手册;
            this.tsmi_readme.Name = "tsmi_readme";
            this.tsmi_readme.Size = new System.Drawing.Size(124, 22);
            this.tsmi_readme.Text = "使用手册";
            this.tsmi_readme.Click += new System.EventHandler(this.tsmi_readme_Click);
            // 
            // tsmi_about
            // 
            this.tsmi_about.Image = global::SuperSQLInjection.Properties.Resources.关于;
            this.tsmi_about.Name = "tsmi_about";
            this.tsmi_about.Size = new System.Drawing.Size(124, 22);
            this.tsmi_about.Text = "关 于";
            this.tsmi_about.Click += new System.EventHandler(this.tsmi_about_Click);
            // 
            // tsmi_update
            // 
            this.tsmi_update.Image = global::SuperSQLInjection.Properties.Resources.更新;
            this.tsmi_update.Name = "tsmi_update";
            this.tsmi_update.Size = new System.Drawing.Size(124, 22);
            this.tsmi_update.Text = "在线更新";
            this.tsmi_update.Click += new System.EventHandler(this.tsmi_update_Click);
            // 
            // tsmi_mustRead
            // 
            this.tsmi_mustRead.Image = global::SuperSQLInjection.Properties.Resources.声明;
            this.tsmi_mustRead.Name = "tsmi_mustRead";
            this.tsmi_mustRead.Size = new System.Drawing.Size(124, 22);
            this.tsmi_mustRead.Text = "声 明";
            this.tsmi_mustRead.Click += new System.EventHandler(this.tsmi_mustRead_Click);
            // 
            // 版本ToolStripMenuItem
            // 
            this.版本ToolStripMenuItem.Image = global::SuperSQLInjection.Properties.Resources.版本;
            this.版本ToolStripMenuItem.Name = "版本ToolStripMenuItem";
            this.版本ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.版本ToolStripMenuItem.Text = "版本";
            this.版本ToolStripMenuItem.Click += new System.EventHandler(this.版本ToolStripMenuItem_Click);
            // 
            // tsmi_bugReport
            // 
            this.tsmi_bugReport.Image = global::SuperSQLInjection.Properties.Resources.bug;
            this.tsmi_bugReport.Name = "tsmi_bugReport";
            this.tsmi_bugReport.Size = new System.Drawing.Size(124, 22);
            this.tsmi_bugReport.Text = "Bug反馈";
            this.tsmi_bugReport.Click += new System.EventHandler(this.tsmi_bugReport_Click);
            // 
            // Main
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(861, 729);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.gb_logo);
            this.Controls.Add(this.mytab);
            this.Controls.Add(this.gb_basic);
            this.Controls.Add(this.menuStrip_main);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip_main;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "超级SQL注入工具 v1.0 正式版";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Shown += new System.EventHandler(this.Main_Shown);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Main_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Main_DragEnter);
            this.gb_basic.ResumeLayout(false);
            this.gb_basic.PerformLayout();
            this.gb_logo.ResumeLayout(false);
            this.tab_logCenter.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.log_cms_dataifo.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tab_file.ResumeLayout(false);
            this.tab_file.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.tab_dataCenter.ResumeLayout(false);
            this.tabC_dataCenter.ResumeLayout(false);
            this.tab_vers.ResumeLayout(false);
            this.tab_vers.PerformLayout();
            this.toolStrip_getVers.ResumeLayout(false);
            this.toolStrip_getVers.PerformLayout();
            this.data_cms_vers.ResumeLayout(false);
            this.tab_dbs.ResumeLayout(false);
            this.spc_dbs.Panel1.ResumeLayout(false);
            this.spc_dbs.Panel1.PerformLayout();
            this.spc_dbs.Panel2.ResumeLayout(false);
            this.spc_dbs.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spc_dbs)).EndInit();
            this.spc_dbs.ResumeLayout(false);
            this.data_dbs_ts.ResumeLayout(false);
            this.data_dbs_ts.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.data_cms_dbs.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.cms_data_dbs_lvw_data.ResumeLayout(false);
            this.tab_injectCenter.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tab_datapack.ResumeLayout(false);
            this.cms_dataPacks.ResumeLayout(false);
            this.tab_tokenset.ResumeLayout(false);
            this.groupBox17.ResumeLayout(false);
            this.groupBox19.ResumeLayout(false);
            this.groupBox19.PerformLayout();
            this.tab_sencond_inject.ResumeLayout(false);
            this.groupBox20.ResumeLayout(false);
            this.groupBox21.ResumeLayout(false);
            this.groupBox21.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox16.ResumeLayout(false);
            this.groupBox16.PerformLayout();
            this.groupBox15.ResumeLayout(false);
            this.groupBox15.PerformLayout();
            this.mytab.ResumeLayout(false);
            this.tab_proxy.ResumeLayout(false);
            this.groupBox25.ResumeLayout(false);
            this.groupBox25.PerformLayout();
            this.groupBox24.ResumeLayout(false);
            this.groupBox24.PerformLayout();
            this.toolStrip_proxyList.ResumeLayout(false);
            this.toolStrip_proxyList.PerformLayout();
            this.groupBox26.ResumeLayout(false);
            this.groupBox26.PerformLayout();
            this.proxy_cms.ResumeLayout(false);
            this.tab_cmd.ResumeLayout(false);
            this.tab_cmd.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.tab_bypass.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox18.ResumeLayout(false);
            this.groupBox18.PerformLayout();
            this.groupBox23.ResumeLayout(false);
            this.groupBox23.PerformLayout();
            this.bypass_lvw_replaceString_cms.ResumeLayout(false);
            this.groupBox22.ResumeLayout(false);
            this.groupBox22.PerformLayout();
            this.tab_encoding.ResumeLayout(false);
            this.groupBox13.ResumeLayout(false);
            this.groupBox13.PerformLayout();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.tab_scanInjection.ResumeLayout(false);
            this.groupBox14.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.scanInjectionURL_cms.ResumeLayout(false);
            this.scanInjection_cms.ResumeLayout(false);
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            this.tab_injectLog.ResumeLayout(false);
            this.injectLog_cm.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip_main.ResumeLayout(false);
            this.menuStrip_main.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gb_basic;
        private System.Windows.Forms.TextBox txt_basic_host;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuStrip menuStrip_main;
        private System.Windows.Forms.ToolStripMenuItem tsmi_menu;
        private System.Windows.Forms.ToolStripMenuItem tsmi_openConfig;
        private System.Windows.Forms.ToolStripMenuItem tsmi_saveConfig;
        private System.Windows.Forms.ToolStripMenuItem tsmi_tools;
        private System.Windows.Forms.ToolStripMenuItem tsmi_help;
        private System.Windows.Forms.ToolStripMenuItem tsmi_readme;
        private System.Windows.Forms.ToolStripMenuItem tsmi_about;
        private System.Windows.Forms.ToolStripMenuItem tsmi_update;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbox_basic_dbType;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbox_basic_injectType;
        private System.Windows.Forms.GroupBox gb_logo;
        private System.Windows.Forms.TextBox txt_basic_port;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_autoInject;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cbox_basic_encoding;
        private System.Windows.Forms.ComboBox cbox_basic_timeOut;
        private System.Windows.Forms.ComboBox cbox_basic_reTryCount;
        private System.Windows.Forms.ComboBox cbox_basic_threadSize;
        private System.Windows.Forms.Button btn_stop;
        private System.Windows.Forms.TabPage tab_logCenter;
        private System.Windows.Forms.TabPage tab_file;
        private System.Windows.Forms.TabPage tab_dataCenter;
        private System.Windows.Forms.TabPage tab_injectCenter;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btn_inject_clearRequest;
        private System.Windows.Forms.Button btn_inject_sendData;
        private System.Windows.Forms.CheckBox chk_inject_foward_302;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_inject_setInject;
        private System.Windows.Forms.TextBox txt_inject_key;
        private System.Windows.Forms.TextBox txt_inject_unionColumnsCount;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txt_inject_showIndex;
        private System.Windows.Forms.TabControl mytab;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ListView log_lvw_httpLog;
        private System.Windows.Forms.ColumnHeader log_col_payload;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tsmi_seting;
        private System.Windows.Forms.ToolStripMenuItem tsmi_mustRead;
        private System.Windows.Forms.Button btn_inject_setEncodingRange;
        private System.Windows.Forms.ContextMenuStrip data_cms_vers;
        private System.Windows.Forms.ToolStripMenuItem data_cms_tsmi_getVariable;
        private System.Windows.Forms.ToolStripMenuItem data_cms_tsmi_stopGetVariable;
        private System.Windows.Forms.ContextMenuStrip data_cms_dbs;
        private System.Windows.Forms.ToolStripMenuItem data_dbs_tsmi_addDBS;
        private System.Windows.Forms.ToolStripMenuItem data_dbs_tsmi_deleteNode;
        private System.Windows.Forms.ToolStripMenuItem data_dbs_tsmi_getTableNames;
        private System.Windows.Forms.ImageList myicon_list;
        private System.Windows.Forms.TabControl tabC_dataCenter;
        private System.Windows.Forms.TabPage tab_vers;
        private System.Windows.Forms.ListView data_lvw_ver;
        private System.Windows.Forms.ColumnHeader data_lvw_ver_verName;
        private System.Windows.Forms.TabPage tab_dbs;
        private System.Windows.Forms.ToolStrip data_dbs_ts;
        private System.Windows.Forms.ToolStripButton data_dbs_tsl_getTables;
        private System.Windows.Forms.ToolStripButton data_dbs_tsl_getColumns;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ListView data_dbs_lvw_data;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TreeView data_tvw_dbs;
        private System.Windows.Forms.ColumnHeader log_col_code;
        private System.Windows.Forms.ColumnHeader log_col_bodyLength;
        private System.Windows.Forms.ColumnHeader log_col_index;
        private System.Windows.Forms.ColumnHeader col_runtime;
        private System.Windows.Forms.ContextMenuStrip log_cms_dataifo;
        private System.Windows.Forms.ToolStripMenuItem data_cms_clearLog;
        private System.Windows.Forms.ToolStripMenuItem data_dbs_tsmi_stopGetInfos;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer spc_dbs;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox data_dbs_txt_count;
        private System.Windows.Forms.ToolStripButton data_dbs_tsl_getDatas;
        private System.Windows.Forms.ToolStripButton data_dbs_tsl_exportDatas;
        private System.Windows.Forms.ToolStripButton data_dbs_tsl_getDBS;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox data_dbs_cob_db_encoding;
        private System.Windows.Forms.CheckBox chk_openURLEncoding;
        private System.Windows.Forms.ToolStripMenuItem data_cms_tsmi_copyVerValue;
        private System.Windows.Forms.TabPage tab_encoding;
        private System.Windows.Forms.TabPage tab_cmd;
        private System.Windows.Forms.CheckBox chk_useSSL;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.WebBrowser webBro_log;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripTextBox data_dbs_txt_start;
        private System.Windows.Forms.ImageList imglist_database;
        private System.Windows.Forms.ContextMenuStrip cms_data_dbs_lvw_data;
        private System.Windows.Forms.ToolStripMenuItem data_dbs_lvw_tsmi_stop;
        private System.Windows.Forms.ToolStripMenuItem data_dbs_lvw_tsmi_copyLineData;
        private System.Windows.Forms.CheckBox chk_inject_reverseKey;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox file_txt_filePath;
        private System.Windows.Forms.ComboBox file_cbox_readWrite;
        private System.Windows.Forms.Button file_btn_start;
        private System.Windows.Forms.TextBox file_txt_result;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Button cmd_btn_start;
        private System.Windows.Forms.TextBox cmd_txt_cmd;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox cmd_txt_result;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel status_lbl_time;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel status_lbl_threadStatus;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel status_lbl_dbsCount;
        private System.Windows.Forms.ToolStripStatusLabel status_lbl_tableCount;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel6;
        private System.Windows.Forms.ToolStripStatusLabel status_lbl_dataCount;
        private System.Windows.Forms.Timer timer_status;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel7;
        private System.Windows.Forms.ToolStripStatusLabel status_lbl_runStatus;
        private System.Windows.Forms.CheckBox cmd_chk_showCmdResult;
        private System.Windows.Forms.TabPage tab_bypass;
        private System.Windows.Forms.ContextMenuStrip bypass_lvw_replaceString_cms;
        private System.Windows.Forms.ToolStripMenuItem bypass_tsmi_delselect;
        private System.Windows.Forms.Button injectConfig_btn_checkKey;
        private System.Windows.Forms.ColumnHeader log_col_sleepTime;
        private System.Windows.Forms.TextBox encode_txt_result;
        private System.Windows.Forms.TextBox encode_txt_input;
        private System.Windows.Forms.ComboBox encode_cbox_decode;
        private System.Windows.Forms.ComboBox encode_cbox_encode;
        private System.Windows.Forms.TabPage tab_scanInjection;
        private System.Windows.Forms.Button inject_btn_autoFindKey;
        private System.Windows.Forms.Button scanInjection_btn_spider;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox scanInjection_txt_domainsPath;
        private System.Windows.Forms.Button scanInjection_importDomains;
        private System.Windows.Forms.ListView scanInjection_lvw_result;
        private System.Windows.Forms.ColumnHeader col_index;
        private System.Windows.Forms.ColumnHeader col_injectionType;
        private System.Windows.Forms.ColumnHeader col_param;
        private System.Windows.Forms.ColumnHeader col_testURL;
        private System.Windows.Forms.Label scanInjection_domainsCount;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label scanInjection_scanedURLSCount;
        private System.Windows.Forms.Label scanInjection_findURLSCount;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label scanInjection_scanedDomainCount;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Timer timer_scanInjection;
        private System.Windows.Forms.ContextMenuStrip scanInjection_cms;
        private System.Windows.Forms.ToolStripMenuItem scanInjection_cms_exportResult;
        private System.Windows.Forms.ToolStripMenuItem scanInjection_cms_copyURL;
        private System.Windows.Forms.ColumnHeader col_mark;
        private System.Windows.Forms.ToolStripMenuItem scanInjection_cms_clearResult;
        private System.Windows.Forms.ToolStripMenuItem scanInjection_cms_delThisLine;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListBox scanInject_lsb_links;
        private System.Windows.Forms.CheckBox scanInect_chk_isSpider;
        private System.Windows.Forms.CheckBox scanInect_chk_scanError;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.GroupBox groupBox13;
        private System.Windows.Forms.ToolStripMenuItem data_dbs_tsmi_addTableOrColumn;
        private System.Windows.Forms.Button scanInjection_btn_scan;
        private System.Windows.Forms.GroupBox groupBox14;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.ContextMenuStrip scanInjectionURL_cms;
        private System.Windows.Forms.ToolStripMenuItem tsmi_exportScanInjectionURL;
        private System.Windows.Forms.ToolStripMenuItem tsmi_clearScanInjectionURL;
        private System.Windows.Forms.GroupBox groupBox15;
        private System.Windows.Forms.GroupBox groupBox16;
        private System.Windows.Forms.ColumnHeader col_injectionDB;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel8;
        private System.Windows.Forms.ToolStripStatusLabel status_lbl_all_status;
        private System.Windows.Forms.ComboBox cbox_file_readFileEncoding;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.ToolStripMenuItem 版本ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem data_dbs_tsmi_saveDTCStruct;
        private System.Windows.Forms.ToolStripMenuItem data_dbs_tsmi_loadDTCStruct;
        private System.Windows.Forms.ToolStripMenuItem data_dbs_tsmi_clearDTCStruct;
        private System.Windows.Forms.ComboBox cbox_inject_type;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.ContextMenuStrip cms_dataPacks;
        private System.Windows.Forms.ToolStripMenuItem tsmi_createGetTemplate;
        private System.Windows.Forms.ToolStripMenuItem tsmi_createPOSTTemplate;
        private System.Windows.Forms.ToolStripMenuItem tsmi_changeRequestMethod;
        private System.Windows.Forms.ToolStripMenuItem tsmi_clearColumns;
        private System.Windows.Forms.ToolStripMenuItem tsmi_bugReport;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.ComboBox cob_keyRepalce;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox bypass_cbox_randIPToHeader;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ComboBox bypass_cbox_sendHTTPSleepTime;
        private System.Windows.Forms.CheckBox bypass_chk_inculdeStr;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox bypass_txt_replaceTo;
        private System.Windows.Forms.TextBox bypass_txt_replace;
        private System.Windows.Forms.ListView bypass_lvw_replaceString;
        private System.Windows.Forms.ColumnHeader col_replace;
        private System.Windows.Forms.ColumnHeader col_replaceTo;
        private System.Windows.Forms.Button bypass_btn_addReplaceStr;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.CheckBox chk_reaplaceBeforURLEncode;
        private System.Windows.Forms.ToolStripStatusLabel lbl_packsCount;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel11;
        private System.Windows.Forms.ToolStripMenuItem tsmi_createPackByURL;
        private System.Windows.Forms.ColumnHeader col_url;
        private System.Windows.Forms.ToolStripMenuItem tsmi_openURL;
        private System.Windows.Forms.ToolStripMenuItem tsmi_tsmi_opentestURL;
        private System.Windows.Forms.ComboBox bypass_cbox_loadTemplate;
        private System.Windows.Forms.Button bypass_btn_saveTemplate;
        private System.Windows.Forms.ComboBox cbox_base64Count;
        private System.Windows.Forms.ToolStripMenuItem tsmi_tsmi_exortOldURL;
        private System.Windows.Forms.ToolStripMenuItem tsmi_tsmi_exortTestURL;
        private System.Windows.Forms.ComboBox cbox_bypass_urlencode_count;
        private System.Windows.Forms.CheckBox bypass_chk_usebetween;
        private System.Windows.Forms.GroupBox groupBox18;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tab_datapack;
        private System.Windows.Forms.TabPage tab_tokenset;
        private System.Windows.Forms.GroupBox groupBox19;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.TextBox token_txt_endStr;
        private System.Windows.Forms.TextBox token_txt_startStr;
        private System.Windows.Forms.Button token_btn_testGetToken;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.GroupBox groupBox17;
        private System.Windows.Forms.Button btn_inject_setTokenLocation;
        private System.Windows.Forms.Button btn_inject_randStr;
        private System.Windows.Forms.TabPage tab_sencond_inject;
        private System.Windows.Forms.GroupBox groupBox20;
        private System.Windows.Forms.CheckBox bypass_hex;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.GroupBox groupBox21;
        private System.Windows.Forms.CheckBox chk_sencondInject;
        private System.Windows.Forms.CheckBox bypass_chk_use_unicode;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.ToolStripMenuItem tsmi_lang;
        private System.Windows.Forms.ToolStripMenuItem tsmi_zh_cn;
        private System.Windows.Forms.ToolStripMenuItem tsmi_en_us;
        private System.Windows.Forms.ToolStripMenuItem data_dbs_tsmi_selectAllSubNode;
        private System.Windows.Forms.ToolStripMenuItem data_dbs_tsmi_selectReversSubNode;
        private System.Windows.Forms.RichTextBox txt_log;
        private System.Windows.Forms.TabPage tab_injectLog;
        private System.Windows.Forms.ListView lvw_injectLog;
        private System.Windows.Forms.ColumnHeader injectlog_col_uri;
        private System.Windows.Forms.ColumnHeader injectlog_col_pname;
        private System.Windows.Forms.ColumnHeader injectlog_col_injectType;
        private System.Windows.Forms.ColumnHeader injectlog_col_dbType;
        private System.Windows.Forms.ColumnHeader injectlog_col_payload;
        private System.Windows.Forms.ColumnHeader injectlog_col_time;
        private System.Windows.Forms.ContextMenuStrip injectLog_cm;
        private System.Windows.Forms.ToolStripMenuItem tsmi_injectLog_useCLog;
        private System.Windows.Forms.ToolStripMenuItem tsmi_injectLog_delSLog;
        private System.Windows.Forms.ToolStripMenuItem tsmi_injectLog_clearAllLog;
        private System.Windows.Forms.ColumnHeader injectlog_col_ip;
        private System.Windows.Forms.ColumnHeader injectlog_col_port;
        private System.Windows.Forms.Button cmd_btn_stop;
        private System.Windows.Forms.Button file_btn_stop;
        private System.Windows.Forms.ToolStripMenuItem data_cms_copyPaylaod;
        private System.Windows.Forms.ToolStripMenuItem bypass_tsmi_copy;
        private System.Windows.Forms.GroupBox groupBox23;
        private System.Windows.Forms.GroupBox groupBox22;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.ColumnHeader data_lvw_ver_val;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.TextBox txt_inject_unionTemplate;
        private System.Windows.Forms.ToolStripStatusLabel lbl_info;
        private System.Windows.Forms.TabPage tab_proxy;
        private System.Windows.Forms.ToolStripMenuItem data_cms_tsmi_selectAllVers;
        private System.Windows.Forms.ToolStripMenuItem data_cms_tsmi_selectReversVers;
        private System.Windows.Forms.GroupBox groupBox25;
        private System.Windows.Forms.GroupBox groupBox24;
        private System.Windows.Forms.ListView proxy_lvw_proxyList;
        private System.Windows.Forms.ColumnHeader col_host;
        private System.Windows.Forms.ColumnHeader col_port;
        private System.Windows.Forms.ColumnHeader col_username;
        private System.Windows.Forms.ColumnHeader col_password;
        private System.Windows.Forms.ColumnHeader col_conectIsOK;
        private System.Windows.Forms.ColumnHeader col_checkTime;
        private System.Windows.Forms.ColumnHeader col_useTime;
        private System.Windows.Forms.ContextMenuStrip proxy_cms;
        private System.Windows.Forms.ToolStripMenuItem proxy_checkSelectedProxy;
        private System.Windows.Forms.ToolStripMenuItem proxy_checkAllProxy;
        private System.Windows.Forms.ToolStripMenuItem proxy_importProxy;
        private System.Windows.Forms.ToolStripMenuItem proxy_exportProxy;
        private System.Windows.Forms.ToolStripMenuItem proxy_tsmi_setCurrentProxy;
        private System.Windows.Forms.ToolStripMenuItem proxy_delSelectedProxy;
        private System.Windows.Forms.ToolStripMenuItem proxy_copySelectedProxy;
        private System.Windows.Forms.ToolStripMenuItem proxy_clearAllProxy;
        private System.Windows.Forms.ColumnHeader col_proxyType;
        private System.Windows.Forms.GroupBox groupBox26;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Button proxy_btn_addProxy;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.TextBox proxy_txt_addProxyHost;
        private System.Windows.Forms.TextBox proxy_txt_addProxyPort;
        private System.Windows.Forms.ComboBox proxy_txt_addProxyType;
        private System.Windows.Forms.Button proxy_btn_importProxy;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.TextBox proxy_txt_addProxyPassword;
        private System.Windows.Forms.TextBox proxy_txt_addProxyUserName;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label proxy_lbl_proxyType;
        private System.Windows.Forms.Label proxy_lbl_proxy_port;
        private System.Windows.Forms.Label proxy_lbl_proxy_password;
        private System.Windows.Forms.Label proxy_lbl_proxy_username;
        private System.Windows.Forms.Label proxy_lbl_proxy_host;
        private System.Windows.Forms.ComboBox proxy_cmb_proxyMode;
        private System.Windows.Forms.ToolStripMenuItem proxy_clearAllFailedProxy;
        private System.Windows.Forms.ToolStrip toolStrip_proxyList;
        private System.Windows.Forms.ToolStripLabel proxy_ts_btn_clearAllFailedProxy;
        private System.Windows.Forms.ColumnHeader col_proxy;
        private System.Windows.Forms.ToolStripMenuItem proxy_checkNoCheckProxy;
        private System.Windows.Forms.ToolStripLabel proxy_ts_btn_proxy_checkNoCheckProxy;
        private System.Windows.Forms.ToolStrip toolStrip_getVers;
        private System.Windows.Forms.ToolStripButton toolStrip_vers_btn_getVariable;
        private System.Windows.Forms.ToolStripButton toolStrip_vers_btn_stopGetVariable;
        private System.Windows.Forms.ToolStripLabel data_dbs_tsl_stopGetDatas;
        private System.Windows.Forms.RichTextBox txt_inject_request;
        private System.Windows.Forms.RichTextBox log_txt_response;
        private System.Windows.Forms.RichTextBox token_txt_http_request;
        private System.Windows.Forms.RichTextBox txt_sencond_request;
        private System.Windows.Forms.RichTextBox log_txt_request;
        private System.Windows.Forms.ToolStripButton toolStrip_vers_btn_selectAll;
        private System.Windows.Forms.ToolStripButton toolStrip_vers_btn_selectReverse;
        private System.Windows.Forms.ImageList img_line;
    }
}

