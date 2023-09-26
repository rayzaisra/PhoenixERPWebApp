using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("employee_loan", Schema = "hr")]
    public partial class EmployeeLoan : GenericEntity<string>
    {
        public string name { get; set; }
        public string loan_category_id { get; set; }
        public DateTime? date_loan { get; set; }
        public decimal value { get; set; }
        public int payroll_deduction { get; set; }
        public DateTime? date_repaymentstartdate { get; set; }
        public string description { get; set; }
        public string grade { get; set; }
    }
}