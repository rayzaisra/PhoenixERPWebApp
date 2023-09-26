using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities.Hris
{
    [Table("job_vacancy_job_source", Schema = "hr")]
    public partial class JobVacancyJobSouce : GenericEntity<string>
    {
        public string code { get; set; }
        public string souce { get; set; }
        public string url { get; set; }
        public string files { get; set; }
        public DateTime? date_posting { get; set; }
        public string job_vacancy_id { get; set; }

    }
}
