using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class ApprovalDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string application_entity_id { get; set; }
        [StringLength(50)]
        [Required]
        public string approval_group_id { get; set; }
        [StringLength(50)]
        [Required]
        public string record_id { get; set; }
        [StringLength(50)]
        public string status { get; set; }
    }
    public class ApprovalNew
    {
        [StringLength(50)]
        [Required]
        public string application_entity_id { get; set; }
        [StringLength(50)]
        [Required]
        public string approval_group_id { get; set; }
        [StringLength(50)]
        [Required]
        public string record_id { get; set; }
        [StringLength(50)]
        public string status { get; set; }
    }            
    public class ApprovalSearch : SearchParameter
    {
        public string Search_application_entity_id { get; set; }
        public string Search_approval_group_id { get; set; }
        public string Search_record_id { get; set; }
        public string Search_status { get; set; }
        
    }
}