using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Finance
{
    public class PettyCashDetail : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Petty Cash Id")]
        public string PettyCashId { get; set; }

        [DisplayName("Account ID-Name")]
        public string AccountId { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? TransactionDate { get; set; }

        [DisplayName("Description")]
        public string TransactionDetail { get; set; }
         
        [DisplayName("Amount")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Credit { get; set; }

        [DisplayName("Job ID-Name")]
        public string JobId { get; set; }

        [DisplayName("PCE ID")]
        public string PceId { get; set; }

        [DisplayName("PCA ID")]
        public string PcaId { get; set; }

        [DisplayName("Task List Name")]
        public string PcaTaskId { get; set; }

        [DisplayName("Business Unit")]
        public string BusinessUnitId { get; set; }

        ////Not Mapped 

        [DisplayName("Job Name")]
        public string JobName { get; set; } 

        public string PcaTask { get; set; }

        public string AccountName { get; set; }

        public string BusinessUnitName { get; set; }
    }
}
