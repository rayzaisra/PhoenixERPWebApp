using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("organization_job_level",Schema ="hr")]
    public partial class OrganizationJobLevel : GenericEntity<string>
    {
        public string business_unit_id { get; set; }
        public string job_level_id { get; set; }
    }
}