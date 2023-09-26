using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("package",Schema ="pm")]
    public partial class Package : GenericEntity<string>
    {
        public string description { get; set; }
        public long? job_category_id { get; set; }
        public string package { get; set; }
        public int? value { get; set; }
    }
}