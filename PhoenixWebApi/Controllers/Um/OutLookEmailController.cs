using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Graph;
using Newtonsoft.Json;
using Phoenix.ApiExtension;
using Phoenix.Data.Attributes;
using Phoenix.Data.MsGraphExtention;
using Phoenix.Data.Services.Um;

namespace PhoenixWebApi.Controllers.Core
{
    [Route("api/olmail")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = @"Um")]
    public class OutLookEmailController : Controller
    {
        private readonly IManageAzurePortalSetupService _repoAzure;
        public OutLookEmailController(IManageAzurePortalSetupService repoAzure)
        {
            _repoAzure = repoAzure;
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("getinbox")]
        public async Task<IActionResult> GetInbox()
        {
            var model = _repoAzure.GetOnlyOne().Result;
            var graph = new MsGraph(model.ClientId, model.TenantId);
            var authResult = await graph.GetAuthentication(model.EmailAccount, model.SecurityPword);
            var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);

            var inbox = await graphClient.Me.MailFolders.Inbox.Request().GetAsync();
            var unread = inbox.UnreadItemCount;
            var allmail = inbox.TotalItemCount;

            var sentResults = await graphClient.Me.MailFolders.SentItems.Request().GetAsync();
            var sentItemCount = sentResults.TotalItemCount;

            var mailInboxResults = await graphClient.Me.MailFolders.Inbox.Messages.Request()
                .Select(m => new { m.Sender, m.Subject, m.ReceivedDateTime, m.From, m.HasAttachments, m.IsRead })
                .OrderBy("receivedDateTime DESC")
                //.Top(Convert.ToInt32(allmail))
                .Top(25)
                .GetAsync();

            var mailSentResults = await graphClient.Me.MailFolders.SentItems.Messages.Request()
                .Select(m => new { m.Sender, m.Subject, m.SentDateTime, m.From, m.HasAttachments })
                //.Select(m => new { m.Sender, m.Subject, m.SentDateTime, m.From, m.Attachments})
                .OrderBy("sentDateTime DESC")
                .Top(Convert.ToInt32(sentItemCount))
                .GetAsync();

            var vmMail = new
            {
                cu = unread,
                am = allmail,
                listInbox = mailInboxResults,
                listSent = mailSentResults
            };
            return Ok(vmMail);
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetMailInboxContent/{id?}")]
        public async Task<IActionResult> GetMailInboxContent(string id)
        {
            var model = _repoAzure.GetOnlyOne().Result;
            var graph = new MsGraph(model.ClientId, model.TenantId);
            var authResult = await graph.GetAuthentication(model.EmailAccount, model.SecurityPword);
            var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);

            var inbox = await graphClient.Me.MailFolders.Inbox.Request().GetAsync();
            var countMail = Convert.ToInt32(inbox.TotalItemCount);

            var mailInboxResults = await graphClient.Me.MailFolders.Inbox.Messages.Request().Top(countMail).GetAsync();
            var mail = mailInboxResults.Where(x => x.Id.Equals(id)).First();
            return Ok(mail);
        }

        [AllowAnonymousRole]
        [HttpGet]
        [Route("GetMailSentContent/{id?}")]
        public async Task<IActionResult> GetMailSentContent(string id)
        {
            var model = _repoAzure.GetOnlyOne().Result;
            var graph = new MsGraph(model.ClientId, model.TenantId);
            var authResult = await graph.GetAuthentication(model.EmailAccount, model.SecurityPword);
            var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);

            var sentResults = await graphClient.Me.MailFolders.SentItems.Request().GetAsync();
            var countSentItem = Convert.ToInt32(sentResults.TotalItemCount);

            var mailSentResults = await graphClient.Me.MailFolders.SentItems.Messages.Request().Top(countSentItem).GetAsync();
            var mail = mailSentResults.Where(x => x.Id.Equals(id)).First();
            return Ok(mail);
        }

        [AllowAnonymousRole]
        [HttpPost]
        [Route("SentMail")]
        public async Task<IActionResult> SentMail([FromForm]VmSentEmail data)
        {
            var model = _repoAzure.GetOnlyOne().Result;
            var graph = new MsGraph(model.ClientId, model.TenantId);
            var authResult = await graph.GetAuthentication(model.EmailAccount, model.SecurityPword);
            var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);

            MessageAttachmentsCollectionPage attachments = new MessageAttachmentsCollectionPage();
            List<Recipient> recipients = new List<Recipient>();

            var listEmailTo = JsonConvert.DeserializeObject<List<EmailAddressTo>>(data.EmailAddressTos);
            foreach (var item in listEmailTo)
            {
                recipients.Add(new Recipient
                {
                    EmailAddress = new EmailAddress
                    {
                        Address = item.To
                    }
                });
            }

            if (data.Files != null)
            {
                foreach (var file in data.Files)
                {
                    var fileAttachment = file.OpenReadStream();
                    attachments.Add(new FileAttachment
                    {
                        ODataType = "#microsoft.graph.fileAttachment",
                        ContentBytes = ReadFully(fileAttachment),
                        ContentType = file.ContentType,
                        ContentId = Guid.NewGuid().ToString(),
                        Name = file.FileName
                    });
                }
            }

            Message email = new Message
            {
                Body = new ItemBody
                {
                    Content = data.ContentEmail,
                    ContentType = BodyType.Html
                },
                Subject = data.Subject,
                ToRecipients = recipients,
                Attachments = attachments
            };

            // Send the message.
            await graphClient.Me.SendMail(email, null).Request().PostAsync();
            return Ok(new { message = "success" });
        }
        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

    }

    public class VmSentEmail
    {
        public List<IFormFile> Files { get; set; }
        public string EmailAddressTos { get; set; }
        public string Subject { get; set; }
        public string ContentEmail { get; set; }
    }
    public class EmailAddressTo
    {
        public string To { get; set; }
    }

}