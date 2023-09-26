using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models.HrisNew.MasterData.MasterChecklistCategory
{
    [Table("employee_checklist_category", Schema = "hr")]
    public class MasterChecklistCategory : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }
        [MaxLength(255)]
        [Column("checklist_category"), StringLength(255)]
        public string ChecklistCategory { get; set; }
    }
}
