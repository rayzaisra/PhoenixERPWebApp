using Microsoft.AspNetCore.SignalR;
using Microsoft.Graph;
using Phoenix.Data.MsGraphExtention;
using Phoenix.Shared.Core.Areas.General.Dtos;
using Phoenix.Shared.Core.Areas.General.Services;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Repositories;
using server.Hubs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Phoenix.Shared.Core.Areas.General.Helpers
{
    public class Notify
    {
        protected IHubContext<NotificationHub> _hubContext;
        IEFRepository<Notification, string> _repo;
        IEFRepository<TmEmailTemplate, string> _repoTemplateEmail;
        IEFRepository<NotificationType, string> _repoType;
        IEFRepository<TmUserApp, string> _repoUserApp;
        private readonly Phoenix.Data.Services.Um.IManageAzurePortalSetupService _repoAzure;
        protected NotificationHub _notif = new NotificationHub();

        private string noticode = "ntfks_";

        public Notify(IHubContext<NotificationHub> hubContext,
            IEFRepository<Notification, string> repo, IEFRepository<NotificationType, string> repoType, IEFRepository<TmEmailTemplate, string> repoTemplateEmail, IEFRepository<TmUserApp, string> repoUserApp, Phoenix.Data.Services.Um.IManageAzurePortalSetupService repoAzure)
        {
            _hubContext = hubContext;
            _repo = repo;
            _repoType = repoType;
            _repoTemplateEmail = repoTemplateEmail;
            _repoUserApp = repoUserApp;
            _repoAzure = repoAzure;

        }
        public void user(string user_id, string item_id, string msg)
        {
            Notification data = new Notification();
            data.user_id = user_id;
            data.notification_type_id = msg;
            data.item_id = item_id;
            _repo.Create(data);
            _hubContext.Clients.Group(noticode + user_id).SendAsync("ReceiveMessage", msg);
        }


        public void createNotification(string notifUser, string notifItemId, string notifModule, string notifTypeModule, string notifItemName)
        {
            Notification _notification = new Notification();
            _notification.Id = Guid.NewGuid().ToString().ToUpper();
            _notification.notification_type_id = Guid.NewGuid().ToString().ToUpper();
            _notification.user_id = notifUser;
            _notification.item_id = notifItemId;
            _notification.module = notifModule;
            _notification.is_active = true;
            _repo.Create(_notification);

            //var _notifType = _repoType.FindByID(_notification.notification_type_id);
            //if (_notifType == null) {
            NotificationType _notifType = new NotificationType();
            _notifType.Id = _notification.notification_type_id;
            _notifType.is_active = true;
            _notifType.shorttext = shortextProjectManagementNotification(notifTypeModule, notifItemName);
            _notifType.longtext = longtextProjectManagementNotification(notifTypeModule);
            _notifType.href = urlProjectManagementNotification(notifTypeModule, notifItemId);
            _repoType.Create(_notifType);

            /*_hubContext.Clients.Group(noticode + notifUser).SendAsync("ReceiveMessage", _notifType.href);*/
            _hubContext.Clients.Group(noticode + notifUser).SendAsync("ReceiveNTFCenter", _notifType.href);
            sendEmail(notifUser, _notifType.shorttext, _notifType.longtext);
        }

        public async void sendEmail(string notifUser, string shortText, string longText)
        {
            var emailTemplate = _repoTemplateEmail.FindByID("79ccc4c4-4d4b-41af-b90e-dfe8a806e583");
            var user = _repoUserApp.FindByID(notifUser);
            var aliasName = user.AliasName;

            emailTemplate.Subject = emailTemplate.Subject.Replace("@Subject", shortText);
            emailTemplate.Body = emailTemplate.Body.Replace("@JobMessage", longText);
            emailTemplate.Body = emailTemplate.Body.Replace("@User", aliasName);
            var plainText = HTMLToText(emailTemplate.Body);
            plainText = plainText.Replace("\n", "<br>");

            var model = _repoAzure.GetOnlyOne().Result;
            var graph = new MsGraph(model.ClientId, model.TenantId);
            var authResult = await graph.GetAuthentication(model.EmailAccount, model.SecurityPword);
            var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);

            List<Recipient> recipients = new List<Recipient>();
            recipients.Add(new Recipient
            {
                EmailAddress = new EmailAddress
                {
                    Address = notifUser.Trim()
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
            try
            {
                await graphClient.Me.SendMail(email, null).Request().PostAsync();
            }
            catch 
            {
                try 
	{
                    await graphClient.Me.SendMail(email, null).Request().PostAsync();
                }
	catch 
	{
	}
     
            }
          
        }

        public string urlProjectManagementNotification(string type, string id)
        {
            string ret = "";
            switch (type)
            {
                case "jobdetail":
                    ret = String.Format("/projectmanagement/job/detail/{0}", id);
                    break;
                case "jobassignmember":
                    ret = String.Format("/projectmanagement/job/detail/{0}", id);
                    break;
                case "kanban":
                    ret = String.Format("/projectmanagement/job/kanban/{0}", id);
                    break;
                case "task":
                    ret = String.Format("/projectmanagement/job/task/{0}", id);
                    break;
                default: break;

            }
            return ret;
        }

        public string shortextProjectManagementNotification(string type, string ItemName)
        {
            string ret = "";
            switch (type)
            {
                case "jobdetail":
                    ret = "Job Created - "+ ItemName  + " has been created " ;
                    break;
                case "jobassignmember":
                    ret = "Assign PM & Member - " + ItemName;
                    break;
                case "kanban":
                    ret = "Approved All Task - " + ItemName;
                    break;
                case "task":
                    ret = ItemName;
                    break;
                default: break;

            }
            return ret;
        }

        public string longtextProjectManagementNotification(string type)
        {
            string ret = "";
            switch (type)
            {
                case "jobdetail":
                    ret = "New Job has been assign to you";
                    break;
                case "jobassignmember":
                    ret = "Job has been assign to PM";
                    break;
                case "kanban":
                    ret = "Task already approved all";
                    break;
                case "task":
                    ret = "New Task has been assign to you";
                    break;
                default: break;

            }
            return ret;
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

    }


}
