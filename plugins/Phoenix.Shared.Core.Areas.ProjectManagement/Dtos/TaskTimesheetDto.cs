using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class TaskTimesheetDto
    {
        [Required]
        public string Id { get; set; }
        public DateTime? date_worksheet { get; set; }
        public DateTime? end_time { get; set; }
        [StringLength(2147483647)]
        public string notes { get; set; }
        public DateTime? start_time { get; set; }
        [StringLength(100)]
        public string task { get; set; }
        [StringLength(50)]
        public string sub_task_id { get; set; }
        [StringLength(2147483647)]
        public string worksheet_description { get; set; }
        public int? working_hour { get; set; }
        public int? working_minutes { get; set; }
        public string user_id { get; set; }
    }

    public class TaskTimesheetNew
    {
        public DateTime? date_worksheet { get; set; }
        public DateTime? end_time { get; set; }
        [StringLength(2147483647)]
        public string notes { get; set; }
        public DateTime? start_time { get; set; }
        [StringLength(100)]
        public string task { get; set; }
        [StringLength(50)]
        public string task_id { get; set; }
        [StringLength(2147483647)]
        public string worksheet_description { get; set; }
        public int? working_hour { get; set; }
        public int? working_minutes { get; set; }
        public string user_id { get; set; }
    }          
    
    public class TaskTimesheetSearch : SearchParameter
    {
        public DateTime? Search_date_worksheet { get; set; }
        public DateTime? Search_end_time { get; set; }
        public string Search_notes { get; set; }
        public DateTime? Search_start_time { get; set; }
        public string Search_task { get; set; }
        public string Search_sub_task_id { get; set; }
        public string Search_worksheet_description { get; set; }
        public int? Search_working_hour { get; set; }
        public int? Search_working_minutes { get; set; }
        public string Search_user_id { get; set; }

    }

    public class TimesheetParameterInputDto : SearchParameter
    {
        public string Id { get; set; }
        public string JobNumber { get; set; }
        public string JobName { get; set; }
        public string MonthName { get; set; }
        public string TaskID { get; set; }
        public string SubTaskID { get; set; }
        public DateTime? TimesheetStartDate { get; set; }
        public DateTime? TimesheetEndDate { get; set; }
        public string filterSearch { get; set; }
        public string filterCategory { get; set; }
        public string userID { get; set; }
    }

    public class TimesheetDetailDto
    {
        public string UserID { get; set; }
        public string JobNumber { get; set; }
        public string JobName { get; set; }
        public string ClientName { get; set; }
        public string Brand { get; set; }
        public string CampaignName { get; set; }
        public string TaskId { get; set; }
        public string TaskName { get; set; }
        public string SubTaskId { get; set; }
        public string SubTaskName { get; set; }
        public string DeliverableTask { get; set; }
        public DateTime? dateWorksheet { get; set; }
        public string WorksheetDesc { get; set; }
        public string Notes { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? workingHours { get; set; }
        public int? workingMinutes { get; set; }
    }

    public class TimesheetDurationDto
    {
        public string TimesheetId { get; set; }
        public string SubTaskId { get; set; }
        public string Task { get; set; }
        public int? WorkingHours { get; set; }
        public int? WorkingMinutes { get; set; }
        public DateTime? dateWorksheet { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string UserId { get; set; }
        public string Remarks { get; set; }
    }

    public class TimesheetPlayDto
    {
        public string TimesheetId { get; set; }
        public string SubTaskId { get; set; }
    }

    public class MultipleTimesheetDurationDto
    {
        public List<TimesheetDurationDto> Data { get; set; }
    }

    public class TableTimelineJobDto
    {
        public string JobNumber { get; set; }
        public string JobName { get; set; }
        public string TaskId { get; set; }
        public string TaskName { get; set; }
        public string TaskStatus { get; set; }
        public string SubTaskId { get; set; }
        public string SubTaskName { get; set; }
        public string SubTaskStatus { get; set; }
        public string Central_resource_id { get; set; }
        public string Central_resource_name { get; set; }
        public string Month { get; set; }
        public DateTime? TaskStartDate { get; set; }
        public DateTime? TaskEndDate { get; set; }
        public DateTime? SubTaskStartDate { get; set; }
        public DateTime? SubTaskEndDate { get; set; }
        public List<TaskUserTimelineDto> ListUserTask { get; set; }
    }

    public class TaskUserTimelineDto
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string CentralResourcesID { get; set; }
        public string CentralResourcesName { get; set; }
    }

    public class TimelineTaskNameByUserDto
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }

    public class TimelineParameterInputDto : SearchParameter
    {
        public string Id { get; set; }
        public string JobNumber { get; set; }
        public string JobName { get; set; }
        public string MonthName { get; set; }
        public string TaskID { get; set; }
        public string SubTaskID { get; set; }
        public List<string> Jobs { get; set; }
        public DateTime? TimelineStartDate { get; set; }
        public DateTime? TimelineEndDate { get; set; }
        public string filterSearch { get; set; }
        public string filterCategory { get; set; }
        public string userID { get; set; }
    }



}