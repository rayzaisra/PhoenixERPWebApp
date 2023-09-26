using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class DamDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string job_detail_id { get; set; }
        [StringLength(50)]
        public string dam_type_id { get; set; }
        [StringLength(50)]
        public string file_id { get; set; }
        [StringLength(250)]
        public string notes { get; set; }
        [StringLength(50)]
        public string sub_task_id { get; set; }
        [StringLength(50)]
        public string task_id { get; set; }
    }
    public class DamJob: JobDetailForm
    {
        public List<DamNew> DamFile { get; set; }
    }
    public class DamTypeStatusList
    {
        public string dam_type_name { get; set; }
        public string dam_type_id { get; set; }
        public string job_status_id { get; set; }
        public string job_status_name { get; set; }
        public bool? is_default { get; set; }
    }
 
    public class DamNew
    {
        [StringLength(50)]
        public string job_detail_id { get; set; }
        [StringLength(50)]
        public string dam_type_id { get; set; }
        [StringLength(50)]
        public string file_id { get; set; }
        [StringLength(250)]
        public string notes { get; set; }
        [StringLength(50)]
        public string sub_task_id { get; set; }
        [StringLength(50)]
        public string task_id { get; set; }
    }            
    public class DamSearch : SearchParameter
    {
        public string Search_dam { get; set; }
        public string Search_dam_type_id { get; set; }
        public string Search_file_id { get; set; }
        public string Search_notes { get; set; }
        public string Search_sub_task_id { get; set; }
        public string Search_task_id { get; set; }
        
    }

    public class DamFileSharingDto
    {
        public string[] ShareToEmployeeId { get; set; }
        public string[] FileShareId { get; set; }
    }
}