using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models
{
    public class RptBudgetBalance
    {
        public string AccountId { get; set; }
        public string CodeRec { get; set; }

        [DisplayName("Account COA Level 5")]
        public string AccountName { get; set; }

        public string BusinessUnitId { get; set; }
        public string BusinessUnitName { get; set; }

        [DisplayName("TotalBudget")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal TotalBudget { get; set; }

        [DisplayName("Total PR")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal TotalPR { get; set; }

        [DisplayName("Total Balance PR")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal TotalPRBalance { get { return TotalBudget - TotalPR; } }

        [DisplayName("Total PO")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal TotalPO { get; set; }

        [DisplayName("Total Balance PO")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal TotalPOBalance { get { return TotalBudget - TotalPO; } }

        [DisplayName("Total AP")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal TotalAP { get; set; }

        [DisplayName("Total Balance AP")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal TotalAPBalance { get { return TotalBudget - TotalAP; } }
    }

    public class RptBudgetBalanceFilter
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
