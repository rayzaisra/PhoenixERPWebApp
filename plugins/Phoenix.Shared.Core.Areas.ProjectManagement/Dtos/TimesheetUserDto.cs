using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class TimesheetUserDto
    {

        public string id { get; set; }
        public string task_id { get; set; }
        public string Non_job { get; set; }
        public string task_name { get; set; }
        public string job_number { get; set; }
        public string job_name { get; set; }
        public string name { get; set; }

    }

    public class JobResultDto {
        public string Id { get; set; }
        public string job_name { get; set; }
    }

    public class TimesheetUserDtoNew
    {
       public string id { get; set; }
        public string code { get; set; }
        public string task_id { get; set; }
        public string tasknj_id { get; set; }
        public string tasknj_name { get; set; }
        public string task_name { get; set; }
        public string timesheet_hour { get; set; }

        public DateTime timesheet_date { get; set; }

        public string timesheet_userid { get; set; }
        public string remarks { get; set; }
        public string job_id { get; set; }
        public string Non_job { get; set; }
        public string autocount_time { get; set; }
        public string job_number { get; set; }
        public string user_id { get; set; }
        public string business_unit_division_id { get; set; }
        public string business_unit_departement_id { get; set; }
    }

    public class TimeSheetRow
    {
        public List<TimeSheetCreateDto> data { get; set; }
    }

    public class  TimeSheetCreateDto
    {

        public string id { get; set; }
        public string user_id { get; set; }
        public string job_name { get; set; }
        public string job_id { get; set; }
        public string remark { get; set; }
        public DateTime mon { get; set; }
        public DateTime start_date_weekly { get; set; }
        public DateTime end_date_weekly { get; set; }
        public List<TimeSheetUserDataDto> time {get; set;}
     
       
    }

    public class TimeSheetUserDataDto
    {
        public string id { get; set; }
        public string timesheet_userid { get; set; }
        public string remarks { get; set; }
        public DateTime timesheet_date { get; set; }
        public int? timesheet_hour { get; set; }
        public string user_id { get; set; }
    }

   
}
