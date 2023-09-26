using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class FinancialYear : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Legal Entity Id")]
        public string LegalEntityId { get; set; }

        [DisplayName("Code")]
        public string FinanceYearNumber { get; set; }

        [DisplayName("Fiscal Name")]
        public string FinancialYearName { get; set; }

        [DisplayName("Start Period")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        [DisplayName("End Period")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        [DisplayName("Is Close")]
        public bool? IsClose { get; set; }

        //Not Mapped
        [DisplayName("Legal Entity")]
        public string LegalEntityName { get; set; }

        [DisplayName("Remarks")]
        public string RemarksProcess { get; set; }

        [DisplayName("Status")]
        public int? StatusApproval { get; set; }

        [DisplayName("Year")]
        public int? Year { get; set; }
    }
}