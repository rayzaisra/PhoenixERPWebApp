using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class EmployeeHealthInsuranceFamily : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Employee Basic Info Id")]
        public string EmployeeBasicInfoId { get; set; }

        [Required]
        [DisplayName("Family")]
        public string FamilyId { get; set; }

        //Not Mapped
        [DisplayName("Status")]
        public string FamilyStatus { get; set; }

        [DisplayName("Sex")]
        public string Gender { get; set; }

        [DisplayName("Date Birth")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateBirth { get; set; }

        [DisplayName("Family")]
        public string FamilyName { get; set; }
    }
}