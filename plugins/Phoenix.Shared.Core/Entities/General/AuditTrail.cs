using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("audit_trail",Schema ="dbo")]
    public partial class AuditTrail : GenericEntity<string>
    {
        public string application_entity_id { get; set; }
        public string application_user_id { get; set; }
        public string app_fullname { get; set; }
        public string app_username { get; set; }
        public string new_record { get; set; }
        public string new_view { get; set; }
        public string old_record { get; set; }
        public string old_view { get; set; }
        public string organization_id { get; set; }
        public string organization_name { get; set; }
        public string record_id { get; set; }
        public string user_action { get; set; }
    }
}