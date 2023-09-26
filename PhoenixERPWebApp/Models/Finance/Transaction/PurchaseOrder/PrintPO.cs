using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models
{
    public class PrintPO
    {
        public string Client { get; set; }
        public string ClientAddress { get; set; }
        public string ClientTelp { get; set; }
        public string ClientFax { get; set; }
        public string ClientAttn { get; set; }
        public string ClientEmail { get; set; }

        public string LegalEntity { get; set; }
        public string LegalEntityAddress { get; set; }
        public string LegalEntityTelp { get; set; }
        public string LegalEntityFax { get; set; }
        public string LegalEntityEmail { get; set; }
        public string NPWP { get; set; }

        public string PoNo { get; set; }
        public DateTime? Date { get; set; }
        public string JobNo { get; set; }
        public string RefNo { get; set; }
        public string Brand { get; set; }

        //public string BudgetNo { get; set; }
        //public string JobTitle { get; set; }
        //public string JobDescription { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }

        public decimal? Total { get; set; }
        public decimal? VatPercent { get; set; }
        public decimal? Vat { get; set; }
        public decimal? TotalAmount { get; set; }
        public string Memo { get; set; }
        public List<PrintPODetail> PrintPODetail { get; set; }
    }

    public class PrintPODetail
    {
        public string Id { get; set; }
        public string PODtlId { get; set; }
        public string Desription { get; set; }
        public decimal Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Cost { get; set; }
        public string TaskId { get; set; }
        public string ParentTaskId { get; set; }
        public string ItemTypeId { get; set; }
    }
}
