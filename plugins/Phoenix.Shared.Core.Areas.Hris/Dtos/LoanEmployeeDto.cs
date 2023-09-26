using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class LoanEmployeeDto
    {
        [Required]
            public string Id { get; set; }
        public int? count_repayment { get; set; }
        [Required]
        public DateTime? date_loan { get; set; }
        [Required]
        public long? employee_basic_info_id { get; set; }
        public int? monthly_repayment { get; set; }
        [StringLength(2147483647)]
        [Required]
        public string reason { get; set; }
        public DateTime? repayment_end_date { get; set; }
        public DateTime? repayment_start_date { get; set; }
        [Required]
        public int? value { get; set; }
    }
    public class LoanEmployeeNew
    {
        public int? count_repayment { get; set; }
        [Required]
        public DateTime? date_loan { get; set; }
        [Required]
        public long? employee_basic_info_id { get; set; }
        public int? monthly_repayment { get; set; }
        [StringLength(2147483647)]
        [Required]
        public string reason { get; set; }
        public DateTime? repayment_end_date { get; set; }
        public DateTime? repayment_start_date { get; set; }
        [Required]
        public int? value { get; set; }
    }            
    public class LoanEmployeeSearch : SearchParameter
    {
        public int? Search_count_repayment { get; set; }
        public DateTime? Search_date_loan { get; set; }
        public long? Search_employee_basic_info_id { get; set; }
        public int? Search_monthly_repayment { get; set; }
        public string Search_reason { get; set; }
        public DateTime? Search_repayment_end_date { get; set; }
        public DateTime? Search_repayment_start_date { get; set; }
        public int? Search_value { get; set; }
        
    }
}