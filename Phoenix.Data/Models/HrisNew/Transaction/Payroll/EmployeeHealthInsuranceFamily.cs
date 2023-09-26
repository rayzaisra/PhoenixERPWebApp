using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("employee_health_insurance_family", Schema = "hr")]
    public class EmployeeHealthInsuranceFamily : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("employee_basic_info_id"), StringLength(50)]
        public string EmployeeBasicInfoId { get; set; }

        [MaxLength(50)]
        [Column("family_id"), StringLength(50)]
        public string FamilyId { get; set; }

        [NotMapped]
        public string FamilyStatus { get; set; }
        [NotMapped]
        public string Gender { get; set; }
        [NotMapped]
        public DateTime? DateBirth { get; set; }
        [NotMapped]
        public string FamilyName { get; set; }
    }
}