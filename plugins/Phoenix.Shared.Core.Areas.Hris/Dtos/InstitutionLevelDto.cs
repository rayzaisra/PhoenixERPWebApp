using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class InstitutionLevelDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string competence_id { get; set; }
        [StringLength(50)]
        [Required]
        public string level_position_id { get; set; }
        [StringLength(50)]
        [Required]
        public string level_req { get; set; }
    }
    public class InstitutionLevelNew
    {
        [StringLength(50)]
        [Required]
        public string competence_id { get; set; }
        [StringLength(50)]
        [Required]
        public string level_position_id { get; set; }
        [StringLength(50)]
        [Required]
        public string level_req { get; set; }
    }            
    public class InstitutionLevelSearch : SearchParameter
    {
        public string Search_competence_id { get; set; }
        public string Search_level_position_id { get; set; }
        public string Search_level_req { get; set; }
        
    }
}