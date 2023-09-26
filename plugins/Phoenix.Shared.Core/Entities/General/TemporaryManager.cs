using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("temporary_manager",Schema ="dbo")]
    public partial class TemporaryManager : GenericEntity<string>
    {
        public string application_user_id { get; set; }
        public string description { get; set; }
        public DateTime? end_date { get; set; }
        public string manager_id { get; set; }
        public DateTime? start_date { get; set; }
    }
}