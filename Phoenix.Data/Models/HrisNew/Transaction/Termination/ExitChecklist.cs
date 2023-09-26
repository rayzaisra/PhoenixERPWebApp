using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("exit_checklist", Schema = "hr")]
    public class ExitChecklist : HrisEntityBase
    {
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("termination_id"), StringLength(50)]
        public string TerminationId { get; set; }

        [MaxLength(50)]
        [Column("employee_checklist_id"), StringLength(50)]
        public string EmployeeChecklistId { get; set; }

        [MaxLength(255)]
        [Column(nameof(Notes)), StringLength(255)]
        public string Notes { get; set; }

        [Column("is_done")]
        public bool? IsDone { get; set; }

        [Column("end_date")]
        public DateTime? EndDate { get; set; }

        [NotMapped]
        public Termination Termination { get; set; }
        
        [NotMapped]
        public string Checklistitem { get; set; }
        [NotMapped]
        public string InCharge { get; set; }
    }
}