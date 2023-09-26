using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("ptkp_setting", Schema = "hr")]
    public partial class PTKPSetting : GenericEntity<string>
    {
        public string name { get; set; }
        public decimal value { get; set; }
        public string description { get; set; }
    }
}
