using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Um;
using Phoenix.Data.Models.ViewModel;
using Phoenix.Data.Services.Um;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;
using System.Text;
using Phoenix.ApiExtension.Helpers;
using Phoenix.Data.MsGraphExtention;
using Microsoft.Graph;
using Phoenix.Shared.Helpers;
using static Microsoft.Graph.Constants;
using Microsoft.Extensions.Options;

namespace Phoenix.Data.Attributes
{
    public class GlobalFunctionApproval
    {
        readonly DataContext _context;

        private readonly IManageTransactionApprovalService _repo;
        private readonly IManageTemplateApprovalService _repoTemplate;
        private readonly ITransactionUserApprovalService _repoManageUserApproval;
        private readonly ITransactionConditionalApprovalService _repoManageConditionApproval;
        private readonly IManageMenuService _menuService;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IManageUserAppService _repoUserApp;
        public GlobalFunctionApproval(DataContext context, IManageTransactionApprovalService repo, ITransactionUserApprovalService repoManageUserApproval, ITransactionConditionalApprovalService repoManageConditionApproval,
            IManageTemplateApprovalService repoTemplate, IManageMenuService menuService, IHubContext<NotificationHub> hubContext, IManageUserAppService repoUserApp)
        {
            _context = context;
            _repo = repo;
            _repoManageUserApproval = repoManageUserApproval;
            _repoManageConditionApproval = repoManageConditionApproval;
            _repoTemplate = repoTemplate;
            _menuService = menuService;
            _hubContext = hubContext;
            _repoUserApp = repoUserApp;
        }

        public async Task<int> SubmitApproval<T>(bool isEdit, string Id, int? status, string detailLink, string idTemplate, string uniqueName, T model) where T : class
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var save = await UnsafeSubmitApproval(isEdit, Id, status, detailLink, idTemplate, uniqueName, model);
                    transaction.Commit();
                    return save;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<int> UnsafeSubmitApproval<T>(bool isEdit, string Id, int? status, string detailLink, string idTemplate, string uniqueName, T model, string module = null, string NotificationType = null) where T : class
        {
            if (isEdit)
            {
                _context.PhoenixEdit(model);
            }
            else
            {
                await _context.PhoenixAddAsync(model);
            }
            var isApprove = true;
            if (status != 0)//0: Draft
            {

                var dataMenu = await _menuService.GetByUniqueName(uniqueName).ConfigureAwait(false);
                var vm = new Models.ViewModel.TransApprovalHrisVm()
                {
                    MenuId = dataMenu.Id,
                    RefId = Id,
                    DetailLink = detailLink,
                    //Tname = "hr.Rotation",//"SP#NAMA SP NYA",
                    IdTemplate = idTemplate,
                    Module = module,
                    NotificationType = NotificationType
                };

                var subGroupId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessSubGroup(_context.HttpContext);
                var divisionId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessUnitDivisi(_context.HttpContext);
                var employeeId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(_context.HttpContext);
                isApprove = await Save(vm, subGroupId, divisionId, employeeId);

                if (isApprove)
                {
                    return await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("please check the approval template that will be processed");
                }
            }
            else
            {
                return await _context.SaveChangesAsync();
            }
        }



        public async Task<int> ProcessApproval<T>(string Id, int? status, string remarks, T model) where T : class
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var employeeId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(_context.HttpContext);
                    var save = await UnsafeProcessApproval(Id, status, remarks, model);
                    transaction.Commit();
                    var notifApproved = await AddNotificationAfterApproval(employeeId, Id, status);
                    return save;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<int> UnsafeProcessApproval<T>(string Id, int? status, string remarks, T model) where T : class
        {
            _context.PhoenixApprove(model);

            var vm = new TrUpdateStatusApprovalViewModel()
            {
                RefId = Id,
                StatusApproved = status ?? 0,
                Remarks = remarks
            };

            var employeeId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(_context.HttpContext);
            var isApprove = await UpdateStatusApproval(vm, employeeId);

            if (isApprove)
            {
               
                return await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception();
            }
        }

        public async Task<int> UnsafeProcessApproval(string Id, int? status, string remarks)
        {
            var vm = new TrUpdateStatusApprovalViewModel()
            {
                RefId = Id,
                StatusApproved = status ?? 0,
                Remarks = remarks
            };

            var employeeId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(_context.HttpContext);
            var isApprove = await UpdateStatusApproval(vm, employeeId);

            if (isApprove)
            {
                
                return await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception();
            }
        }

