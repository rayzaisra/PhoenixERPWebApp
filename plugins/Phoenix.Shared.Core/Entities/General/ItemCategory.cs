using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("item_category",Schema ="dbo")]
    public partial class ItemCategory : GenericEntity<string>
    {
        public string category_name { get; set; }
        public string organization_id { get; set; }
    }
}