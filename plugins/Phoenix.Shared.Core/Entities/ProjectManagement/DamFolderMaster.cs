using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("dam_folder_master", Schema = "pm")]
    public partial class DamFolderMaster : GenericEntity<string>
    {
        public string root_name { get; set; }
        public string folder_name { get; set; }
        public string application_entity_id { get; set; }
        public bool is_root { get; set; }
    }
}