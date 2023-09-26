using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class FinancePeriod : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Fiscal Year Id")]
        public string FiscalYearId { get; set; }

        [DisplayName("Period Name")]
        public string FinancePeriodName { get; set; }

        [DisplayName("Code")]
        public string FinancePeriodNumber { get; set; }

        [DisplayName("Legal Entity Id")]
        public string LegalEntityId { get; set; }

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

        [DisplayName("Account Payable")]
        public bool? Ap { get; set; }

        [DisplayName("Account Receivable")]
        public bool? Ar { get; set; }

        [DisplayName("Request Payment Salary")]
        public bool? Cas { get; set; }

        [DisplayName("Petty cash")]
        public bool? Pcs { get; set; }

        [DisplayName("Bank payment")]
        public bool? Bp { get; set; }

        [DisplayName("Bank Received")]
        public bool? Br { get; set; }

        [DisplayName("Journal Entry")]
        public bool? Je { get; set; }

        [DisplayName("Journal adjustment")]
        public bool? Ja { get; set; }

        [DisplayName("Amortization")]
        public bool? Amor { get; set; }

        [DisplayName("Revaluation")]
        public bool? Dep { get; set; }

        [DisplayName("Salary Allocation")]
        public bool? Sa { get; set; }

        [DisplayName("Job Closing")]
        public bool? Jc { get; set; }

        //Not Mapped
        [DisplayName("Legal Entity")]
        public string LegalEntityName { get; set; }

        [DisplayName("Remarks")]
        public string RemarksProcess { get; set; }

        [DisplayName("Status")]
        public int? StatusApproval { get; set; }
    }
}