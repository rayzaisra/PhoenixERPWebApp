using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models
{
	[Table("employee_emergency_contact", Schema = "hr")]
	public class EmployeeEmergencyContact : HrisEntityBase
	{
		[Key]
		[MaxLength(50)]
		[Column(nameof(Id)), StringLength(50)]
		public string Id { get; set; }

		[MaxLength(50)]
		[Column("employee_basic_info_id"), StringLength(50)]
		public string EmployeeBasicInfoId { get; set; }

		[MaxLength(50)]
		[Column("family_status_id"), StringLength(50)]
		public string FamilyStatusId { get; set; }

		[MaxLength(100)]
		[Column("full_name"), StringLength(100)]
		public string FullName { get; set; }

		[MaxLength(50)]
		[Column("phone_no"), StringLength(50)]
		public string PhoneNo { get; set; }

		[MaxLength(50)]
		[Column("other_no"), StringLength(50)]
		public string OtherNo { get; set; }

		[MaxLength(2000)]
		[Column(nameof(Address)), StringLength(2000)]
		public string Address { get; set; }
	}
}
