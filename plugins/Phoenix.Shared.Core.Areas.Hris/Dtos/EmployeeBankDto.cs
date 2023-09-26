using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class EmployeeBankDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(100)]
        public string account_name { get; set; }
        [StringLength(60)]
        public string account_no { get; set; }
        [StringLength(10)]
        public string bank_address { get; set; }
        [StringLength(10)]
        public string bank_city { get; set; }
        [StringLength(50)]
        public string bank_id { get; set; }
        [StringLength(10)]
        public string branch { get; set; }
        [StringLength(50)]
        public string currency_id { get; set; }
        [StringLength(50)]
        public string employee_basic_info_id { get; set; }
        public DateTime? end_date { get; set; }
        public DateTime? start_date { get; set; }
        [StringLength(50)]
        public string sub_bank_id { get; set; }
    }
    public class EmployeeBankNew
    {
        [StringLength(100)]
        public string account_name { get; set; }
        [StringLength(60)]
        public string account_no { get; set; }
        [StringLength(10)]
        public string bank_address { get; set; }
        [StringLength(10)]
        public string bank_city { get; set; }
        [StringLength(50)]
        public string bank_id { get; set; }
        [StringLength(10)]
        public string branch { get; set; }
        [StringLength(50)]
        public string currency_id { get; set; }
        [StringLength(50)]
        public string employee_basic_info_id { get; set; }
        public DateTime? end_date { get; set; }
        public DateTime? start_date { get; set; }
        [StringLength(50)]
        public string sub_bank_id { get; set; }
    }            
    public class EmployeeBankSearch : SearchParameter
    {
        public string Search_account_name { get; set; }
        public string Search_account_no { get; set; }
        public string Search_bank_address { get; set; }
        public string Search_bank_city { get; set; }
        public string Search_bank_id { get; set; }
        public string Search_branch { get; set; }
        public string Search_currency_id { get; set; }
        public string Search_employee_basic_info_id { get; set; }
        public DateTime? Search_end_date { get; set; }
        public DateTime? Search_start_date { get; set; }
        public string Search_sub_bank_id { get; set; }
        
    }
}