using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Phoenix.Data.MsGraphExtention;
using Phoenix.Data.Models.Um;
using Phoenix.ApiExtension.Helpers;
using Microsoft.Graph;

namespace Phoenix.Data
{
    public class SendEmailMsGraph : ISendEmailMsGraph
    {
        readonly DataContext context;
        /// <summary>
        /// And endpoint to manage MasterDivisionSelfservice
        /// </summary>
        /// <param name="context">Database context</param>
        public SendEmailMsGraph(DataContext context)
        {
            this.context = context;
        }
        public async Task SendEmail(VmEmailMsGraph vm)
        {
            List<Recipient> recipients = new List<Recipient>();
            foreach (var item in vm.ListEmailAddessTo)
            {
                recipients.Add(new Recipient
                {
                    EmailAddress = new EmailAddress
                    {
                        Address = item.To
                    }
                });
            }

            Message email = new Message
            {
                Body = new ItemBody
                {
                    Content = vm.ContentEmail,
                    ContentType = BodyType.Html,
                },
                Subject = vm.Subject,
                ToRecipients = recipients
            };

            // Send the message.
            var model = GetOnlyOne().Result;
            var graph = new MsGraph(model.ClientId, model.TenantId);
            var authResult = await graph.GetAuthentication(model.EmailAccount, model.SecurityPword);
            var graphClient = graph.CreateGraphServiceClient(authResult.AccessToken);
            await graphClient.Me.SendMail(email, false).Request().PostAsync();
        }
        private async Task<AzurePortalSetup> GetOnlyOne()
        {
            var data = await (from x in context.AzurePortalSetups.AsNoTracking().Where(x => !(x.IsDeleted ?? false))
                              select new AzurePortalSetup
                              {
                                  Id = x.Id,
                                  TenantId = x.TenantId,
                                  ClientId = x.ClientId,
                                  EmailAccount = x.EmailAccount,
                                  SecurityPword = SecurityHelper.DecryptString(x.SecurityPword)
                              }).Take(1).FirstOrDefaultAsync();
            return data;
        }

    }
}
