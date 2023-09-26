using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("account",Schema ="fn")]
    public partial class Account : GenericEntity<string>
    {
        public string account_group_id { get; set; }
        public string account_name { get; set; }
        public string account_number { get; set; }
        public int account_level { get; set; }
        public string account_parent_id { get; set; }
        public string cost_sharing_allocation_id { get; set; }
        public string organization_id { get; set; }
    }
}