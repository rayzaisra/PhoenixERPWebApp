using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
	public class Family : HrisEntityBase
	{
		[Key]
		[MaxLength(50)]
		[Column(nameof(Id)), StringLength(50)]
		[DisplayName("Id")]
		public string Id { get; set; }
		
		[MaxLength(50)]
		[Column("employee_basic_info_id"), StringLength(50)]
		[DisplayName("Employee Basic Info Id")]
		public string EmployeeBasicInfoId { get; set; }

		[Required]
		[MaxLength(50)]
		[Column("family_status_id"), StringLength(50)]
		[DisplayName("Family Status Id")]
		public string FamilyStatusId { get; set; }

		[Required]
		[MaxLength(50)]
		[StringLength(50)]
		[DisplayName("Name")]
		public string fName { get; set; }

		[MaxLength(50)]
		[Column("phone_no"), StringLength(50)]
		[DisplayName("Phone No")]
		public string fPhoneNo { get; set; }

		[MaxLength(50)]
		[Column("other_no"), StringLength(50)]
		[DisplayName("Other No")]
		public string fOtherNo { get; set; }

		[MaxLength(2000)]
		[Column("Address"), StringLength(2000)]
		[DisplayName("Address")]
		public string fAddress { get; set; }

		[MaxLength(50)]
		[Column("place_birth"), StringLength(50)]
		[DisplayName("Place Birth")]
		public string PlaceBirth { get; set; }

		[Column(nameof(Birthday))]
		[DisplayName("Birthday")]
		[DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
		public DateTime? Birthday { get; set; }

		[MaxLength(50)]
		[StringLength(50)]
		[DisplayName("Gender")]
		public string fGender { get; set; }
		public string Gender { get; set; }

		[MaxLength(50)]
		[Column("religion_id"), StringLength(50)]
		[DisplayName("Religion Id")]
		public string fReligionId { get; set; }
		public string fReligion { get; set; }

		[MaxLength(50)]
		[Column("education_level_id"), StringLength(50)]
		[DisplayName("Education Level")]
		public string fEducationLevelId { get; set; }

		public string fEducationLevel { get; set; }

		[MaxLength(50)]
		[Column("institution_id"), StringLength(50)]
		[DisplayName("Institution Id")]
		public string fInstitutionId { get; set; }

		[Column(nameof(Description)), StringLength(2147483647)]
		[DisplayName("Description")]
		public string Description { get; set; }

		[MaxLength(50)]
		[Column("married_status_id"), StringLength(50)]
		[DisplayName("Married Status")]
		public string fMarriedStatusId { get; set; }
		
		[DisplayName("Family Status")]
		public string fFamilyStatus { get; set; }

		[DisplayName("Married Status")]
		public string fMarriedStatus { get; set; }
		
	}
}
