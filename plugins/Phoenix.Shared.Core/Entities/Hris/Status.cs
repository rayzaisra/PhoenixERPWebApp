using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("status", Schema = "hr")]
    public partial class Status : GenericEntity<string>
    {
        public string code { get; set; }
        public string description { get; set; }

        public string status_name { get; set; }
        public int? value { get; set; }
    }
}