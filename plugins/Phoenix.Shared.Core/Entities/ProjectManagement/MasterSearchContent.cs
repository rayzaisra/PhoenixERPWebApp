using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("master_search_content",Schema ="pm")]
    public partial class MasterSearchContent : GenericEntity<string>
    {
        public DateTime? created_date { get; set; }
        public bool isdeleted { get; set; }
        public string screen { get; set; }
        public string search_content { get; set; }
        public string updated_by { get; set; }
        public DateTime? updated_date { get; set; }
    }
}