using CodeMarvel.Infrastructure;
using CodeMarvel.Infrastructure.Options;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace PhoenixERPWebApp.Helper
{
    public static class GenerateMenu
    {
        public static NLog.Logger appLogger = NLog.LogManager.GetCurrentClassLogger();


        public static String BuildMenu(this IHtmlHelper helper, String Module ,object htmlAttributes = null)
        {
            using (var _client = new HttpClient())
            {
                try
                {
                    var context = StaticHttpContextAccessor.Current;
                    var user = CodeMarvel.Infrastructure.Sessions.SessionManager.UserSession;
                    var appSetting = context.RequestServices.GetService<IOptions<AppSettingsOption>>().Value;
                    //return user.Token;
                    _client.BaseAddress = new Uri(appSetting.ApiServerUrl);
                    _client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", user.Token));
                    var task = _client.GetAsync("core/Menu/BuildMenu").Result;
                    
                    if (task.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var response = task.Content.ReadAsAsync<dynamic>().Result;

                        if (((bool)((JValue)response.status.success).Value))
                        {
                            return response.data;
                        }

                    }
                }
                catch (Exception ex)
                {

                    appLogger.Error(ex);
                    return ex.Message;
                }
            }


            return "";
        }

    }
}
