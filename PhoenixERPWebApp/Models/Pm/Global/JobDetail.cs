using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class JobDetail : PmEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("ClientBriefId")]
        public string ClientBriefId { get; set; }

        [DisplayName("JobNumber")]
        public string JobNumber { get; set; }

        [DisplayName("JobName")]
        public string JobName { get; set; }

        [DisplayName(nameof(Deadline))]
        public DateTime? Deadline { get; set; }

        [DisplayName("FinalDelivery")]
        public DateTime? FinalDelivery { get; set; }

        [DisplayName("PmId")]
        [Column("pm_id")]
        public string PmId { get; set; }

        [DisplayName(nameof(Pa))]
        public double? Pa { get; set; }

        [DisplayName(nameof(Pe))]
        public double? Pe { get; set; }

        [DisplayName("JobStatusDetail")]
        [Column("job_status_detail")]
        public string JobStatusDetail { get; set; }

        [DisplayName("JobDescription")]
        public string JobDescription { get; set; }

        [DisplayName("CashAdvance")]
        public double? CashAdvance { get; set; }

        [DisplayName("JobStatusId")]
        public string JobStatusId { get; set; }

        [DisplayName("Company Id")]
        public string CompanyId { get; set; }

        [DisplayName("StartJob")]
        public DateTime? StartJob { get; set; }

        [DisplayName("FinishJob")]
        public DateTime? FinishJob { get; set; }

        [DisplayName("TeamId")]
        public string TeamId { get; set; }

        [DisplayName("TypeId")]
        public string TypeId { get; set; }

        [DisplayName("JobType ")]
        public string JobType { get; set; }

        [DisplayName("Extended Detail")]
        public string ExtendedDetail { get; set; }

        [DisplayName("Legal Entity Id")]
        public string LegalEntityId { get; set; }

        [DisplayName("Main Service Category")]
        public string MainServiceCategory { get; set; }

        [DisplayName("Affiliation Id")]
        public string AffiliationId { get; set; }

        [DisplayName("Brand Id")]
        public string BrandId { get; set; }

        [DisplayName("Client Name")]
        public string ClientName { get; set; }

        [DisplayName("Nature Of Expense Id")]
        public string NatureOfExpenseId { get; set; }

        [DisplayName("Cost Allocation")]
        public string CostAllocation { get; set; }

        [DisplayName("Business Unit Id")]
        public string BusinessUnitId { get; set; }

        [DisplayName("Master Outlet Id")]
        public string MasterOutletId { get; set; }

        //Not Mapped
        public string JobPaId { get; set; }
        public string JobPaIdName { get; set; }

        public string JobPeId { get; set; }
        public string JobPeName { get; set; }
        public string CompanyName { get; set; }
        public string BrandName { get; set; }
    }
}