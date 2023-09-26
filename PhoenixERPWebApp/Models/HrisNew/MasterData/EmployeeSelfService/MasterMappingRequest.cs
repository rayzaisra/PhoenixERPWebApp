using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class MasterMappingRequest : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Service Name")]
        public string ServiceName { get; set; }

        [DisplayName("Division")]
        public string MasterDivisionSelfserviceId { get; set; }


        [DisplayName("Employee Name")]
        public string EmployeeBasicInfoId { get; set; }


        [DisplayName("Remarks")]
        public string Remarks { get; set; }

        //Not Mapped
        [DisplayName("Employee Name")]
        public string NameEmployee { get; set; }

        [DisplayName("Division")]
        public string BusinessUnitName { get; set; }
    }
}