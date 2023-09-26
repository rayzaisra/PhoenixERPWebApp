using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Finance.FinanceReport
{
    public class TimeSheetByJob
    {
        
        public string UserId { get; set; }
        public string EmployeeName { get; set; }
        public string DivisionName { get; set; }
        public string BusinessUnitDivisionId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string DepartementId { get; set; }
        public string JobId { get; set; }
        public string JobName { get; set; }
        public DateTime JobDate { get; set; }
        public DateTime JobHours { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string TotalHours { get; set; }
        public int TotalMinutes { get; set; }
        public string BusinessUnit { get; set; }
        public string LegalEntityName { get; set; }
    }

    public class TimeSheetReportByBusinessUnit : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        [DisplayName("User Id")]
        public string UserId { get; set; }

        [DisplayName("Month")]
        public string MonthPeriod { get; set; }

        [DisplayName("Year")]
        public string YearPeriod { get; set; }

        [DisplayName("Start Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        [DisplayName("End Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityId { get; set; }
        [DisplayName("Client Name")]
        public string CompanyId { get; set; }
        [DisplayName("Job")]
        public string JobId { get; set; }

        [DisplayName("Legal Entity")]
        public string LegalEntityName { get; set; }
        [DisplayName("Name Employee")]
        public string RequestName { get; set; }
        [DisplayName("Client Name")]
        public string CompanyName { get; set; }
        [DisplayName("Job")]
        public string JobName { get; set; }
        [DisplayName("Name Employee")]
        public string EmployeeBasicInfoId { get; set; }
        [DisplayName("Division")]
        public string DivisionId { get; set; }
        [DisplayName("Division")]
        public string DivisionName { get; set; }

        public List<TimeSheetReportByBusinessUnitDetail> Detail { get; set; }

    }


}

