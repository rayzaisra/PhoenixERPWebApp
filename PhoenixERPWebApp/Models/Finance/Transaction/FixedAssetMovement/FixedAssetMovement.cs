using PhoenixERPWebApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PhoenixERPWebApp.Models
{
    public class FixedAssetMovement : FinanceEntityBase
    {   
        public string Id { get; set; }
     
        public string FixedAssetId { get; set; }
        [DisplayName("FA Number")]
        public string FaaNumber { get; set; }
        [DisplayName("Request Date")]
        public DateTime? FaaDateTime { get; set; }
     
        public string FromLocationID { get; set; }
     
        public string ToLocationID { get; set; }
     
        public string FromBusinessUnitID { get; set; }
     
        public string ToBusinessUnitID { get; set; }
     
        public DateTime? ExecutionDate { get; set; }
     
        public string BusinessUnitID { get; set; }
     
        public string LegalEntityID { get; set; }
     
        public string AffiliationID { get; set; }
        [DisplayName("Status")]
        public string FixedAssetMovementStatusID { get; set; }
     
        public string Remark { get; set; }
 
        public string ApprovalID { get; set; }

        public string Condition { get; set; }

        public string Employee { get; set; }

        [NotMapped]
        [DisplayName("Location name")]
        public string LocationName { get; set; }
        [NotMapped]
        [DisplayName("Condition Name")]
        public string ConditionName { get; set; }
        [NotMapped]
        [DisplayName("Requester By")]
        public string EmployeeName { get; set; }
        [NotMapped]
        public string Division { get; set; }
        [NotMapped]
        public string LegalEntityName { get; set; }
        [NotMapped]
        public string AffiliationName { get; set; }
        [NotMapped]
        public string StatusName { get; set; }


        [NotMapped]
        public string Type { get; set; }
        [NotMapped]
        public string RemarkLog { get; set; }
        [NotMapped]
        public string InvFaName { get; set; }
       
        public int? Status { get; set; }
        [NotMapped]
        public string inv_status { get; set; }
        [NotMapped]
        public string InvFaId { get; set; }
        [NotMapped]
        public string LogDescription { get; set; }
        [NotMapped]
        public string FromLocationName { get; set; }
        [NotMapped]
        public string ToLocationName { get; set; }
    }
}
