using PhoenixERPWebApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace PhoenixERPWebApp.Models.Finance.Transaction.FixedAssetOutMovement
{
    public class FixedAssetOutMovement : FinanceEntityBase
    {
        public string Id { get; set; }
        [DisplayName("FA Out Movement Number")]
        public string FaNumber { get; set; }
        [DisplayName("Date")]
        public DateTime? FaDatetime { get; set; }
        [DisplayName("FA Out Movement Category Id")]
        public string FixedAssetOutCategoryId { get; set; }
        [DisplayName("Location Id")]
        public string LocationId { get; set; }
        [DisplayName("Execution Date")]
        public DateTime? ExecutionDate { get; set; }
        [DisplayName("Business Unit Id")]
        public string BusinessUnitId { get; set; }
        [DisplayName("Legal Entity Id")]
        public string LegalEntityId { get; set; }
        [DisplayName("Affiliation Id")]
        public string AffiliationId { get; set; }
        [DisplayName("Remark")]
        public string Remark { get; set; }
        [DisplayName("Approval Id")]
        public string ApprovalId { get; set; }
        [DisplayName("Approval Name")]
        public string ApprovalName { get; set; }
        [DisplayName("Affiliation Name")]
        public string AffiliationName { get; set; }
        [DisplayName("Legal Entity Name")]
        public string LegalEntityName { get; set; }
        [DisplayName("Business Unit Name")]
        public string BusinessUnitName { get; set; }
        [DisplayName("Location Name")]
        public string LocationName { get; set; }
        [DisplayName("FA Out Movement Category Name")]
        public string FixedAssetOutCategoryName { get; set; }
        [DisplayName("Status")]
        public int? Status { get; set; }
        [DisplayName("Status")]
        public string StatusName { get; set; }
        [DisplayName("Requestor Id")]
        public string RequestorId { get; set; }
        [DisplayName("Requestor Name")]
        public string RequestorName { get; set; }
        [DisplayName("Division Requestor Name")]
        public string DivisionRequestorName { get; set; }
        [DisplayName("Division Requestor Id")]
        public string DivisionRequestorId { get; set; }
        [DisplayName("On Behalf Id")]
        public string OnBehalfId { get; set; }
        [DisplayName("On Behalf Name")]
        public string OnBehalfName { get; set; }
        [DisplayName("Division On Behalf Name")]
        public string DivisionOnBehalfName { get; set; }
        [DisplayName("Division On Behalf Id")]
        public string DivisionOnBehalfId { get; set; }
        public string LogDescription { get; set; }

    }
}
