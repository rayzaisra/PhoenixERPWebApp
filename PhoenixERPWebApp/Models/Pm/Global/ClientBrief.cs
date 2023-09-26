using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class ClientBrief
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Campaign Name")]
        public string CampaignName { get; set; }

        [DisplayName("Campaign Type Id")]
        public string CampaignTypeId { get; set; }

        [DisplayName("Bussiness Type Id")]
        public string BussinessTypeId { get; set; }

        [DisplayName("External Brand Id")]
        public string ExternalBrandId { get; set; }

        [DisplayName("Deadline")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Deadline { get; set; }

        [DisplayName("Final Delivery")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FinalDelivery { get; set; }

        [DisplayName("Nationality Id")]
        public string NationalityId { get; set; }

        [DisplayName("Job Category Id")]
        public string JobCategoryId { get; set; }

        [DisplayName("Client Brief File Id")]
        public string ClientBriefFileId { get; set; }

        [DisplayName("Assigment Id")]
        public string AssigmentId { get; set; }

        [DisplayName("Campaign Progress Id")]
        public long? CampaignProgressId { get; set; }

        [DisplayName("Start Campaign")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? StartCampaign { get; set; }

        [DisplayName("Finish Campaign")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FinishCampaign { get; set; }

        [DisplayName("Responsible Person")]
        public long? ResponsiblePerson { get; set; }

        [DisplayName("Job Pe Id")]
        public long? JobPeId { get; set; }

        [DisplayName("Proposal File Id")]
        public long? ProposalFileId { get; set; }

        [DisplayName("Campaign Status Id")]
        public long? CampaignStatusId { get; set; }

        [DisplayName("Created By")]
        public string CreatedBy { get; set; }

        [DisplayName("Created On")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedOn { get; set; }

        [DisplayName("Modified By")]
        public string ModifiedBy { get; set; }

        [DisplayName("Modified On")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ModifiedOn { get; set; }

        [DisplayName("Approved By")]
        public string ApprovedBy { get; set; }

        [DisplayName("Approved On")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ApprovedOn { get; set; }

        [DisplayName("Is Active")]
        public bool? IsActive { get; set; }

        [DisplayName("Is Locked")]
        public bool? IsLocked { get; set; }

        [DisplayName("Is Default")]
        public bool? IsDefault { get; set; }

        [DisplayName("Is Deleted")]
        public bool? IsDeleted { get; set; }

        [DisplayName("Owner Id")]
        public string OwnerId { get; set; }

        [DisplayName("Business Unit Id")]
        public string BusinessUnitId { get; set; }

        [DisplayName("Deleted By")]
        public string DeletedBy { get; set; }

        [DisplayName("Deleted On")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DeletedOn { get; set; }

        [DisplayName("Business Unit Division Id")]
        public string BusinessUnitDivisionId { get; set; }

        [DisplayName("Business Unit Departement Id")]
        public string BusinessUnitDepartementId { get; set; }
    }
}