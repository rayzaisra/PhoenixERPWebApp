using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities.Hris
{
    [Table("job_vacancy", Schema = "hr")]
    public partial class JobVacancy : GenericEntity<string>
    {
        public string code { get; set; }
        public string posting_status { get; set; }
        public string vacancy_name { get; set; }
        public string master_location_id { get; set; }
        public DateTime? closing_date { get; set; }
        public string hiring_request_id { get; set; }
        public string approval_id { get; set; }

    }
}
