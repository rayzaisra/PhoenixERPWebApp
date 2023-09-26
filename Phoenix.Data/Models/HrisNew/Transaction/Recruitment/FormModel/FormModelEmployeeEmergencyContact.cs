using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models
{
	public class FormModelEmployeeEmergencyContact 
	{
		public string Id { get; set; }

		[MaxLength(50)]
		[Column("employee_basic_info_id"), StringLength(50)]
		public string EmployeeBasicInfoId { get; set; }

		[MaxLength(50)]
		[Column("family_status_id"), StringLength(50)]
		public string FamilyStatusId { get; set; }
		
		public string FamilyStatus { get; set; }

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

		//model fake to create id on form
		
		public string EmergencyFamilyStatusId { get; set; }

		[MaxLength(100)]
		[Column("full_name"), StringLength(100)]
		public string EmergencyFullName { get; set; }

		[MaxLength(50)]
		[Column("phone_no"), StringLength(50)]
		public string EmergencyPhoneNo { get; set; }

		[MaxLength(50)]
		[Column("other_no"), StringLength(50)]
		public string EmergencyOtherNo { get; set; }

		[MaxLength(100)]
		[Column(nameof(Address)), StringLength(100)]
		public string EmergencyAddress { get; set; }
	}
}
