using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("competence",Schema ="hr")]
    public partial class Competence : GenericEntity<string>
    {
        public string competence_name { get; set; }
        public string level1 { get; set; }
        public string level2 { get; set; }
        public string level3 { get; set; }
        public string level4 { get; set; }
        public string level5 { get; set; }
    }
}