using Microsoft.AspNetCore.SignalR;
using Phoenix.Data.Models;
using Phoenix.Data.Services.Um;
using System;
using System.Threading.Tasks;
using Phoenix.Data.MsGraphExtention;
using Microsoft.Graph;
using System.Collections.Generic;
using Phoenix.Shared.Helpers;

namespace PhoenixWebApi.Services
{
    public interface INotificationCenter
    {
        Task AddNotification(Notification entity);
        Task AddNotification(string notificationType, string userId, string itemId, string moduleName);
        Task AddNotification(string notificationType, string userId, string moduleName);
        Task AddNotificationPM(string notificationType, string userId, string moduleName, string jobId);
    }
    public class NotificationCenter : INotificationCenter
    {
        private readonly INotificationService _repoNotif;
        private readonly INotificationTypeService _repoType;
        private readonly IHubContext<server.Hubs.NotificationHub> _hubContext;
        //private string office365Username = Phoenix.ApiExtension.DataConfiguration.Configuration.MsGraphSettings.office365Username;
        //private string office365Password = Phoenix.ApiExtension.DataConfiguration.Configuration.MsGraphSettings.office365Password;
        private readonly IManageAzurePortalSetupService _repoAzure;
        public NotificationCenter(INotificationService repoNotif, INotificationTypeService repoType, IHubContext<server.Hubs.NotificationHub> hubContext, IManageAzurePortalSetupService repoAzure)
        {
            _hubContext = hubContext;
            _repoNotif = repoNotif;
            _repoType = repoType;
            _repoAzure = repoAzure;
        }

        /// <summary>
        /// Function for Add Notification, and PLEASE DO NOT PARSE ID, CAUSE WE GENERATE THEM FOR INTERNAL PURPOSE...
        /// </summary>
        /// <param name="entity">Only : NotificationTypeId, UserId, ItemId (optional), ModuleName</param>
        /// <returns></returns>
        public async Task AddNotification(Notification entity)
        {
            var codex = Guid.NewGuid().ToString().ToUpper();
            entity.Id = codex;
            entity.IsSeen = false;
            entity.ItemId = entity.ItemId;
            await _repoNotif.AddAsync(entity);
            // get notif type....
            var data = await _repoType.Get(entity.NotificationTypeId);
            if (data != null)
            {
                // if need send email...start here....
                if (!string.IsNullOrEmpty(entity.ItemId))
                {
                    if (data.Href.Contains("{id}"))
                    {
                        data.Href = data.Href.Replace("{id}", entity.ItemId);
                    }
                }
                //var tipe = 1;
                //var templateHtml = "<a href='javascript:directToDetailNotif(\"" + codex + "\",\"" + data.Href + "\")' title='click here to see detail'><div><p>" + data.ShortText + "</p></div></a>";
                //if (entity.NotificationTypeId.Contains("approv")) tipe = 2;
                //await _hubContext.Clients.All.SendAsync("ReceiveNTFCenter", entity.UserId,templateHtml,tipe);
                var tipe = 1;
                var templateHtml = "<a href='javascript:directToDetailNotif(\"" + codex + "\",\"" + data.Href + "\")' title='click here to see detail'><div><p>" + data.ShortText + "</p></div></a>";

                if (entity.NotificationTypeId.Contains("approv"))
                {
                    tipe = 2;
                    var tipe2 = 1;
                    var _model = new Notification();
                    codex = Guid.NewGuid().ToString().ToUpper();
                    _model.Id = codex;
                    _model.NotificationTypeId = codex;
                    _model.UserId = Glosing.Instance.Username;
                    _model.IsSeen = false;
                    _model.Module = entity.Module;
                    _model.ItemId = entity.ItemId;
                    await _repoNotif.AddAsync(_model);

                    NotificationType _notifType = new NotificationType();
                    _notifType.Id = codex;
                    _notifType.IsActive = true;
                    _notifType.ShortText = "Waiting approval for : " + entity.NotificationTypeId;
                    _notifType.LongText = "Waiting approval for : " + entity.NotificationTypeId;
                    _notifType.Href = data.Href;
                    await _repoType.AddAsync(_notifType);

                    var templateHtml2 = "<a href='javascript:directToDetailNotif(\"" + codex + "\",\"" + data.Href + "\")' title='click here to see detail'><div><p>" + data.ShortText + "</p></div></a>";
                    await _hubContext.Clients.All.SendAsync("ReceiveNTFCenter", entity.UserId, templateHtml, tipe);
                    await _hubContext.Clients.All.SendAsync("ReceiveNTFCenter", Glosing.Instance.Username, templateHtml2, tipe2);
                }
                else
                {
                    await _hubContext.Clients.All.SendAsync("ReceiveNTFCenter", Glosing.Instance.Username, templateHtml, tipe);
                }
            }
        }

