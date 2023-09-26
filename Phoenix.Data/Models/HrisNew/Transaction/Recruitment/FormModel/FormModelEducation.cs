using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models
{
	public class FormModelEducation
	{
		[Key]
		[MaxLength(50)]
		[Column(nameof(Id)), StringLength(50)]
		public string Id { get; set; }

		[MaxLength(100)]
		[Column(nameof(Majors)), StringLength(100)]
		public string Majors { get; set; }

		[MaxLength(50)]
		[Column("employee_basic_info_id"), StringLength(50)]
		public string EmployeeBasicInfoId { get; set; }

		[MaxLength(100)]
		[Column(nameof(Qualification)), StringLength(100)]
		public string Qualification { get; set; }

		//fake model for create id in form
		
		public string EEducationLevelId { get; set; }
		
		public string EEducationLevel { get; set; }

		[Required]
		[MaxLength(60)]
		[Column("institution_id"), StringLength(60)]
		public string EInstitutionId { get; set; }
		public string Institution { get; set; }

		[MaxLength(100)]
		[Column(nameof(Majors)), StringLength(100)]
		public string EMajors { get; set; }

		[MaxLength(2000)]
		[StringLength(2000)]
		public string EAddress { get; set; }

		[Required]
		[Column("start_education")]
		[DisplayFormat(DataFormatString = "{0:yyyy}", ApplyFormatInEditMode = true)]
		public DateTime EStartEducation { get; set; }

		[Required]
		[Column("end_education")]
		[DisplayFormat(DataFormatString = "{0:yyyy}", ApplyFormatInEditMode = true)]
		public DateTime EEndEducation { get; set; }

		[MaxLength(50)]
		[Column("employee_basic_info_id"), StringLength(50)]
		public string EEmployeeBasicInfoId { get; set; }

		[MaxLength(100)]
		[Column(nameof(Qualification)), StringLength(100)]
		public string EQualification { get; set; }

		[MaxLength(150)]
		[Column("other_institution"), StringLength(150)]
		public string OtherInstitution { get; set; }
	}
}
