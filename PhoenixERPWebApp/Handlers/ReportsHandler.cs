using CodeMarvel.Infrastructure;
using CodeMarvel.Infrastructure.Options;
using CodeMarvel.Infrastructure.Sessions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Handlers
{
    public class ReportsHandler
    {
        private readonly RequestDelegate _next;
        Logger appLogger = LogManager.GetCurrentClassLogger();
        private AppSettingsOption appSetting;

        public ReportsHandler(RequestDelegate next, IOptions<AppSettingsOption> AppSetting)
        {
            _next = next;
            appSetting = AppSetting.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            var defaultReportPath = Directory.GetCurrentDirectory();
            var reportFile = context.Request.Query["fileName"].ToString();
            defaultReportPath = Path.Combine(defaultReportPath, "wwwroot", "Reports", reportFile);
            appLogger.Debug("Stimul Handler");
            appLogger.Debug(defaultReportPath);
            if (File.Exists(defaultReportPath))
            {
                try
                {
                    var result = Encoding.UTF8.GetBytes("asdasdas");
                    var mrt = File.ReadAllBytes(defaultReportPath);
                    System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
                    {
                        FileName = reportFile,
                        Inline = true
                    };
                    context.Response.Headers.Add("Content-Disposition", cd.ToString());
                    await context.Response.Body.WriteAsync(mrt);
                    await context.Response.Body.FlushAsync();
                }
                catch (Exception ex)
                {
                    appLogger.Error(ex);
                }
            }

            //var userSession = SessionManager.UserSession;
            //if (userSession != null)
            //    using (var _client = new HttpClient())
            //    {
            //        try
            //        {
            //            _client.BaseAddress = new Uri(appSetting.ApiServerUrl);
            //            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", userSession.Token);
            //            var recordID = context.Request.Query["id"].ToString();
            //            var requestUri = string.Format("core/User/{0}/imageprofile?thumbnail=true", recordID);
            //            byte[] task = _client.GetByteArrayAsync(requestUri).Result;
            //            if (task != null)
            //            {
            //                context.Response.ContentType = "image/jpeg";

            //                await context.Response.Body.WriteAsync(task);
            //                await context.Response.Body.FlushAsync();
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            appLogger.Error(ex);

            //        }
            //    }
        }
    }

    public static class ReportsHandlerMiddleware
    {
        public static IApplicationBuilder UseReportsHandlerMiddleware
                                     (this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ReportsHandler>();
        }
    }

}
