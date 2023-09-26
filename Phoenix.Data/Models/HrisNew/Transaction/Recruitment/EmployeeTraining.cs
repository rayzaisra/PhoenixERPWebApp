using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models
{
	[Table("employee_training", Schema = "hr")]
	public class EmployeeTraining : HrisEntityBase
	{
		[Key]
		[MaxLength(50)]
		[Column(nameof(Id)), StringLength(50)]
		[DisplayName("Id")]
		public string Id { get; set; }

		[Required]
		[MaxLength(150)]
		[Column("name_training"), StringLength(150)]
		[DisplayName("Name Training")]
		public string NameTraining { get; set; }

		[Required]
		[MaxLength(50)]
		[Column("institution_id"), StringLength(50)]
		[DisplayName("Institution Id")]
		public string InstitutionId { get; set; }

		[Required]
		[MaxLength(2000)]
		[Column(nameof(Address)), StringLength(2000)]
		[DisplayName("Address")]
		public string Address { get; set; }

		[Required]
		[MaxLength(100)]
		[Column("start_training"), StringLength(100)]
		[DisplayName("Start Training")]
		public DateTime? StartTraining { get; set; }

		[Required]
		[MaxLength(100)]
		[Column("end_training"), StringLength(100)]
		[DisplayName("End Training")]
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
		[Column(nameof(Description)), StringLength(2000)]
		[DisplayName("Description")]
		public string Description { get; set; }

		[Required]
		[MaxLength(50)]
		[Column("employee_basic_info_id"), StringLength(50)]
		[DisplayName("Employee Basic Info Id")]
		public string EmployeeBasicInfoId { get; set; }

		[MaxLength(50)]
		[Column("filemaster_id"), StringLength(50)]
		[DisplayName("Filemaster Id")]
		public string FilemasterId { get; set; }
	}
}
