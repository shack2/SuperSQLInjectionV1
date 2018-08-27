using System;
using System.Collections.Generic;
using System.Text;

namespace SuperSQLInjection.model
{
    [Serializable]
    public enum KeyType
    {
        Key=0,
        Reg = 1,
        Code =2,
        Time = 3,
        EQLen = 4,
        MaxLen =5,
        MinLen =6
    }
}
