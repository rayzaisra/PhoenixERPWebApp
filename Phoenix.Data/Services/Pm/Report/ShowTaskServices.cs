using Phoenix.Data.Models.Pm.ViewModel;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Pm.Report
{
    public interface IShowTaskServices
    {
        Task<List<ShowTaskViewModel>> GetShowTask(ParamSearchTaskListViewModel model);
    }
   public class ShowTaskServices : IShowTaskServices
    {
        readonly DataContext context;
        public ShowTaskServices(DataContext context)
        {
            this.context = context;
        }

        public async Task<List<ShowTaskViewModel>> GetShowTask(ParamSearchTaskListViewModel model)
        {
            List<ShowTaskViewModel> result = new List<ShowTaskViewModel>();
            var paramType = new SqlParameter[] {
                                new SqlParameter("@jobId", model.jobId),
                                new SqlParameter("@startDate", model.startDate),
                                new SqlParameter("@endDate", model.endDate),
                                new SqlParameter("@taskName", model.taskName),
                                new SqlParameter("@userTask", model.userTask),
                                new SqlParameter("@divisionId", model.divisionId),
                                new SqlParameter("@legalEntityId", model.legalEntityId)
                            };

            var ds = await context.ExecuteStoredProcedureDataSet("uspGetListTask", paramType);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ShowTaskViewModel newObj = new ShowTaskViewModel();
                newObj.JobNumber = dr["JobNumber"].ToString();
                newObj.JobName = dr["JobName"].ToString();
                newObj.Startjob = dr["Startjob"].ToString();
                newObj.DeadLineJob = dr["DeadLineJob"].ToString();
                newObj.FinishJob = dr["FinishJob"].ToString();
                newObj.ClientId = dr["ClientId"].ToString();
                newObj.ClientName = dr["ClientName"].ToString();
                newObj.BusinessUnitClientBrief = dr["BusinessUnitClientBrief"].ToString();
                newObj.BusinessUnitNameClientBrief = dr["BusinessUnitNameClientBrief"].ToString();
                newObj.JobStatusId = dr["JobStatusId"].ToString();
                newObj.JobStatusName = dr["JobStatusName"].ToString();
                newObj.JobType = dr["JobType"].ToString();
                newObj.JobTypeName = dr["JobTypeName"].ToString();
                newObj.Revenue = dr["Revenue"].ToString();
                newObj.TaskName = dr["TaskName"].ToString();
                newObj.TaskId = dr["TaskId"].ToString();
                newObj.UserTask = dr["UserTask"].ToString();
                result.Add(newObj);
            }

            return result;
        }
    }
}
