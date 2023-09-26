using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("employee_bank",Schema ="hr")]
    public partial class EmployeeBank : GenericEntity<string>
    {
        public string account_name { get; set; }
        public string account_no { get; set; }
        public string bank_address { get; set; }
        public string bank_city { get; set; }
        public string bank_id { get; set; }
        public string branch { get; set; }
        public string currency_id { get; set; }
        public string employee_basic_info_id { get; set; }
        public DateTime? end_date { get; set; }
        public DateTime? start_date { get; set; }
        public string sub_bank_id { get; set; }
    }
}