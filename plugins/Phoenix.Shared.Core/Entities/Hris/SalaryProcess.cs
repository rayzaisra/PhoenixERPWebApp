using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("salary_process",Schema ="hr")]
    public partial class SalaryProcess : GenericEntity<string>
    {
        public int? basic_salary { get; set; }
        public int? bonus { get; set; }
        public int? bruto { get; set; }
        public int? bruto_ofyears { get; set; }
        public string employee_basic_info_id { get; set; }
        public string employement_status_id { get; set; }
        public int? loan { get; set; }
        public int? location { get; set; }
        public int? month { get; set; }
        public int? netto { get; set; }
        public int? netto_ofyears { get; set; }
        public int? pkp_ofyears { get; set; }
        public int? position_cost { get; set; }
        public int? ptkp_ofmonths { get; set; }
        public int? ptkp_ofyears { get; set; }
        public string status_employee_id { get; set; }
        public int? tax_no_npwp { get; set; }
        public int? tax_ofmonths { get; set; }
        public int? tax_ofyears { get; set; }
        public int? transport { get; set; }
        public int? year { get; set; }
    }
}