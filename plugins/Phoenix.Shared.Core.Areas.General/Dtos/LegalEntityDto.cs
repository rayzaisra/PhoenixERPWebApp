using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class LegalEntityDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(400)]
        [Required]
        public string legal_entity_name { get; set; }
        [StringLength(50)]
        [Required]
        public string organization_id { get; set; }
    }
    public class LegalEntityNew
    {
        [StringLength(400)]
        [Required]
        public string legal_entity_name { get; set; }
        [StringLength(50)]
        [Required]
        public string organization_id { get; set; }
    }            
    public class LegalEntitySearch : SearchParameter
    {
        public string Search_legal_entity_name { get; set; }
        public string Search_organization_id { get; set; }
        
    }
}