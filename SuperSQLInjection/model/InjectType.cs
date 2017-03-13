using System;
using System.Collections.Generic;
using System.Text;

namespace SuperSQLInjection.model
{
    [Serializable]
    public enum InjectType
    {
        UnKnow = 0,
        Bool=1,
        Error=2,
        Union = 3,
        Sleep=4
    }
}
