using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("vacancy",Schema ="hr")]
    public partial class Vacancy : GenericEntity<string>
    {
        public int? applicant_total { get; set; }
        public string approval_id { get; set; }
        public string business_unit_id { get; set; }
        public int? hiring_request_id { get; set; }
        public string reg { get; set; }
        public string vacancy_name { get; set; }
    }
}