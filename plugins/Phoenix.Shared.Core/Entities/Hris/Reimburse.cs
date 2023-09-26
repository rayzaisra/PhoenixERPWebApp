using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("reimburse",Schema ="hr")]
    public partial class Reimburse : GenericEntity<string>
    {
        public int? amount { get; set; }
        public string employee_basic_info_id { get; set; }
        public string receipt { get; set; }
        public DateTime? reimburse_date { get; set; }
        public string reimburse_description { get; set; }
        public string type_reimburse { get; set; }
    }
}