using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class ApprovalStatusDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string approval_group_id { get; set; }
        [StringLength(50)]
        public string approval_status { get; set; }
        public int? status_level { get; set; }
        [StringLength(400)]
        [Required]
        public string status_name { get; set; }
    }
    public class ApprovalStatusNew
    {
        [StringLength(50)]
        [Required]
        public string approval_group_id { get; set; }
        [StringLength(50)]
        public string approval_status { get; set; }
        public int? status_level { get; set; }
        [StringLength(400)]
        [Required]
        public string status_name { get; set; }
    }            
    public class ApprovalStatusSearch : SearchParameter
    {
        public string Search_approval_group_id { get; set; }
        public string Search_approval_status { get; set; }
        public int? Search_status_level { get; set; }
        public string Search_status_name { get; set; }
        
    }
}