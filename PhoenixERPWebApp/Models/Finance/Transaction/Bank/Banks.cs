using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Finance.Transaction
{
    public class Banks : FinanceEntityBase
    {
    
        public string Id { get; set; }

    
        public string OrganizationId { get; set; }

        public string BankName { get; set; }

     
        public string BankCode { get; set; }

     
        public string SwiftCode { get; set; }

     
        public string AccountNumber { get; set; }
        public string LegalEntityId { get; set; }
        public string CurrencyId { get; set; }
        public string LegalEntityName { get; set; }
        public string CoaId { get; set; }
    }
}
