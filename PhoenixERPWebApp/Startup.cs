using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CodeMarvel.Infrastructure;
using CodeMarvel.Infrastructure.Logger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using CodeMarvel.Infrastructure.Options;
using PhoenixERPWebApp.Handlers;
using DataTables.AspNet.AspNetCore;
using Newtonsoft.Json;
using NLog;
using Phoenix.WebExtension.Attributes;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Http.Features;

namespace PhoenixERPWebApp
{
    public class Startup
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public Startup(IConfiguration configuration)
        {
            try
            {
                Configuration = configuration;
            }
            catch (Exception ex)
            {
                appLogger.Error(ex.ToString());
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //try
            //{
            services.AddHttpContextAccessorMarvel();

            //RAMZI
            services.AddMemoryCache();

            services.AddSingleton<IConfiguration>(Configuration);

            // Add our Config object so it can be injected
            services.Configure<AppSettingsOption>(Configuration.GetSection("AppConfiguration"));
            services.Configure<ConnectionStringOption>(Configuration.GetSection("ConnectionStrings"));

            services.AddOptions();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //services.RegisterDataTables(ctx =>
            //{
            //    var appJson = ctx.ValueProvider.GetValue("additionalParameters").FirstValue ?? "{}";
            //    return JsonConvert.DeserializeObject<IDictionary<string, object>>(appJson);
            //}, true);

            //RAMZI
            services.AddHttpClient<Phoenix.WebExtension.RestApi.ApiClientFactory>();
            services.AddScoped<ExceptionResponse>();
            services.AddScoped(typeof(MenuAuthorization));
            services.AddScoped(typeof(MenuViewComponent)); //perubahan code view component jadi injection. karena ada problem ketika di publish ke azzure.

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(CodeMarvel.Infrastructure.WebNetCore.ActionFilterBase));

                ////RAMZI
                //options.Filters.Add(typeof(Phoenix.WebExtension.Attributes.TransactionLogAttribute));
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddSessionStateTempDataProvider();
            services.AddMvc().AddRazorPagesOptions(o =>
            {
                o.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
            });

            ///m.salih
            ///configure this code for getting ipclient from x-header-forwarder
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services.Configure<FormOptions>(options =>
            {
                options.ValueCountLimit = int.MaxValue;
            });

            services.AddSession();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(5);
            });
            services.AddRouting();
            services.AddKendo();
            //}
            //catch (Exception ex)
            //{
            //    appLogger.Error(ex.ToString());
            //}
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)//, ILoggerFactory loggerFactory)
        {
            app.UseForwardedHeaders();
            //if (env.IsDevelopment())
            //{
            //     app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //    app.UseHsts();
            //}
            app.UseExceptionMiddleware();
            app.UseStaticHttpContextMarvel();
            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            //KETIKA DI PUBLISH ERROR
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), @"Areas")),
                RequestPath = new PathString("/Areas")
            });
            app.UseCookiePolicy();

            //NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").Configuration.Reload();
            //ApplicationLogging.ConfigureLogger(loggerFactory);
            //ApplicationLogging.LoggerFactory = loggerFactory;

            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();

            app.Use((context, next) =>
            {
                context.Request.Headers["Server-Type"] = AppConstants.ServerTypeWeb;
                return next.Invoke();
            });

            //app.UseStatusCodePages(async context =>
            //{

            //    await context.HttpContext.Response.WriteAsync(
            //        "Status code page, status code: " +
            //        context.HttpContext.Response.StatusCode);
            //});
            //app.UseExceptionHandler(()=>
            //{
            //    ErrorEventHandler.
            //});

            app.MapWhen(context => context.Request.Path.ToString().EndsWith(".ImageProfile"),
                 appBuilder =>
                 {
                     appBuilder.UseSession();
                     appBuilder.UseCookiePolicy();
                     appBuilder.UseImageProfileHandlerMiddleware();
                 });
            app.MapWhen(context => context.Request.Path.ToString().EndsWith(".stimulsoft"),
             appBuilder =>
             {
                 appBuilder.UseSession();
                 appBuilder.UseCookiePolicy();
                 appBuilder.UseReportsHandlerMiddleware();
             });

            app.UseSession();
            app.UseMvc(routes =>
            {
                //app.UseMvc(routes =>
                //{
                //    routes.MapRoute(
                //      name: "areas",
                //      template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                //    );
                //});

                routes.MapRoute(
                        name: "area",
                        template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


                routes.MapAreaRoute(
                    name: "default",
                    areaName: "Core",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "errorcustom",
                    template: "{controller=Error}/{action=Http}/{id?}");
            });
        }
    }
}
