using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("application_user",Schema ="dbo")]
    public partial class ApplicationUser : GenericEntity<string>
    {
        public string access_token { get; set; }
        public string app_fullname { get; set; }
        public string app_password { get; set; }
        public string app_username { get; set; }
        public string business_unit_id { get; set; }
        public string email { get; set; }
        public bool is_system_administrator { get; set; }
        public string manager_id { get; set; }
        public string mime_type { get; set; }
        public string organization_id { get; set; }
        public string phone { get; set; }
        public string photo_base64 { get; set; }
        public string primary_team { get; set; }
        public string thumbnail_base64 { get; set; }
        public DateTime? token_expiry { get; set; }
        public string employee_basic_info_id { get; set; }
    }
}