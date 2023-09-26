using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class MasterBudgetCode : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Budget Code")]
        public string BudgetCode { get; set; }

        [DisplayName("Budget Name")]
        public string BudgetName { get; set; }

        [DisplayName("COA Level 5")]
        public string CoaId { get; set; }

        [DisplayName("COA Level 5")]
        public string CoaName { get; set; }

        [DisplayName("Division")]
        public string HrisDivisionId { get; set; }

        [DisplayName("Owner")]
        public string BugetOwnerId { get; set; }

        [DisplayName("Source")]
        public string Source { get; set; }

        //Not Mapped 
        [DisplayName("COA ID Level 5")]
        public string CodeRec { get; set; }

        [DisplayName("Division")]
        public string HrisDivisionName { get; set; }

        [DisplayName("Owner")]
        public string BugetOwnerName { get; set; }
    }
}