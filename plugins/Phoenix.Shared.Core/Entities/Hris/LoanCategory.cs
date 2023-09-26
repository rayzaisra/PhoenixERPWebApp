using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("loan_category", Schema = "hr")]
    public partial class LoanCategory : GenericEntity<string>
    {
        public string code { get; set; }
        public string description { get; set; }

        public string name { get; set; }
        public int? interest_month { get; set; }
    }
}