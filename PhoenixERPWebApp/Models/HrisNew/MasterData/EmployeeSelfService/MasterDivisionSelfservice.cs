using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class MasterDivisionSelfservice : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Division")]
        public string BusinessUnitId { get; set; }

        [DisplayName("Division Name")]
        public string BusinessUnitName { get; set; }
    }
}