using Phoenix.Data.Models.Pm.Report;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Pm.Report
{
    public interface IPceReportServices
    {
        Task<List<Models.Pm.Report.ReportPCEDiego>> GetReportPceDiego();
    }
    public class PceReportServices : IPceReportServices
    {
        readonly DataContext context;
        public PceReportServices(DataContext context)
        {
            this.context = context;
        }
        public async Task<List<ReportPCEDiego>> GetReportPceDiego()
        {
            try
            {
                List<ReportPCEDiego> list = new List<ReportPCEDiego>();
                //var param = new SqlParameter[]
                //        {
                //        new SqlParameter("@p_client_id",client_id),

                //        };
                list = await context.ExecuteStoredProcedure<ReportPCEDiego>("pm.usp_report_pce_diego");
                return list;
            }
            catch (Exception e)
            {
                throw new Exception("Error !, " + e.Message);
            }
        }
    }
}
