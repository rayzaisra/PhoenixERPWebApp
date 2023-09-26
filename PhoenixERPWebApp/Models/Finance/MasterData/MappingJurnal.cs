using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class MappingJurnal : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Division Name")]
        public string EmployeeDivisionId { get; set; }

        [DisplayName("Depertment Name")]
        public string EmployeeDepertmentId { get; set; }

        [DisplayName("Debit")]
        public string Debit { get; set; }

        [DisplayName("Credit")]
        public string Credit { get; set; }

        [DisplayName("Prosess Name")]
        public string ProsessName { get; set; }

        [DisplayName("Division Name")]
        public string DivisionName { get; set; }

        [DisplayName("Department Name")]
        public string DepartmentName { get; set; }

    }
}