using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class ReimburseDto
    {
        [Required]
            public string Id { get; set; }
        public int? amount { get; set; }
        [StringLength(50)]
        public string employee_basic_info_id { get; set; }
        [StringLength(50)]
        public string receipt { get; set; }
        public DateTime? reimburse_date { get; set; }
        [StringLength(2147483647)]
        public string reimburse_description { get; set; }
        [StringLength(50)]
        public string type_reimburse { get; set; }
    }
    public class ReimburseNew
    {
        public int? amount { get; set; }
        [StringLength(50)]
        public string employee_basic_info_id { get; set; }
        [StringLength(50)]
        public string receipt { get; set; }
        public DateTime? reimburse_date { get; set; }
        [StringLength(2147483647)]
        public string reimburse_description { get; set; }
        [StringLength(50)]
        public string type_reimburse { get; set; }
    }            
    public class ReimburseSearch : SearchParameter
    {
        public int? Search_amount { get; set; }
        public string Search_employee_basic_info_id { get; set; }
        public string Search_receipt { get; set; }
        public DateTime? Search_reimburse_date { get; set; }
        public string Search_reimburse_description { get; set; }
        public string Search_type_reimburse { get; set; }
        
    }
}