using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperSQLInjection.model
{
    [Serializable]
    public class DataBase
    {
        public SerializableDictionary<String, SerializableDictionary<String, List<String>>> tables=new SerializableDictionary<String, SerializableDictionary<String, List<String>>>();
    }
}
