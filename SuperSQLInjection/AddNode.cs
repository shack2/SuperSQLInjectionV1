using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using tools;

namespace SuperSQLInjection
{
    public partial class AddNode : Form
    {
        public AddNode()
        {
            InitializeComponent();
        }

        public TreeNode tn = null;
        public TreeView tvw = null;
        public int type = 0;
  
        private void btn_addNode_Click(object sender, EventArgs e)
        {
            if (this.txt_node_text.TextLength <= 0)
            {
                MessageBox.Show("请输入节点的值！");
                return;
            }
            TreeNode ctn = new TreeNode(this.txt_node_text.Text);
            if (type == 1)
            {
                ctn.Tag = "dbs";
                
                if (Tools.isExistsNode(tvw.Nodes,this.txt_node_text.Text))
                {
                    MessageBox.Show("已存在相同的节点！");
                }
                else { 
                    tvw.Nodes.Add(ctn);
                }
            }

            else {

                if (tn != null)
                {
                    if ("dbs".Equals(tn.Tag))
                    {
                        ctn.Tag = "table";
                    }
                    else if ("table".Equals(tn.Tag))
                    {
                        ctn.Tag = "column";
                    }
                    if (Tools.isExistsNode(tn.Nodes, this.txt_node_text.Text))
                    {
                        MessageBox.Show("已存在相同的节点！");
                    }
                    else
                    {
                       tn.Nodes.Add(ctn);
                    }
                    
                }
                else
                {
                    MessageBox.Show("请选择添加表或列对应的数据库或表！");
                }
            }
            
           
        }
    }
}
