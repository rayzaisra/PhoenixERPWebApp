using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("role_access",Schema ="dbo")]
    public partial class RoleAccess : GenericEntity<string>
    {
        public long? access_activate { get; set; }
        public long? access_append { get; set; }
        public long? access_approve { get; set; }
        public long? access_create { get; set; }
        public long? access_delete { get; set; }
        public long? access_level { get; set; }
        public long? access_lock { get; set; }
        public long? access_read { get; set; }
        public long? access_share { get; set; }
        public long? access_update { get; set; }
        public string application_entity_id { get; set; }
        public string application_role_id { get; set; }
    }
}