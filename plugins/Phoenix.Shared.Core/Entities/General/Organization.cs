using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("organization",Schema ="dbo")]
    public partial class Organization : GenericEntity<string>
    {
        public bool is_default_organization { get; set; }
        public string organization_name { get; set; }
        public string parent_organization_id { get; set; }
        public string smtp_from_address { get; set; }
        public string smtp_password { get; set; }
        public int? smtp_port { get; set; }
        public string smtp_server { get; set; }
        public string smtp_username { get; set; }
    }
}