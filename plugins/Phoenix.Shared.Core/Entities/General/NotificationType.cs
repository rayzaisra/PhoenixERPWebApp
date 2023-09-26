using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("notification_type", Schema ="dbo")]
    public partial class NotificationType : GenericEntity<string>
    {
        public string shorttext { get; set; }
        public string longtext { get; set; }
        public string href { get; set; }
        public bool sendmail { get; set; }
         
    }
}