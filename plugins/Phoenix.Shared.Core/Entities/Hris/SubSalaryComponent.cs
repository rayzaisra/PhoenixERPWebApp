using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("sub_salary_component",Schema ="hr")]
    public partial class SubSalaryComponent : GenericEntity<string>
    {
        public string salary_component_id { get; set; }
        public string name { get; set; }
        public int value { get; set; }
        public string description { get; set; }
    }
}