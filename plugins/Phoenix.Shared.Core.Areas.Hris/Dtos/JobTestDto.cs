using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class JobTestDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string job_posting_requisition_id { get; set; }
        [StringLength(2147483647)]
        public string job_test_description { get; set; }
        [StringLength(2147483647)]
        public string notes { get; set; }
        [StringLength(150)]
        public string test_title { get; set; }
    }
    public class JobTestNew
    {
        [StringLength(50)]
        public string job_posting_requisition_id { get; set; }
        [StringLength(2147483647)]
        public string job_test_description { get; set; }
        [StringLength(2147483647)]
        public string notes { get; set; }
        [StringLength(150)]
        public string test_title { get; set; }
    }            
    public class JobTestSearch : SearchParameter
    {
        public string Search_job_posting_requisition_id { get; set; }
        public string Search_job_test_description { get; set; }
        public string Search_notes { get; set; }
        public string Search_test_title { get; set; }
        
    }
}