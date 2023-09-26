using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("status_employee",Schema ="hr")]
    public partial class StatusEmployee : GenericEntity<string>
    {
        public string compare { get; set; }
        public string level_position_id { get; set; }
        public string month { get; set; }
        public string name { get; set; }
        public string ptkp { get; set; }
        public string tax { get; set; }
    }
}