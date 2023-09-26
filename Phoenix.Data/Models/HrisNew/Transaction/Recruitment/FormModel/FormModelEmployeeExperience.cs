using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models
{
	public class FormModelEmployeeExperience
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
		[Column("Phone"), StringLength(50)]
		[DisplayName("Company Phone")]
		public string EmplyeeExperiencePhone { get; set; }

		[MaxLength(2000)]
		[Column(nameof(Remark)), StringLength(2000)]
		[DisplayName("Remark")]
		public string Remark { get; set; }

		[Required]
		[MaxLength(2000)]
		[Column("Address"), StringLength(2000)]
		[DisplayName("Address")]
		public string EmplyeeExperienceAddress { get; set; }

		[Required]
		[Column("start_date")]
		[DisplayName("Start Year")]
		[DisplayFormat(DataFormatString = "{0:yyyy}", ApplyFormatInEditMode = true)]
		public DateTime? EmplyeeExperienceStartDate { get; set; }

		[Required]
		[Column("end_date")]
		[DisplayName("End Year")]
		[DisplayFormat(DataFormatString = "{0:yyyy}", ApplyFormatInEditMode = true)]
		public DateTime? EmplyeeExperienceEndDate { get; set; }

		[Required]
		[MaxLength(50)]
		[Column(nameof(Position)), StringLength(50)]
		[DisplayName("Position")]
		public string Position { get; set; }

		[MaxLength(50)]
		[Column("employee_basic_info_id"), StringLength(50)]
		[DisplayName("Employee Basic Info Id")]
		public string EmployeeBasicInfoId { get; set; }
	}
}
