using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("institution_competence",Schema ="hr")]
    public partial class InstitutionCompetence : GenericEntity<string>
    {
        public string competence_id { get; set; }
        public string institution_id { get; set; }
    }
}