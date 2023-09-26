using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("medical_claim",Schema ="hr")]
    public partial class MedicalClaim : GenericEntity<string>
    {
        public DateTime? date_claim { get; set; }
        public string employee_basic_info_id { get; set; }
        public string reason { get; set; }
        public int? value { get; set; }
        public string year_periode { get; set; }
    }
}