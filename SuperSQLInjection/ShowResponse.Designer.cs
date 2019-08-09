namespace SuperSQLInjection
{
    partial class ShowResponse
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
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tab_showInBrowser = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tab_requestBody = new System.Windows.Forms.TabControl();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txt_responseBody = new System.Windows.Forms.RichTextBox();
            this.txt_requestHeader = new System.Windows.Forms.RichTextBox();
            this.txt_requestBody = new System.Windows.Forms.RichTextBox();
            this.txt_responseHeader = new System.Windows.Forms.RichTextBox();
            this.tabPage3.SuspendLayout();
            this.tab_showInBrowser.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tab_requestBody.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.tab_showInBrowser);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(663, 415);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Response Body";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tab_showInBrowser
            // 
            this.tab_showInBrowser.Controls.Add(this.tabPage4);
            this.tab_showInBrowser.Controls.Add(this.tabPage5);
            this.tab_showInBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tab_showInBrowser.Location = new System.Drawing.Point(0, 0);
            this.tab_showInBrowser.Name = "tab_showInBrowser";
            this.tab_showInBrowser.SelectedIndex = 0;
            this.tab_showInBrowser.Size = new System.Drawing.Size(663, 415);
            this.tab_showInBrowser.TabIndex = 1;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.txt_responseBody);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(655, 389);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "查看文本Text";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.webBrowser1);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(655, 389);
            this.tabPage5.TabIndex = 1;
            this.tabPage5.Text = "在浏览器中显示";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(3, 3);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(649, 383);
            this.webBrowser1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.txt_requestHeader);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(663, 415);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Request Header";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tab_requestBody
            // 
            this.tab_requestBody.Controls.Add(this.tabPage1);
            this.tab_requestBody.Controls.Add(this.tabPage6);
            this.tab_requestBody.Controls.Add(this.tabPage2);
            this.tab_requestBody.Controls.Add(this.tabPage3);
            this.tab_requestBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tab_requestBody.Location = new System.Drawing.Point(0, 0);
            this.tab_requestBody.Name = "tab_requestBody";
            this.tab_requestBody.SelectedIndex = 0;
            this.tab_requestBody.Size = new System.Drawing.Size(671, 441);
            this.tab_requestBody.TabIndex = 0;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.txt_requestBody);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(663, 415);
            this.tabPage6.TabIndex = 3;
            this.tabPage6.Text = "Request Body";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txt_responseHeader);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(663, 415);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Response Header";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // txt_responseBody
            // 
            this.txt_responseBody.DetectUrls = false;
            this.txt_responseBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_responseBody.Location = new System.Drawing.Point(3, 3);
            this.txt_responseBody.Name = "txt_responseBody";
            this.txt_responseBody.Size = new System.Drawing.Size(649, 383);
            this.txt_responseBody.TabIndex = 0;
            this.txt_responseBody.Text = "";
            this.txt_responseBody.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_responseBody_KeyDown);
            // 
            // txt_requestHeader
            // 
            this.txt_requestHeader.DetectUrls = false;
            this.txt_requestHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_requestHeader.Location = new System.Drawing.Point(3, 3);
            this.txt_requestHeader.Name = "txt_requestHeader";
            this.txt_requestHeader.Size = new System.Drawing.Size(657, 409);
            this.txt_requestHeader.TabIndex = 0;
            this.txt_requestHeader.Text = "";
            // 
            // txt_requestBody
            // 
            this.txt_requestBody.DetectUrls = false;
            this.txt_requestBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_requestBody.Location = new System.Drawing.Point(0, 0);
            this.txt_requestBody.Name = "txt_requestBody";
            this.txt_requestBody.Size = new System.Drawing.Size(663, 415);
            this.txt_requestBody.TabIndex = 0;
            this.txt_requestBody.Text = "";
            // 
            // txt_responseHeader
            // 
            this.txt_responseHeader.DetectUrls = false;
            this.txt_responseHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_responseHeader.Location = new System.Drawing.Point(3, 3);
            this.txt_responseHeader.Name = "txt_responseHeader";
            this.txt_responseHeader.Size = new System.Drawing.Size(657, 409);
            this.txt_responseHeader.TabIndex = 0;
            this.txt_responseHeader.Text = "";
            // 
            // ShowResponse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 441);
            this.Controls.Add(this.tab_requestBody);
            this.Name = "ShowResponse";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "请求响应";
            this.Shown += new System.EventHandler(this.ShowResponse_Shown);
            this.tabPage3.ResumeLayout(false);
            this.tab_showInBrowser.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tab_requestBody.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabControl tab_showInBrowser;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabControl tab_requestBody;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.RichTextBox txt_responseBody;
        private System.Windows.Forms.RichTextBox txt_requestHeader;
        private System.Windows.Forms.RichTextBox txt_requestBody;
        private System.Windows.Forms.RichTextBox txt_responseHeader;
    }
}