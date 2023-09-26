using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("salary_component",Schema ="hr")]
    public partial class SalaryComponent : GenericEntity<string>
    {
        public string code { get; set; }
        public string salary_component { get; set; }
        public string description { get; set; }
    }
}