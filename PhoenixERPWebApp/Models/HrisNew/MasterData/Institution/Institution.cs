using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class Institution : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Institution Name")]
        public string InstitutionName { get; set; }

        [DisplayName("Institution Address")]
        public string InstitutionAddress { get; set; }

        [DisplayName("Institution Type")]
        public string InstitutionType { get; set; }

        [DisplayName("City")]
        public string City { get; set; }

        [DisplayName("Institution Type")]
        public string InstitutionTypeName { get { return InstitutionTypes.GetName(InstitutionType); } }
    }
}