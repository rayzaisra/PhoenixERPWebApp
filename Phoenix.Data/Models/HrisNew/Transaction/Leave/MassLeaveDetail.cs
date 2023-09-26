using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("mass_leave_detail", Schema = "hr")]
    public class MassLeaveDetail : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("mass_leave_id"), StringLength(50)]
        public string MassLeaveId { get; set; }

        [MaxLength(50)]
        [Column("except_employee"), StringLength(50)]
        public string ExceptEmployee { get; set; }

        //public MassLeave MassLeave { get; set; }

        [NotMapped]
        public InfoEmployee InfoEmployee { get; set; }
    }
}