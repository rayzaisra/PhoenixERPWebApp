using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Data.Models
{
    public class InfoEmployee
    {
        public string EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public string JobTitle { get; set; }

        public DateTime? DateEmployed { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string EmployeeContractDocument { get; set; }
        public string EmployementStatusId { get; set; }
        public string EmployementStatus { get; set; }

        public string Grade { get; set; }
        public string BusinessUnitId { get; set; }
        public string Division { get; set; }
        public string DivisionId { get; set; }

        public DateTime? DateBirth { get; set; }
        public int Age { get; set; }
        public string EmployeeContractDocumentName { get; set; }
        public string Mobile { get; set; }
        public string Gender { get; set; }
        public string MarriedStatusName { get; set; }
        public string EmployeeJobDescription { get; set; }
        public string EmployeeJobDescriptionName { get; set; }
        public string GradeId { get; set; }
        public string JobTitleId { get; set; }
        public string PtkpSettingId { get; set; }
        public decimal Pph21 { get; set; }
        public decimal? LoanBalance { get; set; }
        public decimal NetSalary { get; set; }
        public string PtkpSettingName { get; set; }
        public string LegalEntityName { get; set; }
        public string LegalEntityId { get; set; }

        public decimal? BalanceLeaveCurrentYear { get; set; }
        public decimal? BalanceLeaveCarryOver { get; set; }
    }

    public class VmCountUserApproval
    {
        public string Id { get; set; }
        public string EmployeeBasicInfoId { get; set; }
        public bool IsCondition { get; set; }
        public string TrTempApprovalId { get; set; }
        public float Pph21 { get; set; }
    }
}
