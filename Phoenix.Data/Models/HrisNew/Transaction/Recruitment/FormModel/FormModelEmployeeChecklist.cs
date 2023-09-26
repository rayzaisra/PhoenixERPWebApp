using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    public class FormModelEmployeeChecklist : HrisEntityBase
	{
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("employee_basic_info_id"), StringLength(50)]
        public string EchkEmployeeBasicInfoId { get; set; }

        [MaxLength(255)]
        [StringLength(255)]
        public string EchkChecklistitem { get; set; }

        [StringLength(2147483647)]
        public string EchkDescription { get; set; }
		
        public DateTime? EchkWhentoalert { get; set; }		

        [MaxLength(50)]
        [Column("employee_checklist_category_id"), StringLength(50)]
        public string EchkEmployeeChecklistCategoryId { get; set; }

        [MaxLength(50)]
        [Column("business_unit_pic_id"), StringLength(50)]
        public string EchkBusinessUnitPicId { get; set; }

		[NotMapped]
		[DisplayName("Checlist Category")]
		public string EchkEmployeeChecklistCategory { get; set; }

		//public EmployeeBasicInfo EmployeeBasicInfo { get; set; }
		//public EmployeeChecklistCategory EmployeeChecklistCategory { get; set; }
	}

    public class FormModelEmployeeHistory : HrisEntityBase
    {
        public string Id { get; set; }

        public string EHistEmployeeBasicInfoId { get; set; }

        public string EHistEmployeeName { get; set; }

        public DateTime? Date { get; set; }

        public string Category { get; set; }

        public string Status { get; set; }

        public string Remarks { get; set; }
        public string FileId { get; set; }
    }

    public class EmployeeContractSign
    {
        public string ContractName { get; set; }
        public string ContractFile { get; set; }
       
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string FileId { get; set; }
        public string Ptpp { get; set; }
        public string ContractType { get; set; }
    }
}