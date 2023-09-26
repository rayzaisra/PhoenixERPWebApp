using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models
{
    public class PrintPce
    {
        public string Client { get; set; }
        public string ClientAddress { get; set; }
        public string ClientTelp { get; set; }
        public string ClientFax { get; set; }
        public string ClientEmail { get; set; }
        public string LegalEntity { get; set; }
        public string LegalEntityAddress { get; set; }
        public string LegalEntityTelp { get; set; }
        public string LegalEntityFax { get; set; }
        public string LegalEntityEmail { get; set; }
        public DateTime? Date { get; set; }
        public string JobNo { get; set; }
        public string RefNo { get; set; }
        public string Brand { get; set; }
        public string BudgetNo { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal? OtherFee { get; set; }
        public string OtherFeeName { get; set; }
        public decimal? OtherFeePercentage { get; set; }
        public decimal? SubtotalFee { get { return Subtotal + OtherFee; } }
        public decimal? Vat { get; set; }
        public decimal? Total { get; set; }
        public List<PrintTask> PrintTasks { get; set; }
    }

    public class PrintTask
    {
        public string TaskId { get; set; }
        public string TaskName { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
