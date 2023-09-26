using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PhoenixERPWebApp.Models
{
    public class Demotion : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Employee Basic Info Id")]
        public string EmployeeBasicInfoId { get; set; }

        [DisplayName("Demotion Effective Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EffectiveDate { get; set; }

        [DisplayName("Proposed Job Title/Position")]
        public string JobTitleId { get; set; }

        [DisplayName("Purposed Grade")]
        public string JobGradeId { get; set; }

        [DisplayName("Purposed Level")]
        public string JobLevelId { get; set; }

        [DisplayName("Notes/Reason")]
        public string Notes { get; set; }

        [DisplayName("Warning Letter (if any)")]
        public string WarningLetter { get; set; }

        [DisplayName("Status")]
        public int? Status { get; set; }

        [DisplayName("Requester")]
        public string Requester { get; set; }

        [DisplayName("Request Code")]
        public string RequestCode { get; set; }

        [DisplayName("Request Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? RequestDate { get; set; }

        //Not Mapped
        public InfoEmployee requestEmployee { get; set; }
        public InfoEmployee infoEmployee { get; set; }

        public string WarningLetterName { get; set; }

        [DisplayName("Changes Type")]
        public string ChangesType { get { return "Demotion"; } }

        [DisplayName("Status")]
        public string StatusDesc { get; set; }

        [DisplayName("Remarks")]
        public string RemarkRejected { get; set; }

        public bool IsApprove { get; set; }
    }
}