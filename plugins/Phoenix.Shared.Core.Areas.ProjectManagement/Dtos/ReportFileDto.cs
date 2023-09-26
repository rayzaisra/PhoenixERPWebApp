using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class ReportFileDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string client_brief_id { get; set; }
        [StringLength(50)]
        public string file_id { get; set; }
    }
    public class ReportFileNew
    {
        [StringLength(50)]
        public string client_brief_id { get; set; }
        [StringLength(50)]
        public string file_id { get; set; }
    }            
    public class ReportFileSearch : SearchParameter
    {
        public string Search_client_brief_id { get; set; }
        public string Search_file_id { get; set; }
        
    }
}