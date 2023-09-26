using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("thr_setting", Schema = "hr")]
    public partial class THRSetting : GenericEntity<string>
    {
        public string month_payroll { get; set; }
        public int year { get; set; }
        public string description { get; set; }
    }
}
