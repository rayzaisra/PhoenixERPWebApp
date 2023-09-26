using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class AuditTrailDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        [Required]
        public string application_entity_id { get; set; }
        [StringLength(50)]
        [Required]
        public string application_user_id { get; set; }
        [StringLength(450)]
        public string app_fullname { get; set; }
        [StringLength(450)]
        public string app_username { get; set; }
        [StringLength(-1)]
        public string new_record { get; set; }
        [StringLength(-1)]
        public string new_view { get; set; }
        [StringLength(-1)]
        public string old_record { get; set; }
        [StringLength(-1)]
        public string old_view { get; set; }
        [StringLength(50)]
        [Required]
        public string organization_id { get; set; }
        [StringLength(450)]
        public string organization_name { get; set; }
        [StringLength(50)]
        [Required]
        public string record_id { get; set; }
        [StringLength(50)]
        [Required]
        public string user_action { get; set; }
    }
    public class AuditTrailNew
    {
        [StringLength(50)]
        [Required]
        public string application_entity_id { get; set; }
        [StringLength(50)]
        [Required]
        public string application_user_id { get; set; }
        [StringLength(450)]
        public string app_fullname { get; set; }
        [StringLength(450)]
        public string app_username { get; set; }
        [StringLength(-1)]
        public string new_record { get; set; }
        [StringLength(-1)]
        public string new_view { get; set; }
        [StringLength(-1)]
        public string old_record { get; set; }
        [StringLength(-1)]
        public string old_view { get; set; }
        [StringLength(50)]
        [Required]
        public string organization_id { get; set; }
        [StringLength(450)]
        public string organization_name { get; set; }
        [StringLength(50)]
        [Required]
        public string record_id { get; set; }
        [StringLength(50)]
        [Required]
        public string user_action { get; set; }
    }            
    public class AuditTrailSearch : SearchParameter
    {
        public string Search_application_entity_id { get; set; }
        public string Search_application_user_id { get; set; }
        public string Search_app_fullname { get; set; }
        public string Search_app_username { get; set; }
        public string Search_new_record { get; set; }
        public string Search_new_view { get; set; }
        public string Search_old_record { get; set; }
        public string Search_old_view { get; set; }
        public string Search_organization_id { get; set; }
        public string Search_organization_name { get; set; }
        public string Search_record_id { get; set; }
        public string Search_user_action { get; set; }
        
    }
}