using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class VacancyJobSourceDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string file_id { get; set; }
        [StringLength(255)]
        public string links { get; set; }
        [StringLength(255)]
        public string source_vacancy { get; set; }
    }
    public class VacancyJobSourceNew
    {
        [StringLength(50)]
        public string file_id { get; set; }
        [StringLength(255)]
        public string links { get; set; }
        [StringLength(255)]
        public string source_vacancy { get; set; }
    }            
    public class VacancyJobSourceSearch : SearchParameter
    {
        public string Search_file_id { get; set; }
        public string Search_links { get; set; }
        public string Search_source_vacancy { get; set; }
        
    }
}