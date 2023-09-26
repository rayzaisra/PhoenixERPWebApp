using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models
{
    public class BankReceivedDetail : FinanceEntityBase
    {
        public string Id { get; set; }
        
        public string BankReceivedId { get; set; }
        
        public string AccountReceivebleNumber { get; set; }
        
        public string LegalEntityId { get; set; }
        
        public string BusinessUnitId { get; set; }
        
        public string AffiliationId { get; set; }
        
        public string AccountId { get; set; }
        
        public string BrandId { get; set; }
        
        public string JobId { get; set; }
        
        public DateTime? TransactionDate { get; set; }
        
        public DateTime? DueDate { get; set; }
        
        public string CurrencyId { get; set; }
        
        public decimal? Amount { get; set; }
        
        public decimal? ExchangeRate { get; set; }
        
        public string BankAccountDestination { get; set; }
        
        public decimal? Charges { get; set; }
        
        public string ClientId { get; set; }
        
        public string TaxPayable { get; set; }
        
        public string SourceDoc { get; set; }
        
        public string TaxPayableCoa { get; set; }

       
        public string BankAccountDestinationName { get; set; }

       
        public string LegalEntity { get; set; }

       
        public string ClientBrand { get; set; }

       
        public string ClientName { get; set; }

       
        public string CompanyName { get; set; }

       
        public string JobType { get; set; }
       
        public string JobName { get; set; }
       
        public string CurrencyCode { get; set; }

       
        public string BusinessUnitName { get; set; }
    }
}
