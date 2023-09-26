using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Pm.Report
{
	public class ShowTaskViewModel
	{
		public string JobNumber { get; set; }
		public string JobName { get; set; }
		[DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
		public DateTime? Startjob { get; set; }
		[DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
		public DateTime? DeadLineJob { get; set; }
		[DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
		public DateTime? FinishJob { get; set; }
		public string ClientId { get; set; }
		public string ClientName { get; set; }
		public string BusinessUnitClientBrief { get; set; }
		public string BusinessUnitNameClientBrief { get; set; }
		public string JobStatusId { get; set; }
		public string JobStatusName { get; set; }
		public string JobType { get; set; }
		public string JobTypeName { get; set; }
		public string Revenue { get; set; }
		public string TaskName { get; set; }
		public string TaskId { get; set; }
		public string UserTask { get; set; }

	}

	public class ParamSearchTaskListViewModel
	{
		[DisplayName("Job Number")]
		public string jobId { get; set; }
		[DisplayName("Start Date Job")]
		public DateTime? startDate { get; set; }
		[DisplayName("Finish Date Job")]
		public DateTime? endDate { get; set; }
		[DisplayName("Task Name")]
		public string taskName { get; set; }
		[DisplayName("Name Employee")]
		public string userTask { get; set; }
		[DisplayName("Division")]
		public string divisionId { get; set; }
		[DisplayName("Legal Entity")]
		public string legalEntityId { get; set; }
	}
}
