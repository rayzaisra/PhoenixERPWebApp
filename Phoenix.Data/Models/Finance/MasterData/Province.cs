using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table(nameof(Province), Schema = "dbo")]
    public class Province : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(400)]
        [Column("province_name"), StringLength(400)]
        public string ProvinceName { get; set; }

        [MaxLength(50)]
        [Column("country_id"), StringLength(50)]
        public string CountryId { get; set; }

        [NotMapped]
        public string CountryName { get; set; }
    }
}