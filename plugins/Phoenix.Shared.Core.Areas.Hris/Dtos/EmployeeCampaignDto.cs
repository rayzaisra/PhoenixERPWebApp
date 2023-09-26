using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class EmployeeCampaignDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string client_brief_id { get; set; }
        [StringLength(50)]
        public string employee_basic_info_id { get; set; }
        public DateTime? end_date { get; set; }
        [StringLength(150)]
        public string file_upload { get; set; }
        [StringLength(2147483647)]
        public string notes { get; set; }
        public DateTime? start_date { get; set; }
        [StringLength(50)]
        public string subcampaign_id { get; set; }
    }
    public class EmployeeCampaignNew
    {
        [StringLength(50)]
        public string client_brief_id { get; set; }
        [StringLength(50)]
        public string employee_basic_info_id { get; set; }
        public DateTime? end_date { get; set; }
        [StringLength(150)]
        public string file_upload { get; set; }
        [StringLength(2147483647)]
        public string notes { get; set; }
        public DateTime? start_date { get; set; }
        [StringLength(50)]
        public string subcampaign_id { get; set; }
    }            
    public class EmployeeCampaignSearch : SearchParameter
    {
        public string Search_client_brief_id { get; set; }
        public string Search_employee_basic_info_id { get; set; }
        public DateTime? Search_end_date { get; set; }
        public string Search_file_upload { get; set; }
        public string Search_notes { get; set; }
        public DateTime? Search_start_date { get; set; }
        public string Search_subcampaign_id { get; set; }
        
    }
}