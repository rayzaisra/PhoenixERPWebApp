using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("shareservices", Schema = "pm")]
    public class ShareServices : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(150)]
        [Column("shareservice_name"), StringLength(150)]
        public string ShareserviceName { get; set; }

        [MaxLength(255)]
        [Column("detail_shareservices"), StringLength(255)]
        public string DetailShareservices { get; set; }

        [MaxLength(50)]
        [Column("mainservice_category_id"), StringLength(50)]
        public string MainserviceCategoryId { get; set; }

        [NotMapped]
        public string MainserviceCategoryName { get; set; }
    }
}