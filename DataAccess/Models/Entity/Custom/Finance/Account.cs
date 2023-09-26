using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models.Entity.Custom.Finance
{
    public partial class Account
    {
        public string LegalEntityName { get; set; }

        public string AccountNumber { get; set; }

        public string AccountName { get; set; }

        public int AccountLevel { get; set; }

        public string ParentAccountNumber { get; set; }

        public string AccountGroupName { get; set; }

        public string NormalCondition { get; set; }

        public string ReportUsage { get; set; }
    }
}
