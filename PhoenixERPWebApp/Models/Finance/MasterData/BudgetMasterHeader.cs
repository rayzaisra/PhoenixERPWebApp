using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class BudgetMasterHeader : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Budget Year")]
        public int? BudgetYear { get; set; }

        [DisplayName("Budget Name")]
        public string BudgetName { get; set; }

        [DisplayName("Periode From")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PeriodeFrom { get; set; }

        [DisplayName("Periode To")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PeriodeTo { get; set; }

        [DisplayName("Currency Id")]
        public string CurrencyId { get; set; }

        [DisplayName("Exchange Rate")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? ExchangeRate { get; set; }

        [DisplayName("Source")]
        public string Source { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        [DisplayName("Version")]
        public int? Version { get; set; }

        [DisplayName("Created By")]
        public string CreatedByName { get; set; }

        [DisplayName("Currency")]
        public string CurrencyName { get; set; }

        public bool IsOwner { get; set; }

        //Proses curd detail
        public BudgetMaster BudgetMaster { get; set; }
        public List<BudgetMasterDetail> BudgetMasterDetail { get; set; }
    }
}