using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class PeFileDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(250)]
        public string file_name { get; set; }
        [StringLength(250)]
        public string file_path { get; set; }
        public long file_size { get; set; }
        [StringLength(250)]
        public string file_type { get; set; }
        public int? flag { get; set; }
        public DateTime? upload_date { get; set; }
        public string tags { get; set; }
        public string created_by { get; set; }
        public string mimetype { get; set; }
        public string parent_id { get; set; }
    }
    public class PeFileNew
    {
        [StringLength(250)]
        public string file_name { get; set; }
        [StringLength(250)]
        public string file_path { get; set; }
        public int? file_size { get; set; }
        [StringLength(250)]
        public string file_type { get; set; }
        public int? flag { get; set; }
        public DateTime? upload_date { get; set; }
    }            
    public class PeFileSearch : SearchParameter
    {
        public string Search_file_name { get; set; }
        public string Search_file_path { get; set; }
        public int? Search_file_size { get; set; }
        public string Search_file_type { get; set; }
        public int? Search_flag { get; set; }
        public DateTime? Search_upload_date { get; set; }
        
    }
}