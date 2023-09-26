using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class JobPaDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string approval_id { get; set; }
 
        [StringLength(50)]
        public string department { get; set; }
        public string company_name { get; set; }
        public string campaign_name { get; set; }
        public string job_name { get; set; }
        [StringLength(50)]
        public string currency { get; set; }
        [StringLength(50)]
        public string feetype { get; set; }
        public double? fee_percentage { get; set; }
        public double? feetotal { get; set; }
        public int? hour_per_day { get; set; }
        [StringLength(50)]
        public string job_detail_id { get; set; }
        [StringLength(50)]
        public string mother_pca { get; set; }
        public DateTime? pa_date { get; set; }
        [StringLength(50)]
        public string pa_number { get; set; }
        [StringLength(10)]
        public string rate_card_id { get; set; }
    }
    public class JobPaNew
    {
        [StringLength(50)]
        public string approval_id { get; set; }
 
        [StringLength(50)]
        public string department { get; set; }
        [StringLength(50)]
        public string currency { get; set; }
        [StringLength(50)]
        public string feetype { get; set; }
        public int? hour_per_day { get; set; }
        [StringLength(50)]
        public string job_detail_id { get; set; }
        public double? margin { get; set; }
        [StringLength(50)]
        public string mother_pca { get; set; }
        public decimal? fee_percentage { get; set; }
        public decimal? feetotal { get; set; }
        public DateTime? pa_date { get; set; }
        [StringLength(50)]
        public string pa_number { get; set; }

        [StringLength(10)]
        public string rate_card_id { get; set; }
    }            
    public class JobPaSearch : SearchParameter
    {
        public string Search_approval_id { get; set; }
        public decimal? Search_asf { get; set; }
        public string Search_department { get; set; }
        public string Search_detail { get; set; }
        public string Search_division { get; set; }
        public int? Search_hour_per_day { get; set; }
        public string Search_job_detail_id { get; set; }
        public double? Search_margin { get; set; }
        public string Search_mother_pca { get; set; }
        public decimal? Search_pa_cost { get; set; }
        public DateTime? Search_pa_date { get; set; }
        public string Search_pa_number { get; set; }
        public decimal? Search_pph { get; set; }
        public decimal? Search_ppn { get; set; }
        public string Search_rate_card_id { get; set; }
        
    }
    public class AccountDto
    {
        public string account_group_id { get; set; }
        public string account_name { get; set; }
        public string account_number { get; set; }
        public int account_level { get; set; }
        public string account_parent_id { get; set; }
        public string cost_sharing_allocation_id { get; set; }
        public string organization_id { get; set; }
    }
    public class Listsubtask
    {
        List<PaDetailDto> detail { get; set; }
    }
}