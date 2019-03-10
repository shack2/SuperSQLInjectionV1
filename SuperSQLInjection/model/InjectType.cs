using System;
using System.Collections.Generic;
using System.Text;

namespace SuperSQLInjection.model
{
    [Serializable]
    public enum InjectType
    {
        UnKnow = 0,
        Blind= 1,
        Error=2,
        Union = 3
    }
}
