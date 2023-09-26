using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class PackageDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(250)]
        public string description { get; set; }
        public long? job_category_id { get; set; }
        [StringLength(50)]
        public string package { get; set; }
        public int? value { get; set; }
    }
    public class PackageNew
    {
        [StringLength(250)]
        public string description { get; set; }
        public long? job_category_id { get; set; }
        [StringLength(50)]
        public string package { get; set; }
        public int? value { get; set; }
    }            
    public class PackageSearch : SearchParameter
    {
        public string Search_description { get; set; }
        public long? Search_job_category_id { get; set; }
        public string Search_package { get; set; }
        public int? Search_value { get; set; }
        
    }
}