using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class MasterSearchContentDto
    {
        [Required]
            public string Id { get; set; }
        public DateTime? created_date { get; set; }
        public bool isdeleted { get; set; }
        [StringLength(250)]
        public string screen { get; set; }
        [StringLength(50)]
        public string search_content { get; set; }
        [StringLength(50)]
        public string updated_by { get; set; }
        public DateTime? updated_date { get; set; }
    }
    public class MasterSearchContentNew
    {
        public DateTime? created_date { get; set; }
        public bool isdeleted { get; set; }
        [StringLength(250)]
        public string screen { get; set; }
        [StringLength(50)]
        public string search_content { get; set; }
        [StringLength(50)]
        public string updated_by { get; set; }
        public DateTime? updated_date { get; set; }
    }            
    public class MasterSearchContentSearch : SearchParameter
    {
        public DateTime? Search_created_date { get; set; }
        public bool Search_isdeleted { get; set; }
        public string Search_screen { get; set; }
        public string Search_search_content { get; set; }
        public string Search_updated_by { get; set; }
        public DateTime? Search_updated_date { get; set; }
        
    }
}