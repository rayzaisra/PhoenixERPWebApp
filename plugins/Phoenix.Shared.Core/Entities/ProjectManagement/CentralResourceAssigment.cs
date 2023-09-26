using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("central_resource_assigment",Schema ="pm")]
    public partial class CentralResourceAssigment : GenericEntity<string>
    {
        public string approval_status_id { get; set; }
        public string central_resource_id { get; set; }
        public string client_brief_id { get; set; }
        public string jobnumber { get; set; }
    }
}