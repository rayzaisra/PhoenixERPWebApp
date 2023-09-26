using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class CostCenterCampaignDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(255)]
        public string address { get; set; }
        public int? cost { get; set; }
        [StringLength(50)]
        public string employee_basic_info_id { get; set; }
        public DateTime? end_date { get; set; }
        public long? external_client_id { get; set; }
        [StringLength(2147483647)]
        public string notes { get; set; }
        [StringLength(50)]
        public string phone_no { get; set; }
        public DateTime? start_date { get; set; }
    }
    public class CostCenterCampaignNew
    {
        [StringLength(255)]
        public string address { get; set; }
        public int? cost { get; set; }
        [StringLength(50)]
        public string employee_basic_info_id { get; set; }
        public DateTime? end_date { get; set; }
        public long? external_client_id { get; set; }
        [StringLength(2147483647)]
        public string notes { get; set; }
        [StringLength(50)]
        public string phone_no { get; set; }
        public DateTime? start_date { get; set; }
    }            
    public class CostCenterCampaignSearch : SearchParameter
    {
        public string Search_address { get; set; }
        public int? Search_cost { get; set; }
        public string Search_employee_basic_info_id { get; set; }
        public DateTime? Search_end_date { get; set; }
        public long? Search_external_client_id { get; set; }
        public string Search_notes { get; set; }
        public string Search_phone_no { get; set; }
        public DateTime? Search_start_date { get; set; }
        
    }
}