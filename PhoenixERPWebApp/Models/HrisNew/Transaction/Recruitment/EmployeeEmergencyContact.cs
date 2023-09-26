using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models
{
	public class EmployeeEmergencyContact : HrisEntityBase
	{
		[DisplayName("Id")]
		public string Id { get; set; }

		[MaxLength(50)]
		[Column("employee_basic_info_id"), StringLength(50)]
		[DisplayName("Employee Basic Info Id")]
		public string EmployeeBasicInfoId { get; set; }

		[MaxLength(50)]
		[Column("family_status_id"), StringLength(50)]
		[DisplayName("Family Status")]
		public string FamilyStatusId { get; set; }

		[DisplayName("Family Status")]
		public string FamilyStatus { get; set; }

		[MaxLength(100)]
		[Column("full_name"), StringLength(100)]
		[DisplayName("Full Name")]
		public string FullName { get; set; }

		[MaxLength(50)]
		[Column("phone_no"), StringLength(50)]
		[DisplayName("Phone No")]
		public string PhoneNo { get; set; }

		[MaxLength(50)]
		[Column("other_no"), StringLength(50)]
		[DisplayName("Other No")]
		public string OtherNo { get; set; }

		[MaxLength(100)]
		[Column(nameof(Address)), StringLength(100)]
		[DisplayName("Address")]
		public string Address { get; set; }

		//model fake to create id on form

		[DisplayName("Family Status")]
		public string EmergencyFamilyStatusId { get; set; }

		[MaxLength(100)]
		[Column("full_name"), StringLength(100)]
		[DisplayName("Full Name")]
		public string EmergencyFullName { get; set; }

		[MaxLength(50)]
		[Column("phone_no"), StringLength(50)]
		[DisplayName("Phone No")]
		public string EmergencyPhoneNo { get; set; }

		[MaxLength(50)]
		[Column("other_no"), StringLength(50)]
		[DisplayName("Other No")]
		public string EmergencyOtherNo { get; set; }

		[MaxLength(2000)]
		[Column(nameof(Address)), StringLength(2000)]
		[DisplayName("Address")]
		public string EmergencyAddress { get; set; }
	}
}
