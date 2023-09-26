using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models
{
	public class Education : HrisEntityBase
	{
		[Key]
		[MaxLength(50)]
		[Column(nameof(Id)), StringLength(50)]
		[DisplayName("Id")]
		public string Id { get; set; }		

		[MaxLength(100)]
		[Column(nameof(Majors)), StringLength(100)]
		[DisplayName("Majors")]
		public string Majors { get; set; }
		
		[MaxLength(50)]
		[Column("employee_basic_info_id"), StringLength(50)]
		[DisplayName("Employee Basic Info Id")]
		public string EmployeeBasicInfoId { get; set; }

		[MaxLength(100)]
		[Column(nameof(Qualification)), StringLength(100)]
		[DisplayName("Qualification")]
		public string Qualification { get; set; }

		//fake model for create id in form

		[DisplayName("Education Level Id")]
		public string EEducationLevelId { get; set; }

		[DisplayName("Education Level")]
		public string EEducationLevel { get; set; }

		[Required]
		[MaxLength(60)]
		[Column("institution_id"), StringLength(60)]
		[DisplayName("Institution Id")]
		public string EInstitutionId { get; set; }

		[DisplayName("Institution")]
		public string Institution { get; set; }

		[MaxLength(100)]
		[Column(nameof(Majors)), StringLength(100)]
		[DisplayName("Majors")]
		public string EMajors { get; set; }
		
		[MaxLength(2000)]
		[StringLength(2000)]
		[DisplayName("Location")]
		public string EAddress { get; set; }

		[Required]
		[Column("start_education")]
		[DisplayName("Start Education")]
		[DisplayFormat(DataFormatString = "{0:yyyy}", ApplyFormatInEditMode = true)]
		public DateTime EStartEducation { get; set; }

		[Required]
		[Column("end_education")]
		[DisplayName("End Education")]
		[DisplayFormat(DataFormatString = "{0:yyyy}", ApplyFormatInEditMode = true)]
		public DateTime EEndEducation { get; set; }
		
		[MaxLength(50)]
		[Column("employee_basic_info_id"), StringLength(50)]
		[DisplayName("Employee Basic Info Id")]
		public string EEmployeeBasicInfoId { get; set; }

		[MaxLength(100)]
		[Column(nameof(Qualification)), StringLength(100)]
		[DisplayName("Qualification Attained")]
		public string EQualification { get; set; }

		[MaxLength(100)]
		[Column("other_institution"), StringLength(150)]
		[DisplayName("Institution")]
		public string OtherInstitution { get; set; }
	}
}
