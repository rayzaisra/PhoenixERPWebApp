using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class JobVacancyJobSourceDto
    {
        [Required]
            public string Id { get; set; }
        public string code { get; set; }
        public string source { get; set; }
        public string url { get; set; }
        public string files { get; set; }
        public DateTime? date_posting { get; set; }
        public string job_vacancy_id { get; set; }
        public DateTime? created_on { get; set; }
    }
    public class JobVacancyJobSourceNew
    {
      
        public string Id { get; set; }
        public string code { get; set; }
        public string source { get; set; }
        public string url { get; set; }
        public string files { get; set; }
        public DateTime? date_posting { get; set; }
        public string job_vacancy_id { get; set; }
        public DateTime? created_on { get; set; }
    }            
    public class JobVacancyJobSourceSearch : SearchParameter
    {
        
        
    }
}