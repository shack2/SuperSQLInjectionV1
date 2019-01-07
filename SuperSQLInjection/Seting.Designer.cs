namespace SuperSQLInjection
{
    partial class Seting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chk_redirectDoGet = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmb_unionFill = new System.Windows.Forms.ComboBox();
            this.lbl_maxColumnsCount = new System.Windows.Forms.Label();
            this.cmb_maxClolumnsCount = new System.Windows.Forms.ComboBox();
            this.chk_isAutoSaveConfig = new System.Windows.Forms.CheckBox();
            this.chk_autoCheckUpdate = new System.Windows.Forms.CheckBox();
            this.chk_mysqlMuStr = new System.Windows.Forms.CheckBox();
            this.chk_openHTTPLog = new System.Windows.Forms.CheckBox();
            this.chk_openInfoLog = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmb_oneDomainMaxSpiderCount = new System.Windows.Forms.ComboBox();
            this.cmb_oneDomainMaxScanCount = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_proxy_host = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_proxy_port = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_proxy_keys = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chk_redirectDoGet);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cmb_unionFill);
            this.groupBox1.Controls.Add(this.lbl_maxColumnsCount);
            this.groupBox1.Controls.Add(this.cmb_maxClolumnsCount);
            this.groupBox1.Controls.Add(this.chk_isAutoSaveConfig);
            this.groupBox1.Controls.Add(this.chk_autoCheckUpdate);
            this.groupBox1.Controls.Add(this.chk_mysqlMuStr);
            this.groupBox1.Controls.Add(this.chk_openHTTPLog);
            this.groupBox1.Controls.Add(this.chk_openInfoLog);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(487, 171);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "系统配置";
            // 
            // chk_redirectDoGet
            // 
            this.chk_redirectDoGet.AutoSize = true;
            this.chk_redirectDoGet.Location = new System.Drawing.Point(19, 131);
            this.chk_redirectDoGet.Name = "chk_redirectDoGet";
            this.chk_redirectDoGet.Size = new System.Drawing.Size(126, 16);
            this.chk_redirectDoGet.TabIndex = 7;
            this.chk_redirectDoGet.Text = "重定向使用GET请求";
            this.chk_redirectDoGet.UseVisualStyleBackColor = true;
            this.chk_redirectDoGet.CheckedChanged += new System.EventHandler(this.chk_redirectDoGet_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(274, 132);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "Union填充：";
            // 
            // cmb_unionFill
            // 
            this.cmb_unionFill.FormattingEnabled = true;
            this.cmb_unionFill.Items.AddRange(new object[] {
            "1",
            "null"});
            this.cmb_unionFill.Location = new System.Drawing.Point(351, 129);
            this.cmb_unionFill.Name = "cmb_unionFill";
            this.cmb_unionFill.Size = new System.Drawing.Size(70, 20);
            this.cmb_unionFill.TabIndex = 5;
            this.cmb_unionFill.SelectedValueChanged += new System.EventHandler(this.cmb_unionFill_SelectedValueChanged);
            // 
            // lbl_maxColumnsCount
            // 
            this.lbl_maxColumnsCount.AutoSize = true;
            this.lbl_maxColumnsCount.Location = new System.Drawing.Point(17, 89);
            this.lbl_maxColumnsCount.Name = "lbl_maxColumnsCount";
            this.lbl_maxColumnsCount.Size = new System.Drawing.Size(65, 12);
            this.lbl_maxColumnsCount.TabIndex = 6;
            this.lbl_maxColumnsCount.Text = "最大列数：";
            // 
            // cmb_maxClolumnsCount
            // 
            this.cmb_maxClolumnsCount.FormattingEnabled = true;
            this.cmb_maxClolumnsCount.Items.AddRange(new object[] {
            "5",
            "10",
            "15",
            "20",
            "30",
            "40",
            "50",
            "60",
            "70",
            "80",
            "90",
            "100",
            "150",
            "200"});
            this.cmb_maxClolumnsCount.Location = new System.Drawing.Point(88, 86);
            this.cmb_maxClolumnsCount.Name = "cmb_maxClolumnsCount";
            this.cmb_maxClolumnsCount.Size = new System.Drawing.Size(70, 20);
            this.cmb_maxClolumnsCount.TabIndex = 5;
            this.cmb_maxClolumnsCount.SelectedValueChanged += new System.EventHandler(this.cob_maxClolumnsCount_SelectedValueChanged);
            // 
            // chk_isAutoSaveConfig
            // 
            this.chk_isAutoSaveConfig.AutoSize = true;
            this.chk_isAutoSaveConfig.Location = new System.Drawing.Point(276, 88);
            this.chk_isAutoSaveConfig.Name = "chk_isAutoSaveConfig";
            this.chk_isAutoSaveConfig.Size = new System.Drawing.Size(144, 16);
            this.chk_isAutoSaveConfig.TabIndex = 4;
            this.chk_isAutoSaveConfig.Text = "软件关闭自动保存配置";
            this.chk_isAutoSaveConfig.UseVisualStyleBackColor = true;
            this.chk_isAutoSaveConfig.CheckedChanged += new System.EventHandler(this.chk_isAutoSaveConfig_CheckedChanged);
            // 
            // chk_autoCheckUpdate
            // 
            this.chk_autoCheckUpdate.AutoSize = true;
            this.chk_autoCheckUpdate.Location = new System.Drawing.Point(276, 20);
            this.chk_autoCheckUpdate.Name = "chk_autoCheckUpdate";
            this.chk_autoCheckUpdate.Size = new System.Drawing.Size(120, 16);
            this.chk_autoCheckUpdate.TabIndex = 1;
            this.chk_autoCheckUpdate.Text = "开启自动检测更新";
            this.chk_autoCheckUpdate.UseVisualStyleBackColor = true;
            this.chk_autoCheckUpdate.CheckedChanged += new System.EventHandler(this.chk_autoCheckUpdate_CheckedChanged);
            // 
            // chk_mysqlMuStr
            // 
            this.chk_mysqlMuStr.AutoSize = true;
            this.chk_mysqlMuStr.Location = new System.Drawing.Point(19, 20);
            this.chk_mysqlMuStr.Name = "chk_mysqlMuStr";
            this.chk_mysqlMuStr.Size = new System.Drawing.Size(150, 16);
            this.chk_mysqlMuStr.TabIndex = 0;
            this.chk_mysqlMuStr.Text = "开启MySQL多字节取数据";
            this.chk_mysqlMuStr.UseVisualStyleBackColor = true;
            this.chk_mysqlMuStr.CheckedChanged += new System.EventHandler(this.chk_mysqlMuStr_CheckedChanged);
            // 
            // chk_openHTTPLog
            // 
            this.chk_openHTTPLog.AutoSize = true;
            this.chk_openHTTPLog.Location = new System.Drawing.Point(276, 51);
            this.chk_openHTTPLog.Name = "chk_openHTTPLog";
            this.chk_openHTTPLog.Size = new System.Drawing.Size(96, 16);
            this.chk_openHTTPLog.TabIndex = 2;
            this.chk_openHTTPLog.Text = "开启发包日志";
            this.chk_openHTTPLog.UseVisualStyleBackColor = true;
            this.chk_openHTTPLog.CheckedChanged += new System.EventHandler(this.chk_openHTTPLog_CheckedChanged);
            // 
            // chk_openInfoLog
            // 
            this.chk_openInfoLog.AutoSize = true;
            this.chk_openInfoLog.Location = new System.Drawing.Point(19, 51);
            this.chk_openInfoLog.Name = "chk_openInfoLog";
            this.chk_openInfoLog.Size = new System.Drawing.Size(96, 16);
            this.chk_openInfoLog.TabIndex = 1;
            this.chk_openInfoLog.Text = "开启底部日志";
            this.chk_openInfoLog.UseVisualStyleBackColor = true;
            this.chk_openInfoLog.CheckedChanged += new System.EventHandler(this.chk_openInfoLog_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "单域名最大爬行数：";
            // 
            // cmb_oneDomainMaxSpiderCount
            // 
            this.cmb_oneDomainMaxSpiderCount.FormattingEnabled = true;
            this.cmb_oneDomainMaxSpiderCount.Items.AddRange(new object[] {
            "3",
            "5",
            "10",
            "15",
            "20",
            "30",
            "40",
            "50"});
            this.cmb_oneDomainMaxSpiderCount.Location = new System.Drawing.Point(136, 31);
            this.cmb_oneDomainMaxSpiderCount.Name = "cmb_oneDomainMaxSpiderCount";
            this.cmb_oneDomainMaxSpiderCount.Size = new System.Drawing.Size(70, 20);
            this.cmb_oneDomainMaxSpiderCount.TabIndex = 7;
            this.cmb_oneDomainMaxSpiderCount.SelectedValueChanged += new System.EventHandler(this.cob_oneDomainMaxSpiderCount_SelectedValueChanged);
            // 
            // cmb_oneDomainMaxScanCount
            // 
            this.cmb_oneDomainMaxScanCount.FormattingEnabled = true;
            this.cmb_oneDomainMaxScanCount.Items.AddRange(new object[] {
            "3",
            "5",
            "10",
            "15",
            "20"});
            this.cmb_oneDomainMaxScanCount.Location = new System.Drawing.Point(393, 31);
            this.cmb_oneDomainMaxScanCount.Name = "cmb_oneDomainMaxScanCount";
            this.cmb_oneDomainMaxScanCount.Size = new System.Drawing.Size(70, 20);
            this.cmb_oneDomainMaxScanCount.TabIndex = 7;
            this.cmb_oneDomainMaxScanCount.SelectedValueChanged += new System.EventHandler(this.cob_oneDomainMaxScanCount_SelectedValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(274, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "单域名最大扫描数：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cmb_oneDomainMaxScanCount);
            this.groupBox2.Controls.Add(this.cmb_oneDomainMaxSpiderCount);
            this.groupBox2.Location = new System.Drawing.Point(12, 204);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(487, 75);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "批量注入设置";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txt_proxy_port);
            this.groupBox3.Controls.Add(this.txt_proxy_keys);
            this.groupBox3.Controls.Add(this.txt_proxy_host);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(12, 285);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(487, 75);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "代理验证设置";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "验证域名：";
            // 
            // txt_proxy_host
            // 
            this.txt_proxy_host.Location = new System.Drawing.Point(88, 30);
            this.txt_proxy_host.Name = "txt_proxy_host";
            this.txt_proxy_host.Size = new System.Drawing.Size(96, 21);
            this.txt_proxy_host.TabIndex = 9;
            this.txt_proxy_host.TextChanged += new System.EventHandler(this.txt_proxy_host_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(190, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "端口：";
            // 
            // txt_proxy_port
            // 
            this.txt_proxy_port.Location = new System.Drawing.Point(237, 30);
            this.txt_proxy_port.Name = "txt_proxy_port";
            this.txt_proxy_port.Size = new System.Drawing.Size(38, 21);
            this.txt_proxy_port.TabIndex = 9;
            this.txt_proxy_port.TextChanged += new System.EventHandler(this.txt_proxy_port_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(281, 34);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 8;
            this.label6.Text = "关键词：";
            // 
            // txt_proxy_keys
            // 
            this.txt_proxy_keys.Location = new System.Drawing.Point(340, 30);
            this.txt_proxy_keys.Name = "txt_proxy_keys";
            this.txt_proxy_keys.Size = new System.Drawing.Size(123, 21);
            this.txt_proxy_keys.TabIndex = 9;
            this.txt_proxy_keys.TextChanged += new System.EventHandler(this.txt_proxy_keys_TextChanged);
            // 
            // Seting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 379);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Seting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "系统设置";
            this.Shown += new System.EventHandler(this.Seting_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chk_mysqlMuStr;
        private System.Windows.Forms.CheckBox chk_openInfoLog;
        private System.Windows.Forms.CheckBox chk_openHTTPLog;
        private System.Windows.Forms.CheckBox chk_autoCheckUpdate;
        private System.Windows.Forms.CheckBox chk_isAutoSaveConfig;
        private System.Windows.Forms.Label lbl_maxColumnsCount;
        private System.Windows.Forms.ComboBox cmb_maxClolumnsCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmb_oneDomainMaxSpiderCount;
        private System.Windows.Forms.ComboBox cmb_oneDomainMaxScanCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chk_redirectDoGet;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmb_unionFill;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_proxy_port;
        private System.Windows.Forms.TextBox txt_proxy_host;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_proxy_keys;
        private System.Windows.Forms.Label label6;
    }
}