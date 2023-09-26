using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class EmployeeLoan : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Employee ID")]
        public string EmployeeBasicInfoId { get; set; }

        [DisplayName("Loan Category")]
        public string LoanCategoryId { get; set; }

        [DisplayName("Loan Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateLoan { get; set; }

        [DisplayName("Value (Rp)")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal Value { get; set; }

        [DisplayName("Payroll Deduction")]
        public int PayrollDeduction { get; set; }

        [DisplayName("Repayment Start Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? RepaymentStartDate { get; set; }


        //Not Mapped
        //public LoanCategory LoanCategory { get; set; }
        public InfoEmployee infoEmployee { get; set; }

        public string LoanCategoryName { get; set; }

        [DisplayName("Interest Value (Rp)")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal InterestValue { get; set; }

        [DisplayName("Interest per Month (%)")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal InterestMonth { get; set; }

        [DisplayName("Repayment End Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? RepaymentEndDate
        {
            get
            {
                if (PayrollDeduction != 0 && RepaymentStartDate != null)
                {
                    return Convert.ToDateTime(RepaymentStartDate).AddMonths(PayrollDeduction);
                }
                else { return null; }
            }
        }

        [DisplayName("Remaining Balance (Rp)")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal Remaining { get; set; }

        public bool isPaid { get; set; }

        [DisplayName("Total Installment (Rp)")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? ValueTotalInstallment { get; set; }

        [DisplayName("Monthly Installement Amount")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? ValueInstallmentAmount { get; set; }

        [DisplayName("Interest Amount")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? ValueInterestAmount { get; set; }

        public int? Status { get; set; }

        public string LogDescription { get; set; }

        public int? StatusApproval { get; set; }

        public bool IsCorporateLoan { get; set; }

        public string StatusDesc { get; set; }
    }
}