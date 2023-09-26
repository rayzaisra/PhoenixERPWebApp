using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("client_brief_content",Schema ="pm")]
    public partial class ClientBriefContent : GenericEntity<string>
    {
        public string client_brief_id { get; set; }
        public string file { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
}