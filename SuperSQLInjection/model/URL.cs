using System;
using System.Collections.Generic;
using System.Text;

namespace SuperSQLInjection.model
{
    class URL
    {
        public String url;
        public int level;
        public URL(String url, int level) {

            this.url = url;
            this.level = level;
        
        }
    }
}
