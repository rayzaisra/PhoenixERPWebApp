using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class AffiliationDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(400)]
        [Required]
        public string affiliation_name { get; set; }
        [StringLength(50)]
        [Required]
        public string organization_id { get; set; }
    }
    public class AffiliationNew
    {
        [StringLength(400)]
        [Required]
        public string affiliation_name { get; set; }
        [StringLength(50)]
        [Required]
        public string organization_id { get; set; }
    }            
    public class AffiliationSearch : SearchParameter
    {
        public string Search_affiliation_name { get; set; }
        public string Search_organization_id { get; set; }
        
    }
}