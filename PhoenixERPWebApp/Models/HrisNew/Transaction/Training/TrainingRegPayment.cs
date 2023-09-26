using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class TrainingRegPayment : HrisEntityBase
    {

        public TrainingRegPayment()
        {
           
        }
        public string Id { get; set; }
        public string TrainingId { get; set; }
        public string EmployeeBasicInfoId { get; set; }
        public string TaskName { get; set; }
        public string JobName { get; set; }
        public string Note { get; set; }
        public string Venue { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal TotalAddFee { get; set; }
        public string Status { get; set; }
        public string ApprovalId { get; set; }
        public string AddRecomendation { get; set; }

        public string BussinessUnitFinanceId { get; set; }
      
        public List<TrainingRegpaymentDetail> trainingRegPaymentDeail { get; set; }
        public TrainingRequisition trainingRequisition { get; set; }

        [NotMapped]
        public string Action { get; set; }
        [NotMapped]
        public string Code { get; set; }


    }
}
