using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class TemporaryManagerDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string application_user_id { get; set; }
        [StringLength(450)]
        public string description { get; set; }
        [Required]
        public DateTime? end_date { get; set; }
        [StringLength(50)]
        [Required]
        public string manager_id { get; set; }
        [Required]
        public DateTime? start_date { get; set; }
    }
    public class TemporaryManagerNew
    {
        [StringLength(50)]
        [Required]
        public string application_user_id { get; set; }
        [StringLength(450)]
        public string description { get; set; }
        [Required]
        public DateTime? end_date { get; set; }
        [StringLength(50)]
        [Required]
        public string manager_id { get; set; }
        [Required]
        public DateTime? start_date { get; set; }
    }            
    public class TemporaryManagerSearch : SearchParameter
    {
        public string Search_application_user_id { get; set; }
        public string Search_description { get; set; }
        public DateTime? Search_end_date { get; set; }
        public string Search_manager_id { get; set; }
        public DateTime? Search_start_date { get; set; }
        
    }
}