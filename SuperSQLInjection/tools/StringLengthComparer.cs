using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tools
{
    class StringLengthComparer : System.Collections.IComparer
    {
        public int Compare(object x, object y)
        {
            string s1 = (string)x;

            string s2 = (string)y;

            if (s1.Length > s2.Length) return -1;

            if (s1.Length < s2.Length) return 1;
            return 0;

        }
        
    }
}
