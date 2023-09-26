using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Phoenix.Shared.Helpers;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class SubTaskDto
    {
        public string Id { get; set; }
        [StringLength(2147483647)]
        public string attachment { get; set; }
        [StringLength(2147483647)]
        public string description { get; set; }
       
        public DateTime? due_date { get; set; }
        public int? hour { get; set; }
        public Single? percentage { get; set; }
        [DateGreaterThan("due_date")]
        public DateTime? start_date { get; set; }
        [StringLength(50)]
        public string sub_task { get; set; }
        [StringLength(50)]
        public string task_id { get; set; }
        [StringLength(50)]
        public string task_status_id { get; set; }
        public List<string> subtaskusers { get; set; }
        public List<SubTaskUserDto> subtaskuser { get; set; }
        [StringLength(50)]
        public string user_id { get; set; }
    }
    public class SubTaskUserDto
    {
        public string id { get; set; }
        public string user_id { get; set; }
        public string app_username { get; set; }
        public string app_fullname { get; set; }
        
        public string sub_task_id { get; set; }
    }
        public class SubTaskNew
    {
        public string description { get; set; }
        public DateTime? due_date { get; set; }
        public int? hour { get; set; } = 0;
        public DateTime? start_date { get; set; }
        [StringLength(50)]
        public string sub_task { get; set; }
        [StringLength(50)]
        public string task_id { get; set; }
         
    }
    public class SubTaskUpdatedate
    {
        public string id { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? due_date { get; set; }
    }
        public class SubTaskUpdate
    {
        public string id { get; set; }
        public string description { get; set; }
        public DateTime? due_date { get; set; }
        public int? hour { get; set; }
        public DateTime? start_date { get; set; }
        [StringLength(50)]
        public string sub_task { get; set; }
        [StringLength(50)]
        public string task_id { get; set; }

    }
    public class SubTaskSearch : SearchParameter
    {
        public string Search_attachment { get; set; }
        public string Search_description { get; set; }
        public DateTime? Search_due_date { get; set; }
        public DateTime? Search_hour { get; set; }
        public Single? Search_percentage { get; set; }
        public DateTime? Search_start_date { get; set; }
        public string Search_sub_task { get; set; }
        public string Search_task_id { get; set; }
        public string Search_task_status_id { get; set; }
        public string Search_user_id { get; set; }
        
    }
}