using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table(nameof(GetFrom), Schema = "fn")]
    public class GetFrom : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("get_from_name"), StringLength(50)]
        public string GetFromName { get; set; }

        [Column("Description"), StringLength(200)]
        public string Description { get; set; }


    }
}
