using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
	[Table("training_share_session", Schema = "hr")]
	public class TrainingShareSession :  HrisEntityBase
	{
        [Key]
        [MaxLength(50)]
        [Column("id"), StringLength(50)]
        public string Id { get; set; }

        [Column("training_id"), StringLength(50)]
        public string TrainingId { get; set; }

        [Column("start_date")]
        public DateTime? StartDate { get; set; }

        [Column("end_date")]
        public DateTime? EndDate { get; set; }

        [Column("training_requisition_id"), StringLength(50)]
        public string TrainingRequisitionId { get; set; }

        [Column("employee_basic_info_id"), StringLength(50)]
        public string EmployeeBasicInfoId { get; set; }

        [Column("training_reg_payment_id"), StringLength(50)]
        public string TrainingRegPaymentId { get; set; }

        [Column("note"), StringLength(500)]
        public string Note { get; set; }

        [NotMapped]
        public string Action { get; set; }

        [NotMapped]
        public string TrainingName { get; set; }
        [NotMapped]
        public string TrainingType  { get; set; }

        [NotMapped]
        [DisplayName("Propose Start Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PurposeStartDate { get; set; }

        [NotMapped]
        [DisplayName("Propose End Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PurposeEndDate { get; set; }

        [NotMapped]
        public string TrainerEmployeeBasicInfoId { get; set; }

        [NotMapped]
        public string TrainerEmployeeBasicInfoName { get; set; }

       
    }
}
