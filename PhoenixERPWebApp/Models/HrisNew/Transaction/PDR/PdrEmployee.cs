using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class PdrEmployee : HrisEntityBase
    {
        public PdrEmployee()
        {
            PdrRecomendation = new PdrRecomendation();
        }

        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Employee Basic Info Id")]
        public string EmployeeBasicInfoId { get; set; }

        [DisplayName("Review Period Start")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ReviewStart { get; set; }

        [DisplayName("To")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ReviewEnd { get; set; }

        [DisplayName("Date Appraisal")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateAppraisal { get; set; }

        [DisplayName("Final Score")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? FinalScore { get; set; }

        [DisplayName("Grade")]
        public string Grade { get; set; }

        [DisplayName("Date Submit")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateSubmit { get; set; }

        [DisplayName("File Result")]
        public string FileResult { get; set; }

        [DisplayName("Year")]
        public int Year { get; set; }

        [DisplayName("Salary Increase")]
        public int? SalaryIncrease { get; set; }

        [DisplayName("Job Grade Id")]
        public string JobGradeId { get; set; }

        [DisplayName("Notes")]
        public string Notes { get; set; }

        public InfoEmployee infoEmployee { get; set; }

        public string Period
        {
            get
            {
                var start = Convert.ToDateTime(ReviewStart);
                var end = Convert.ToDateTime(ReviewEnd);
                if (start.Month == end.Month && start.Year == end.Year)
                {
                    return end.ToString("MMM yyyy");
                }
                else if (start.Year == end.Year)
                {
                    return start.ToString("MMM") + " - " + end.ToString("MMM yyyy");
                }
                else
                {
                    return start.ToString("MMM yyyy") + " - " + end.ToString("MMM yyyy");
                }
            }
        }
        public bool IsApprove { get; set; }

        public List<PdrCategory> PdrCategorys { get; set; }
        public List<PdrGrade> PdrGrades { get; set; }
        public List<PdrCategory> modelPDRCategory { get; set; }

        //Process Save
        public PdrRecomendation PdrRecomendation { get; set; }
        public List<PdrDetailM2below> PdrDetailM2belows { get; set; }
        public List<PdrOverallScore> PdrOverallScores { get; set; }
        public List<PdrDevelopmentPlan> PdrDevelopmentPlans { get; set; }

        [DisplayName("Status")]
        public int? Status { get; set; }

        [DisplayName("Status")]
        public string StatusDesc { get; set; }

        [DisplayName("Remarks")]
        public string RemarkRejected { get; set; }

        public string poor { get; set; }
        public string fair { get; set; }
        public string good { get; set; }
        public string veryGood { get; set; }
        public string excellent { get; set; }

       


    }
   

}