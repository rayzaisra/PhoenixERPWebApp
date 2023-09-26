using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
     public class TimesheetNonJobDto :TimesheetNonJobNew
    {
        public string Id { get; set; }
        public string code { get; set; }
        public string business_unit_name { get; set; }
        public int revision { get; set; }
        

    }
    public class TimesheetNonJobNew
    {
        public string description { get; set; }
        public string business_unit_id { get; set; }
        public List<TimesheetNonJobTask> detail { get; set; }
        public string name { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }

    }
    public class TimesheetNonJobTask
    {
        public string id { get; set; }
        public string task_name { get; set; }
        public string category_id { get; set; }
        public string category_name { get; set; }
    }
 
    public class TimesheetNonJobSearch : SearchParameter
    {
        public bool active { get; set; } = false;
        public bool allversion { get; set; } = false;

    }
}