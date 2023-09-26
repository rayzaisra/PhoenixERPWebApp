using Microsoft.AspNetCore.Http;

using PhoenixERPWebApp.Models.HrisNew.Transaction.Payroll;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class InfoEmployee
    {
        [DisplayName("Employee ID")]
        public string EmployeeId { get; set; }

        [DisplayName("Employee ID")]
        public string Id
        {
            get { return EmployeeId; }
            set { EmployeeId = value; }
        }

        [DisplayName("Employee Name")]
        public string EmployeeName { get; set; }

        [DisplayName("Job Title")]
        public string JobTitle { get; set; }

        [DisplayName("Date Employed")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateEmployed { get; set; }

        [DisplayName("Phone")]
        public string Phone { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Employee Contract")]
        public string EmployeeContractDocument { get; set; }

        [DisplayName("Employment Status Id")]
        public string EmployementStatusId { get; set; }

        [DisplayName("Employment Status")]
        public string EmployementStatus { get; set; }

        [DisplayName("Grade")]
        public string Grade { get; set; }

        [DisplayName("Division")]
        public string Division { get; set; }

        [DisplayName("DivisionId")]
        public string DivisionId { get; set; }

        [DisplayName("Birth Date")]
        public DateTime? DateBirth { get; set; }

        [DisplayName("Age")]
        public int Age { get; set; }

        [DisplayName("Estimate Severance Pay")]
        public long? EstimateSeverancePay { get; set; }

        [DisplayName("Employee Contract")]
        public string EmployeeContractDocumentName { get; set; }

        [DisplayName("Mobile")]
        public string Mobile { get; set; }

        [DisplayName("Gender")]
        public string Gender { get; set; }

        [DisplayName("Married Status")]
        public string MarriedStatusName { get; set; }

        [DisplayName("Employee Job Description")]
        public string EmployeeJobDescription { get; set; }

        [DisplayName("Employee Job Description")]
        public string EmployeeJobDescriptionName { get; set; }
        public string GradeId { get; set; }
        public string BusinessUnitId { get; set; }
        public string JobTitleId { get; set; }

        [DisplayName("PTKP Setting")]
        public string PtkpSettingId { get; set; }

        public decimal? LoanBalance { get; set; }
        [DisplayName("PPh 21")]
        public decimal Pph21 { get; set; }
        [DisplayName("Net Salary")]
        public decimal NetSalary { get; set; }
        public EmployeeHistoryPayroll EmployeeHistoryPayroll { get; set; }

        [DisplayName("Status Pph21")]
        public string PtkpSettingName { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityId { get; set; }
        [DisplayName("Legal Entity")]
        public string LegalEntityName { get; set; }
        [DisplayName("Balance Leave Current Year")]
        public decimal? BalanceLeaveCurrentYear { get; set; }
        [DisplayName("Balance Leave Carry Over")]
        public decimal? BalanceLeaveCarryOver { get; set; }
    }
}
