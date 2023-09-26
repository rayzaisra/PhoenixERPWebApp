using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("item", Schema = "dbo")]
    public class Item : FinanceEntityBase
    {
        [Key, Required]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [NotMapped]
        public string type_name { get; set; }

        [Column("organization_id")]
        public string organization_id { get; set; }

        [Column("item_category_id")]
        public string ItemCategoryId { get; set; }

        [Column("item_type_id")]
        public string ItemTypeId { get; set; }

        [Column("item_name")]
        public string ItemName { get; set; }

        [Column("item_code")]
        public string ItemCode { get; set; }

        [Column("uom_id")]
        public string UomId { get; set; }


    }
}