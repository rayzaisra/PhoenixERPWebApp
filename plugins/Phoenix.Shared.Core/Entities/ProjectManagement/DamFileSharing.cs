using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities.ProjectManagement
{
    [Table("dam_file_sharing", Schema = "pm")]
    public class DamFileSharing : GenericEntity<string>
    {
        public string job_id { get; set; }
        public string file_id { get; set; }
        public string share_to { get; set; }
        public string status { get; set; }
        public string file_path { get; set; }
    }
}
