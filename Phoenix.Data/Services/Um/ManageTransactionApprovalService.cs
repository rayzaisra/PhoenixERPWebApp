using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Um;
using Phoenix.Data.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Um
{
    public class ManageTransactionApprovalService : IManageTransactionApprovalService
    {
        readonly DataContext context;

        public string UserId => context.GetEmployeeId();

        public ManageTransactionApprovalService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(TrTemplateApproval entity)
        {
            try
            {
                await context.PhoenixAddApprovalAsync(entity);
                return await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

        }

        public Task<int> AddRangeAsync(params TrTemplateApproval[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(TrTemplateApproval entity)
        {
            throw new NotImplementedException();
        }
        public async Task<int> DeleteRefTable(string tableName, string refId)
        {
            var commandText = "UPDATE " + tableName + " SET IsDeleted = 1 WHERE Id = @refId";
            var name = new SqlParameter("@refId", refId);
            return await context.Database.ExecuteSqlCommandAsync(commandText, name);
        }

        public Task<int> DeleteRageAsync(params TrTemplateApproval[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(TrTemplateApproval entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params TrTemplateApproval[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(TrTemplateApproval entity)
        {
            context.PhoenixEditApproval(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(TrTemplateApproval entity)
        {
            throw new NotImplementedException();
        }


        public async Task<List<TrTemplateApproval>> Get()
        {
            return await context.TrTemplateApprovals.AsNoTracking().ToListAsync();
        }

        public Task<List<TrTemplateApproval>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }
        public async Task<TrTemplateApproval> GetByRefIdForUpdate(string id)
        {
            return await context.TrTemplateApprovals.Where(x => x.RefId.Equals(id)).FirstOrDefaultAsync();
        }
        public async Task<TrTemplateApproval> Get(string id)
        {
            var data = await (from t in context.TrTemplateApprovals.Where(x => x.Id.Equals(id))
                              select new TrTemplateApproval
                              {
                                  TemplateName = t.TemplateName,
                                  Id = t.Id,
                                  IsNotifyByEmail = t.IsNotifyByEmail,
                                  IsNotifyByWeb = t.IsNotifyByWeb,
                                  DueDate = t.DueDate,
                                  Reminder = t.Reminder,
                                  DetailLink = t.DetailLink,
                                  DivisionId = t.DivisionId,
                                  FormReqName = t.FormReqName,
                                  RefId = t.RefId,
                                  RemarkRejected = t.RemarkRejected,
                                  StatusApproved = t.StatusApproved,
                                  SubGroupId = t.SubGroupId,
                                  ApprovalCategory = t.ApprovalCategory,
                                  ApprovalType = t.ApprovalType,
                                  CreatedBy = t.CreatedBy,
                                  CreatedOn = t.CreatedOn,
                                  ListUserApprovals = null
                              }).OrderByDescending(x => x.CreatedOn).FirstOrDefaultAsync();

            if (data == null) return data;
            data.ListUserApprovals = (from lu in context.TrUserApprovals.Where(x => x.TrTempApprovalId.Equals(data.Id))
                                      join emp in context.EmployeeBasicInfos on lu.EmployeeBasicInfoId equals emp.Id
                                      select new TrUserApproval
                                      {
                                          Id = lu.Id,
                                          EmployeeBasicInfoId = lu.EmployeeBasicInfoId,
                                          EmployeeName = emp.NameEmployee,
                                          IsCondition = lu.IsCondition,
                                          IndexUser = lu.IndexUser,
                                          StatusApproved = lu.StatusApproved,
                                          ListConditionalApprovals = null,
                                          IsSpecialCase = lu.IsSpecialCase
                                      }).OrderBy(c => c.IndexUser).OrderByDescending(x => x.IsSpecialCase).ToList();

            foreach (var x in data.ListUserApprovals)
            {
                x.ListConditionalApprovals = (from ca in context.TrConditionUserApprovals.Where(c => c.TrUserApprovalId.Equals(x.Id))
                                              join e in context.EmployeeBasicInfos on ca.EmployeeBasicInfoId equals e.Id
                                              select new TrConditionUserApproval
                                              {
                                                  Id = ca.Id,
                                                  EmployeeBasicInfoId = ca.EmployeeBasicInfoId,
                                                  EmployeeName = e.NameEmployee,
                                                  GroupConditionIndex = ca.GroupConditionIndex,
                                                  GroupConditionApproval = ca.GroupConditionApproval,
                                                  ConditionalApproval = ca.ConditionalApproval,
                                                  TrUserApprovalId = ca.TrUserApprovalId
                                              }).OrderBy(c => c.GroupConditionIndex).ToList();
            }
            return data;
        }

        ///====Additional Function Service in Bottom line here ===///
        public async Task<List<TrTemplateApproval>> GetBySubGroup(string groupId, string divisionId)
        {
            var data = await (from t in context.TrTemplateApprovals.Where(x => x.SubGroupId.Equals(groupId) && x.DivisionId.Equals(divisionId))
                              select new TrTemplateApproval
                              {
                                  TemplateName = t.TemplateName,
                                  Id = t.Id,
                                  IsNotifyByEmail = t.IsNotifyByEmail,
                                  IsNotifyByWeb = t.IsNotifyByWeb,
                                  DueDate = t.DueDate,
                                  Reminder = t.Reminder,
                                  ApprovalCategory = t.ApprovalCategory,
                                  ApprovalType = t.ApprovalType,
                                  CreatedBy = t.CreatedBy,
                                  CreatedOn = t.CreatedOn,
                                  ListUserApprovals = null
                              }).OrderByDescending(x => x.CreatedOn).ToListAsync();

            if (data.Count == 0) return data;
            foreach (var item in data)
            {
                if (!string.IsNullOrEmpty(item.Id))
                    item.ListUserApprovals = (from lu in context.TrUserApprovals.Where(x => x.TrTempApprovalId.Equals(item.Id))
                                              join emp in context.EmployeeBasicInfos on lu.EmployeeBasicInfoId equals emp.Id
                                              select new TrUserApproval
                                              {
                                                  Id = lu.Id,
                                                  EmployeeBasicInfoId = lu.EmployeeBasicInfoId,
                                                  EmployeeName = emp.NameEmployee,
                                                  IsCondition = lu.IsCondition,
                                                  ListConditionalApprovals = null,
                                                  IsSpecialCase = lu.IsSpecialCase
                                              }).OrderBy(c => c.IndexUser).OrderByDescending(x => x.IsSpecialCase).ToList();
            }

            foreach (var item in data)
            {
                if (item.ListUserApprovals.Count > 0)
                {
                    foreach (var x in item.ListUserApprovals)
                    {
                        x.ListConditionalApprovals = (from ca in context.TrConditionUserApprovals.Where(c => c.TrUserApprovalId.Equals(x.Id))
                                                      join e in context.EmployeeBasicInfos on ca.EmployeeBasicInfoId equals e.Id
                                                      select new TrConditionUserApproval
                                                      {
                                                          Id = ca.Id,
                                                          EmployeeBasicInfoId = ca.EmployeeBasicInfoId,
                                                          EmployeeName = e.NameEmployee,
                                                          GroupConditionIndex = ca.GroupConditionIndex,
                                                          GroupConditionApproval = ca.GroupConditionApproval,
                                                          ConditionalApproval = ca.ConditionalApproval,
                                                          TrUserApprovalId = ca.TrUserApprovalId
                                                      }).OrderBy(c => c.GroupConditionIndex).ToList();
                    }
                }
            }
            return data;
        }

        public async Task<List<TemplateApprovalVm>> GetByCategory(int categoryId)
        {
            var data = await (from t in context.TmTemplateApprovals.Where(x => x.ApprovalCategory == categoryId)
                              select new TemplateApprovalVm
                              {
                                  TemplateName = t.TemplateName,
                                  Id = t.Id,
                                  IsNotifyByEmail = t.IsNotifyByEmail,
                                  IsNotifyByWeb = t.IsNotifyByWeb,
                                  DueDate = Convert.ToInt32(t.DueDate),
                                  Reminder = Convert.ToInt32(t.Reminder),
                                  ApprovalCategory = Convert.ToInt32(t.ApprovalCategory),
                                  ApprovalType = Convert.ToInt32(t.ApprovalType),
                                  ListUserApprovals = null
                              }).OrderBy(x => x.TemplateName).ToListAsync();
            if (data.Count == 0) return data;
            foreach (var item in data)
            {
                if (!string.IsNullOrEmpty(item.Id))
                    item.ListUserApprovals = (from lu in context.TmTemplateUserApprovals.Where(x => x.TmTempApprovalId.Equals(item.Id))
                                              join emp in context.EmployeeBasicInfos on lu.EmployeeBasicInfoId equals emp.Id
                                              select new TemplateUserApprovalVm
                                              {
                                                  Id = lu.Id,
                                                  EmployeeBasicInfoId = lu.EmployeeBasicInfoId,
                                                  EmployeeName = emp.NameEmployee,
                                                  IndexUser = lu.IndexUser,
                                                  IsCondition = lu.IsCondition,
                                                  ListConditionalApprovals = null
                                              }).OrderBy(c => c.IndexUser).ToList();
            }

            foreach (var item in data)
            {
                if (item.ListUserApprovals.Count > 0)
                {
                    foreach (var x in item.ListUserApprovals)
                    {
                        x.ListConditionalApprovals = (from ca in context.TmConditionUserApprovals.Where(c => c.TemplateUserApprovalId.Equals(x.Id))
                                                      join e in context.EmployeeBasicInfos on ca.EmployeeBasicInfoId equals e.Id
                                                      select new TempConditionalApprovalVm
                                                      {
                                                          Id = ca.Id,
                                                          EmployeeBasicInfoId = ca.EmployeeBasicInfoId,
                                                          EmployeeName = e.NameEmployee,
                                                          GroupConditionIndex = ca.GroupConditionIndex,
                                                          GroupConditionApproval = ca.GroupConditionApproval,
                                                          ConditionalApproval = ca.ConditionalApproval,
                                                          TemplateUserApprovalId = ca.TemplateUserApprovalId
                                                      }).OrderBy(c => c.GroupConditionIndex).ToList();
                    }
                }
            }
            return data;
        }

        public async Task<TrTemplateApproval> GetByRefId(string id)
        {
            var data = await (from t in context.TrTemplateApprovals.Where(x => x.RefId.Equals(id))
                              select new TrTemplateApproval
                              {
                                  TemplateName = t.TemplateName,
                                  Id = t.Id,
                                  RefId = t.RefId,
                                  IsNotifyByEmail = t.IsNotifyByEmail,
                                  IsNotifyByWeb = t.IsNotifyByWeb,
                                  DueDate = t.DueDate,
                                  Reminder = t.Reminder,
                                  ApprovalCategory = t.ApprovalCategory,
                                  ApprovalType = t.ApprovalType,
                                  CreatedBy = t.CreatedBy,
                                  CreatedOn = t.CreatedOn,
                                  StatusApproved = t.StatusApproved,
                                  ListUserApprovals = null,
                                   DetailLink = t.DetailLink
                              }).OrderByDescending(x => x.CreatedOn).FirstOrDefaultAsync();

            if (data == null) return data;
            data.ListUserApprovals = (from lu in context.TrUserApprovals.Where(x => x.TrTempApprovalId.Equals(data.Id))
                                      join emp in context.EmployeeBasicInfos on lu.EmployeeBasicInfoId equals emp.Id
                                      select new TrUserApproval
                                      {
                                          Id = lu.Id,
                                          EmployeeBasicInfoId = lu.EmployeeBasicInfoId,
                                          EmployeeName = emp.NameEmployee,
                                          TrTempApprovalId = lu.TrTempApprovalId,
                                          IsCondition = lu.IsCondition,
                                          IndexUser = lu.IndexUser,
                                          StatusApproved = lu.StatusApproved,
                                          ListConditionalApprovals = null,
                                          IsSpecialCase = lu.IsSpecialCase
                                      }).OrderBy(c => c.IndexUser).OrderByDescending(x => x.IsSpecialCase).ToList();

            foreach (var x in data.ListUserApprovals)
            {
                x.ListConditionalApprovals = (from ca in context.TrConditionUserApprovals.Where(c => c.TrUserApprovalId.Equals(x.Id))
                                              join e in context.EmployeeBasicInfos on ca.EmployeeBasicInfoId equals e.Id
                                              select new TrConditionUserApproval
                                              {
                                                  Id = ca.Id,
                                                  EmployeeBasicInfoId = ca.EmployeeBasicInfoId,
                                                  EmployeeName = e.NameEmployee,
                                                  GroupConditionIndex = ca.GroupConditionIndex,
                                                  GroupConditionApproval = ca.GroupConditionApproval,
                                                  ConditionalApproval = ca.ConditionalApproval,
                                                  TrUserApprovalId = ca.TrUserApprovalId
                                              }).OrderBy(c => c.GroupConditionIndex).ToList();
            }
            return data;
        }

        public async Task<List<TrUserApproval>> GetListApprovalByRefId(string id)
        {
            //join tmp in context.TrTemplateApprovals on lu.TrTempApprovalId equals tmp.Id
            return await (from ap in context.TrTemplateApprovals
                          join lu in context.TrUserApprovals on ap.Id equals lu.TrTempApprovalId
                          join emp in context.EmployeeBasicInfos on lu.EmployeeBasicInfoId equals emp.Id
                          join a in context.BusinessUnitJobLevels on emp.BusinessUnitJobLevelId equals a.Id into aleft
                          from al in aleft.DefaultIfEmpty()
                          join b in context.JobTitles on al.JobTitleId equals b.Id into bleft
                          from bl in bleft.DefaultIfEmpty()
                          where ap.RefId.Equals(id) && !(ap.IsDeleted ?? false)
                          select new TrUserApproval
                          {
                              Id = lu.Id,
                              EmployeeBasicInfoId = lu.EmployeeBasicInfoId,
                              EmployeeName = emp.NameEmployee,
                              TrTempApprovalId = lu.TrTempApprovalId,
                              IsCondition = lu.IsCondition,
                              IndexUser = lu.IndexUser,
                              StatusApproved = (ap.StatusApproved == StatusApproved.Rejected) ? (lu.StatusApproved == StatusApproved.Approved) ? lu.StatusApproved : StatusApproved.Rejected : lu.StatusApproved,
                              RemarkRejected = lu.StatusApproved == StatusApproved.Rejected ? ap.RemarkRejected : "",
                              ListConditionalApprovals = null,
                              IsSpecialCase = lu.IsSpecialCase,
                              Jobtitle = bl.Id,
                              JobTitleName = bl.TitleName

                          }).OrderBy(c => c.IndexUser).OrderByDescending(x => x.IsSpecialCase).ToListAsync();
        }



        public async Task<List<TrTemplateApproval>> GetListApprovalByEmpId(string id)
        {
            //gak bisa di join, harus select masing2, karena ada yang IsCondition nya false...

            var listUserConditional = await (from x in context.TrConditionUserApprovals.Where(c => c.EmployeeBasicInfoId.Equals(id))
                                             join u in context.TrUserApprovals on x.TrUserApprovalId equals u.Id
                                             join t in context.TrTemplateApprovals.Where(x => x.StatusApproved.Equals(StatusApproved.WaitingApproval) || x.StatusApproved.Equals(StatusApproved.CurrentApproval)) on u.TrTempApprovalId equals t.Id
                                             select new TrTemplateApproval
                                             {
                                                 TemplateName = t.TemplateName,
                                                 Id = t.Id,
                                                 RefId = t.RefId,
                                                 FormReqName = t.FormReqName,
                                                 DetailLink = t.DetailLink,
                                                 StatusApproved = t.StatusApproved,
                                                 CreatedOn = t.CreatedOn
                                             }).OrderByDescending(x => x.CreatedOn).ToListAsync();

            var listUserConCurrent = await (from x in context.TrConditionUserApprovals
                                            join u in context.TrUserApprovals.Where(c => c.EmployeeBasicInfoId.Equals(id)) on x.TrUserApprovalId equals u.Id
                                            join t in context.TrTemplateApprovals.Where(x => x.StatusApproved.Equals(StatusApproved.WaitingApproval) || x.StatusApproved.Equals(StatusApproved.CurrentApproval)) on u.TrTempApprovalId equals t.Id
                                            select new TrTemplateApproval
                                            {
                                                TemplateName = t.TemplateName,
                                                Id = t.Id,
                                                RefId = t.RefId,
                                                FormReqName = t.FormReqName,
                                                DetailLink = t.DetailLink,
                                                StatusApproved = t.StatusApproved,
                                                CreatedOn = t.CreatedOn
                                            }).OrderByDescending(x => x.CreatedOn).ToListAsync();
            var data = listUserConditional.ToList().Union(listUserConCurrent.ToList()).ToList();

            return data;
        }
        public bool IsCurrentApproval(string id)
        {
            var result = (from lu in context.TrUserApprovals
                          join tmp in context.TrTemplateApprovals on lu.TrTempApprovalId equals tmp.Id
                          where tmp.RefId.Equals(id) && lu.StatusApproved == StatusApproved.CurrentApproval && lu.EmployeeBasicInfoId == context.GetEmployeeId() && !(tmp.IsDeleted ?? false)
                          select lu).ToList();

            //jika result nya 0, maka cari di DOA, apakah ada atau tidak...
            if (result.Count == 0)
            {
                var sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@RefId",id??""),
                    new SqlParameter("@UserId",context.GetUserId())
                };
                var resultDoa = context.ExecuteStoredProcedure<VmCountUserApproval>("dbo.uspGet_CurrentApprovalFromDOA", sqlParameters).ConfigureAwait(false).GetAwaiter().GetResult();
                return resultDoa.Count > 0;
            }

            return result.Count > 0;
        }
    }
}
