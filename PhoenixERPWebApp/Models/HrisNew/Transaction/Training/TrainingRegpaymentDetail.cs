using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace PhoenixERPWebApp.Models
{
    public class TrainingRegpaymentDetail : HrisEntityBase
    {
        public TrainingRegpaymentDetail()
        {

        }
        public string Id { get; set; }
        public string RegPaymentId { get; set; }
        public string MasterTrainingRequestId { get; set; }
        public string StatusRequest { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Remarks { get; set; }
        public string ApprovalId { get; set; }
        public string AnyNeeds { get; set; }
        public string BussinessUnitFinanceId { get; set; }
        public string Action { get; set; }
        [NotMapped]
        public string MasterTrainingRequestName { get; set; }

    }
}
