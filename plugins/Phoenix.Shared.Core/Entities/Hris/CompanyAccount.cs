using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("company_account",Schema ="hr")]
    public partial class CompanyAccount : GenericEntity<string>
    {
        public string bankbranch_id { get; set; }
        public string bank_account_id { get; set; }
        public string bank_id { get; set; }
        public string branchcomapany_id { get; set; }
        public string branch_name { get; set; }
    }
}