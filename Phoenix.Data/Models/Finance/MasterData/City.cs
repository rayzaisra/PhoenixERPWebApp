using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table(nameof(City), Schema = "dbo")]
    public class City : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("country_id"), StringLength(50)]
        public string CountryId { get; set; }

        [MaxLength(50)]
        [Column("province_id"), StringLength(50)]
        public string ProvinceId { get; set; }

        [MaxLength(400)]
        [Column("city_name"), StringLength(400)]
        public string CityName { get; set; }

        [NotMapped]
        public string CountryName { get; set; }

        [NotMapped]
        public string ProvinceName { get; set; }
    }
}