using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("uom", Schema = "dbo")]
    public class Uom : FinanceEntityBase
    {
        [Key, Required]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

      
        [Column("unit_name")]
        public string unit_name { get; set; }

       
        [Column("unit_symbol")]
        public string unit_symbol { get; set; }

       
    }
}