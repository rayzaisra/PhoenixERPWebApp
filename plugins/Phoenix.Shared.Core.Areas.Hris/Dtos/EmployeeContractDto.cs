using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class EmployeeContractDto
    {
        [Required]
            public string Id { get; set; }
        public int? active { get; set; }
        [StringLength(50)]
        public string candidate_recruitment_id { get; set; }
        [StringLength(2147483647)]
        public string contract_upload { get; set; }
        [StringLength(50)]
        public string employee_basic_info_id { get; set; }
        public DateTime? end_date { get; set; }
        [StringLength(2147483647)]
        public string notes { get; set; }
        public DateTime? start_date { get; set; }
    }
    public class EmployeeContractNew
    {
        public int? active { get; set; }
        [StringLength(50)]
        public string candidate_recruitment_id { get; set; }
        [StringLength(2147483647)]
        public string contract_upload { get; set; }
        [StringLength(50)]
        public string employee_basic_info_id { get; set; }
        public DateTime? end_date { get; set; }
        [StringLength(2147483647)]
        public string notes { get; set; }
        public DateTime? start_date { get; set; }
    }            
    public class EmployeeContractSearch : SearchParameter
    {
        public int? Search_active { get; set; }
        public string Search_candidate_recruitment_id { get; set; }
        public string Search_contract_upload { get; set; }
        public string Search_employee_basic_info_id { get; set; }
        public DateTime? Search_end_date { get; set; }
        public string Search_notes { get; set; }
        public DateTime? Search_start_date { get; set; }
        
    }
}