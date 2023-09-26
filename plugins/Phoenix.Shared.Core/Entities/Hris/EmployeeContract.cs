using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("employee_contract",Schema ="hr")]
    public partial class EmployeeContract : GenericEntity<string>
    {
        public int? active { get; set; }
        public string candidate_recruitment_id { get; set; }
        public string contract_upload { get; set; }
        public string employee_basic_info_id { get; set; }
        public DateTime? end_date { get; set; }
        public string notes { get; set; }
        public DateTime? start_date { get; set; }
        public string filemaster_id { get; set; }
        public string comment { get; set; }
    }
}