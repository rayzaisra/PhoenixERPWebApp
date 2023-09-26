using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class InstitutionCompetenceDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string competence_id { get; set; }
        [StringLength(50)]
        [Required]
        public string institution_id { get; set; }
    }
    public class InstitutionCompetenceNew
    {
        [StringLength(50)]
        [Required]
        public string competence_id { get; set; }
        [StringLength(50)]
        [Required]
        public string institution_id { get; set; }
    }            
    public class InstitutionCompetenceSearch : SearchParameter
    {
        public string Search_competence_id { get; set; }
        public string Search_institution_id { get; set; }
        
    }
}