        public async Task<bool> Save(TransApprovalHrisVm dataVm, string subGroupId, string divisionId, string employeeId)
        {
            if (!string.IsNullOrEmpty(dataVm.Tname) && !string.IsNullOrEmpty(dataVm.RefId))
            {
                if (!string.IsNullOrEmpty(dataVm.MenuId) || !string.IsNullOrEmpty(dataVm.DetailLink))
                {
                    await _repo.DeleteRefTable(dataVm.Tname, dataVm.RefId).ConfigureAwait(false);
                    return await Task.FromResult(false);
                }
            }

            if (!string.IsNullOrEmpty(dataVm.RefId))
            {
                var trApproval = _context.TrTemplateApprovals.AsNoTracking().Where(x => x.RefId == dataVm.RefId && !(x.IsDeleted ?? false));
                if (trApproval != null)
                {
                    foreach (var item in trApproval)
                    {
                        _context.Remove(item);
                    }
                }
            }

            try
            {
                var jobTitleId = "0";
                var data = new TmTemplateApproval();
                if (!string.IsNullOrEmpty(dataVm.IdTemplate))
                {
                    data = await _repoTemplate.GetById(dataVm.IdTemplate).ConfigureAwait(false);
                }
                else
                {
                    if (string.IsNullOrEmpty(divisionId))
                    {
                        throw new Exception("this employee doesn't have a division, please check this employee data again.");
                    }
                    data = await _repoTemplate.GetByMenuId(subGroupId, divisionId, dataVm.MenuId, jobTitleId).ConfigureAwait(false);

                    if (data == null)
                    {
                        throw new Exception("please check the approval template that will be processed");
                    }
                }

                var model = new TrTemplateApproval();
                model.TemplateName = data.TemplateName;
                model.IsNotifyByWeb = data.IsNotifyByWeb;
                model.IsNotifyByEmail = data.IsNotifyByEmail;
                model.DueDate = data.DueDate;
                model.DetailLink = dataVm.DetailLink;
                model.StatusApproved = StatusApproved.WaitingApproval;
                model.Reminder = data.Reminder;
                model.ApprovalCategory = data.ApprovalCategory;
                model.ApprovalType = data.ApprovalType;
                model.FormReqName = data.TemplateName;
                model.CreatedOn = DateTime.Now;
                model.CreatedBy = employeeId;
                model.DivisionId = divisionId;
                model.IsDeleted = false;
                model.RefId = dataVm.RefId;
                var tempId = Guid.NewGuid().ToString();
                model.Id = tempId;

                await _context.PhoenixAddAsync(model);
                //await _repo.AddAsync(model).ConfigureAwait(false);


                var isFirst = 0;
                foreach (var item in data.ListUserApprovals.OrderBy(x => x.IndexUser).OrderByDescending(x => x.IsSpecialCase))
                {
                    var vmUser = new TrUserApproval();
                    vmUser.EmployeeBasicInfoId = item.EmployeeBasicInfoId;
                    vmUser.IndexUser = item.IndexUser;
                    vmUser.IsCondition = item.IsCondition;
                    vmUser.IsSpecialCase = item.IsSpecialCase;
                    vmUser.TrTempApprovalId = tempId;
                    vmUser.Id = Guid.NewGuid().ToString();
                    vmUser.NotificationTypeId = dataVm.NotificationType;
                    vmUser.Module = dataVm.Module;
                    if (isFirst == 0)
                    {
                        vmUser.StatusApproved = StatusApproved.CurrentApproval;

                        //notifikasi hanya muncul untuk yg sedang melakukan approval
                        if (!string.IsNullOrEmpty(dataVm.NotificationType) && !string.IsNullOrEmpty(dataVm.Module))
                        {
                            var notif = await _context.TmUserApps.AsNoTracking().Where(x => x.EmployeeBasicInfoId == item.EmployeeBasicInfoId && x.IsDeleted == false).FirstOrDefaultAsync();
                            if (notif != null)
                            {
                                await AddNotification(dataVm.NotificationType, notif.Id, dataVm.Module, dataVm.RefId, data.IsNotifyByWeb, data.IsNotifyByEmail, employeeId);
                            }
                        }
                    }
                    else
                    {
                        vmUser.StatusApproved = StatusApproved.WaitingApproval;
                    }
                    await _context.PhoenixAddAsync(vmUser);
                    //await _repoManageUserApproval.AddAsync(vmUser).ConfigureAwait(false);

                    //looping data conditional user approval.....
                    if (item.ListConditionalApprovals != null)
                    {
                        foreach (var con in item.ListConditionalApprovals)
                        {
                            var modelCon = new TrConditionUserApproval();
                            modelCon.Id = Guid.NewGuid().ToString();
                            modelCon.EmployeeBasicInfoId = con.EmployeeBasicInfoId;
                            modelCon.GroupConditionIndex = con.GroupConditionIndex;
                            modelCon.GroupConditionApproval = con.GroupConditionApproval;
                            modelCon.ConditionalApproval = con.ConditionalApproval;
                            if (isFirst == 0) modelCon.StatusApproved = StatusApproved.CurrentApproval;
                            else modelCon.StatusApproved = StatusApproved.WaitingApproval;
                            modelCon.TrUserApprovalId = vmUser.Id;
                            modelCon.CreatedOn = DateTime.Now;
                            modelCon.CreatedBy = employeeId;
                            await _context.PhoenixAddAsync(modelCon);
                            //await _repoManageConditionApproval.AddAsync(modelCon).ConfigureAwait(false);
                        }
                    }
                    
                    await _context.SaveChangesAsync();
                    isFirst++;
                }
                var vm = new
                {
                    IdTemp = tempId
                };

                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (!string.IsNullOrEmpty(dataVm.RefId) && !string.IsNullOrEmpty(dataVm.Tname))
                {
                    await _repo.DeleteRefTable(dataVm.Tname, dataVm.RefId).ConfigureAwait(false);
                }
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> AddNotificationAfterApproval(string userId, string refid, int? status)
        {
            try
            {
                var dataApproval = await _repo.GetByRefId(refid).ConfigureAwait(false);
                var modelNotifType = new NotificationType();
                var codex = Guid.NewGuid().ToString().ToUpper();
                var message = "Your request " + dataApproval.FormReqName + " with refid " + refid + " has been approved.";
                var dataUserId = await _context.EmployeeBasicInfos.Where(x => x.Id == userId).AsNoTracking().FirstOrDefaultAsync();
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
                await _context.PhoenixAddAsync(modelNotifType);
                await _context.PhoenixAddAsync(model);
                await _context.SaveChangesAsync();

                var tipe = 1;
                var templateHtml = "<a href='javascript:directToDetailNotif(\"" + codex + "\",\"" + dataApproval.DetailLink + "\")' title='click here to see detail'><div><p>" + message + "</p></div></a>";

                if (dataApproval.IsNotifyByWeb == true)
                {
                    await _hubContext.Clients.All.SendAsync("ReceiveNTFCenter", dataUserId.Email, templateHtml, tipe); //notification
                }

                if (dataApproval.IsNotifyByEmail == true)
                {
                    var emailTemplate =  await _context.TmEmailTemplates.AsNoTracking().Where(x => x.Id == "APPROVE-NOTIF" && x.IsDeleted == false).FirstOrDefaultAsync();
                    var aliasName = dataUserId.NameEmployee;

                    string baseUrl = string.Format("{0}://{1}{2}", _context.HttpContext.Request.Scheme, _context.configuration.GetSection("AppConfiguration").GetSection("WebServerUrl").Value, dataApproval.DetailLink);
                    var clicklink = "<a href='" + baseUrl + "'>Please click this link to get more detail</a>";

                    emailTemplate.Subject = emailTemplate.Subject.Replace("@1", "Process Approval Notification " + refid);
                    emailTemplate.Body = emailTemplate.Body.Replace("@user", aliasName);
                    emailTemplate.Body = emailTemplate.Body.Replace("@Messages", message);
                  
                    var plainText = HTMLToText(emailTemplate.Body);
                    plainText = plainText.Replace("\n", "<br>");
                    plainText = plainText.Replace("@2", clicklink);

                    var modelAzure = await (from x in _context.AzurePortalSetups.AsNoTracking().Where(x => !(x.IsDeleted ?? false))
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

        public async Task<bool> AddNotification(string notificationTypeId, string userId, string moduleName, string refid = null, bool? isWeb = null, bool? isEmail = null,string requestby = null)
        {
            try
            {
                var requestByEmail = string.Empty;
                if (!string.IsNullOrEmpty(requestby))
                {
                    requestByEmail = await _context.EmployeeBasicInfos.Where(x => x.Id == requestby).Select(x => x.Email).FirstOrDefaultAsync();
                }

                var model = new Notification();
                var codex = Guid.NewGuid().ToString().ToUpper();
                model.Id = codex;
                model.NotificationTypeId = notificationTypeId;
                model.UserId = userId;
                model.IsSeen = false;
                model.Module = moduleName;
                model.ItemId = refid;
                await _context.PhoenixAddAsync(model);

                // Get data Notification Type...
                var data = await _context.NotificationTypes.AsNoTracking().Where(x => x.Id == notificationTypeId).AsNoTracking().FirstOrDefaultAsync();

                if (data != null)
                {
                    // if need send email...start here....
                    if (!string.IsNullOrEmpty(refid))
                    {
                        if (data.Href.Contains("{id}"))
                        {
                            data.Href = data.Href.Replace("{id}", refid);
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
                        var codex2 = Guid.NewGuid().ToString().ToUpper();
                        var newNotifType = Guid.NewGuid().ToString().ToUpper();
                        _model.Id = codex2;
                        _model.NotificationTypeId = newNotifType;
                        _model.UserId = requestByEmail;
                        _model.IsSeen = false;
                        _model.Module = moduleName;
                        _model.ItemId = refid;
                        await _context.PhoenixAddAsync(_model);

                        NotificationType _notifType = new NotificationType();
                        _notifType.Id = newNotifType;
                        _notifType.IsActive = true;
                        _notifType.ShortText = "Waiting approval for : " + notificationTypeId;
                        _notifType.LongText = "Waiting approval for : " + notificationTypeId;
                        _notifType.Href = data.Href;
                        await _context.PhoenixAddAsync(_notifType);

                        var templateHtml2 = "<a href='javascript:directToDetailNotif(\"" + codex2 + "\",\"" + data.Href + "\")' title='click here to see detail'><div><p>" + data.ShortText + "</p></div></a>";
                        if (isWeb == true)
                        {
                            await _hubContext.Clients.All.SendAsync("ReceiveNTFCenter", userId, templateHtml, tipe); //approval
                            await _hubContext.Clients.All.SendAsync("ReceiveNTFCenter", requestByEmail, templateHtml2, tipe2); //notification
                        }

                        if (isEmail == true)
                        {
                            var emailTemplate = await _context.TmEmailTemplates.AsNoTracking().Where(x => x.Id == "EMAIL-NOTIF" && x.IsDeleted == false).FirstOrDefaultAsync();
                            var aliasName = await _context.TmUserApps.Where(x => x.Id == userId).Select(x => x.AliasName).AsNoTracking().FirstOrDefaultAsync();
                            var aliasNameRequest = await _context.TmUserApps.Where(x => x.Email == requestByEmail).Select(x => x.AliasName).AsNoTracking().FirstOrDefaultAsync();
                            string baseUrl = string.Format("{0}://{1}{2}", _context.HttpContext.Request.Scheme, _context.configuration.GetSection("AppConfiguration").GetSection("WebServerUrl").Value, data.Href);
                            var clicklink = "<a href='" + baseUrl + "'>Please click this link to get more detail</a>";
                            emailTemplate.Subject = emailTemplate.Subject.Replace("@1", data.ShortText + " : " + refid);
                            emailTemplate.Body = emailTemplate.Body.Replace("@userrequest", aliasNameRequest);
                            emailTemplate.Body = emailTemplate.Body.Replace("@user", aliasName);
                            var plainText = HTMLToText(emailTemplate.Body);
                            plainText = plainText.Replace("\n", "<br>");
                            plainText = plainText.Replace("@2", clicklink);

                            var modelAzure = await (from x in _context.AzurePortalSetups.AsNoTracking().Where(x => !(x.IsDeleted ?? false))
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
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                //throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateStatusApproval(TrUpdateStatusApprovalViewModel data, string empId)
        {
            int? nextIndex = 0;
            if (data.StatusApproved == 4 && string.IsNullOrEmpty(data.Remarks))
            {
                return await Task.FromResult(false);
            }
            //1. Cek user/employee yang approve di conditional or di concurrent....

            //--Get Template Approval nya dulu...
            var dataApproval = await _repo.GetByRefId(data.RefId).ConfigureAwait(false);
            var userApproval = dataApproval.ListUserApprovals.OrderBy(x => x.IndexUser).OrderByDescending(x => x.IsSpecialCase).ToList();
            var requestApproval = dataApproval.CreatedBy;
            
            for (int i = 0; i < userApproval.Count(); i++)
            {
                if (userApproval[i].IsCondition == true && userApproval[i].StatusApproved == StatusApproved.CurrentApproval)
                {
                    // -- Cek di Conditional nya...
                    foreach (var x in userApproval[i].ListConditionalApprovals)
                    {
                        if (x.EmployeeBasicInfoId == empId)
                        {
                            if (x.ConditionalApproval == "OR")
                            {
                                if (x.GroupConditionApproval == "OR")
                                {
                                    CheckAllOrCondition(empId, data, x.Id, userApproval[i].Id, dataApproval.Id, userApproval);
                                    break;
                                }
                                else // kalau kondisi nya AND..
                                {
                                    if (data.StatusApproved == 4) // jika rejected...kelar alias close..
                                    {
                                        //update Table Conditional...
                                        var dataCon = await _repoManageConditionApproval.Get(x.Id).ConfigureAwait(false);
                                        dataCon.StatusApproved = (StatusApproved)Enum.ToObject(typeof(StatusApproved), data.StatusApproved);
                                        dataCon.CreatedBy = empId;
                                        dataCon.CreatedOn = DateTime.Now;
                                        await _repoManageConditionApproval.EditAsync(dataCon);

                                        //update table User APproval...
                                        var dataUserApp = await _repoManageUserApproval.Get(x.TrUserApprovalId).ConfigureAwait(false);
                                        dataUserApp.CreatedBy = empId;
                                        dataUserApp.CreatedOn = DateTime.Now;
                                        dataUserApp.StatusApproved = (StatusApproved)Enum.ToObject(typeof(StatusApproved), data.StatusApproved);
                                        await _repoManageUserApproval.EditAsync(dataUserApp).ConfigureAwait(false);

                                        var dataApp = await _repo.Get(dataApproval.Id);
                                        dataApp.RemarkRejected = data.Remarks;
                                        dataApp.StatusApproved = StatusApproved.Rejected;
                                        dataApp.RejectedBy = empId;
                                        dataApp.RejectedOn = DateTime.Now;
                                        await _repo.EditAsync(dataApp).ConfigureAwait(false);
                                        goto FinishedProcess;
                                    }
                                    else
                                    {
                                        CheckConditionGroupForOrAnd(userApproval[i].ListConditionalApprovals, empId, data, userApproval, dataApproval.Id);
                                        goto FinishedProcess;
                                    }
                                }
                            }
                            else // conditional = AND
                            {
                                if (x.GroupConditionApproval == "OR")
                                {
                                    CheckConditionGroupForAndPlusOr(userApproval[i].ListConditionalApprovals, empId, data, userApproval);
                                    goto FinishedProcess;
                                }
                                else // Contional Group = AND
                                {
                                    CheckConditionGroupForAndPlusOr(userApproval[i].ListConditionalApprovals, empId, data, userApproval);
                                    goto FinishedProcess;
                                }
                            }
                        }
                    }
                }
                else // JIka tidak ada Conditional approval... 
                {
                    if (userApproval[i].StatusApproved == StatusApproved.CurrentApproval)
                    {
                        if (userApproval[i].EmployeeBasicInfoId == empId)
                        {

                            //update table User APproval...
                            var dataUserApp = await _repoManageUserApproval.Get(userApproval[i].Id).ConfigureAwait(false);
                            //var dataUserApp = await _repoManageUserApproval.GetByTemplateSingleId(item.TrTempApprovalId).ConfigureAwait(false);
                            dataUserApp.CreatedBy = empId;
                            dataUserApp.CreatedOn = DateTime.Now;
                            dataUserApp.StatusApproved = (StatusApproved)Enum.ToObject(typeof(StatusApproved), data.StatusApproved);
                            await _repoManageUserApproval.EditAsync(dataUserApp).ConfigureAwait(false);

                            if (data.StatusApproved == 4)
                            {
                                //update table tr approval header nya...and request form is Close...
                                // var dataApp = await _repo.Get(dataUserApp.TrTempApprovalId).ConfigureAwait(false);
                                var dataApp = await _repo.Get(dataUserApp.TrTempApprovalId).ConfigureAwait(false);
                                dataApp.RemarkRejected = data.Remarks;
                                dataApp.StatusApproved = StatusApproved.Rejected;
                                dataApp.RejectedBy = empId;
                                dataApp.RejectedOn = DateTime.Now;
                                await _repo.EditAsync(dataApp).ConfigureAwait(false);
                            }
                            else
                            {
                                //baca list user approval nya, masih ada atau nggak ada...
                                var dataUserConCurrentApproval = userApproval.Where(x => x.StatusApproved == StatusApproved.WaitingApproval).ToList();

                                if (dataUserConCurrentApproval.Count == 0)
                                {
                                    var dataApp = await _repo.Get(dataUserApp.TrTempApprovalId).ConfigureAwait(false);
                                    dataApp.StatusApproved = StatusApproved.Approved;
                                    await _repo.EditAsync(dataApp).ConfigureAwait(false);
                                }
                                else
                                {
                                    nextIndex = userApproval[i + 1].IndexUser;
                                    var nEmployeeId = userApproval[i + 1].EmployeeBasicInfoId;
                                    if (!string.IsNullOrEmpty(nEmployeeId))
                                    {
                                        var NextEmployee = await _repoUserApp.GetByEmployeeBasicInfoId(nEmployeeId).ConfigureAwait(false);
                                        await _repoManageUserApproval.UpdateNextApproval(dataUserApp.TrTempApprovalId, Convert.ToInt32(nextIndex)).ConfigureAwait(false);
                                        if (NextEmployee != null)
                                        {
                                            var checkNotifIsExists = await _context.Notifications.Where(x => x.NotificationTypeId == dataUserApp.NotificationTypeId && x.UserId == NextEmployee.Email && x.ItemId == data.RefId).FirstOrDefaultAsync();
                                            if (checkNotifIsExists == null)
                                            {
                                                await AddNotification(dataUserApp.NotificationTypeId, NextEmployee.Email, dataUserApp.Module, data.RefId, dataApproval.IsNotifyByWeb, dataApproval.IsNotifyByEmail, dataApproval.CreatedBy);
                                            }
                                        }
                                    }
                                    
                                    var dataConcurrent = _repoManageUserApproval.GetCurrentApproval(dataUserApp.TrTempApprovalId).Result;
                                    if (dataConcurrent != null)
                                    {
                                        await _repoManageConditionApproval.UpdateNextApproval(dataConcurrent.Id).ConfigureAwait(false);
                                        var ConcurrentEmployee = await _repoUserApp.GetByEmployeeBasicInfoId(dataConcurrent.EmployeeBasicInfoId).ConfigureAwait(false);
                                        var checkNotifConcurrentEmployee = await _context.Notifications.Where(x => x.NotificationTypeId == dataUserApp.NotificationTypeId && x.UserId == ConcurrentEmployee.Email && x.ItemId == data.RefId).FirstOrDefaultAsync();
                                        if (checkNotifConcurrentEmployee == null)
                                        {
                                            await AddNotification(dataUserApp.NotificationTypeId, ConcurrentEmployee.Email, dataUserApp.Module, data.RefId, dataApproval.IsNotifyByWeb, dataApproval.IsNotifyByEmail, dataApproval.CreatedBy);
                                        }
                                    }
                                        
                                }
                            }
                            break;
                        }
                    }
                }
            }
            #region comment
            //foreach (var item in userApproval)
            //{
            //    if (item.IsCondition == true && item.StatusApproved == StatusApproved.CurrentApproval)
            //    {
            //        // -- Cek di Conditional nya...
            //        foreach (var x in item.ListConditionalApprovals)
            //        {
            //            if (x.EmployeeBasicInfoId == empId)
            //            {
            //                if (x.ConditionalApproval == "OR")
            //                {
            //                    if (x.GroupConditionApproval == "OR")
            //                    {
            //                        CheckAllOrCondition(empId, data, x.Id, item.Id, dataApproval.Id, userApproval);
            //                        break;
            //                    }
            //                    else // kalau kondisi nya AND..
            //                    {
            //                        if (data.StatusApproved == 4) // jika rejected...kelar alias close..
            //                        {
            //                            //update Table Conditional...
            //                            var dataCon = await _repoManageConditionApproval.Get(x.Id).ConfigureAwait(false);
            //                            dataCon.StatusApproved = (StatusApproved)Enum.ToObject(typeof(StatusApproved), data.StatusApproved);
            //                            dataCon.CreatedBy = empId;
            //                            dataCon.CreatedOn = DateTime.Now;
            //                            await _repoManageConditionApproval.EditAsync(dataCon);

            //                            //update table User APproval...
            //                            var dataUserApp = await _repoManageUserApproval.Get(x.TrUserApprovalId).ConfigureAwait(false);
            //                            dataUserApp.CreatedBy = empId;
            //                            dataUserApp.CreatedOn = DateTime.Now;
            //                            dataUserApp.StatusApproved = (StatusApproved)Enum.ToObject(typeof(StatusApproved), data.StatusApproved);
            //                            await _repoManageUserApproval.EditAsync(dataUserApp).ConfigureAwait(false);

            //                            var dataApp = await _repo.Get(dataApproval.Id);
            //                            dataApp.RemarkRejected = data.Remarks;
            //                            dataApp.StatusApproved = StatusApproved.Rejected;
            //                            dataApp.RejectedBy = empId;
            //                            dataApp.RejectedOn = DateTime.Now;
            //                            await _repo.EditAsync(dataApp).ConfigureAwait(false);
            //                            goto FinishedProcess;
            //                        }
            //                        else
            //                        {
            //                            CheckConditionGroupForOrAnd(item.ListConditionalApprovals, empId, data, userApproval, dataApproval.Id);
            //                            goto FinishedProcess;
            //                        }
            //                    }
            //                }
            //                else // conditional = AND
            //                {
            //                    if (x.GroupConditionApproval == "OR")
            //                    {
            //                        CheckConditionGroupForAndPlusOr(item.ListConditionalApprovals, empId, data, userApproval);
            //                        goto FinishedProcess;
            //                    }
            //                    else // Contional Group = AND
            //                    {
            //                        CheckConditionGroupForAndPlusOr(item.ListConditionalApprovals, empId, data, userApproval);
            //                        goto FinishedProcess;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    else // JIka tidak ada Conditional approval... 
            //    {
            //        if (item.StatusApproved == StatusApproved.CurrentApproval)
            //        {
            //            if (item.EmployeeBasicInfoId == empId)
            //            {

            //                //update table User APproval...
            //                var dataUserApp = await _repoManageUserApproval.Get(item.Id).ConfigureAwait(false);
            //                //var dataUserApp = await _repoManageUserApproval.GetByTemplateSingleId(item.TrTempApprovalId).ConfigureAwait(false);
            //                dataUserApp.CreatedBy = empId;
            //                dataUserApp.CreatedOn = DateTime.Now;
            //                dataUserApp.StatusApproved = (StatusApproved)Enum.ToObject(typeof(StatusApproved), data.StatusApproved);
            //                await _repoManageUserApproval.EditAsync(dataUserApp).ConfigureAwait(false);

            //                if (data.StatusApproved == 4)
            //                {
            //                    //update table tr approval header nya...and request form is Close...
            //                    // var dataApp = await _repo.Get(dataUserApp.TrTempApprovalId).ConfigureAwait(false);
            //                    var dataApp = await _repo.Get(dataUserApp.TrTempApprovalId).ConfigureAwait(false);
            //                    dataApp.RemarkRejected = data.Remarks;
            //                    dataApp.StatusApproved = StatusApproved.Rejected;
            //                    dataApp.RejectedBy = empId;
            //                    dataApp.RejectedOn = DateTime.Now;
            //                    await _repo.EditAsync(dataApp).ConfigureAwait(false);
            //                }
            //                else
            //                {
            //                    //baca list user approval nya, masih ada atau nggak ada...
            //                    var dataUserConCurrentApproval = userApproval.Where(x => x.StatusApproved == StatusApproved.CurrentApproval || x.StatusApproved == StatusApproved.WaitingApproval).ToList();
            //                    if (dataUserConCurrentApproval.Count == 0)
            //                    {
            //                        // var dataApp = await _repo.Get(dataUserApp.TrTempApprovalId).ConfigureAwait(false);
            //                        var dataApp = await _repo.Get(dataUserApp.Id).ConfigureAwait(false);
            //                        dataApp.StatusApproved = StatusApproved.Approved;
            //                        dataApp.RejectedBy = empId;
            //                        dataApp.RejectedOn = DateTime.Now;
            //                        await _repo.EditAsync(dataApp).ConfigureAwait(false);
            //                    }
            //                    else if (dataUserConCurrentApproval.Count == 1)
            //                    {

            //                        nextIndex = dataUserApp.IndexUser + 1;
            //                        await _repoManageUserApproval.UpdateNextApproval(dataUserApp.TrTempApprovalId, Convert.ToInt32(nextIndex)).ConfigureAwait(false);
            //                        var dataConcurrent = _repoManageUserApproval.GetCurrentApproval(dataUserApp.TrTempApprovalId).Result;
            //                        if (dataConcurrent != null)
            //                            await _repoManageConditionApproval.UpdateNextApproval(dataConcurrent.Id).ConfigureAwait(false);

            //                        // var dataApp = await _repo.Get(dataUserApp.Id).ConfigureAwait(false);
            //                        var dataApp = await _repo.Get(dataUserApp.TrTempApprovalId).ConfigureAwait(false);
            //                        dataApp.StatusApproved = StatusApproved.Approved;
            //                        dataApp.RejectedBy = empId;
            //                        dataApp.RejectedOn = DateTime.Now;
            //                        await _repo.EditAsync(dataApp).ConfigureAwait(false);
            //                    }
            //                    else
            //                    {

            //                        nextIndex = dataUserApp.IndexUser + 1;
            //                        await _repoManageUserApproval.UpdateNextApproval(dataUserApp.TrTempApprovalId, Convert.ToInt32(nextIndex)).ConfigureAwait(false);
            //                        var dataConcurrent = _repoManageUserApproval.GetCurrentApproval(dataUserApp.TrTempApprovalId).Result;
            //                        if (dataConcurrent != null)
            //                            await _repoManageConditionApproval.UpdateNextApproval(dataConcurrent.Id).ConfigureAwait(false);
            //                    }
            //                }
            //                break;
            //            }
            //        }
            //    }

            //}
            #endregion
            FinishedProcess:
            //var isApprove = true;
            //if (data.StatusApproved == 4)
            //{
            //    isApprove = false;
            //}
            //var notification = AddNotificationAfterApproval(requestApproval, data.RefId, dataApproval.FormReqName, dataApproval.DetailLink, isApprove, dataApproval.IsNotifyByWeb, dataApproval.IsNotifyByEmail);
            return await Task.FromResult(true);
        }

       

        public async Task clearApproval(string refId)
        {
            var template = await _context.TrTemplateApprovals.AsNoTracking().Where(x => x.RefId == refId).ToListAsync();
            var templateUser = await (from b in _context.TrUserApprovals.AsNoTracking()
                                      join c in _context.TrTemplateApprovals on b.TrTempApprovalId equals c.Id
                                      where c.RefId == refId
                                      select b).ToListAsync();

            var templateCondition = await (from a in _context.TrConditionUserApprovals.AsNoTracking()
                                           join b in _context.TrUserApprovals.AsNoTracking() on a.TrUserApprovalId equals b.Id
                                           join c in _context.TrTemplateApprovals on b.TrTempApprovalId equals c.Id
                                           where c.RefId == refId
                                           select a).ToListAsync();

            _context.TrConditionUserApprovals.RemoveRange(templateCondition.ToArray());
            _context.TrUserApprovals.RemoveRange(templateUser.ToArray());
            _context.TrTemplateApprovals.RemoveRange(template.ToArray());
        }

        #region Condition OR (or + And)
        private void CheckConditionGroupForOrAnd(List<TrConditionUserApproval> listConditional, string empId, TrUpdateStatusApprovalViewModel data, List<TrUserApproval> listConcurrentUser, string trApprovalId)
        {
            //1. Cek Self Group...
            //--Cek and Insert Group Index...Gropuing By GroupIndexApproval...
            var vmListGroup = new List<VmGrouping>();
            foreach (var item in listConditional)
            {
                var isready = false;
                foreach (var g in vmListGroup)
                {
                    if (item.GroupConditionIndex == g.GroupIndex) isready = true;
                }
                if (!isready)
                {
                    vmListGroup.Add(new VmGrouping { GroupIndex = Convert.ToInt32(item.GroupConditionIndex) });
                }
            }

            //update Table Conditional...
            var conData = listConditional.Where(c => c.EmployeeBasicInfoId == empId).FirstOrDefault();
            var dataCon = _repoManageConditionApproval.Get(conData.Id).Result;
            dataCon.StatusApproved = (StatusApproved)Enum.ToObject(typeof(StatusApproved), data.StatusApproved);
            dataCon.CreatedBy = empId;
            dataCon.CreatedOn = DateTime.Now;
            _repoManageConditionApproval.EditAsync(dataCon).ConfigureAwait(false);

            //--Add List Condition to Grouping INdex and CheckIN if user already approved all....
            foreach (var item in vmListGroup)
            {
                var dataConditionList = listConditional.Where(c => c.GroupConditionIndex == item.GroupIndex && (c.StatusApproved == StatusApproved.WaitingApproval || c.StatusApproved == StatusApproved.CurrentApproval)).ToList();
                if (dataConditionList.Count == 0)// semua dalam group sudah approved..
                {
                    var dataUserConCurrentApproval = listConcurrentUser.Where(x => x.StatusApproved == StatusApproved.WaitingApproval).ToList();
                    if (dataUserConCurrentApproval.Count == 0) // jika semua user dalam Group itu sudah approved..
                    {
                        //update table User APproval...
                        var dataUserApp = _repoManageUserApproval.Get(conData.TrUserApprovalId).Result;
                        dataUserApp.CreatedBy = empId;
                        dataUserApp.CreatedOn = DateTime.Now;
                        dataUserApp.StatusApproved = (StatusApproved)Enum.ToObject(typeof(StatusApproved), data.StatusApproved);
                        _repoManageUserApproval.EditAsync(dataUserApp).ConfigureAwait(false);

                        // Update table Header (TrTemplateApproval)....
                        var dataApp = _repo.Get(trApprovalId).Result;
                        dataApp.StatusApproved = StatusApproved.Approved;
                        dataApp.RejectedBy = empId;
                        dataApp.RejectedOn = DateTime.Now;
                        _repo.EditAsync(dataApp).ConfigureAwait(false);

                        var nextIndex = dataUserApp.IndexUser + 1;
                        //update User Approval Concurrent next Index...
                        _repoManageUserApproval.UpdateNextApproval(trApprovalId, Convert.ToInt32(nextIndex)).ConfigureAwait(false);
                        var dataConcurrent = _repoManageUserApproval.GetCurrentApproval(trApprovalId).Result;
                        if (dataConcurrent != null)
                            _repoManageConditionApproval.UpdateNextApproval(dataConcurrent.Id).ConfigureAwait(false);
                        break;
                    }
                }
            }
        }

        private void CheckAllOrCondition(string empId, TrUpdateStatusApprovalViewModel data, string idConditional, string idUserApproval, string idApproval, List<TrUserApproval> listConcurrentUser)
        {
            //update Table Conditional...
            var dataCon = _repoManageConditionApproval.Get(idConditional).Result;
            dataCon.StatusApproved = (StatusApproved)Enum.ToObject(typeof(StatusApproved), data.StatusApproved);
            dataCon.CreatedBy = empId;
            dataCon.CreatedOn = DateTime.Now;
            _repoManageConditionApproval.EditAsync(dataCon).ConfigureAwait(false);

            //update table User APproval...
            var dataUserApp = _repoManageUserApproval.Get(idUserApproval).Result;
            dataUserApp.CreatedBy = empId;
            dataUserApp.CreatedOn = DateTime.Now;
            dataUserApp.StatusApproved = (StatusApproved)Enum.ToObject(typeof(StatusApproved), data.StatusApproved);
            _repoManageUserApproval.EditAsync(dataUserApp).ConfigureAwait(false);

            if (data.StatusApproved == 4)// rejected
            {
                //update table tr approval header nya...and request form is Close...
                var dataApp = _repo.Get(idApproval).Result;
                dataApp.RemarkRejected = data.Remarks;
                dataApp.StatusApproved = StatusApproved.Rejected;
                dataApp.RejectedBy = empId;
                dataApp.RejectedOn = DateTime.Now;
                _repo.EditAsync(dataApp).ConfigureAwait(false);
            }
            else
            {
                //baca list user approval nya, masih ada atau nggak ada...
                var dataUserConCurrentApproval = listConcurrentUser.Where(x => x.StatusApproved == StatusApproved.CurrentApproval || x.StatusApproved == StatusApproved.WaitingApproval).ToList();
                if (dataUserConCurrentApproval.Count == 0)
                {
                    var dataApp = _repo.Get(idApproval).Result;
                    dataApp.StatusApproved = StatusApproved.Approved;
                    dataApp.RejectedBy = empId;
                    dataApp.RejectedOn = DateTime.Now;
                    _repo.EditAsync(dataApp).ConfigureAwait(false);
                }
                else
                {
                    var nextIndex = dataUserApp.IndexUser + 1;
                    //update User Approval Concurrent next Index...
                    _repoManageUserApproval.UpdateNextApproval(idApproval, Convert.ToInt32(nextIndex)).ConfigureAwait(false);
                    var dataConcurrent = _repoManageUserApproval.GetCurrentApproval(idApproval).Result;
                    if (dataConcurrent != null)
                        _repoManageConditionApproval.UpdateNextApproval(dataConcurrent.Id).ConfigureAwait(false);
                }
            }
        }
        #endregion

        #region Condition AND (or + And)
        private void CheckConditionGroupForAndPlusOr(List<TrConditionUserApproval> listConditional, string empId, TrUpdateStatusApprovalViewModel data, List<TrUserApproval> listConcurrentUser)
        {
            //cek status approved nya Reject or Approve...
            if (data.StatusApproved == 4)
            {
                //update Table Conditional...
                var conData = listConditional.Where(c => c.EmployeeBasicInfoId == empId).FirstOrDefault();
                var dataCon = _repoManageConditionApproval.Get(conData.Id).Result;
                dataCon.StatusApproved = (StatusApproved)Enum.ToObject(typeof(StatusApproved), data.StatusApproved);
                dataCon.CreatedBy = empId;
                dataCon.CreatedOn = DateTime.Now;
                _repoManageConditionApproval.EditAsync(dataCon).ConfigureAwait(false);
                //update table User APproval...
                var dataUserApp = _repoManageUserApproval.Get(conData.TrUserApprovalId).Result;
                dataUserApp.CreatedBy = empId;
                dataUserApp.CreatedOn = DateTime.Now;
                dataUserApp.StatusApproved = (StatusApproved)Enum.ToObject(typeof(StatusApproved), data.StatusApproved);
                _repoManageUserApproval.EditAsync(dataUserApp).ConfigureAwait(false);

                //update table tr approval header nya...and request form is Close...
                var dataApp = _repo.Get(dataUserApp.TrTempApprovalId).Result;
                dataApp.RemarkRejected = data.Remarks;
                dataApp.StatusApproved = StatusApproved.Rejected;
                dataApp.RejectedBy = empId;
                dataApp.RejectedOn = DateTime.Now;
                _repo.EditAsync(dataApp).ConfigureAwait(false);
            }
            else
            {
                //1. Cek Self Group...
                //--Cek and Insert Group Index...Gropuing By GroupIndexApproval...
                var vmListGroup = new List<VmGrouping>();
                foreach (var item in listConditional)
                {
                    var isready = false;
                    foreach (var g in vmListGroup)
                    {
                        if (item.GroupConditionIndex == g.GroupIndex) isready = true;
                    }
                    if (!isready)
                    {
                        vmListGroup.Add(new VmGrouping { GroupIndex = Convert.ToInt32(item.GroupConditionIndex) });
                    }
                }

                //update Table Conditional...
                var conData = listConditional.Where(c => c.EmployeeBasicInfoId == empId).FirstOrDefault();
                var dataCon = _repoManageConditionApproval.Get(conData.Id).Result;
                dataCon.StatusApproved = (StatusApproved)Enum.ToObject(typeof(StatusApproved), data.StatusApproved);
                dataCon.CreatedBy = empId;
                dataCon.CreatedOn = DateTime.Now;
                _repoManageConditionApproval.EditAsync(dataCon).ConfigureAwait(false);

                //...cek other Group ,satu satu apakah group conditional nya AND atau OR..
                var vmListTempGroup = new List<VmGroupingChekclist>();
                vmListTempGroup.Add(new VmGroupingChekclist { GroupIndex = Convert.ToInt32(conData.GroupConditionIndex), IsAllApproved = true });

                foreach (var item in vmListGroup)
                {
                    if (conData.GroupConditionIndex != item.GroupIndex) // yang di cek Group INdex nya yang berbeda dari yang nge-Prove..
                    {
                        var vmGI = vmListTempGroup.Where(c => c.GroupIndex == item.GroupIndex).FirstOrDefault();
                        if (vmGI == null)
                        {
                            //..cek group approval nya..
                            var dataCondisi = listConditional.Where(c => c.GroupConditionIndex == item.GroupIndex).FirstOrDefault();
                            if (dataCondisi.GroupConditionApproval == "OR")//jika Or..
                            {
                                var dt = listConditional.Where(c => c.GroupConditionIndex == item.GroupIndex && c.StatusApproved == StatusApproved.Approved).ToList();
                                if (dt.Count > 0)
                                    vmListTempGroup.Add(new VmGroupingChekclist { GroupIndex = item.GroupIndex, IsAllApproved = true });
                                else vmListTempGroup.Add(new VmGroupingChekclist { GroupIndex = item.GroupIndex, IsAllApproved = false });

                            }
                            else // JIka AND group conditional nya..
                            {
                                var dt = listConditional.Where(c => c.GroupConditionIndex == item.GroupIndex && c.StatusApproved != StatusApproved.Approved).ToList();
                                if (dt.Count > 0)
                                    vmListTempGroup.Add(new VmGroupingChekclist { GroupIndex = item.GroupIndex, IsAllApproved = false });
                                else vmListTempGroup.Add(new VmGroupingChekclist { GroupIndex = item.GroupIndex, IsAllApproved = true });

                            }
                        }
                    }
                }

                var result = vmListTempGroup.Where(c => c.IsAllApproved == false).ToList();
                if (result.Count == 0) // sudah approved semua groupnya..
                {
                    //update table User APproval...
                    var dataUserApp = _repoManageUserApproval.Get(conData.TrUserApprovalId).Result;
                    dataUserApp.CreatedBy = empId;
                    dataUserApp.CreatedOn = DateTime.Now;
                    dataUserApp.StatusApproved = StatusApproved.Approved;
                    _repoManageUserApproval.EditAsync(dataUserApp).ConfigureAwait(false);

                    //baca list user approval nya, masih ada atau nggak ada...
                    var dataUserConCurrentApproval = listConcurrentUser.Where(x => x.StatusApproved == StatusApproved.CurrentApproval || x.StatusApproved == StatusApproved.WaitingApproval).ToList();
                    if (dataUserConCurrentApproval.Count == 0)
                    {
                        var dataApp = _repo.Get(dataUserApp.TrTempApprovalId).Result;
                        dataApp.StatusApproved = StatusApproved.Approved;
                        dataApp.RejectedBy = empId;
                        dataApp.RejectedOn = DateTime.Now;
                        _repo.EditAsync(dataApp).ConfigureAwait(false);
                    }
                    else
                    {
                        var nextIndex = dataUserApp.IndexUser + 1;
                        _repoManageUserApproval.UpdateNextApproval(dataUserApp.TrTempApprovalId, Convert.ToInt32(nextIndex)).ConfigureAwait(false);
                        var dataConcurrent = _repoManageUserApproval.GetCurrentApproval(dataUserApp.TrTempApprovalId).Result;
                        if (dataConcurrent != null)
                            _repoManageConditionApproval.UpdateNextApproval(dataConcurrent.Id).ConfigureAwait(false);
                    }
                }

            }

        }

        #endregion

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

        public class VmListGroupCondition
        {
            public string Id { get; set; }
            public string EmpId { get; set; }
            public int IndexGroup { get; set; }
            public string GroupConditionalApproval { get; set; }
            public StatusApproved StatusApproved { get; set; }
        }

        public class VmGrouping
        {
            public int GroupIndex { get; set; }
        }
        public class VmGroupingChekclist
        {
            public int GroupIndex { get; set; }
            public bool IsAllApproved { get; set; }
        }

    }

}