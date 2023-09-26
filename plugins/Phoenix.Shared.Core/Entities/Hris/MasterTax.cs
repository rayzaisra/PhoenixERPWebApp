using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("master_tax",Schema ="hr")]
    public partial class MasterTax : GenericEntity<string>
    {
        public string bpjs_tk { get; set; }
        public string bruto_calculate { get; set; }
        public string calculatetax { get; set; }
        public string code { get; set; }
        public string percent_value { get; set; }
        public string tax_name { get; set; }
        public string tax_no { get; set; }
        public string type { get; set; }
        public string value { get; set; }
    }
}