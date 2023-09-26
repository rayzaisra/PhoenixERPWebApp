using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("item_category", Schema = "dbo")]
    public class ItemCategories : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(400)]
        [Column("category_name"), StringLength(400)]
        public string CategoryName { get; set; }

        [Column("life_time_month")]
        public int? LifeTimeMonth { get; set; }

        [MaxLength(50)]
        [Column("coa_debit"), StringLength(50)]
        public string COADebit { get; set; }

        [MaxLength(50)]
        [Column("coa_credit"), StringLength(50)]
        public string COACredit { get; set; }
    }
}
