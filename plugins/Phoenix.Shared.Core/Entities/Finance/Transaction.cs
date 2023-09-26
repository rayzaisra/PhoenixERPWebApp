using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("transaction",Schema ="fn")]
    public partial class Transaction : GenericEntity<string>
    {
        public string organization_id { get; set; }
        public DateTime? transaction_datetime { get; set; }
        public string transaction_number { get; set; }
    }
}