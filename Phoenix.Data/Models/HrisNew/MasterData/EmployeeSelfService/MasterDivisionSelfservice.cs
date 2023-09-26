using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("master_division_selfservice", Schema = "hr")]
    public class MasterDivisionSelfservice : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        [Column("business_unit_id"), StringLength(50)]
        public string BusinessUnitId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("business_unit_name"), StringLength(50)]
        public string BusinessUnitName { get; set; }
    }
}