using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class JobRepositoryDto
    {
        [Required]
            public string Id { get; set; }
        public DateTime? date { get; set; }
        [StringLength(50)]
        public string external_id { get; set; }
        [StringLength(50)]
        public string job_posting_requisition_id { get; set; }
        [StringLength(50)]
        public string pe_file_id { get; set; }
        public int? remark { get; set; }
        public int? version { get; set; }
    }
    public class JobRepositoryNew
    {
        public DateTime? date { get; set; }
        [StringLength(50)]
        public string external_id { get; set; }
        [StringLength(50)]
        public string job_posting_requisition_id { get; set; }
        [StringLength(50)]
        public string pe_file_id { get; set; }
        public int? remark { get; set; }
        public int? version { get; set; }
    }            
    public class JobRepositorySearch : SearchParameter
    {
        public DateTime? Search_date { get; set; }
        public string Search_external_id { get; set; }
        public string Search_job_posting_requisition_id { get; set; }
        public string Search_pe_file_id { get; set; }
        public int? Search_remark { get; set; }
        public int? Search_version { get; set; }
        
    }
}