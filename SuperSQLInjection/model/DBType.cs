using System;
using System.Collections.Generic;
using System.Text;

namespace SuperSQLInjection.model
{
    [Serializable]
    public enum DBType
    {
        UnKnow=0,
        Access=1,  
        MySQL5 = 2,
        SQLServer = 3,
        Oracle = 4,
        MySQL4 = 5
    }
}
