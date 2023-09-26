using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class CompetenceDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string competence_name { get; set; }
        [StringLength(50)]
        public string level1 { get; set; }
        [StringLength(50)]
        public string level2 { get; set; }
        [StringLength(50)]
        public string level3 { get; set; }
        [StringLength(50)]
        public string level4 { get; set; }
        [StringLength(50)]
        public string level5 { get; set; }
    }
    public class CompetenceNew
    {
        [StringLength(50)]
        public string competence_name { get; set; }
        [StringLength(50)]
        public string level1 { get; set; }
        [StringLength(50)]
        public string level2 { get; set; }
        [StringLength(50)]
        public string level3 { get; set; }
        [StringLength(50)]
        public string level4 { get; set; }
        [StringLength(50)]
        public string level5 { get; set; }
    }            
    public class CompetenceSearch : SearchParameter
    {
        public string Search_competence_name { get; set; }
        public string Search_level1 { get; set; }
        public string Search_level2 { get; set; }
        public string Search_level3 { get; set; }
        public string Search_level4 { get; set; }
        public string Search_level5 { get; set; }
        
    }
}