using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("pe_file_folder", Schema = "pm")]
    public partial class PeFileFolder : GenericEntity<string>
    {
        public string parent_id { get; set; }
        public string parent_entity { get; set; }
        public string folder_path { get; set; }
    }
}