using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("account_group",Schema ="fn")]
    public partial class AccountGroup : GenericEntity<string>
    {
        public string account_group_name { get; set; }
        public string organization_id { get; set; }
    }
}