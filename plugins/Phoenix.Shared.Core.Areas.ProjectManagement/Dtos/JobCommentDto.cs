using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class JobCommentDto
    {
        [Required]
        public string Id { get; set; }
        [StringLength(50)]
        public string job_number { get; set; }
        [StringLength(250)]
        public string comment_message { get; set; }
        public DateTime? comment_date { get; set; }
        [StringLength(50)]
        public string user_id { get; set; }
    }

    public class JobCommentModel
    {
        public string job_number { get; set; }
        public string comment_message { get; set; }
        public DateTime? comment_date { get; set; }
        public string hour_comment_date { get; set; }
        public string user_id { get; set; }
        public string user_name { get; set; }
    }
}
