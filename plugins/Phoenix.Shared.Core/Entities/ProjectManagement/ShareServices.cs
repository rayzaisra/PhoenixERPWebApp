using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("shareservices", Schema = "pm")]
    public partial class ShareServices : GenericEntity<string>
    {
        public string shareservice_name { get; set; }
        public string detail_shareservices { get; set; }
    }
}
