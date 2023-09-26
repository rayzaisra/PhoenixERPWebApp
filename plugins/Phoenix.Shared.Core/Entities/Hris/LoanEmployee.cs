using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("loan_employee",Schema ="hr")]
    public partial class LoanEmployee : GenericEntity<string>
    {
        public int? count_repayment { get; set; }
        public DateTime? date_loan { get; set; }
        public long? employee_basic_info_id { get; set; }
        public int? monthly_repayment { get; set; }
        public string reason { get; set; }
        public DateTime? repayment_end_date { get; set; }
        public DateTime? repayment_start_date { get; set; }
        public int? value { get; set; }
    }
}