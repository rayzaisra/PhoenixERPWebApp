using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities.Finance
{
    [Table("mainservice_category", Schema ="fn")]
    public partial class MainserviceCategory : BaseEntity<string>
    {
        public string owner_id { get; set; }
        public string organization_id { get; set; }
        public string name { get; set; }

        public bool is_deleted { get; set; }
        public bool? is_default { get; set; }
    }
}
