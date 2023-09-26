using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class OrganizationJobLevelDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string business_unit_id { get; set; }
        [StringLength(50)]
        public string job_level_id { get; set; }
    }
    public class OrganizationJobLevelNew
    {
        [StringLength(50)]
        public string business_unit_id { get; set; }
        [StringLength(50)]
        public string job_level_id { get; set; }
    }            
    public class OrganizationJobLevelSearch : SearchParameter
    {
        public string Search_business_unit_id { get; set; }
        public string Search_job_level_id { get; set; }
        
    }
}