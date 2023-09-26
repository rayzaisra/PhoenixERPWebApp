using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("label_attachment",Schema ="pm")]
    public partial class LabelAttachment : GenericEntity<string>
    {
        public string attachement { get; set; }
        public string client_brief_id { get; set; }
        public string description { get; set; }
        public string file_id { get; set; }
        public string label { get; set; }
    }
}