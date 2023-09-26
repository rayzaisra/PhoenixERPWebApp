using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class PdrSalaryIncrease : HrisEntityBase
    {
        public PdrSalaryIncrease()
        {
            PdrSalaryGrade = new List<string>();
        }

        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Result")]
        public string PdrGradeId { get; set; }

        //[DisplayName("Salary Increase (%)")]
        //public decimal? SalaryIncrease { get; set; }

        [DisplayName("Salary Increase (%)")]
        public double? SalaryIncrease { get; set; }
        [DisplayName("Result")]
        public string PdrGrade { get; set; }

        [DisplayName("Grade")]
        public string PdrSalaryGradeName { get; set; }

        [DisplayName("Grade")]
        public List<string> PdrSalaryGrade { get; set; }

        [NotMapped]
        public string SalaryIncreaseString { get; set; }

    }
}