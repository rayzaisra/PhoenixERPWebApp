using NPoco;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Common
{
    public class BaseDB
    {
        public IDatabase Database { get; set; }
        public Database TestDatabase { get; set; }
    }
}
