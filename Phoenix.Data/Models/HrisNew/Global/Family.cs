using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table(nameof(Family), Schema = "hr")]
    public class Family : HrisEntityBase
    {
        [Key, Required]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("employee_basic_info_id"), StringLength(50)]
        public string EmployeeBasicInfoId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("family_status_id"), StringLength(50)]
        public string FamilyStatusId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column(nameof(Name)), StringLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        [Column("phone_no"), StringLength(50)]
        public string PhoneNo { get; set; }

        [MaxLength(50)]
        [Column("other_no"), StringLength(50)]
        public string OtherNo { get; set; }

        [MaxLength(2000)]
        [Column(nameof(Address)), StringLength(2000)]
        public string Address { get; set; }

        [MaxLength(50)]
        [Column("place_birth"), StringLength(50)]
        public string PlaceBirth { get; set; }

        [Column(nameof(Birthday))]
        public DateTime? Birthday { get; set; }

        [MaxLength(50)]
        [Column(nameof(Gender)), StringLength(50)]
        public string Gender { get; set; }

        [MaxLength(50)]
        [Column("religion_id"), StringLength(50)]
        public string ReligionId { get; set; }

        [MaxLength(50)]
        [Column("education_level_id"), StringLength(50)]
        public string EducationLevelId { get; set; }

        [MaxLength(50)]
        [Column("institution_id"), StringLength(50)]
        public string InstitutionId { get; set; }

        [Column(nameof(Description)), StringLength(2147483647)]
        public string Description { get; set; }

        [MaxLength(50)]
        [Column("married_status_id"), StringLength(50)]
        public string MarriedStatusId { get; set; }		

		[NotMapped]
        public FamilyStatus FamilyStatus { get; set; }
		
		/// <summary>
		/// Not Mapped
		/// </summary>
		[NotMapped]
		public string FamilyStatusName { get; set; }

		/// <summary>
		/// Not Mapped
		/// </summary>
		[NotMapped]
		public string MarriedStatusName { get; set; }

	}
}