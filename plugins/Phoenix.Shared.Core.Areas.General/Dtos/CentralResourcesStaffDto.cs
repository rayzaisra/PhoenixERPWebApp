using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class CentralResourcesStaffDto
    {
        [Required]
        public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string central_resources_id { get; set; }
        public string central_resources_name { get; set; }
        [StringLength(50)]
        [Required]
        public string user_id { get; set; }
        public string user_full_name { get; set; }
        public string user_name { get; set; }
    }
    public class CentralResourcesStaffList
    {

        public string Id { get; set; }

        public string central_resources_id { get; set; }
        public string central_resources_name { get; set; }

        public string user_id { get; set; }
        public string app_fullname { get; set; }
        public string app_username { get; set; }
    }
    public class CentralResourcesStaffNew
    {
        [StringLength(50)]
        [Required]
        public string central_resources_id { get; set; }
        [StringLength(50)]
        [Required]
        public string user_id { get; set; }
    }            
    public class CentralResourcesStaffSearch : SearchParameter
    {
        public string Search_central_resources_id { get; set; }
        public string Search_user_id { get; set; }
        
    }
}