        /// <summary>
        /// Function untuk add notifikasi dengan item id...
        /// </summary>
        /// <param name="notificationType">Id Notification Type</param>
        /// <param name="userId">User App Id yang akan di berikan notifikasi nya, ini biasa nya berupa email</param>
        /// <param name="itemId">Boleh parsing string.empty jika tidak ada kepentingan ke sini</param>
        /// <param name="moduleName">Nama module (HRIS, PM, FInance, Media etc....)</param>
        /// <returns></returns>
        public async Task AddNotification(string notificationType, string userId, string itemId, string moduleName)
        {
            var model = new Notification();
            var codex = Guid.NewGuid().ToString().ToUpper();
            model.Id = codex;
            model.NotificationTypeId = notificationType;
            model.UserId = userId;
            model.ItemId = itemId;
            model.IsSeen = false;
            model.Module = moduleName;
            await _repoNotif.AddAsync(model);

            // Get data Notification Type...
            var data = await _repoType.Get(notificationType);
            if (data != null)
            {
                // if need send email...start here....
                if (!string.IsNullOrEmpty(itemId))
                {
                    if (data.Href.Contains("{id}"))
                    {
                        data.Href = data.Href.Replace("{id}", itemId);
                    }
                }
                //var tipe = 1;
                //if (notificationType.Contains("approv")) tipe = 2;
                //var templateHtml = "<a href='javascript:directToDetailNotif(\"" + codex + "\",\"" + data.Href + "\")' title='click here to see detail'><div><p>" + data.ShortText + "</p></div></a>";
                //await _hubContext.Clients.All.SendAsync("ReceiveNTFCenter", userId, templateHtml,tipe);

                var tipe = 1;
                var templateHtml = "<a href='javascript:directToDetailNotif(\"" + codex + "\",\"" + data.Href + "\")' title='click here to see detail'><div><p>" + data.ShortText + "</p></div></a>";

                if (notificationType.Contains("approv"))
                {
                    tipe = 2;
                    await _hubContext.Clients.All.SendAsync("ReceiveNTFCenter", userId, templateHtml, tipe);
                    var tipe2 = 1;
                    var _model = new Notification();
                    codex = Guid.NewGuid().ToString().ToUpper();
                    _model.Id = codex;
                    _model.NotificationTypeId = codex;
                    _model.UserId = userId;
                    _model.IsSeen = false;
                    _model.Module = moduleName;
                    _model.ItemId = itemId;
                    await _repoNotif.AddAsync(_model);

                    NotificationType _notifType = new NotificationType();
                    _notifType.Id = codex;
                    _notifType.IsActive = true;
                    _notifType.ShortText = "Success submit data = " + notificationType + " " + itemId;
                    _notifType.LongText = "Success submit data = " + notificationType + " " + itemId;
                    _notifType.Href = data.Href;
                    await _repoType.AddAsync(_notifType);

                    var templateHtml2 = "<a href='javascript:directToDetailNotif(\"" + codex + "\",\"" + data.Href + "\")' title='click here to see detail'><div><p>" + data.ShortText + "</p></div></a>";
                    await _hubContext.Clients.All.SendAsync("ReceiveNTFCenter", Glosing.Instance.Username, templateHtml2, tipe2);
                }
                else
                {
                    await _hubContext.Clients.All.SendAsync("ReceiveNTFCenter", Glosing.Instance.Username, templateHtml, tipe);
                }

            }

        }

        /// <summary>
        /// Function untuk add notifikasi tanpa item id...
        /// </summary>
        /// <param name="notificationType">Id Notification Type</param>
        /// <param name="userId">User App Id yang akan di berikan notifikasi nya, ini biasa nya berupa email</param>
        /// <param name="moduleName">Nama module (HRIS, PM, FInance, Media etc....)</param>
        /// <returns></returns>
        public async Task AddNotification(string notificationType, string userId, string moduleName)
        {
            try
            {
                var model = new Notification();
                var codex = Guid.NewGuid().ToString().ToUpper();
                model.Id = codex;
                model.NotificationTypeId = notificationType;
                model.UserId = userId;
                model.IsSeen = false;
                model.Module = moduleName;

                await _repoNotif.AddAsync(model);
                // Get data Notification Type...
                var data = await _repoType.Get(notificationType);
                if (data != null)
                {
                    // if need send email...start here....

                    var tipe = 1;
                    if (notificationType.Contains("approv")) tipe = 2;
                    var templateHtml = "<a href='javascript:directToDetailNotif(\"" + codex + "\",\"" + data.Href + "\")' title='click here to see detail'><div><p>" + data.ShortText + "</p></div></a>";
                    await _hubContext.Clients.All.SendAsync("ReceiveNTFCenter", userId, templateHtml, tipe);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("application getting error");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException.Message);
            }

        }

