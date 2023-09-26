using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("pe_file",Schema ="pm")]
    public partial class PeFile : GenericEntity<string>
    {
        public string file_name { get; set; }
        public string file_path { get; set; }
        public long file_size { get; set; }
        public string file_type { get; set; }
        public int? flag { get; set; }
        public DateTime? upload_date { get; set; }
        public string parent_id { get; set; }
        public string tags { get; set; }
        public string mimetype { get; set; }
    }
}