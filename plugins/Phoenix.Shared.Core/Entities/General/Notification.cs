using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("notification", Schema = "dbo")]
    public partial class Notification : GenericEntity<string>
    {
        public string notification_type_id { get; set; }
        public string user_id { get; set; }
        public string module { get; set; }
        public string item_id { get; set; }
        public bool is_seen { get; set; }
    }
}