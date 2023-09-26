using System;
using System.Collections.Generic;
using System.Text;

namespace CodeMarvel.Infrastructure.Options
{
    [Serializable]
    public class ConnectionStringOption
    {
            
        public string LKJReportController { get; set; }
        public string LKJReport { get; set; }
        public string baseConnectionString { get; set; }
    }
}
