using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("bast",Schema ="pm")]
    public partial class Bast : GenericEntity<string>
    {
        public string bast_number { get; set; }
        public string external_id { get; set; }
        public string po_id { get; set; }
    }
}