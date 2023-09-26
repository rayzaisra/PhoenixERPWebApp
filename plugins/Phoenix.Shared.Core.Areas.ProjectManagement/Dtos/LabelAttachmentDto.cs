using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class LabelAttachmentDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(10)]
        public string attachement { get; set; }
        [StringLength(50)]
        public string client_brief_id { get; set; }
        [StringLength(250)]
        public string description { get; set; }
        [StringLength(250)]
        public string file_id { get; set; }
        [StringLength(10)]
        public string label { get; set; }
    }
    public class LabelAttachmentNew
    {
        [StringLength(10)]
        public string attachement { get; set; }
        [StringLength(50)]
        public string client_brief_id { get; set; }
        [StringLength(250)]
        public string description { get; set; }
        [StringLength(250)]
        public string file_id { get; set; }
        [StringLength(10)]
        public string label { get; set; }
    }            
    public class LabelAttachmentSearch : SearchParameter
    {
        public string Search_attachement { get; set; }
        public string Search_client_brief_id { get; set; }
        public string Search_description { get; set; }
        public string Search_file_id { get; set; }
        public string Search_label { get; set; }
        
    }
}