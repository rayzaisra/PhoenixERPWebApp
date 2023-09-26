using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Phoenix.Data.Models.ViewModel
{
    public class ChatAndCommentVm
    {
        [Required]
        public string ChatText { get; set; }
        public string FileExt { get; set; }
        [Required]
        public string CollaborateCode { get; set; }
        [Required]
        public string MemberName { get; set; }

        [Required]
        public string Type { get; set; }
    }

    public class ChatAndCommentFileVm
    {
        [Required]
        public IFormFile File { get; set; }
        public string FileExt { get; set; }
        [Required]
        public string CollaborateCode { get; set; }
        [Required]
        public string MemberName { get; set; }
    }

    public class ContactMemberChatVm
    {
        [Required]
        public string MemberCode { get; set; }
        [Required]
        public string MemberName { get; set; }
        [Required]
        public string MemberToCode { get; set; }
        [Required]
        public string MemberToName { get; set; }
    }


    public class ChatGroupInfoVm
    {
        [Required]
        public string ChatText { get; set; }
        public string FileExt { get; set; }
        [Required]
        public string CollaborateCode { get; set; }
        [Required]
        public string MemberCode { get; set; }
        [Required]
        public string MemberName { get; set; }
    }

    public class TeamInfoVm
    {
        public string Id { get; set; }
        public string JobName { get; set; }
        public string TeamName { get; set; }
    }
    public class ListTeamMember
    {
        public string MemberCode { get; set; }
    }

    public class NotificationVm
    {
        public string TemplateNotify { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedOnString { get; set; }
        public int Tipe { get; set; }
        public string CreatedBy { get; set; }
        public string Module { get; set; }
        public bool? IsSeen { get; set; }
        public string ItemId { get; set; }
    }
    public class CodeNotifyVm
    {
        public string Codex { get; set; }
    }

    public class VmDsGeneral
    {
        public int CurrentTotal { get; set; }
        public int PercentageTime { get; set; }
        public int TotalLeave { get; set; }
        public int UsedLeave { get; set; }
    }

    public class VmJobStatus
    {
        public string id { get; set; }
        public string name { get; set; }
    }
    public class VmDsKanban
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<VmJobDetail> ListJobDetails { get; set; }
    }

    public class VmJobDetail
    {
        public string Id { get; set; }
        public string JobName { get; set; }
    }

    public class VwDsPmMyTask
    {
        public string taskname { get; set; }
        public string detaillink { get; set; }
    }

    public class VwDsPmApproval
    {
        public string refid { get; set; }
        public string text { get; set; }
        public string detaillink { get; set; }
        //public string statusapproved { get; set; }
        //public string statusname { get; set; }
        //public string requestname { get; set; }
        //public string approvalname { get; set; }
        //public string createdon { get; set; }
    }
    public class VmDsSummary
    {
        public int TotalHiring { get; set; }
        public int TotalPromotion { get; set; }
        public int TotalSanction { get; set; }
        public int TotalTraining { get; set; }
        public int TotalEndOfContract { get; set; }
        public int JobOpen { get; set; }
        public int JobInProgress { get; set; }
        public int JobDelivered { get; set; }
        public int JobClosed { get; set; }
        public int TotalRFQ { get; set; }
        public int TotalPO { get; set; }
        public int TotalGR { get; set; }

        public int totalPmApproved { get; set; }
        public int totalPmRejected { get; set; }
        public int totalPmWaiting { get; set; }
        public int totalPmTask { get; set; }
        public int totalPmOverdue { get; set; }
        public int totalPmInprogress { get; set; }


    }

    public class VmTableOfApp
    {
        public string TableSchema { get; set; }
        public string TableName { get; set; }
    }

    public class VmDataOfTable
    {
        public DataTable DataTables { get; set; }
        public string[] ListOfColumns { get; set; }
    }

    public class VmListOfColumn
    {
        public string ColumnName { get; set; }
    }

    public class VmDsNewPm
    {
        public vmHomePM  Data { get; set; }
        public List<VmMostUpdateJob> DataMostUpdateJob { get; set; }
        public List<VmProjectType> DataProjectType { get; set; }
        public List<VmRevenuePce> DataRevenue { get; set; }
        public List<VmJobTaskThisWeek> DataJobThisWeek { get; set; }
        public List<VmJobDivisionThisWeek> DataDivisionThisWeek { get; set; }
        public List<VmJobGrade> DataJobGrade { get; set; }
    }

    public class VmProjectType
    {
        public string job_type_name { get; set; }
        public string job_type_count { get; set; }
    }

    public class VmRevenuePce
    {
        public string division_name { get; set; }
        public string total_division_pce { get; set; }
    }

    public class VmMostUpdateJob
    { 
        public string ProjectName { get; set; }
        public string Deadline { get; set; }
        public string Status { get; set; }
        public string StartJob { get; set; }
        public string job_number { get; set; }
    }

    public class vmHomePM
    {
        public string my_hours_this_week { get; set; }
        public string my_hours_this_week_percent { get; set; }
        public string my_hours_this_month { get; set; }
        public string my_hours_this_month_percent { get; set; }
        public string my_billable_hours { get; set; }
        public string my_billable_hours_percent { get; set; }
        public string my_non_billable_hours { get; set; }
        public string my_non_billable_hours_percent { get; set; }
        public string team_billable_hours { get; set; }
        public string team_billable_hours_percent { get; set; }
        public string team_non_billable_hours { get; set; }
        public string team_non_billable_hours_percent { get; set; }
        public string mine_job { get; set; }
        public string div_job { get; set; }
        public string mine_job_percent { get; set; }
        public string div_job_percent { get; set; }
    }

    public class VmJobTaskThisWeek
    {
        public string my_job_task_this_week { get; set; }
        public string my_job_task_count_this_week { get; set; }
    }

    public class VmJobDivisionThisWeek { 
        public string section_division_job { get; set; }
        public string section_division_job_count_this_week { get; set; }
    }

    public class VmJobGrade
    {
        public string is_job_grade_id_e1 { get; set; }
        public string is_show_revenue { get; set; }
    }

    public class LinkToTimesheetGrid { 
        public string link { get; set; }
    }
}
