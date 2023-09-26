using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("shared_record",Schema ="dbo")]
    public partial class SharedRecord : GenericEntity<string>
    {
        public string application_entity_id { get; set; }
        public bool can_read { get; set; }
        public bool can_write { get; set; }
        public string record_id { get; set; }
        public string shared_to { get; set; }
    }
}