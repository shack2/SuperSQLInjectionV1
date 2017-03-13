using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SuperSQLInjection.model
{
    public class SelectNode
    {
        public TreeNode tn = new TreeNode();
        public int limit = 0;
        public String dbname = "";
        public String tableName = "";
        public String columnName = "";
    }
}
