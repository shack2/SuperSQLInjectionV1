using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SuperSQLInjection.model
{
    class GetDataPam
    {

        public List<String> columns = null;
        public int limit = 0;
        public String dbname = "";
        public String table = "";
        public Boolean isMuStr = false;//开启多字节
        public ListViewItem lvi = null;
        public int data_count =0;
    }
}
