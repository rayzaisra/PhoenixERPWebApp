using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models
{
    public class EntityBase
    {
        public bool? IsActive { get; set; }
        [MaxLength(50)]
        public string CreatedBy { get; set; }

        [DisplayName("Created Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedOn { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Modified Date")]
        public DateTime? ModifiedOn { get; set; }

        [MaxLength(50)]
        public string DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public bool? IsDeleted { get; set; }

        [NotMapped]
        public int? RowNumber { get; set; }
    }

    public class FinanceEntityBase : EntityBase
    {
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string RejectedBy { get; set; }
        public DateTime? RejectedOn { get; set; }
        public bool? IsLocked { get; set; }
        public bool? IsDefault { get; set; }
        public string OwnerId { get; set; }
    }

    public class PmEntityBase : EntityBase
    {
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        [DisplayName("Is Locked")]
        public bool? IsLocked { get; set; }
        [DisplayName("Is Default")]
        public bool? IsDefault { get; set; }
        public string OwnerId { get; set; }
        public string OrganizationId { get; set; }
        public string BusinessUnitDivisionId { get; set; }
        public string BusinessUnitDepartementId { get; set; }
    }

    public class HrisEntityBase : EntityBase
    {
        [MaxLength(50)]
        public string ApprovedBy { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ApprovedOn { get; set; }
        [DisplayName("Is Locked")]
        public bool? IsLocked { get; set; }
        [DisplayName("Is Default")]
        public bool? IsDefault { get; set; }
        public string OwnerId { get; set; }
        public string BusinessUnitDivisionId { get; set; }
        public string BusinessUnitDepartementId { get; set; }
    }

    public class MediaEntityBase : EntityBase
    {
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public bool? IsLocked { get; set; }
        public bool? IsDefault { get; set; }
        public string OwnerId { get; set; }
        public string DivisionId { get; set; }
        public string DepartementId { get; set; }
    }
}
