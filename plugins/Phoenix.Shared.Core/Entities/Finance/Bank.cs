using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("bank",Schema ="fn")]
    public partial class Bank : GenericEntity<string>
    {
        public string bank_code { get; set; }
        public string bank_name { get; set; }
        public string organization_id { get; set; }
        public string swift_code { get; set; }
    }
}