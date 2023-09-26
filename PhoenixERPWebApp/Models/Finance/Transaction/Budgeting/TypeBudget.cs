using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Finance.Transaction.Budget
{
    public class TypeBudget : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Type Budget Header Id")]
        public string type_budget_hdr_id { get; set; }

        [DisplayName("Financial Year Name")]
        public string financialyear { get; set; }

        [DisplayName("Financial Year Month")]
        public string bulan { get; set; }   

        [DisplayName("Revision")]
        public int? revisi { get; set; }

        [DisplayName("Account Name")]
        public string accountname { get; set; }

        [DisplayName("Skor")]
        public decimal? skor { get; set; }

        [DisplayName("Interface")]
        public decimal? Interface { get; set; }

        [DisplayName("Trade Markering Advis")]
        public decimal? tm_advis { get; set; }

        [DisplayName("Brandcom A")]
        public decimal? brandcoma { get; set; }

        [DisplayName("Brandcom B")]
        public decimal? brandcomb { get; set; }

        [DisplayName("Sampling")]
        public decimal? marketingservicesampling { get; set; }

        [DisplayName("RM")]
        public decimal? marketingservicerm { get; set; }

        [DisplayName("PR")]
        public decimal? pr { get; set; }

        [DisplayName("Pathfinder")]
        public decimal? pathfinder { get; set; }

        [DisplayName("Total Iris")]
        public decimal? totaliris { get; set; }

        [DisplayName("BOD Central")]
        public decimal? bodcentral { get; set; }

        [DisplayName("Total Group")]
        public double? totalgroup { get; set; }


    }
}
