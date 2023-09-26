using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Phoenix.Shared.Helpers;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class TaskDto
    {
        public string Id { get; set; }
        [StringLength(50)]
        public string central_resources_id { get; set; }
        
        public DateTime? deadline_task { get; set; }
        [StringLength(250)]
        public string description { get; set; }
        [StringLength(50)]
        public string job_number { get; set; }
        [StringLength(50)]
        public string job_posting_requisition_id { get; set; }
        [DateGreaterThan("deadline_task")]
        public DateTime? start_task { get; set; }
        [StringLength(200)]
        public string task_name { get; set; }
        [StringLength(50)]
        public string task_priority_id { get; set; }
        [StringLength(50)]
        public string task_status_id { get; set; }
        public List<SubTaskDto> subtask { get; set; }
    }
    public class subTaskUsers
    {
        public string sub_task_id { get; set; }
        public List<string> user_id { get; set; }
        public string job_number_sub_task { get; set; }
    }
    public class Tasknewcomment
    {
        [Required]
        public string task_id { get; set; }
        [Required, StringLength(1000)]
        public string comment { get; set; }
    }
    public class Taskstatusset
    {
        [Required]
        public string task_id { get; set; }
        [Required]
        public string task_status_id { get; set; }
    }
    public class Tasksubpercentage
    {
        [Required]
        public string sub_task_id { get; set; }
        [Required]
        public int percentage { get; set; }
    }
    public class Taskcommentlist
    {
        public string task_id { get; set; }
        public string comment { get; set; }
        public DateTime? comment_date { get; set; }
        public string user_id { get; set; }
        public string user_name { get; set; }
    }

    public class TaskNew
    {
        [StringLength(50)]
        public string Id { get; set; }
        //[Required,StringLength(50)]
        // public string central_resources_id { get; set; }
        [Required,StringLength(50)]
        public string job_number { get; set; }
        [Required, StringLength(500)]
        public string task_name { get; set; }
        [StringLength(50)]
        public string task_priority_id { get; set; }

        [DateGreaterThan("deadline_task")]
        public DateTime? start_task { get; set; }
        public DateTime? deadline_task { get; set; }
        public int hour { get; set; }

    }
    public class TaskUpdate
    {
        [StringLength(50)]
        public string Id { get; set; }
        [Required, StringLength(500)]
        public string task_name { get; set; }
        [StringLength(50)]
        public string task_priority_id { get; set; }

        [DateGreaterThan("deadline_task")]
        public DateTime? start_task { get; set; }
        public DateTime? deadline_task { get; set; }
        public int hour { get; set; }

    }
    public class CentralTaskList
    {
        public string Id { get; set; }
        public string central_resources_id { get; set; }
        public string central_resources_name { get; set; }
        public List<TaskList> task { get; set; }
    }
        public class TaskList
    {
        public string id { get; set; }
        public string central_resources_id { get; set; }
        public string central_resources_name { get; set; }
        public DateTime? deadline_task { get; set; }
        public string description { get; set; }
        public string job_number { get; set; }
        public string job_posting_requisition_id { get; set; }
        public DateTime? start_task { get; set; }

        public string task_name { get; set; }

        public string task_priority_id { get; set; }

        public string task_status_id { get; set; }
    }
    public class TaskListDetil :TaskList
    {
        public List<SubTaskDto> subtask { get; set; }
    }
    public class TaskSearch : SearchParameter
    {
        public string Search_central_resources_id { get; set; }
        public DateTime? Search_deadline_task { get; set; }
        public string Search_description { get; set; }
        public string Search_job_number { get; set; }
        public string Search_job_posting_requisition_id { get; set; }
        public DateTime? Search_start_task { get; set; }
        public string Search_task_name { get; set; }
        public string Search_task_priority_id { get; set; }
        public string Search_task_status_id { get; set; }
        
    }
}