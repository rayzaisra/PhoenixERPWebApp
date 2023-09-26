using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("training_requisition", Schema = "hr")]
    public class TrainingRequisition : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column("id"), StringLength(50)]
        public string Id { get; set; }

        [Column("training_id"), StringLength(50)]
        public string TrainingId { get; set; }

        [MaxLength(50)]
        [Column("employee_basic_info_id"), StringLength(50)]
        public string RequesterEmployeeBasicInfoId { get; set; }

        [Column("purpose_start_date")]
        public DateTime? PurposeStartDate { get; set; }

        [Column("purpose_end_date")]
        public DateTime? PurposeEndDate { get; set; }

        [Column("venue")]
        public string Venue { get; set; }

        [Column("notes")]
        public string Notes { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("add_recomendation")]
        public string AddRecomendation { get; set; }

        [Column("business_unit_id")]
        public string BusinessUnitId { get; set; }

        //notmapped
        [NotMapped]
        public string RequesterEmployeeBasicInfoName { get; set; }
        [NotMapped]
        public string BusinessUnitName { get; set; }
        [NotMapped]
        public string TrainingName { get; set; }
        [NotMapped]
        public string TrainingType { get; set; }
        [NotMapped]
        public List<TrainingRequisitionDetail> ParticipantList { get; set; }
        [NotMapped]
        public string RemarkRejected { get; set; }
        [NotMapped]
        public string ApprovalId { get; set; }

        [NotMapped]
        public string RegPaymentId { get; set; }


    }
}