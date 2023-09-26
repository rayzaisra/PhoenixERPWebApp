using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class RoleAccessDto
    {
        [Required]
            public string Id { get; set; }
        public long? access_activate { get; set; }
        public long? access_append { get; set; }
        public long? access_approve { get; set; }
        [Required]
        public long? access_create { get; set; }
        [Required]
        public long? access_delete { get; set; }
        public long? access_level { get; set; }
        public long? access_lock { get; set; }
        [Required]
        public long? access_read { get; set; }
        public long? access_share { get; set; }
        [Required]
        public long? access_update { get; set; }
        [StringLength(50)]
        [Required]
        public string application_entity_id { get; set; }
        [StringLength(50)]
        [Required]
        public string application_role_id { get; set; }
    }
    public class RoleAccessNew
    {
        public long? access_activate { get; set; }
        public long? access_append { get; set; }
        public long? access_approve { get; set; }
        [Required]
        public long? access_create { get; set; }
        [Required]
        public long? access_delete { get; set; }
        public long? access_level { get; set; }
        public long? access_lock { get; set; }
        [Required]
        public long? access_read { get; set; }
        public long? access_share { get; set; }
        [Required]
        public long? access_update { get; set; }
        [StringLength(50)]
        [Required]
        public string application_entity_id { get; set; }
        [StringLength(50)]
        [Required]
        public string application_role_id { get; set; }
    }            
    public class RoleAccessSearch : SearchParameter
    {
        public long? Search_access_activate { get; set; }
        public long? Search_access_append { get; set; }
        public long? Search_access_approve { get; set; }
        public long? Search_access_create { get; set; }
        public long? Search_access_delete { get; set; }
        public long? Search_access_level { get; set; }
        public long? Search_access_lock { get; set; }
        public long? Search_access_read { get; set; }
        public long? Search_access_share { get; set; }
        public long? Search_access_update { get; set; }
        public string Search_application_entity_id { get; set; }
        public string Search_application_role_id { get; set; }
        
    }
}