using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PhoenixERPWebApp.Models
{
	public class TrainingShareSession : HrisEntityBase
	{
        public TrainingShareSession()
        {
        }

        public string Id { get; set; }
        public string TrainingId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        public string TrainingRequisitionId { get; set; }

        public string EmployeeBasicInfoId { get; set; }

        public string TrainingRegPaymentId { get; set; }

        public string Note { get; set; }

        [NotMapped]
        public string Action { get; set; }

        [NotMapped]
        public string TrainingName { get; set; }
        [NotMapped]
        public string TrainingType { get; set; }

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
        [NotMapped]
        public List<TrainingRequisitionDetail> ParticipantList { get; set; }
       
    }
}
