using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class MppDetailDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(255)]
        public string employment_status { get; set; }
        [StringLength(50)]
        public string job_tittle_id { get; set; }
        public decimal? mpp_budged { get; set; }
        [StringLength(50)]
        public string mpp_id { get; set; }
        [StringLength(255)]
        public string ttf { get; set; }
        public string job_title_name { get; set; }
        public string job_grade_id { get; set; }
        public string Description { get; set; }
        public string status { get; set; }
        public string porject_name { get; set; }
        public string business_unit_id { get; set; }
        public string job_grade_name { get; set; }
        public string budget_souce { get; set; }
    }
    public class MppDetailNew
    {
        [StringLength(255)]
        public string employment_status { get; set; }
        [StringLength(50)]
        public string job_tittle_id { get; set; }
        public decimal? mpp_budged { get; set; }
        [StringLength(50)]
        public string mpp_id { get; set; }
        [StringLength(255)]
        public string ttf { get; set; }
    }            
    public class MppDetailSearch : SearchParameter
    {
        public string Search_employment_status { get; set; }
        public string Search_job_tittle_id { get; set; }
        public double? Search_mpp_budged { get; set; }
        public string Search_mpp_id { get; set; }
        public string Search_ttf { get; set; }
        public string Search_Fiscal_Year { get; set; }
        
    }
}