using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{

    [Table("training_reg_payment", Schema = "hr")]
    public class TrainingRegPayment : HrisEntityBase
    {

        public TrainingRegPayment()
        {
            trainingRegPaymentDeail = new List<TrainingRegpaymentDetail>();
            trainingRequisition = new TrainingRequisition();
        }

        [Key]
        [MaxLength(50)]
        [Column("id"), StringLength(50)]
        public string Id { get; set; }

        [Column("training_id"), StringLength(50)]
        public string TrainingId { get; set; }

        [Column("employee_basic_info_id"), StringLength(50)]
        public string EmployeeBasicInfoId { get; set; }

        [Column("task_name"), StringLength(100)]
        public string TaskName { get; set; }


        [Column("job_name"), StringLength(100)]
        public string JobName { get; set; }

        [Column("note")]
        public string Note { get; set; }

        [Column("venue"), StringLength(100)]
        public string Venue { get; set; }

        [Column("total_add_fee")]
        public decimal TotalAddFee { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("approval_id"), StringLength(100)]
        public string ApprovalId { get; set; }

        [Column("add_recomendation")]
        public string AddRecomendation { get; set; }

        [Column("business_unit_finance_id")]
        public string BussinessUnitFinanceId { get; set; }
        
        [NotMapped]
        public List<TrainingRegpaymentDetail> trainingRegPaymentDeail { get; set; }

        [NotMapped]
        public TrainingRequisition trainingRequisition { get; set; }

        [NotMapped]
        public string Action { get; set; }
        [NotMapped]
        public string Code { get; set; }

    }
}
