using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models
{
	[Table(nameof(Education), Schema = "hr")]
	public class Education : HrisEntityBase
	{
		[Key]
		[MaxLength(50)]
		[Column(nameof(Id)), StringLength(50)]
		public string Id { get; set; }

		[Required]
		[MaxLength(50)]
		[Column("education_level_id"), StringLength(50)]
		public string EducationLevelId { get; set; }

		[Required]
		[MaxLength(60)]
		[Column("institution_id"), StringLength(60)]
		public string InstitutionId { get; set; }

		[MaxLength(100)]
		[Column(nameof(Majors)), StringLength(100)]
		public string Majors { get; set; }

		[Required]
		[MaxLength(2000)]
		[Column(nameof(Address)), StringLength(2000)]
		public string Address { get; set; }

		[Required]
		[MaxLength(100)]
		[Column("start_education"), StringLength(100)]
		public DateTime StartEducation { get; set; }

		[Required]
		[MaxLength(100)]
		[Column("end_education"), StringLength(100)]
		public DateTime EndEducation { get; set; }

		[Required]
		[MaxLength(50)]
		[Column("employee_basic_info_id"), StringLength(50)]
		public string EmployeeBasicInfoId { get; set; }

		[MaxLength(100)]
		[Column(nameof(Qualification)), StringLength(100)]
		public string Qualification { get; set; }

		[MaxLength(150)]
		[Column("other_institution"), StringLength(150)]
		public string OtherInstitution { get; set; }
	}
}
