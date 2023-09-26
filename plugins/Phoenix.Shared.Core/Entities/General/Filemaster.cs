using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("filemaster",Schema ="dbo")]
    public partial class Filemaster : GenericEntity<string>
    {
        public string name { get; set; }
        public string type { get; set; }
        public string mimetype { get; set; }
        public long size { get; set; }
        public string realname { get; set; }
        public string thumbnail { get; set; }
        public string tags { get; set; }
        public string filepath { get; set; }
        public string application_entity_id { get; set; }
        public string record_id { get; set; }
    }
}