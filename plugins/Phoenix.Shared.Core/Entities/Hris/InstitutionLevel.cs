using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("institution_level",Schema ="hr")]
    public partial class InstitutionLevel : GenericEntity<string>
    {
        public string competence_id { get; set; }
        public string level_position_id { get; set; }
        public string level_req { get; set; }
    }
}