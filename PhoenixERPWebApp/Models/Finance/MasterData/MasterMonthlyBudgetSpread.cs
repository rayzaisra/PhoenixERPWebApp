using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class MasterMonthlyBudgetSpread : FinanceEntityBase
    {
        [DisplayName("Master Monthly Budget ID")]
        public string Id { get; set; }

        [DisplayName("Master Monthly Budget Name")]
        public string MasterMonthlyBudgetName { get; set; }

        [DisplayName("Percentage Month 1")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal PercentageMonth1 { get; set; }

        [DisplayName("Percentage Month 2")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal PercentageMonth2 { get; set; }

        [DisplayName("Percentage Month 3")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal PercentageMonth3 { get; set; }

        [DisplayName("Percentage Month 4")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal PercentageMonth4 { get; set; }

        [DisplayName("Percentage Month 5")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal PercentageMonth5 { get; set; }

        [DisplayName("Percentage Month 6")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal PercentageMonth6 { get; set; }

        [DisplayName("Percentage Month 7")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal PercentageMonth7 { get; set; }

        [DisplayName("Percentage Month 8")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal PercentageMonth8 { get; set; }

        [DisplayName("Percentage Month 9")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal PercentageMonth9 { get; set; }

        [DisplayName("Percentage Month 10")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal PercentageMonth10 { get; set; }

        [DisplayName("Percentage Month 11")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal PercentageMonth11 { get; set; }

        [DisplayName("Percentage Month 12")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal PercentageMonth12 { get; set; }

        [DisplayName("Total Budget Spread")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal TotalBudgetSpread { get; set; }
    }
}