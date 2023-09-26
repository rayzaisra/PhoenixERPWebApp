using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("account_management",Schema ="pm")]
    public partial class AccountManagement : GenericEntity<string>
    {
        public string user_id { get; set; }
    }
}