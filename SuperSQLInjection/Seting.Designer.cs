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
            this.lbl_maxColumnsCount = new System.Windows.Forms.Label();
            this.cob_maxClolumnsCount = new System.Windows.Forms.ComboBox();
            this.chk_isAutoSaveConfig = new System.Windows.Forms.CheckBox();
            this.chk_autoCheckUpdate = new System.Windows.Forms.CheckBox();
            this.chk_mysqlMuStr = new System.Windows.Forms.CheckBox();
            this.chk_openHTTPLog = new System.Windows.Forms.CheckBox();
            this.chk_openInfoLog = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cob_oneDomainMaxSpiderCount = new System.Windows.Forms.ComboBox();
            this.cob_oneDomainMaxScanCount = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chk_redirectDoGet = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chk_redirectDoGet);
            this.groupBox1.Controls.Add(this.lbl_maxColumnsCount);
            this.groupBox1.Controls.Add(this.cob_maxClolumnsCount);
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
            // lbl_maxColumnsCount
            // 
            this.lbl_maxColumnsCount.AutoSize = true;
            this.lbl_maxColumnsCount.Location = new System.Drawing.Point(17, 89);
            this.lbl_maxColumnsCount.Name = "lbl_maxColumnsCount";
            this.lbl_maxColumnsCount.Size = new System.Drawing.Size(65, 12);
            this.lbl_maxColumnsCount.TabIndex = 6;
            this.lbl_maxColumnsCount.Text = "最大列数：";
            // 
            // cob_maxClolumnsCount
            // 
            this.cob_maxClolumnsCount.FormattingEnabled = true;
            this.cob_maxClolumnsCount.Items.AddRange(new object[] {
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
            this.cob_maxClolumnsCount.Location = new System.Drawing.Point(88, 86);
            this.cob_maxClolumnsCount.Name = "cob_maxClolumnsCount";
            this.cob_maxClolumnsCount.Size = new System.Drawing.Size(70, 20);
            this.cob_maxClolumnsCount.TabIndex = 5;
            this.cob_maxClolumnsCount.SelectedValueChanged += new System.EventHandler(this.cob_maxClolumnsCount_SelectedValueChanged);
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
            // cob_oneDomainMaxSpiderCount
            // 
            this.cob_oneDomainMaxSpiderCount.FormattingEnabled = true;
            this.cob_oneDomainMaxSpiderCount.Items.AddRange(new object[] {
            "3",
            "5",
            "10",
            "15",
            "20",
            "30",
            "40",
            "50"});
            this.cob_oneDomainMaxSpiderCount.Location = new System.Drawing.Point(136, 31);
            this.cob_oneDomainMaxSpiderCount.Name = "cob_oneDomainMaxSpiderCount";
            this.cob_oneDomainMaxSpiderCount.Size = new System.Drawing.Size(70, 20);
            this.cob_oneDomainMaxSpiderCount.TabIndex = 7;
            this.cob_oneDomainMaxSpiderCount.SelectedValueChanged += new System.EventHandler(this.cob_oneDomainMaxSpiderCount_SelectedValueChanged);
            // 
            // cob_oneDomainMaxScanCount
            // 
            this.cob_oneDomainMaxScanCount.FormattingEnabled = true;
            this.cob_oneDomainMaxScanCount.Items.AddRange(new object[] {
            "3",
            "5",
            "10",
            "15",
            "20"});
            this.cob_oneDomainMaxScanCount.Location = new System.Drawing.Point(393, 31);
            this.cob_oneDomainMaxScanCount.Name = "cob_oneDomainMaxScanCount";
            this.cob_oneDomainMaxScanCount.Size = new System.Drawing.Size(70, 20);
            this.cob_oneDomainMaxScanCount.TabIndex = 7;
            this.cob_oneDomainMaxScanCount.SelectedValueChanged += new System.EventHandler(this.cob_oneDomainMaxScanCount_SelectedValueChanged);
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
            this.groupBox2.Controls.Add(this.cob_oneDomainMaxScanCount);
            this.groupBox2.Controls.Add(this.cob_oneDomainMaxSpiderCount);
            this.groupBox2.Location = new System.Drawing.Point(12, 204);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(487, 75);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "批量注入设置";
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
            // Seting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 300);
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
        private System.Windows.Forms.ComboBox cob_maxClolumnsCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cob_oneDomainMaxSpiderCount;
        private System.Windows.Forms.ComboBox cob_oneDomainMaxScanCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chk_redirectDoGet;
    }
}