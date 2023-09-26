using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models
{
    public class EmployeeChecklist : HrisEntityBase
	{
		[Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("employee_basic_info_id"), StringLength(50)]
		public string EchkEmployeeBasicInfoId { get; set; }

        [MaxLength(255)]
        [StringLength(255)]
		[DisplayName("Checklist Item")]
		public string EchkChecklistitem { get; set; }

        [StringLength(2147483647)]
		[DisplayName("Description")]
		public string EchkDescription { get; set; }

		[DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
		[DisplayName("When to Alert")]
		public DateTime? EchkWhentoalert { get; set; }

        [MaxLength(50)]
        [Column("employee_checklist_category_id"), StringLength(50)]
		[DisplayName("Checklist Category")]
		public string EchkEmployeeChecklistCategoryId { get; set; }

        [MaxLength(50)]
        [Column("business_unit_pic_id"), StringLength(50)]
		public string EchkBusinessUnitPicId { get; set; }

		[NotMapped]
		[DisplayName("Checlist Category")]
		public string EchkEmployeeChecklistCategory { get; set; }

		[NotMapped]
		[DisplayName("Checlist Category")]
		public string[] EchkEmployeeChecklistCategoryArrayId { get; set; }

		//public EmployeeBasicInfo EmployeeBasicInfo { get; set; }
		//public EmployeeChecklistCategory EmployeeChecklistCategory { get; set; }
	}
}