        public async Task AddNotificationPM(string notificationType, string userId, string moduleName, string jobId)
        {
            try
            {
                var model = new Notification();
                var codex = Guid.NewGuid().ToString().ToUpper();
                model.Id = codex;
                model.NotificationTypeId = notificationType;
                model.UserId = userId;
                model.IsSeen = true;
                model.Module = moduleName;
                model.ItemId = jobId;
                await _repoNotif.AddAsync(model);
                // Get data Notification Type...
                var data = await _repoType.Get(notificationType);
                if (data != null)
                {
                    // if need send email...start here....
                    if (!string.IsNullOrEmpty(jobId))
                    {
                        if (data.Href.Contains("{id}"))
                        {
                            data.Href = data.Href.Replace("{id}", jobId);
                        }
                    }
                    //var tipe = 1;
                    //if (notificationType.Contains("approv")) tipe = 2;
                    //var templateHtml = "<a href='javascript:directToDetailNotif(\"" + codex + "\",\"" + data.Href + "\")' title='click here to see detail'><div><p>" + data.ShortText + "</p></div></a>";
                    //await _hubContext.Clients.All.SendAsync("ReceiveNTFCenter", userId, templateHtml, tipe);

                    var tipe = 1;
                    var templateHtml = "<a href='javascript:directToDetailNotif(\"" + codex + "\",\"" + data.Href + "\")' title='click here to see detail'><div><p>" + data.ShortText + "</p></div></a>";

                    if (notificationType.Contains("approv"))
                    {
                        tipe = 2;
                        await _hubContext.Clients.All.SendAsync("ReceiveNTFCenter", userId, templateHtml, tipe);
                        var tipe2 = 1;
                        var _model = new Notification();
                        codex = Guid.NewGuid().ToString().ToUpper();
                        _model.Id = codex;
                        _model.NotificationTypeId = codex;
                        _model.UserId = userId;
                        _model.IsSeen = false;
                        _model.Module = moduleName;
                        _model.ItemId = jobId;
                        await _repoNotif.AddAsync(_model);

                        NotificationType _notifType = new NotificationType();
                        _notifType.Id = codex;
                        _notifType.IsActive = true;
                        _notifType.ShortText = "Success submit data = " + notificationType + " " + jobId;
                        _notifType.LongText = "Success submit data = " + notificationType + " " + jobId;
                        _notifType.Href = data.Href;
                        await _repoType.AddAsync(_notifType);

                        var templateHtml2 = "<a href='javascript:directToDetailNotif(\"" + codex + "\",\"" + data.Href + "\")' title='click here to see detail'><div><p>" + data.ShortText + "</p></div></a>";
                        await _hubContext.Clients.All.SendAsync("ReceiveNTFCenter", Glosing.Instance.Username, templateHtml2, tipe2);
                    }
                    else
                    {
                        await _hubContext.Clients.All.SendAsync("ReceiveNTFCenter", Glosing.Instance.Username, templateHtml, tipe);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("application getting error");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException.Message);
            }

        }

        public async Task SentEmail(string to)
        {
            var model = _repoAzure.GetOnlyOne().Result;
            var graph = new MsGraph(model.ClientId, model.TenantId);
            var authResult = await graph.GetAuthentication(model.EmailAccount, model.SecurityPword);

            var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);
            MessageAttachmentsCollectionPage attachments = new MessageAttachmentsCollectionPage();
            List<Recipient> recipients = new List<Recipient>();
            recipients.Add(new Recipient
            {
                EmailAddress = new EmailAddress
                {
                    Address = to
                }
            });
            Message email = new Message
            {
                Body = new ItemBody
                {
                    Content = TemplateEmail(),
                    ContentType = BodyType.Html,
                },
                Subject = "NOTIFICATION CENTER PHOENIX ERP APPLICATION",
                ToRecipients = recipients
            };
            await graphClient.Me.SendMail(email, false).Request().PostAsync();
        }

        public string TemplateEmail()
        {
            return @"<p>Dear </p>
                    <p></p>";
        }

    }
}

