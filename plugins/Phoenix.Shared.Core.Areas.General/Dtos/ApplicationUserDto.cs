using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class ApplicationUserDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string access_token { get; set; }
        [StringLength(450)]
        [Required]
        public string app_fullname { get; set; }
        [StringLength(450)]
        [Required]
        public string app_password { get; set; }
        [StringLength(400)]
        [Required]
        public string app_username { get; set; }
        [StringLength(50)]
        public string business_unit_id { get; set; }
        [StringLength(450)]
        public string email { get; set; }
        public bool is_system_administrator { get; set; }
        [StringLength(50)]
        public string manager_id { get; set; }
        [StringLength(50)]
        public string mime_type { get; set; }
        [StringLength(50)]
        public string organization_id { get; set; }
        [StringLength(50)]
        public string phone { get; set; }
        [StringLength(-1)]
        public string photo_base64 { get; set; }
        [StringLength(50)]
        public string primary_team { get; set; }
        [StringLength(-1)]
        public string thumbnail_base64 { get; set; }
        public DateTime? token_expiry { get; set; }
    }
    public class ApplicationUserNew
    {
        [StringLength(50)]
        public string access_token { get; set; }
        [StringLength(450)]
        [Required]
        public string app_fullname { get; set; }
        [StringLength(450)]
        [Required]
        public string app_password { get; set; }
        [StringLength(400)]
        [Required]
        public string app_username { get; set; }
        [StringLength(50)]
        public string business_unit_id { get; set; }
        [StringLength(450)]
        public string email { get; set; }
        public bool is_system_administrator { get; set; }
        [StringLength(50)]
        public string manager_id { get; set; }
        [StringLength(50)]
        public string mime_type { get; set; }
        [StringLength(50)]
        public string organization_id { get; set; }
        [StringLength(50)]
        public string phone { get; set; }
        [StringLength(-1)]
        public string photo_base64 { get; set; }
        [StringLength(50)]
        public string primary_team { get; set; }
        [StringLength(-1)]
        public string thumbnail_base64 { get; set; }
        public DateTime? token_expiry { get; set; }
    }            
    public class ApplicationUserSearch : SearchParameter
    {
        public string Search_access_token { get; set; }
        public string Search_app_fullname { get; set; }
        public string Search_app_password { get; set; }
        public string Search_app_username { get; set; }
        public string Search_business_unit_id { get; set; }
        public string Search_email { get; set; }
        public bool Search_is_system_administrator { get; set; }
        public string Search_manager_id { get; set; }
        public string Search_mime_type { get; set; }
        public string Search_organization_id { get; set; }
        public string Search_phone { get; set; }
        public string Search_photo_base64 { get; set; }
        public string Search_primary_team { get; set; }
        public string Search_thumbnail_base64 { get; set; }
        public DateTime? Search_token_expiry { get; set; }
        
    }
}