using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class PdrSalaryGrade : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Job Grade Id")]
        public string JobGradeId { get; set; }

        [DisplayName("Pdr Salary Increase Id")]
        public string PdrSalaryIncreaseId { get; set; }

        public string GradeName { get; set; }
    }
}