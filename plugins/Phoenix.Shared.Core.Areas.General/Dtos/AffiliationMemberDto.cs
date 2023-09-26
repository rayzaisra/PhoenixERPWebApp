using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class AffiliationMemberDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string affiliation_id { get; set; }
        [StringLength(50)]
        [Required]
        public string business_unit_id { get; set; }
    }
    public class AffiliationMemberNew
    {
        [StringLength(50)]
        [Required]
        public string affiliation_id { get; set; }
        [StringLength(50)]
        [Required]
        public string business_unit_id { get; set; }
    }            
    public class AffiliationMemberSearch : SearchParameter
    {
        public string Search_affiliation_id { get; set; }
        public string Search_business_unit_id { get; set; }
        
    }
}