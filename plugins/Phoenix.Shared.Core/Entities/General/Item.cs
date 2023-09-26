using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("item",Schema ="dbo")]
    public partial class Item : GenericEntity<string>
    {
        public string item_category_id { get; set; }
        public string item_code { get; set; }
        public string item_name { get; set; }
        public string organization_id { get; set; }
    }
}