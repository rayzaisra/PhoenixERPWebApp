using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("client_brief_content_ref",Schema ="pm")]
    public partial class ClientBriefContentRef : GenericEntity<string>
    {
        public string description { get; set; }
        public string name { get; set; }
        public int? seq { get; set; }
    }
}