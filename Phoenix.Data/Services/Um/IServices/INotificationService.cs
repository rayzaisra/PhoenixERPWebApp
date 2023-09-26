using Phoenix.Data.Models;
using Phoenix.Data.Models.ViewModel;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Um
{
    public interface INotificationService : IDataService<Notification>
    {
        Task<List<NotificationVm>> GetListNotificationByUserCode(string userCode);
        Task<VmDsGeneral> GetDashboardGeneral(string userId, string empId);
        Task<List<VmDsKanban>> GetDashboardKanban(string userId);
        Task<VmDsSummary> GetDashboardSummary();
        Task<VmDsNewPm> GetDsNewPm(string userId);
        Task<List<TrainingSchedule>> GetDsHris(string userId);
        Task<LinkToTimesheetGrid> Getlinktotimesheetgrid(string type, string userId);
       Task<List<VwDsPmApproval>> GetDashboardPmApproval();
        Task<List<VwDsPmApproval>> GetDashboardPmMyTask(string userid);
        Task<List<VmTableOfApp>> GetListOfTable();
        Task<VmDataOfTable> GetRowsTable(string schemaName, string tableName);
        Task<DataTable> GetTemplateDataForImportData(string schemaName, string tableName);
        Task<int> InsertDataFromImportExcel(DataTable data, string schemaName, string tableName);
    }
    public interface INotificationTypeService : IDataService<NotificationType>
    { }
}
