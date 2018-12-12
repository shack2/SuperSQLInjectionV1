using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SuperSQLInjection
{
    public partial class FindString : Form
    {
        public FindString()
        {
            InitializeComponent();
        }
        public int searchPoint = 0;
        public TextBox txtbox = null;
        private void btn_find_Click(object sender, EventArgs e)
        {
            //查找下一个

            if (txtbox.Text == "")
            {
                //没内容
                MessageBox.Show("查找内容为空，请输入查找内容", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                //有查找内容时
                searchPoint = txtbox.Text.IndexOf(this.txt_key.Text, searchPoint);//用IndexOf索引
                if (searchPoint < 0)
                {
                    //没找到
                    MessageBox.Show("已到文本末尾，没有找到", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    searchPoint = 0;
                }
                else
                {
                    //找到了，选中文本
                    txtbox.Focus();
                    txtbox.Select(searchPoint, this.txt_key.Text.Length);
                    searchPoint = searchPoint + this.txt_key.Text.Length;
                    this.Hide();

                }
            }
        }

        private void txt_key_TextChanged(object sender, EventArgs e)
        {
            int count = 0; //计数器
            string search = this.txt_key.Text; //要查的字符串
            if ("".Equals(search))
            {
                return;
            }

            for (int i = 0; i <= txtbox.Text.Length - search.Length; i++)
            {
                if (txtbox.Text.Substring(i, search.Length).ToLower() == search.ToLower())
                {
                    count++;
                }
            }
            this.label2.Text = "匹配："+count.ToString();
        }
    }
}
