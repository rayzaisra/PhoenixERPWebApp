using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class TaskCommentDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(250)]
        public string comment { get; set; }
        public DateTime? comment_date { get; set; }
        [StringLength(50)]
        public string file_id { get; set; }
        [StringLength(50)]
        public string task_id { get; set; }
        [StringLength(1)]
        public string user_id { get; set; }
    }
    public class TaskCommentNew
    {
        [StringLength(250)]
        public string comment { get; set; }
        public DateTime? comment_date { get; set; }
        [StringLength(50)]
        public string file_id { get; set; }
        [StringLength(50)]
        public string task_id { get; set; }
        [StringLength(1)]
        public string user_id { get; set; }
    }            
    public class TaskCommentSearch : SearchParameter
    {
        public string Search_comment { get; set; }
        public DateTime? Search_comment_date { get; set; }
        public string Search_file_id { get; set; }
        public string Search_task_id { get; set; }
        public string Search_user_id { get; set; }
        
    }
}