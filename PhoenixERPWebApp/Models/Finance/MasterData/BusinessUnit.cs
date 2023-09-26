using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class BusinessUnit : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Business Unit Name")]
        public string UnitName { get; set; }

        [DisplayName("Unit Address")]
        public string UnitAddress { get; set; }

        [DisplayName("Unit Phone")]
        public string UnitPhone { get; set; }

        [DisplayName("Unit Fax")]
        public string UnitFax { get; set; }

        [DisplayName("Unit Email")]
        public string UnitEmail { get; set; }

        [DisplayName("Default Team")]
        public string DefaultTeam { get; set; }

        [DisplayName("Latitude")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? Latitude { get; set; }

        [DisplayName("Longitude")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? Longitude { get; set; }

        [DisplayName("Parent Unit")]
        public string ParentUnit { get; set; }

        [DisplayName("Unit Code")]
        public string UnitCode { get; set; }

        [DisplayName("Unit Description")]
        public string UnitDescription { get; set; }

        [DisplayName("Business Unit Type Id")]
        public string BusinessUnitTypeId { get; set; }

        [DisplayName("Affiliation Name")]
        public string AffiliationId { get; set; }

        [DisplayName("Isfinance")]
        public bool? Isfinance { get; set; }

        [DisplayName("Ishris")]
        public bool? Ishris { get; set; }

        //Not Mapped
        [DisplayName("Affiliation Name")]
        public string AffiliationName { get; set; }
        public int bussnisUnitTypeLevel { get; set; }
        public string ParentUnitName { get; set; }
        public string ParentUnitNameGroup { get; set; }
        public string ParentUnitNameSubgroup { get; set; }
        public string ParentUnitGroup { get; set; }
        public string ParentUnitSubGroup { get; set; }
        public string ParentUnitSubgroup { get; set; }
        //maaping id sp
        public string IdGroup { get; set; }
        public string IdSubGroup { get; set; }
        public string IdDivisi { get; set; }
        public string IdDepartment { get; set; }
        public string OrganizationId { get; set; }
    }
}