using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using model;

namespace SuperSQLInjection
{
    public partial class ShowResponse : Form
    {
        public ShowResponse()
        {
            InitializeComponent();
        }

        public ServerInfo server = new ServerInfo();

        private void ShowResponse_Shown(object sender, EventArgs e)
        {
            this.txt_requestHeader.Text = this.server.reuqestHeader;
            this.txt_requestBody.Text = this.server.reuqestBody;
            this.txt_responseHeader.Text = this.server.header;
            this.txt_responseBody.Text = this.server.body;
            
            this.webBrowser1.ScriptErrorsSuppressed = true;
            this.webBrowser1.DocumentText = this.server.body;
        }

        public void setResult()
        {
            this.txt_requestHeader.Text = this.server.reuqestHeader;
            this.txt_requestBody.Text = this.server.reuqestBody;
            this.txt_responseHeader.Text = this.server.header;
            this.txt_responseBody.Text = this.server.body;

            this.webBrowser1.ScriptErrorsSuppressed = true;
            this.webBrowser1.DocumentText = this.server.body;
        }

        private void txt_responseBody_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.F)
            {
                FindString fs = new FindString();
                fs.txtbox = this.txt_responseBody;
                fs.Show();
            }
        }

    }
}
