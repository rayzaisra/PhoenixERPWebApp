using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class BusinessUnitTypeDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(255)]
        public string business_unit_type_name { get; set; }
        [StringLength(50)]
        [Required]
        public string organization_id { get; set; }
    }
    public class BusinessUnitTypeNew
    {
        [StringLength(255)]
        public string business_unit_type_name { get; set; }
        [StringLength(50)]
        [Required]
        public string organization_id { get; set; }
    }            
    public class BusinessUnitTypeSearch : SearchParameter
    {
        public string Search_business_unit_type_name { get; set; }
        public string Search_organization_id { get; set; }
        
    }
}