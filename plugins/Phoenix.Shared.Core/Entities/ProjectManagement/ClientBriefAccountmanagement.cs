using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("client_brief_accountmanagement",Schema ="pm")]
    public partial class ClientBriefAccountmanagement : GenericEntity<string>
    {
        public string user_id { get; set; }
        public int? account_management_seq { get; set; }
        public string client_brief_id { get; set; }
    }
}