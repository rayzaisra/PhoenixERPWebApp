using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("proposal_file",Schema ="pm")]
    public partial class ProposalFile : GenericEntity<string>
    {
        public string client_brief_id { get; set; }
        public string file_id { get; set; }
    }
}