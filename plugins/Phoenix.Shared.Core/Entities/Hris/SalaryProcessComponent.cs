using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("salary_process_component",Schema ="hr")]
    public partial class SalaryProcessComponent : GenericEntity<string>
    {
        public string component_name { get; set; }
        public long? employee_basic_info_id { get; set; }
        public int? month { get; set; }
        public long? tax_id { get; set; }
        public string type { get; set; }
        public long? value { get; set; }
        public long? value_year { get; set; }
        public int? year { get; set; }
    }
}