using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models
{
	public class FormModelEmployeeTraining
	{
		[Key]
		[MaxLength(50)]
		[Column(nameof(Id)), StringLength(50)]
		[DisplayName("Id")]
		public string Id { get; set; }

		[Required]
		[MaxLength(150)]
		[Column("name_training"), StringLength(50)]
		[DisplayName("Course Name")]
		public string NameTraining { get; set; }

		[Required]
		[MaxLength(50)]
		[Column("institution_id"), StringLength(50)]
		[DisplayName("Institution Id")]
		public string EmployeeTrainingInstitutionId { get; set; }

		[DisplayName("Institution")]
		public string EmployeeTrainingInstitution { get; set; }

		[Required]
		[MaxLength(2000)]
		[Column("Address"), StringLength(2000)]
		[DisplayName("Address")]
		public string EmployeeTrainingAddress { get; set; }

		[Required]
		[Column("start_training")]
		[DisplayName("Start Year")]
		[DisplayFormat(DataFormatString = "{0:yyyy}", ApplyFormatInEditMode = true)]
		public DateTime? StartTraining { get; set; }

		[Required]
		[Column("end_training")]
		[DisplayName("End Year")]
		[DisplayFormat(DataFormatString = "{0:yyyy}", ApplyFormatInEditMode = true)]
		public DateTime? EndTraining { get; set; }

		[Required]
		[MaxLength(50)]
		[Column("no_certification"), StringLength(50)]
		[DisplayName("No Certification")]
		public string NoCertification { get; set; }

		[Column("expired_certificate")]
		[DisplayName("Expired Certificate")]
		[DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
		public DateTime? ExpiredCertificate { get; set; }

		[Required]
		[MaxLength(2000)]
		[Column("Description"), StringLength(2000)]
		[DisplayName("Description")]
		public string EmployeeTrainingDescription { get; set; }

		[MaxLength(50)]
		[Column("employee_basic_info_id"), StringLength(50)]
		[DisplayName("Employee Basic Info Id")]
		public string EmployeeBasicInfoId { get; set; }

		[MaxLength(50)]
		[Column("filemaster_id"), StringLength(50)]
		[DisplayName("Filemaster Id")]
		public string FilemasterId { get; set; }

		public string EQualification { get; set; }
	}
}
