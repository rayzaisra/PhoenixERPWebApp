using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("competence_level",Schema ="hr")]
    public partial class CompetenceLevel : GenericEntity<string>
    {
        public string competence_id { get; set; }
        public string level_position_id { get; set; }
        public int? level_req { get; set; }
    }
}