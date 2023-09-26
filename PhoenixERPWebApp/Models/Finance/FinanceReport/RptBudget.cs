using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models
{
    public class RptBudget
    {
        public string AccountId { get; set; }
        public string CodeRec { get; set; }

        [DisplayName("Account COA Level 5")]
        public string AccountName { get; set; }

        public string BusinessUnitId { get; set; }
        public string BusinessUnitName { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        [DisplayName("Version")]
        public int? Version { get; set; }

        [DisplayName("January")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Budget01 { get; set; }

        [DisplayName("February")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Budget02 { get; set; }

        [DisplayName("March")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Budget03 { get; set; }

        [DisplayName("April")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Budget04 { get; set; }

        [DisplayName("May")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Budget05 { get; set; }

        [DisplayName("June")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Budget06 { get; set; }

        [DisplayName("July")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Budget07 { get; set; }

        [DisplayName("August")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Budget08 { get; set; }

        [DisplayName("September")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Budget09 { get; set; }

        [DisplayName("October")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Budget10 { get; set; }

        [DisplayName("November")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Budget11 { get; set; }

        [DisplayName("December")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Budget12 { get; set; }

        [DisplayName("Total")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Total { get { return Budget01 + Budget02 + Budget03 + Budget04 + Budget05 + Budget06 + Budget07 + Budget08 + Budget09 + Budget10 + Budget11 + Budget12; } }
    }

    public class RptBudgetFilter
    {
        [DisplayName("Year")]
        public int Year { get; set; } 

        [DisplayName("Business Unit")]
        public string BusinessUnitId { get; set; }
        public bool DisplayAmountWaitingApproval { get; set; }

        [DisplayName("Version")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? Version { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        [DisplayName("COA Level")]
        public string CoaLevel { get; set; }
    } 
}
