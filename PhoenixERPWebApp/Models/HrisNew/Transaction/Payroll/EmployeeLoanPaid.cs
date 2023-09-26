using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class EmployeeLoanPaid : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Employee Loan Id")]
        public string EmployeeLoanId { get; set; }

        [DisplayName("Employee Basic Info Id")]
        public string EmployeeBasicInfoId { get; set; }

        [DisplayName("Month")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DatePaid { get; set; }

        public string DatePaidString { get; set; }

        [DisplayName("Value (Rp)")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? Value { get; set; }

        [DisplayName("Status")]
        public int? Status { get; set; }

        [DisplayName("Early Paid")]
        public DateTime? EarlyPaidDate { get; set; }

        [DisplayName("Status")]
        public string StatusDesc { get { return LoanStatus.StatusName(Status); } }
        [DisplayName("Value (Rp)")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? ValuePaid { get; set; }
        [DisplayName("Log")]
        public string Log { get; set; }
    }
}