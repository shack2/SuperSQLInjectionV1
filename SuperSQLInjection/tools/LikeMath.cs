using System;
using System.Collections.Generic;
using System.Text;

namespace SuperSQLInjection.tools
{
   public class LikeMath
    {
        /// <summary&gt;
        /// 取最小的一位数
        /// </summary&gt;
        /// <param name="first"&gt;</param&gt;
        /// <param name="second"&gt;</param&gt;
        /// <param name="third"&gt;</param&gt;
        /// <returns&gt;</returns&gt;
        public static int LowerOfThree(int first, int second, int third)
        {

            int min = Math.Min(first, second);

            return Math.Min(min, third);

        }

        public static int like(string str1, string str2)
        {

            int[,] Matrix;

            int n = str1.Length;

            int m = str2.Length;



            int temp = 0;

            char ch1;

            char ch2;

            int i = 0;

            int j = 0;

            if (n == 0)
            {

                return m;

            }

            if (m == 0)
            {



                return n;

            }

            Matrix = new int[n + 1, m + 1];



            for (i = 0; i <= n; i++)
            {

                //初始化第一列

                Matrix[i, 0] = i;

            }



            for (j = 0; j <= m; j++)
            {

                //初始化第一行

                Matrix[0, j] = j;

            }



            for (i = 1; i <= n; i++)
            {

                ch1 = str1[i - 1];

                for (j = 1; j <= m; j++)
                {

                    ch2 = str2[j - 1];

                    if (ch1.Equals(ch2))
                    {

                        temp = 0;

                    }

                    else
                    {

                        temp = 1;

                    }

                    Matrix[i, j] = LowerOfThree(Matrix[i - 1, j] + 1, Matrix[i, j - 1] + 1, Matrix[i - 1, j - 1] + temp);

                }

            }

            return Matrix[n, m];

        }
        /// <summary&gt;

        /// 计算字符串相似度

        /// </summary&gt;

        /// <param name="str1"&gt;</param&gt;

        /// <param name="str2"&gt;</param&gt;

        /// <returns&gt;</returns&gt;

        public static int likePercent(string str1, string str2)
        {

            //int maxLenth = str1.Length &gt; str2.Length ? str1.Length : str2.Length;

            int val = like(str1, str2); 
            int l=(int)Math.Floor((1 - (decimal)val / Math.Max(str1.Length, str2.Length))*100);
            return l;

        }
    }
}
