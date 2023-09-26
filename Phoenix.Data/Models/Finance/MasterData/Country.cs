using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table(nameof(Country), Schema = "dbo")]
    public class Country : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(2)]
        [Column(nameof(Code)), StringLength(2)]
        public string Code { get; set; }

        [MaxLength(400)]
        [Column("country_name"), StringLength(400)]
        public string CountryName { get; set; }
    }
}