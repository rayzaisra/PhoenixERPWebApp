using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("pe_template",Schema ="pm")]
    public partial class PeTemplate : GenericEntity<string>
    {
        public int? pe_cost_perqty { get; set; }
        public int? pe_item_qty { get; set; }
        public int? pe_line_item { get; set; }
        public int? pe_number { get; set; }
    }
}