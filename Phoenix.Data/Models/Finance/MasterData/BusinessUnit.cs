using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("business_unit", Schema = "dbo")]
    public class BusinessUnit : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [Required]
        [MaxLength(350)]
        [Column("unit_name"), StringLength(350)]
        public string UnitName { get; set; }

        [MaxLength(150)]
        [Column("unit_address"), StringLength(150)]
        public string UnitAddress { get; set; }

        [MaxLength(50)]
        [Column("unit_phone"), StringLength(50)]
        public string UnitPhone { get; set; }

        [MaxLength(50)]
        [Column("unit_fax"), StringLength(50)]
        public string UnitFax { get; set; }

        [MaxLength(50)]
        [Column("unit_email"), StringLength(50)]
        public string UnitEmail { get; set; }

        [MaxLength(50)]
        [Column("default_team"), StringLength(50)]
        public string DefaultTeam { get; set; }

        [Column(nameof(Latitude))]
        public decimal? Latitude { get; set; }

        [Column(nameof(Longitude))]
        public decimal? Longitude { get; set; }

        [MaxLength(50)]
        [Column("parent_unit"), StringLength(50)]
        public string ParentUnit { get; set; }

        [MaxLength(50)]
        [Column("unit_code"), StringLength(50)]
        public string UnitCode { get; set; }

        [MaxLength(255)]
        [Column("unit_description"), StringLength(255)]
        public string UnitDescription { get; set; }

        [MaxLength(50)]
        [Column("business_unit_type_id"), StringLength(50)]
        public string BusinessUnitTypeId { get; set; }

        [MaxLength(50)]
        [Column("affiliation_id"), StringLength(50)]
        public string AffiliationId { get; set; }

        [MaxLength(50)]
        [Column("organization_id"), StringLength(50)]
        public string OrganizationId { get; set; }

        [Column(nameof(Isfinance))]
        public bool? Isfinance { get; set; }

        [Column(nameof(Ishris))]
        public bool? Ishris { get; set; }

        [NotMapped]
        public string AffiliationName { get; set; }
        [NotMapped]
        public int bussnisUnitTypeLevel { get; set; }
        [NotMapped]
        public string ParentUnitName { get; set; }
        [NotMapped]
        public string ParentUnitNameGroup { get; set; }
        [NotMapped]
        public string ParentUnitNameSubgroup { get; set; }


        ////navigation property
        //public BusinessUnit Group { get; set; }
        //public List<BusinessUnit> Subgroup { get; set; }
        //public BusinessUnit Division { get; set; }
        //public BusinessUnit Departement { get; set; }
    }
}