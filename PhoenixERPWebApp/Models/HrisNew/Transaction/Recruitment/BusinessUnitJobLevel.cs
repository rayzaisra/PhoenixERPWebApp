using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    [Table("business_unit_job_level", Schema = "hr")]
    public class BusinessUnitJobLevel : HrisEntityBase
    {
        [Key, Required]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("business_unit_id"), StringLength(50)]
        public string BusinessUnitId { get; set; }

        [MaxLength(50)]
        [Column("job_level_id"), StringLength(50)]
        public string JobLevelId { get; set; }

        [MaxLength(50)]
        [Column("job_grade_id"), StringLength(50)]
        public string JobGradeId { get; set; }

        [MaxLength(50)]
        [Column("parent_id"), StringLength(50)]
        public string ParentId { get; set; }
		

        [MaxLength(50)]
        [Column("job_title_id"), StringLength(50)]
        public string JobTitleId { get; set; }

        [MaxLength(50)]
        [Column("business_unit_group_id"), StringLength(50)]
        public string BusinessUnitGroupId { get; set; }

        [MaxLength(50)]
        [Column("business_unit_subgroup_id"), StringLength(50)]
        public string BusinessUnitSubgroupId { get; set; }

		[NotMapped]
		public string BusinessUnitGroup { get; set; }

		[NotMapped]
		public string BusinessUnitSubgroup { get; set; }

		[NotMapped]
		public string BusinessUnitDivision { get; set; }

		[NotMapped]
		public string BusinessUnitDepartement { get; set; }

		[NotMapped]
		public string Text { get; set; }

        [NotMapped]
        public string UserName { get; set; }
        [NotMapped]
        public string JobGradeName { get; set; }

        [NotMapped]
        public string JobLevel { get; set; }

        [NotMapped]
        public string Supervisor { get; set; }
        [NotMapped]
        public string Remarks { get; set; }

        //public JobLevel JobLevel { get; set; }
        //public JobGrade JobGrade { get; set; }
        //public JobTitle JobTitle { get; set; }
    }
}