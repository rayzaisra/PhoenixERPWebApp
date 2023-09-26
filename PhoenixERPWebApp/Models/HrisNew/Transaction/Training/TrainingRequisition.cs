using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class TrainingRequisition : HrisEntityBase
    {

        public TrainingRequisition()
        {
            TrainingPayment = new TrainingRegPayment();
            TrainingPaymentDetail = new TrainingRegpaymentDetail();

        }
        public string Id { get; set; }
        public string TrainingId { get; set; }
        public string RequesterEmployeeBasicInfoId { get; set; }

        [DisplayName("Propose Start Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PurposeStartDate { get; set; }

        [DisplayName("Propose End Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PurposeEndDate { get; set; }
        public string Venue { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }
        public string Code { get; set; }
        public string AddRecomendation { get; set; }
        public string BusinessUnitId { get; set; }

        //not mapped
        public string GroupId { get; set; }
        public string SubgroupId { get; set; }
        public string DivisionId { get; set; }
        public string DepartementId { get; set; }

        //notmapped
        public string RequesterEmployeeBasicInfoName { get; set; }
        public string BusinessUnitName { get; set; }
        [DisplayName("Training Name")]
        public string TrainingName { get; set; }
        [DisplayName("Training Type")]
        public string TrainingType { get; set; }
        [DisplayName("Training Cost")]
        public decimal? TrainingCost { get; set; }
        [DisplayName("Status Bonding")]
        public bool? IsTrainingBonding { get; set; }
        public int? Participant { get; set; }
        public List<TrainingRequisitionDetail> ParticipantList { get; set; }
        public bool? isApprove { get; set; }
        public string RemarkRejected { get; set; }
        public string ApprovalId { get; set; }
        public string RegPaymentId { get; set; }

        public TrainingRegPayment TrainingPayment { get; set; }
        public TrainingRegpaymentDetail TrainingPaymentDetail { get; set; }


    }
}