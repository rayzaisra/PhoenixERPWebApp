using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("institution",Schema ="hr")]
    public partial class Institution : GenericEntity<string>
    {
        public string institution_address { get; set; }
        public string institution_name { get; set; }
        public string institution_type { get; set; }
    }
}