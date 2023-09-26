using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Phoenix.Shared.Core.Areas.General.Dtos;
using Phoenix.Shared.Responses;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class JobTitleDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string business_unit_id { get; set; }
        [StringLength(100)]
        public string job_grade_id { get; set; }
        [StringLength(100)]
        public string parent_id { get; set; }
        [Required][StringLength(50)]
        public string title_code { get; set; }
        [Required][StringLength(100)]
        public string title_name { get; set; }
        public string job_level_id { get; set; }
        public string superior { get; set; }
        public GeneralResponseList<BusinessUnitParent>parent { get; set; }
        public DateTime? created_on { get; set; }

      
    }
    public class JobTitleNew
    {
        public string Id { get; set; }
        [StringLength(50)]
        public string business_unit_id { get; set; }
        [StringLength(100)]
        public string job_grade_id { get; set; }
        [StringLength(100)]
        public string parent_id { get; set; }
        [StringLength(50)]
        public string title_code { get; set; }
        [StringLength(100)]
        public string title_name { get; set; }
        public string job_level_id { get; set; }
        public string superior { get; set; }
    }            
    public class JobTitleSearch : SearchParameter
    {
        public string Search_business_unit_id { get; set; }
        public string Search_jobgrade_id { get; set; }
        public string Search_parent_id { get; set; }
        public string Search_title_code { get; set; }
        public string Search_title_name { get; set; }
        public string Search_job_title_id { get; set; }
        public string Search_job_grade_id { get; set; }
        public string groupId { get; set; }

    }
}