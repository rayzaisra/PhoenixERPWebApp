using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("transaction_detail",Schema ="fn")]
    public partial class TransactionDetail : GenericEntity<string>
    {
        public string account_id { get; set; }
        public string affiliation_id { get; set; }
        public string business_unit_id { get; set; }
        public string company_id { get; set; }
        public decimal credit { get; set; }
        public string currency_id { get; set; }
        public decimal debit { get; set; }
        public string description { get; set; }
        public string legal_entity_id { get; set; }
        public string transaction_id { get; set; }
    }
}