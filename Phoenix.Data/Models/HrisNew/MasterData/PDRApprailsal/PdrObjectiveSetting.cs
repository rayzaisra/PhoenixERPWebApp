using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("pdr_objective_setting", Schema = "hr")]
    public class PdrObjectiveSetting : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(100)]
        [Column("corporate_objective"), StringLength(100)]
        public string CorporateObjective { get; set; }

        [MaxLength(255)]
        [Column("objective_item"), StringLength(255)]
        public string ObjectiveItem { get; set; }

        [MaxLength(50)]
        [Column("bsc_corporate_objective_id"), StringLength(50)]
        public string BscCorporateObjectiveId { get; set; }

        [NotMapped]
        public string BscCorporateObjectiveName { get; set; }

        [MaxLength(50)]
        [Column("job_grade_id"), StringLength(50)]
        public string JobGradeId { get; set; }

        [NotMapped]
        public string JobGradeName { get; set; }
    }
}