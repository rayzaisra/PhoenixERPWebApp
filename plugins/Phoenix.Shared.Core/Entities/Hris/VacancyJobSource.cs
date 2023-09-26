using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("vacancy_job_source",Schema ="hr")]
    public partial class VacancyJobSource : GenericEntity<string>
    {
        public string file_id { get; set; }
        public string links { get; set; }
        public string source_vacancy { get; set; }
    }
}