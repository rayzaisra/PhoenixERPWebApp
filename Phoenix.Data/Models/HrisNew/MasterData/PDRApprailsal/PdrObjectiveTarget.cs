using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("pdr_objective_target", Schema = "hr")]
    public class PdrObjectiveTarget : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("employee_id"), StringLength(50)]
        public string EmployeeId { get; set; }

        [Column(nameof(Year))]
        public int? Year { get; set; }

        [NotMapped]
        public InfoEmployee infoEmployee { get; set; }

        [NotMapped]
        public List<PdrObjectiveTargetDetail> PdrObjectiveTargetDetails { get; set; }
    }
}