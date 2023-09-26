using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class TaskUserDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string job_posting_requisition_id { get; set; }
        [StringLength(50)]
        public string task_id { get; set; }
        [StringLength(250)]
        public string task_user { get; set; }
        [StringLength(50)]
        public string user_id { get; set; }
    }
    public class TaskUserNew
    {
        [StringLength(50)]
        public string job_posting_requisition_id { get; set; }
        [StringLength(50)]
        public string task_id { get; set; }
        [StringLength(250)]
        public string task_user { get; set; }
        [StringLength(50)]
        public string user_id { get; set; }
    }            
    public class TaskUserSearch : SearchParameter
    {
        public string Search_job_posting_requisition_id { get; set; }
        public string Search_task_id { get; set; }
        public string Search_task_user { get; set; }
        public string Search_user_id { get; set; }
        
    }
}