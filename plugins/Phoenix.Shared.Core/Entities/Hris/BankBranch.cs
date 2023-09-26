using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("bank_branch",Schema ="hr")]
    public partial class BankBranch : GenericEntity<string>
    {
        public string adress { get; set; }
        public string bank_branch_id { get; set; }
        public string bank_id { get; set; }
        public string branch_code { get; set; }
        public string phone { get; set; }
    }
}