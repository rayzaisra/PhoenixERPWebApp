using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using Phoenix.ApiExtension.Helpers;
using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Um;
using Phoenix.Data.MsGraphExtention;
using Phoenix.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Um
{
    public class ManageTemplateApprovalService : IManageTemplateApprovalService
    {
        readonly DataContext context;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IManageTransactionApprovalService _repo;
        public string UserId => context.GetEmployeeId();

        /// <summary>
        /// And endpoint to manage SalaryComponent
        /// </summary>
        /// <param name="context">Database context</param>
        public ManageTemplateApprovalService(DataContext context, IHubContext<NotificationHub> hubContext, IManageTransactionApprovalService repo)
        {
            this.context = context;
            _hubContext = hubContext;
            _repo = repo;
        }

        public async Task<int> AddAsync(TmTemplateApproval entity)
        {
            try
            {
                if (string.IsNullOrEmpty(entity.JobTitleId)) entity.JobTitleId = "0";
                await context.PhoenixAddApprovalAsync(entity);
                return await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException.Message);
                return await Task.Run(() => 0);
            }
        }

        public Task<int> AddRangeAsync(params TmTemplateApproval[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(TmTemplateApproval entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params TmTemplateApproval[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(TmTemplateApproval entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params TmTemplateApproval[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(TmTemplateApproval entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(TmTemplateApproval entity)
        {
            throw new NotImplementedException();
        }


        public async Task<List<TmTemplateApproval>> Get()
        {

            var data = await (from t in context.TmTemplateApprovals.Where(x => x.IsDeleted != true)
                              select new TmTemplateApproval
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
                                  ListUserApprovals = null,
                                  MenuId = string.IsNullOrEmpty(t.MenuId) ? "0" : t.MenuId,
                                  JobTitleId = string.IsNullOrEmpty(t.JobTitleId) ? "0" : t.JobTitleId,
                                  MinBudget = t.MinBudget,
                                  MaxBudget = t.MaxBudget,
                                  ForDivisionId = string.IsNullOrEmpty(t.ForDivisionId) ? "0" : t.ForDivisionId,
                                  IsAllDivision = t.IsAllDivision == null ? false : t.IsAllDivision
                              }).OrderByDescending(x => x.CreatedOn).ToListAsync();

            var listDivision = await GetDivision();
            //enhance, penambahan field di tengah, gak bisa di join, karena data null
            try
            {
                foreach (var item in data.ToList())
                {
                    var c = item.Id;
                    if (item.JobTitleId != "0") item.JobTitleName = context.JobTitles.Where(x => x.Id.Equals(item.JobTitleId)).FirstOrDefault().TitleName;
                    else item.JobTitleName = "There is no Position";
                    if (item.MenuId != "0") item.MenuName = context.TmMenus.Where(x => x.Id.Equals(item.MenuId)).FirstOrDefault().MenuName;
                    else item.MenuId = "There is no menu";

                    if (item.ForDivisionId != "0") { item.ForDivisionName = listDivision.Where(x => x.Id.Equals(item.ForDivisionId)).FirstOrDefault().UnitName; }
                    else if ((item.ForDivisionId == "0") && (item.IsAllDivision == true)) { item.ForDivisionName = "All division"; }
                    else { item.ForDivisionName = "There is no division"; }
                }
            }
            catch (Exception ex)
            {
                var b = ex.Message;
            }

            return data;
        }

        public Task<List<TmTemplateApproval>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<TmTemplateApproval> Get(string id)
        {
            var data = await (from t in context.TmTemplateApprovals.Where(x => x.Id.Equals(id) && x.IsDeleted != true)
                              select new TmTemplateApproval
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
                                  ListUserApprovals = null,
                                  MenuId = string.IsNullOrEmpty(t.MenuId) ? "" : t.MenuId,
                                  JobTitleId = string.IsNullOrEmpty(t.JobTitleId) ? "0" : t.JobTitleId,
                                  MinBudget = t.MinBudget,
                                  MaxBudget = t.MaxBudget,
                                  ForDivisionId = t.ForDivisionId,
                                  IsAllDivision = t.IsAllDivision == null ? false : t.IsAllDivision,
                                  IsSpecialCase = t.IsSpecialCase == null ? false : t.IsSpecialCase,
                              }).OrderByDescending(x => x.CreatedOn).FirstOrDefaultAsync();
            if (data.JobTitleId != "0") data.JobTitleName = context.JobTitles.Where(x => x.Id.Equals(data.JobTitleId)).FirstOrDefault().TitleName;
            else data.JobTitleName = "There is no Position";
            if (data.MenuId != null) data.MenuName = context.TmMenus.Where(x => x.Id.Equals(data.MenuId)).FirstOrDefault().MenuName;

            if (data == null) return data;
            data.ListUserApprovals = (from lu in context.TmTemplateUserApprovals.Where(x => x.TmTempApprovalId.Equals(data.Id) && x.IsDeleted != true)
                                      join emp in context.EmployeeBasicInfos on lu.EmployeeBasicInfoId equals emp.Id
                                      select new TmTemplateUserApproval
                                      {
                                          Id = lu.Id,
                                          EmployeeBasicInfoId = lu.EmployeeBasicInfoId,
                                          EmployeeName = emp.NameEmployee,
                                          IsCondition = lu.IsCondition,
                                          IndexUser = lu.IndexUser,
                                          ListConditionalApprovals = null,
                                          IsSpecialCase = lu.IsSpecialCase,
                                          RemarkSpecialCase = lu.RemarkSpecialCase,
                                          Email = emp.Email
                                      }).OrderBy(c => c.IndexUser).ToList();

            foreach (var x in data.ListUserApprovals)
            {
                x.ListConditionalApprovals = (from ca in context.TmConditionUserApprovals.Where(c => c.TemplateUserApprovalId.Equals(x.Id) && c.IsDeleted != true)
                                              join e in context.EmployeeBasicInfos on ca.EmployeeBasicInfoId equals e.Id
                                              select new TmConditionUserApproval
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
            return data;
        }

        ///====Additional Function Service in Bottom line here ===///
        public async Task<List<TmTemplateApproval>> GetBySubGroup(string groupId, string divisionId)
        {
            //var data = await (from t in context.TmTemplateApprovals.Where(x => x.SubGroupId.Equals(groupId) && x.DivisionId.Equals(divisionId) && x.IsDeleted == false)
            var data = await (from t in context.TmTemplateApprovals.Where(x => x.SubGroupId.Equals(groupId) && x.ForDivisionId.Equals(divisionId) && x.IsDeleted != true)
                              select new TmTemplateApproval
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
                                  ListUserApprovals = null,
                                  MenuId = string.IsNullOrEmpty(t.MenuId) ? "" : t.MenuId,
                                  JobTitleId = string.IsNullOrEmpty(t.JobTitleId) ? "0" : t.JobTitleId,
                                  MinBudget = t.MinBudget,
                                  MaxBudget = t.MaxBudget,
                                  ForDivisionId = t.ForDivisionId,
                                  IsAllDivision = t.IsAllDivision == null ? false : t.IsAllDivision
                              }).OrderByDescending(x => x.CreatedOn).ToListAsync();
            var listDivision = await GetDivision();
            foreach (var item in data)
            {
                if (item.JobTitleId != "0") item.JobTitleName = context.JobTitles.Where(x => x.Id.Equals(item.JobTitleId)).FirstOrDefault()?.TitleName;
                else item.JobTitleName = "There is no Position";
                if (item.MenuId != null) item.MenuName = context.TmMenus.Where(x => x.Id.Equals(item.MenuId)).FirstOrDefault()?.MenuName;
                if (!string.IsNullOrEmpty(item.ForDivisionId)) item.ForDivisionName = listDivision.Where(x => x.Id.Equals(item.ForDivisionId)).FirstOrDefault()?.UnitName;
                else item.ForDivisionName = "There is no division";
            }

            if (data.Count == 0) return data;
            foreach (var item in data)
            {
                if (!string.IsNullOrEmpty(item.Id))
                    item.ListUserApprovals = (from lu in context.TmTemplateUserApprovals.Where(x => x.TmTempApprovalId.Equals(item.Id) && x.IsDeleted != true)
                                              join emp in context.EmployeeBasicInfos on lu.EmployeeBasicInfoId equals emp.Id
                                              select new TmTemplateUserApproval
                                              {
                                                  Id = lu.Id,
                                                  EmployeeBasicInfoId = lu.EmployeeBasicInfoId,
                                                  EmployeeName = emp.NameEmployee,
                                                  IsCondition = lu.IsCondition,
                                                  ListConditionalApprovals = null,
                                                  IsSpecialCase = lu.IsSpecialCase,
                                                  RemarkSpecialCase = lu.RemarkSpecialCase,
                                                  Email = emp.Email
                                              }).OrderBy(c => c.IndexUser).ToList();
            }

            foreach (var item in data)
            {
                if (item.ListUserApprovals.Count > 0)
                {
                    foreach (var x in item.ListUserApprovals)
                    {
                        x.ListConditionalApprovals = (from ca in context.TmConditionUserApprovals.Where(c => c.TemplateUserApprovalId.Equals(x.Id) && c.IsDeleted != true)
                                                      join e in context.EmployeeBasicInfos on ca.EmployeeBasicInfoId equals e.Id
                                                      select new TmConditionUserApproval
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

        public async Task<TmTemplateApproval> GetById(string Id)
        {
            var data = await (from t in context.TmTemplateApprovals.Where(x => x.Id.Equals(Id) && x.IsDeleted != true)
                              select new TmTemplateApproval
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
                                  ListUserApprovals = null,
                                  MenuId = string.IsNullOrEmpty(t.MenuId) ? "" : t.MenuId,
                                  JobTitleId = string.IsNullOrEmpty(t.JobTitleId) ? "0" : t.JobTitleId,
                                  MinBudget = t.MinBudget,
                                  MaxBudget = t.MaxBudget,
                                  ForDivisionId = t.ForDivisionId,
                                  IsAllDivision = t.IsAllDivision == null ? false : t.IsAllDivision,
                                  IsSpecialCase = t.IsSpecialCase
                              }).OrderByDescending(x => x.CreatedOn).FirstOrDefaultAsync();

            if (data.JobTitleId != "0") data.JobTitleName = context.JobTitles.Where(x => x.Id.Equals(data.JobTitleId)).FirstOrDefault().TitleName;
            else data.JobTitleName = "There is no Position";
            if (data.MenuId != null) data.MenuName = context.TmMenus.Where(x => x.Id.Equals(data.MenuId)).FirstOrDefault().MenuName;

            if (data == null) return data;

            if (!string.IsNullOrEmpty(data.Id))
                data.ListUserApprovals = (from lu in context.TmTemplateUserApprovals.Where(x => x.TmTempApprovalId.Equals(data.Id) && x.IsDeleted != true)
                                          join emp in context.EmployeeBasicInfos on lu.EmployeeBasicInfoId equals emp.Id
                                          where lu.IsDeleted != true && emp.IsDeleted != true
                                          select new TmTemplateUserApproval
                                          {
                                              Id = lu.Id,
                                              IndexUser = lu.IndexUser,
                                              EmployeeBasicInfoId = lu.EmployeeBasicInfoId,
                                              EmployeeName = emp.NameEmployee,
                                              IsCondition = lu.IsCondition,
                                              ListConditionalApprovals = null,
                                              IsSpecialCase = lu.IsSpecialCase,
                                              RemarkSpecialCase = lu.RemarkSpecialCase,
                                              Email = emp.Email
                                          }).OrderBy(c => c.IndexUser).OrderByDescending(c => c.IsSpecialCase).ToList();

            if (data.ListUserApprovals.Count > 0)
            {
                foreach (var x in data.ListUserApprovals)
                {
                    x.ListConditionalApprovals = (from ca in context.TmConditionUserApprovals.Where(c => c.TemplateUserApprovalId.Equals(x.Id) && c.IsDeleted != true)
                                                  join e in context.EmployeeBasicInfos on ca.EmployeeBasicInfoId equals e.Id
                                                  where ca.IsDeleted != true && e.IsDeleted != true
                                                  select new TmConditionUserApproval
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

            if (data.IsSpecialCase == true)
            {
                var employeeid = Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);

                var userSpecialCase = await GetSpecialCaseUser(employeeid).ConfigureAwait(false);
                var match = data.ListUserApprovals.FirstOrDefault(x => x.EmployeeBasicInfoId == userSpecialCase.EmployeeId);
                if (match == null)
                {
                    string guid = System.Guid.NewGuid().ToString();
                    guid = guid.ToLower();
                    var dataSpecialCase = new TmTemplateUserApproval();
                    dataSpecialCase.Id = guid;
                    dataSpecialCase.IndexUser = 100;
                    dataSpecialCase.EmployeeBasicInfoId = userSpecialCase.EmployeeId;
                    dataSpecialCase.EmployeeName = userSpecialCase.EmployeeName;
                    dataSpecialCase.IsSpecialCase = true;
                    dataSpecialCase.IsCondition = false;
                    dataSpecialCase.Email = userSpecialCase.Email;

                    data.ListUserApprovals.Add(dataSpecialCase);
                }

            }

           

            return data;
        }

        public async Task<TmTemplateApproval> GetByMenuId(string groupId, string divisionId, string menuId, string jobTitleId)
        {
            var data = await (from t in context.TmTemplateApprovals.Where(x => x.MenuId.Equals(menuId) && x.IsDeleted != true)
                              select new TmTemplateApproval
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
                                  ListUserApprovals = null,
                                  MenuId = string.IsNullOrEmpty(t.MenuId) ? "" : t.MenuId,
                                  JobTitleId = string.IsNullOrEmpty(t.JobTitleId) ? "0" : t.JobTitleId,
                                  MinBudget = t.MinBudget,
                                  MaxBudget = t.MaxBudget,
                                  ForDivisionId = t.ForDivisionId,
                                  IsAllDivision = t.IsAllDivision == null ? false : t.IsAllDivision,
                                  IsSpecialCase = t.IsSpecialCase
                              }).OrderByDescending(x => x.CreatedOn).FirstOrDefaultAsync();

            if (data == null) return null;
            if (data.IsAllDivision == false)
            {
                data = await (from t in context.TmTemplateApprovals.Where(x => x.ForDivisionId.Equals(divisionId) && x.IsDeleted != true
                                  && x.MenuId.Equals(menuId) && x.JobTitleId.Equals(jobTitleId))
                              select new TmTemplateApproval
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
                                  ListUserApprovals = null,
                                  MenuId = string.IsNullOrEmpty(t.MenuId) ? "" : t.MenuId,
                                  JobTitleId = string.IsNullOrEmpty(t.JobTitleId) ? "0" : t.JobTitleId,
                                  MinBudget = t.MinBudget,
                                  MaxBudget = t.MaxBudget,
                                  ForDivisionId = t.ForDivisionId,
                                  IsAllDivision = t.IsAllDivision == null ? false : t.IsAllDivision
                              }).OrderByDescending(x => x.CreatedOn).FirstOrDefaultAsync();
            }

            if (data == null) return null;
            if (data.JobTitleId != "0") data.JobTitleName = context.JobTitles.Where(x => x.Id.Equals(data.JobTitleId)).FirstOrDefault().TitleName;
            else data.JobTitleName = "There is no Position";
            if (data.MenuId != null) data.MenuName = context.TmMenus.Where(x => x.Id.Equals(data.MenuId)).FirstOrDefault().MenuName;

            if (data == null) return data;

            if (!string.IsNullOrEmpty(data.Id))
            {
                if (data.ListUserApprovals == null)
                {
                    data.ListUserApprovals = new List<TmTemplateUserApproval>();
                }
                var queryListUserApproval = (from lu in context.TmTemplateUserApprovals.Where(x => x.TmTempApprovalId.Equals(data.Id) && x.IsDeleted.Equals(false))
                                             join emp in context.EmployeeBasicInfos on lu.EmployeeBasicInfoId equals emp.Id
                                             where lu.IsSpecialCase == false 
                                             select new TmTemplateUserApproval
                                             {
                                                 Id = lu.Id,
                                                 IndexUser = lu.IndexUser,
                                                 EmployeeBasicInfoId = lu.EmployeeBasicInfoId,
                                                 EmployeeName = emp.NameEmployee,
                                                 IsCondition = lu.IsCondition,
                                                 ListConditionalApprovals = null,
                                                 IsSpecialCase = lu.IsSpecialCase,
                                                 RemarkSpecialCase = lu.RemarkSpecialCase,
                                                 Email = emp.Email
                                             }).OrderBy(c => c.IndexUser).OrderByDescending(x => x.IsSpecialCase).ToList();
                data.ListUserApprovals.AddRange(queryListUserApproval);


                if (data.IsSpecialCase == true)
                {
                    var employeeid = Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);

                    var userSpecialCase = await GetSpecialCaseUser(employeeid).ConfigureAwait(false);
                    var match = data.ListUserApprovals.FirstOrDefault(x => x.EmployeeBasicInfoId == userSpecialCase.EmployeeId);
                    if (match == null)
                    {
                        string guid = System.Guid.NewGuid().ToString();
                        guid = guid.ToLower();
                        var dataSpecialCase = new TmTemplateUserApproval();
                        dataSpecialCase.Id = guid;
                        dataSpecialCase.IndexUser = 100;
                        dataSpecialCase.EmployeeBasicInfoId = userSpecialCase.EmployeeId;
                        dataSpecialCase.EmployeeName = userSpecialCase.EmployeeName;
                        dataSpecialCase.IsSpecialCase = true;
                        dataSpecialCase.IsCondition = false;
                        dataSpecialCase.Email = userSpecialCase.Email;

                        data.ListUserApprovals.Add(dataSpecialCase);
                    }
                  
                }
            }

            if (data.ListUserApprovals.Count > 0)
            {
                foreach (var x in data.ListUserApprovals)
                {
                    x.ListConditionalApprovals = (from ca in context.TmConditionUserApprovals.Where(c => c.TemplateUserApprovalId.Equals(x.Id) && c.IsDeleted.Equals(false))
                                                  join e in context.EmployeeBasicInfos on ca.EmployeeBasicInfoId equals e.Id
                                                  select new TmConditionUserApproval
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

            return data;
        }
        public async Task<TmTemplateApproval> GetByBudgeting(string groupId, string divisionId, string menuId, decimal? budget)
        {
            var listTemp = await (from t in
                                    context.TmTemplateApprovals.Where(x => x.SubGroupId.Equals(groupId)
                                    && x.ForDivisionId.Equals(divisionId)
                                    && x.IsDeleted == false
                                    && x.MenuId.Equals(menuId) && x.MaxBudget > budget)
                                  select new TmTemplateApproval
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
                                      ListUserApprovals = null,
                                      MenuId = string.IsNullOrEmpty(t.MenuId) ? "" : t.MenuId,
                                      JobTitleId = string.IsNullOrEmpty(t.JobTitleId) ? "0" : t.JobTitleId,
                                      MinBudget = t.MinBudget,
                                      MaxBudget = t.MaxBudget,
                                      ForDivisionId = t.ForDivisionId,
                                      IsAllDivision = t.IsAllDivision == null ? false : t.IsAllDivision
                                  }).OrderBy(x => x.MinBudget).ToListAsync();

            var tempId = "";
            var data = new TmTemplateApproval();
            if (listTemp.Count > 1)
            {
                foreach (var item in listTemp)
                {
                    if (budget > item.MinBudget && budget < item.MaxBudget)
                    {
                        tempId = item.Id;
                        break;
                    }
                }
                data = listTemp.ToList().Where(x => x.Id.Equals(tempId)).FirstOrDefault();
            }
            else
            {
                data = listTemp.FirstOrDefault();
            }

            if (data.JobTitleId != "0") data.JobTitleName = context.JobTitles.Where(x => x.Id.Equals(data.JobTitleId)).FirstOrDefault().TitleName;
            else data.JobTitleName = "There is no Position";
            if (data.MenuId != null) data.MenuName = context.TmMenus.Where(x => x.Id.Equals(data.MenuId)).FirstOrDefault().MenuName;

            if (data == null) return data;

            if (!string.IsNullOrEmpty(data.Id))
                data.ListUserApprovals = (from lu in context.TmTemplateUserApprovals.Where(x => x.TmTempApprovalId.Equals(data.Id) && x.IsDeleted.Equals(false))
                                          join emp in context.EmployeeBasicInfos on lu.EmployeeBasicInfoId equals emp.Id
                                          select new TmTemplateUserApproval
                                          {
                                              Id = lu.Id,
                                              EmployeeBasicInfoId = lu.EmployeeBasicInfoId,
                                              EmployeeName = emp.NameEmployee,
                                              IsCondition = lu.IsCondition,
                                              ListConditionalApprovals = null,
                                              IsSpecialCase = lu.IsSpecialCase,
                                              RemarkSpecialCase = lu.RemarkSpecialCase,
                                              Email = emp.Email
                                          }).OrderBy(c => c.IndexUser).ToList();

            if (data.ListUserApprovals.Count > 0)
            {
                foreach (var x in data.ListUserApprovals)
                {
                    x.ListConditionalApprovals = (from ca in context.TmConditionUserApprovals.Where(c => c.TemplateUserApprovalId.Equals(x.Id) && x.IsDeleted.Equals(false))
                                                  join e in context.EmployeeBasicInfos on ca.EmployeeBasicInfoId equals e.Id
                                                  select new TmConditionUserApproval
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

            return data;
        }
        public async Task<List<TmTemplateApproval>> GetByCategory(int categoryId)
        {
            var data = await (from t in context.TmTemplateApprovals.Where(x => x.ApprovalCategory == categoryId)
                              select new TmTemplateApproval
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
                                  ListUserApprovals = null,
                                  MenuId = string.IsNullOrEmpty(t.MenuId) ? "" : t.MenuId,
                                  JobTitleId = string.IsNullOrEmpty(t.JobTitleId) ? "0" : t.JobTitleId,
                                  MinBudget = t.MinBudget,
                                  MaxBudget = t.MaxBudget,
                                  ForDivisionId = t.ForDivisionId,
                                  IsAllDivision = t.IsAllDivision == null ? false : t.IsAllDivision
                              }).OrderBy(x => x.TemplateName).ToListAsync();
            var listDivision = await GetDivision();
            foreach (var item in data)
            {
                if (item.JobTitleId != "0") item.JobTitleName = context.JobTitles.Where(x => x.Id.Equals(item.JobTitleId)).FirstOrDefault().TitleName;
                else item.JobTitleName = "There is no Position";
                if (item.MenuId != null) item.MenuName = context.TmMenus.Where(x => x.Id.Equals(item.MenuId)).FirstOrDefault().MenuName;
                if (!string.IsNullOrEmpty(item.ForDivisionId)) item.ForDivisionName = listDivision.Where(x => x.Id.Equals(item.ForDivisionId)).FirstOrDefault()?.UnitName;
                else item.ForDivisionName = "There is no division";
            }

            return data;
        }

        public async Task<List<JobTitle>> GetJobTitlesByDivisionId(string divisionId)
        {
            var sql = @"SELECT  [Id]
                          ,[title_code]
                          ,[title_name]
                          ,[created_by]
                          ,[created_on]
                          ,[modified_by]
                          ,[modified_on]
                          ,[approved_by]
                          ,[approved_on]
                          ,[is_active]
                          ,[is_locked]
                          ,[is_default]
                          ,[is_deleted]
                          ,[owner_id]
                          ,[deleted_by]
                          ,[deleted_on]
                          ,[business_unit_division_id]
                          ,[business_unit_departement_id]
                      FROM [hr].[job_title]
                      where id in (SELECT a.job_title_id FROM hr.business_unit_job_level a
                    where business_unit_division_id = '" + divisionId + "')";
            try
            {
                var listJobTitle = await context.JobTitles.FromSql(sql).ToListAsync();
                return listJobTitle;
            }
            catch (Exception e)
            {
                var msg = e.Message;
                return null;
            }
        }
        public async Task<List<BusinessUnit>> GetDivision()
        {
            return await (from x in context.BusinessUnits.Where(xx => xx.IsDeleted.Equals(false))
                          join p in context.BusinessUnitTypes.Where(xx => xx.BusinessUnitLevel.Equals(800)).ToList()
                            on x.BusinessUnitTypeId equals p.Id
                          select new BusinessUnit
                          {
                              Id = x.Id,
                              UnitName = x.UnitName
                          }).ToListAsync();
        }
        public async Task<BusinessUnitJobLevel> GetBussinessUnitJobLevel(string jobTitleId, string divisionId)
        {
            return await context.BusinessUnitJobLevels.Where(x => x.BusinessUnitDivisionId.Equals(divisionId) && x.JobTitleId.Equals(jobTitleId)).FirstOrDefaultAsync();
        }

        public async Task<VmUser> GetSpecialCaseUser(string id)
        {
            VmUser query = new VmUser();
            var cekReportsTo = await context.EmployeeBasicInfos.Where(x => x.Id == id && x.IsDeleted == false).Select(x => x.ReportsTo).FirstOrDefaultAsync();
            if (string.IsNullOrEmpty(cekReportsTo))
            {
                var defaultData = await context.EmployeeBasicInfos.Where(x => x.Id == "1225624597257392128" && x.IsDeleted == false).Select(x => x.ReportsTo).FirstOrDefaultAsync();
                query = await (from q in context.BusinessUnitJobLevels.Where(x => x.IsDeleted.Equals(false))
                               join r in context.EmployeeBasicInfos.Where(x => x.IsDeleted.Equals(false)) on q.ParentId equals r.BusinessUnitJobLevelId
                               where r.Id == defaultData 
                               select new VmUser()
                               {
                                   EmployeeId = r.Id,
                                   EmployeeName = r.NameEmployee,
                                   Email = r.Email
                               }
                              ).FirstOrDefaultAsync();
            }
            else
            {
                query = await (from q in context.BusinessUnitJobLevels.Where(x => x.IsDeleted.Equals(false))
                               join r in context.EmployeeBasicInfos.Where(x => x.IsDeleted.Equals(false)) on q.Id equals r.BusinessUnitJobLevelId
                               where r.BusinessUnitJobLevelId == cekReportsTo
                               select new VmUser()
                               {
                                   EmployeeId = r.Id,
                                   EmployeeName = r.NameEmployee,
                                   Email = r.Email
                               }
                               ).FirstOrDefaultAsync();
            }


            return query;
        }

        public async Task<bool> CheckNotificationIsExists(string notificationTypeId, string userId, string itemId)
        {
            var result = false;
            var check = await context.Notifications.Where(x => x.NotificationTypeId == notificationTypeId && x.UserId == userId && x.ItemId == itemId).FirstOrDefaultAsync();
            if (check != null)
            {
                result = true;
            }
            return result;
        }

        public async Task<bool> AddNotificationAfterApproval(string userId, string refid, int? status, string linkCustom = null)
        {
            try
            {
                var dataApproval = await _repo.GetByRefId(refid).ConfigureAwait(false);
                var modelNotifType = new NotificationType();
                var codex = Guid.NewGuid().ToString().ToUpper();
                var message = "Your request " + dataApproval.FormReqName + " with refid " + refid + " has been approved.";
                var dataUserId = await context.EmployeeBasicInfos.Where(x => x.Id == userId).AsNoTracking().FirstOrDefaultAsync();
                modelNotifType.Id = codex;
                if (status == 4)
                {
                    message = "Your request " + dataApproval.FormReqName + " with refid " + refid + " has been rejected.";
                }
                modelNotifType.ShortText = message;
                modelNotifType.LongText = message;
                if (dataApproval.IsNotifyByEmail == true)
                {
                    modelNotifType.SendMail = true;
                }
                else
                {
                    modelNotifType.SendMail = false;
                }
                modelNotifType.Href = dataApproval.DetailLink;
                modelNotifType.IsActive = true;
                modelNotifType.IsDeleted = false;


                var model = new Notification();
                model.Id = Guid.NewGuid().ToString().ToUpper();
                model.NotificationTypeId = codex;
                model.UserId = dataUserId.Email;
                model.IsSeen = false;
                model.Module = "NotifApproval";
                model.ItemId = refid;
                await context.PhoenixAddAsync(modelNotifType);
                await context.PhoenixAddAsync(model);
                await context.SaveChangesAsync();

                var tipe = 1;
                if (!string.IsNullOrEmpty(linkCustom))
                {
                    dataApproval.DetailLink = linkCustom;
                }
                var templateHtml = "<a href='javascript:directToDetailNotif(\"" + codex + "\",\"" + dataApproval.DetailLink + "\")' title='click here to see detail'><div><p>" + message + "</p></div></a>";

                if (dataApproval.IsNotifyByWeb == true)
                {
                    await _hubContext.Clients.All.SendAsync("ReceiveNTFCenter", dataUserId.Email, templateHtml, tipe); //notification
                }

                if (dataApproval.IsNotifyByEmail == true)
                {
                    var emailTemplate = await context.TmEmailTemplates.AsNoTracking().Where(x => x.Id == "APPROVE-NOTIF" && x.IsDeleted == false).FirstOrDefaultAsync();
                    var aliasName = dataUserId.NameEmployee;

                    string baseUrl = string.Format("{0}://{1}{2}", context.HttpContext.Request.Scheme, context.configuration.GetSection("AppConfiguration").GetSection("WebServerUrl").Value, dataApproval.DetailLink);

                    emailTemplate.Subject = emailTemplate.Subject.Replace("@1", "Process Approval Notification " + refid);
                    emailTemplate.Body = emailTemplate.Body.Replace("@user", aliasName);
                    emailTemplate.Body = emailTemplate.Body.Replace("@Messages", message);
                    var plainText = HTMLToText(emailTemplate.Body);
                    plainText = plainText.Replace("\n", "<br>");

                    var modelAzure = await (from x in context.AzurePortalSetups.AsNoTracking().Where(x => !(x.IsDeleted ?? false))
                                            select new AzurePortalSetup
                                            {
                                                Id = x.Id,
                                                TenantId = x.TenantId,
                                                ClientId = x.ClientId,
                                                EmailAccount = x.EmailAccount,
                                                SecurityPword = SecurityHelper.DecryptString(x.SecurityPword)
                                            }).Take(1).FirstOrDefaultAsync();

                    var graph = new MsGraph(modelAzure.ClientId, modelAzure.TenantId);
                    var authResult = await graph.GetAuthentication(modelAzure.EmailAccount, modelAzure.SecurityPword);
                    var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);

                    List<Recipient> recipients = new List<Recipient>();
                    recipients.Add(new Recipient
                    {
                        EmailAddress = new EmailAddress
                        {
                            Address = dataUserId.Email
                        }
                    });

                    Message email = new Message
                    {
                        Body = new ItemBody
                        {
                            Content = plainText,
                            ContentType = BodyType.Html
                        },
                        Subject = emailTemplate.Subject,
                        ToRecipients = recipients
                    };
                    await graphClient.Me.SendMail(email, null).Request().PostAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
                //throw new Exception(ex.Message);
            }

        }

        public async Task<bool> SendEmail(string mailTo, string mailFrom, string mailSubject, string mailBody)
        {
            try
            {
                var plainText = HTMLToText(mailBody);
                plainText = plainText.Replace("\n", "<br>");

                var modelAzure = await (from x in context.AzurePortalSetups.AsNoTracking().Where(x => !(x.IsDeleted ?? false))
                                        select new AzurePortalSetup
                                        {
                                            Id = x.Id,
                                            TenantId = x.TenantId,
                                            ClientId = x.ClientId,
                                            EmailAccount = x.EmailAccount,
                                            SecurityPword = SecurityHelper.DecryptString(x.SecurityPword)
                                        }).Take(1).FirstOrDefaultAsync();

                var graph = new MsGraph(modelAzure.ClientId, modelAzure.TenantId);
                var authResult = await graph.GetAuthentication(modelAzure.EmailAccount, modelAzure.SecurityPword);
                var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);

                List<Recipient> recipients = new List<Recipient>();
                recipients.Add(new Recipient
                {
                    EmailAddress = new EmailAddress
                    {
                        Address = mailTo
                    }
                });

                Message email = new Message
                {
                    Body = new ItemBody
                    {
                        Content = plainText,
                        ContentType = BodyType.Html
                    },
                    Subject = mailSubject,
                    ToRecipients = recipients
                };
                await graphClient.Me.SendMail(email, null).Request().PostAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }

        }

        public async Task<bool> AddNotification(string notificationTypeId, string userId, string moduleName, string refid = null, bool? isWeb = null, bool? isEmail = null, string requestby = null, string linkNotifCustom = null)
        {
            try
            {
                var requestByEmail = await context.EmployeeBasicInfos.Where(x => x.Id == requestby).Select(x => x.Email).FirstOrDefaultAsync();
                var model = new Notification();
                var codex = Guid.NewGuid().ToString().ToUpper();
                model.Id = codex;
                model.NotificationTypeId = notificationTypeId;
                model.UserId = userId;
                model.IsSeen = false;
                model.Module = moduleName;
                model.ItemId = refid;
                await context.PhoenixAddAsync(model);

                // Get data Notification Type...
                var data = await context.NotificationTypes.AsNoTracking().Where(x => x.Id == notificationTypeId).AsNoTracking().FirstOrDefaultAsync();
                if (data != null)
                {
                    // if need send email...start here....
                    if (!string.IsNullOrEmpty(refid))
                    {
                        if (data.Href.Contains("{id}"))
                        {
                            data.Href = data.Href.Replace("{id}", refid);
                        }

                        if (!string.IsNullOrEmpty(linkNotifCustom))
                        {
                            data.Href = linkNotifCustom;
                        }
                    }
                    //var tipe = 1;
                    //if (notificationTypeId.Contains("approv")) tipe = 2;
                    //var templateHtml = "<a href='javascript:directToDetailNotif(\"" + codex + "\",\"" + data.Href + "\")' title='click here to see detail'><div><p>" + data.ShortText + "</p></div></a>";
                    //await _hubContext.Clients.All.SendAsync("ReceiveNTFCenter", userId, templateHtml, tipe);
                    
                    var tipe = 1;
                    var templateHtml = "<a href='javascript:directToDetailNotif(\"" + codex + "\",\"" + data.Href + "\")' title='click here to see detail'><div><p>" + data.ShortText + "</p></div></a>";

                    if (notificationTypeId.Contains("approv"))
                    {
                        tipe = 2;
                       
                        var tipe2 = 1;
                        var _model = new Notification();
                        codex = Guid.NewGuid().ToString().ToUpper();
                        _model.Id = codex;
                        _model.NotificationTypeId = codex;
                        _model.UserId = requestByEmail;
                        _model.IsSeen = false;
                        _model.Module = moduleName;
                        _model.ItemId = refid;
                        await context.PhoenixAddAsync(_model);

                        NotificationType _notifType = new NotificationType();
                        _notifType.Id = codex;
                        _notifType.IsActive = true;
                        _notifType.ShortText = "Waiting approval for : " + notificationTypeId;
                        _notifType.LongText = "Waiting approval for : " + notificationTypeId;
                        _notifType.Href = data.Href;
                        await context.PhoenixAddAsync(_notifType);

                        var templateHtml2 = "<a href='javascript:directToDetailNotif(\"" + codex + "\",\"" + data.Href + "\")' title='click here to see detail'><div><p>" + data.ShortText + "</p></div></a>";
                        if (isWeb == true)
                        {
                            await _hubContext.Clients.All.SendAsync("ReceiveNTFCenter", userId, templateHtml, tipe); //approval
                            await _hubContext.Clients.All.SendAsync("ReceiveNTFCenter", requestByEmail, templateHtml2, tipe2); //notification
                        }

                        if (isEmail == true)
                        {
                            var emailTemplate = await context.TmEmailTemplates.AsNoTracking().Where(x => x.Id == "EMAIL-NOTIF" && x.IsDeleted == false).FirstOrDefaultAsync();
                            var aliasName = await context.TmUserApps.Where(x => x.Id == userId).Select(x => x.AliasName).AsNoTracking().FirstOrDefaultAsync();
                            var aliasNameRequest = await context.TmUserApps.Where(x => x.Email == requestByEmail).Select(x => x.AliasName).AsNoTracking().FirstOrDefaultAsync();
                            string baseUrl = string.Format("{0}://{1}{2}", context.HttpContext.Request.Scheme, context.HttpContext.Request.Host, data.Href);

                            emailTemplate.Subject = emailTemplate.Subject.Replace("@1", data.ShortText + " : " + refid);
                            emailTemplate.Body = emailTemplate.Body.Replace("@user", aliasName);
                            emailTemplate.Body = emailTemplate.Body.Replace("@userrequest", aliasNameRequest);
                            emailTemplate.Body = emailTemplate.Body.Replace("@2", data.ShortText + " ( " + baseUrl + " ) ");
                            var plainText = HTMLToText(emailTemplate.Body);
                            plainText = plainText.Replace("\n", "<br>");

                            var modelAzure = await (from x in context.AzurePortalSetups.AsNoTracking().Where(x => !(x.IsDeleted ?? false))
                                                    select new AzurePortalSetup
                                                    {
                                                        Id = x.Id,
                                                        TenantId = x.TenantId,
                                                        ClientId = x.ClientId,
                                                        EmailAccount = x.EmailAccount,
                                                        SecurityPword = SecurityHelper.DecryptString(x.SecurityPword)
                                                    }).Take(1).FirstOrDefaultAsync();

                            var graph = new MsGraph(modelAzure.ClientId, modelAzure.TenantId);
                            var authResult = await graph.GetAuthentication(modelAzure.EmailAccount, modelAzure.SecurityPword);
                            var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);

                            List<Recipient> recipients = new List<Recipient>();
                            recipients.Add(new Recipient
                            {
                                EmailAddress = new EmailAddress
                                {
                                    Address = userId
                                }
                            });

                            Message email = new Message
                            {
                                Body = new ItemBody
                                {
                                    Content = plainText,
                                    ContentType = BodyType.Html
                                },
                                Subject = emailTemplate.Subject,
                                ToRecipients = recipients
                            };
                            await graphClient.Me.SendMail(email, null).Request().PostAsync();

                        }

                    }
                    else
                    {
                        if (isWeb == true)
                        {
                            await _hubContext.Clients.All.SendAsync("ReceiveNTFCenter", userId, templateHtml, tipe); //notification
                        }
                    }
                }
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

        public static string HTMLToText(string HTMLCode)
        {
            // Remove new lines since they are not visible in HTML
            HTMLCode = HTMLCode.Replace("\n", " ");

            // Remove tab spaces
            HTMLCode = HTMLCode.Replace("\t", " ");

            // Remove multiple white spaces from HTML
            HTMLCode = Regex.Replace(HTMLCode, "\\s+", " ");

            // Remove HEAD tag
            HTMLCode = Regex.Replace(HTMLCode, "<head.*?</head>", ""
                                , RegexOptions.IgnoreCase | RegexOptions.Singleline);

            // Remove any JavaScript
            HTMLCode = Regex.Replace(HTMLCode, "<script.*?</script>", ""
              , RegexOptions.IgnoreCase | RegexOptions.Singleline);

            // Replace special characters like &, <, >, " etc.
            StringBuilder sbHTML = new StringBuilder(HTMLCode);
            // Note: There are many more special characters, these are just
            // most common. You can add new characters in this arrays if needed
            string[] OldWords = {"&nbsp;", "&amp;", "&quot;", "&lt;",
   "&gt;", "&reg;", "&copy;", "&bull;", "&trade;","&#39;"};
            string[] NewWords = { " ", "&", "\"", "<", ">", "Â®", "Â©", "â€¢", "â„¢", "\'" };
            for (int i = 0; i < OldWords.Length; i++)
            {
                sbHTML.Replace(OldWords[i], NewWords[i]);
            }

            //sbHTML = sbHTML.Replace("</p>", "</p><br>");
            //sbHTML = sbHTML.Replace("</strong>", "</strong><br>");
            // Check if there are line breaks (<br>) or paragraph (<p>)
            sbHTML.Replace("<br>", "\n<br>");
            sbHTML.Replace("<br ", "\n<br ");
            sbHTML.Replace("<p>", "\n<p>");
            sbHTML.Replace("<p ", "\n<p ");

            // Finally, remove all HTML tags and return plain text
            return System.Text.RegularExpressions.Regex.Replace(
              sbHTML.ToString(), "<[^>]*>", "");
        }

        public class VmUser
        {
            public string EmployeeId { get; set; }
            public string EmployeeName { get; set; }
            public string Email { get; set; }
        }


        public class VmJobTitle
        {
            public string JobTitleId { get; set; }
        }


    }
}
