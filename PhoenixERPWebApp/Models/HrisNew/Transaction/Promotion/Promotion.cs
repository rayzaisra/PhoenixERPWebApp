using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class Promotion : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Employee Basic Info Id")]
        public string EmployeeBasicInfoId { get; set; }

        [DisplayName("Promotion Effective Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EffectiveDate { get; set; }

        [DisplayName("Proposed Job Title/Position")]
        public string JobTitleId { get; set; }

        [DisplayName("Purposed Grade")]
        public string JobGradeId { get; set; }

        [DisplayName("Purposed Level")]
        public string JobLevelId { get; set; }

        [DisplayName("A. Skills & Capability Evaluation")]
        public string Skills { get; set; }

        [DisplayName("B. Attitude Evaluation")]
        public string Attitude { get; set; }

        [DisplayName("C. Performence Evaluation")]
        public string Performence { get; set; }

        [DisplayName("Status")]
        public int? Status { get; set; }

        [DisplayName("Requester")]
        public string Requester { get; set; }

        [DisplayName("Request Code")]
        public string RequestCode { get; set; }

        [DisplayName("Request Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? RequestDate { get; set; }

        [DisplayName("Date Of Latest")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfLatest { get; set; }

        [DisplayName("Receive Warning Letter")]
        public bool? ReceiveWarningLetter { get; set; }

        [DisplayName("Position On Org")]
        public bool? PositionOnOrg { get; set; }

        [DisplayName("Notes")]
        public string Notes { get; set; }

        [DisplayName("Increment (%)")]
        public int? IncrementPercentage { get; set; }

        [DisplayName("Current Salary Zone")]
        public int? CurrentSalaryZone { get; set; }

        [DisplayName("Salary Zone")]
        public int? NextSalaryZone { get; set; }

        [DisplayName("Current Salary")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? CurrentSalary { get; set; }

        [DisplayName("Propose New Salary")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? NextSalary { get; set; }

        [DisplayName("Submit User")]
        public bool? SubmitUser { get; set; }

        [DisplayName("Is Synchronize")]
        public bool? IsSynchronize { get; set; }

        //Not Mapped
        public InfoEmployee requestEmployee { get; set; }
        public InfoEmployee infoEmployee { get; set; }

        [DisplayName("Changes Type")]
        public string ChangesType { get { return "Promotion"; } }

        [DisplayName("Status")]
        public string StatusDesc { get; set; }

        [DisplayName("Remarks")]
        public string RemarkRejected { get; set; }

        public bool IsApprove { get; set; }
        public List<PromotionSubcomponentAdditional> ComponentAdditionalList { get; set; }
        public List<PromotionSubcomponentDeduction> ComponentDeductionList { get; set; }
    }
}