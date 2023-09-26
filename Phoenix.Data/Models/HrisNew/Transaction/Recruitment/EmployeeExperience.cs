using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models
{
	[Table("employee_experience", Schema = "hr")]
	public class EmployeeExperience : HrisEntityBase
	{
		[Key]
		[MaxLength(50)]
		[Column(nameof(Id)), StringLength(50)]
		[DisplayName("Id")]
		public string Id { get; set; }

		[Required]
		[MaxLength(50)]
		[Column("company_name"), StringLength(50)]
		[DisplayName("Company Name")]
		public string CompanyName { get; set; }

		[MaxLength(50)]
		[Column(nameof(Phone)), StringLength(50)]
		[DisplayName("Phone")]
		public string Phone { get; set; }

		[MaxLength(2000)]
		[Column(nameof(Remark)), StringLength(2000)]
		[DisplayName("Remark")]
		public string Remark { get; set; }

		[Required]
		[MaxLength(2000)]
		[Column(nameof(Address)), StringLength(2000)]
		[DisplayName("Address")]
		public string Address { get; set; }

		[Required]
		[MaxLength(100)]
		[Column("start_date"), StringLength(100)]
		[DisplayName("Start Date")]
		public DateTime? StartDate { get; set; }

		[Required]
		[MaxLength(100)]
		[Column("end_date"), StringLength(100)]
		[DisplayName("End Date")]
		public DateTime? EndDate { get; set; }

		[Required]
		[MaxLength(50)]
		[Column(nameof(Position)), StringLength(50)]
		[DisplayName("Position")]
		public string Position { get; set; }

		[Required]
		[MaxLength(50)]
		[Column("employee_basic_info_id"), StringLength(50)]
		[DisplayName("Employee Basic Info Id")]
		public string EmployeeBasicInfoId { get; set; }
	}
}
