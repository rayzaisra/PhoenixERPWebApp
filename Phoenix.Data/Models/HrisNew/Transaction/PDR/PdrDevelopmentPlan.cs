using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("pdr_development_plan", Schema = "hr")]
    public class PdrDevelopmentPlan : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("pdr_employee_id"), StringLength(50)]
        public string PdrEmployeeId { get; set; }

        [Column(nameof(Activities)), StringLength(2147483647)]
        public string Activities { get; set; }

        [MaxLength(10)]
        [Column(nameof(Quarter)), StringLength(10)]
        public string Quarter { get; set; }

        //public PdrEmployee PdrEmployee { get; set; }
    }
}