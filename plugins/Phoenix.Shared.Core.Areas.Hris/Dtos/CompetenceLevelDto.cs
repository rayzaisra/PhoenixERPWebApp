using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class CompetenceLevelDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string competence_id { get; set; }
        [StringLength(50)]
        public string level_position_id { get; set; }
        public int? level_req { get; set; }
    }
    public class CompetenceLevelNew
    {
        [StringLength(50)]
        public string competence_id { get; set; }
        [StringLength(50)]
        public string level_position_id { get; set; }
        public int? level_req { get; set; }
    }            
    public class CompetenceLevelSearch : SearchParameter
    {
        public string Search_competence_id { get; set; }
        public string Search_level_position_id { get; set; }
        public int? Search_level_req { get; set; }
        
    }
}