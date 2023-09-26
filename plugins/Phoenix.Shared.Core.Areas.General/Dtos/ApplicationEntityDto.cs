using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class ApplicationEntityDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(450)]
        public string display_name { get; set; }
        [StringLength(450)]
        public string entity_name { get; set; }
        [StringLength(50)]
        public string organization_id { get; set; }
    }
    public class ApplicationEntityNew
    {
        [StringLength(450)]
        public string display_name { get; set; }
        [StringLength(450)]
        public string entity_name { get; set; }
        [StringLength(50)]
        public string organization_id { get; set; }
    }            
    public class ApplicationEntitySearch : SearchParameter
    {
        public string Search_display_name { get; set; }
        public string Search_entity_name { get; set; }
        public string Search_organization_id { get; set; }
        
    }
}