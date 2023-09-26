using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("hiring_request_type", Schema = "hr")]
    public partial class HiringRequestType : GenericEntity<string>
    {
        public string hiring_type_name { get; set; }
    }
}
