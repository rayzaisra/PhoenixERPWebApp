using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class EmployeeLoanDto
    {
        [StringLength(50)]
        [Required]
        public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string name { get; set; }
        [Required]
        public decimal value { get; set; }
        [Required]
        public DateTime? date_loan { get; set; }
        [StringLength(50)]
        [Required]
        public string description { get; set; }
        [StringLength(50)]
        public string loan_category_name { get; set; }
        [Required]
        public int payroll_deduction { get; set; }
        [Required]
        public DateTime? date_repaymentstartdate { get; set; }
        public string loan_category_id { get; set; }
        public string grade { get; set; }
        public decimal remaining { get; set; }
        //public DateTime created_on { get; set; }
    }

    public class EmployeeLoanEditDto
    {
        [StringLength(50)]
        [Required]
        public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string name { get; set; }
        [Required]
        public decimal value { get; set; }
        [Required]
        public string date_loan { get; set; }
        [StringLength(50)]
        [Required]
        public string description { get; set; }
        [StringLength(50)]
        public string loan_category_name { get; set; }
        [Required]
        public int payroll_deduction { get; set; }
        [Required]
        public string date_repaymentstartdate { get; set; }
        public string loan_category_id { get; set; }
        public string grade { get; set; }
        public decimal remaining { get; set; }
        //public DateTime created_on { get; set; }
    }

    public class EmployeeLoanUpdateDto
    {
        [StringLength(50)]
        [Required]
        public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string name { get; set; }
        [Required]
        public decimal value { get; set; }
        [Required]
        public DateTime? date_loan { get; set; }
        [StringLength(50)]
        [Required]
        public string description { get; set; }
        [Required]
        public int payroll_deduction { get; set; }
        [Required]
        public DateTime? date_repaymentstartdate { get; set; }
        public string loan_category_id { get; set; }

        public string grade { get; set; }
    }
    public class EmployeeLoanNew
    {
        [StringLength(50)]
        [Required]
        public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string name { get; set; }
        [Required]
        public decimal value { get; set; }
        [Required]
        public DateTime? date_loan { get; set; }
        [StringLength(50)]
        [Required]
        public string description { get; set; }
        [Required]
        public int payroll_deduction { get; set; }
        [Required]
        public DateTime? date_repaymentstartdate { get; set; }
        public string loan_category_id { get; set; }

        public string grade { get; set; }
        public DateTime created_on { get; set; }
    }
    public class EmployeeLoanSearch : SearchParameter
    {
        public string Search_name { get; set; }
        public string Search_description { get; set; }
        //public string Search_type { get; set; }

    }
}