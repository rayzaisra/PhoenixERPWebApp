using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("status_tax",Schema ="hr")]
    public partial class StatusTax : GenericEntity<string>
    {
        public string master_tax_id { get; set; }
        public string tax_status { get; set; }
    }
}