using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Phoenix.Data.Models.Pm.ViewModel
{
    public class ShowTaskViewModel 
    {
        public string JobNumber { get; set; }
		public string JobName { get; set; }
		public string Startjob { get; set; }
		public string DeadLineJob { get; set; }
		public string FinishJob { get; set; }
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
		public string jobId { get; set; }
		public DateTime? startDate { get; set; }
		public DateTime? endDate { get; set; }
		public string taskName { get; set; }
		public string userTask { get; set; }
		public string divisionId { get; set; }
		public string legalEntityId { get; set; }
	}
}
