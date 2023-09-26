using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMarvel.Infrastructure.Logger;
using DataTables.AspNet.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PhoenixWebApi.ModelBinder;
using Swashbuckle.AspNetCore.Swagger;
using CodeMarvel.Infrastructure.Acl;
using CodeMarvel.Infrastructure.Options;
using CodeMarvel.Infrastructure;
using Microsoft.AspNetCore.Http;
using PhoenixWebApi.Areas.Finance;
using Phoenix.Shared.Core.Areas.General.Api;
using Phoenix.Shared.Core.Areas.Hris.Api;
using Phoenix.Shared.Core.Areas.ProjectManagement.Api;
using DalSoft.Hosting.BackgroundQueue.DependencyInjection;
using BusinessLogic;
using AutoMapper;
using System.Reflection;
using Phoenix.Shared.Libraries.SnowFlake;
using Phoenix.Shared.Core.Contexts;
using Phoenix.Shared.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Phoenix.Shared.Core.Areas.General.Dtos;
using server.Hubs;
using Newtonsoft.Json;
using Phoenix.Data.ContextAccessor;
using Microsoft.AspNetCore.HttpOverrides;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Microsoft.Extensions.Hosting;
using PhoenixWebApi.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc.Cors.Internal;

namespace PhoenixWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddHttpContextAccessorMarvel();
            services.AddOptions();
            // Add our Config object so it can be injected
            //services.Configure<ConnectionStringOption>(Configuration.GetSection("ConnectionStrings"));
            services.AddSingleton<ConnectionStringOption>(options => new ConnectionStringOption
            {
                baseConnectionString = Configuration.GetConnectionString("baseConnectionString"),
            });
            BusinessLogic.ConnectionString.PhoenixConnectionStringSetup(Configuration.GetConnectionString("baseConnectionString"));
            //services.AddBackgroundQueue(ct =>
            //    DataContext.SetupOrganization("Marvel Dynamic", "admin_codemarvel", "admin"));

            //services.AddAutoMapper(Assembly.GetAssembly(typeof(BusinessLogic.AutoMapEntity.MappingEntityProfile)));
            services.AddAutoMapper();
           
            ///m.salih
            ///configure this code for getting ipclient from x-header-forwarder
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
            //signalR---------------
            services.AddSignalR();
            //
            //Allow Cors  
            services.AddCors(options =>
                {
                    options.AddPolicy("AllowAll",
                            builder =>
                            {
                                builder
                                //.WithOrigins("https://staging-phoenix.azurewebsites.net/", "https://phoenixwebapps.azurewebsites.net/")
                                .AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowCredentials();
                            });
                    options.AddPolicy("AllowAllOrigins",
                            builder =>
                            {
                                builder.AllowAnyOrigin();
                            });
                    options.AddPolicy("AllowAllHeaders",
                            builder =>
                            {
                                builder
                                       //.WithOrigins("http://example.com")
                                       .AllowAnyHeader();
                            });
                    options.AddPolicy("AllowCredentials",
                            builder =>
                            {
                                builder
                                       //.WithOrigins("http://example.com")
                                       .AllowCredentials();
                            });
                });

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("AllowAll"));
                options.Filters.Add<Phoenix.ApiExtension.Attributes.ExceptionHandlerAttribute>();
                options.Filters.Add<Phoenix.ApiExtension.Attributes.ModelStateHandlerAttribute>();
                //options.Filters.Add<Phoenix.Data.Attributes.AuthorizeRoleAttribute>();
            });

            services.AddMvc(options =>
            {
                //options.ModelBinderProviders.Add(new DataTablesModelBinderProviders());
                //options.ModelBinderProviders.Add(new EntityBinderProviders());

                options.ModelBinderProviders.Insert(0, new DataTablesModelBinderProviders());
                //options.ModelBinderProviders.Insert(0, new EntityBinderProviders());

            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

           
            //Register the Swagger generator, defining 1 or more Swagger documents
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new Info { Title = "Phoenix Web API", Version = "v1" });
            //    c.DocInclusionPredicate((_, api) => !string.IsNullOrWhiteSpace(api.GroupName));
            //    c.TagActionsBy(api => api.GroupName);
            //});

            //Swagger Group
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("Core", new Info
                {
                    Title = "CORE API",
                    Version = "v1",
                    Description = "Core",
                    TermsOfService = "None"
                });

                c.SwaggerDoc("Finance", new Info
                {
                    Title = "Finance API",
                    Version = "v1",
                    Description = "Finance",
                    TermsOfService = "None"
                });

                c.SwaggerDoc("Finance-Report", new Info
                {
                    Title = "Finance-Report API",
                    Version = "v1",
                    Description = "Finance-Report",
                    TermsOfService = "None"
                });

                c.SwaggerDoc("Approval-Finance", new Info
                {
                    Title = "Approvals-Finance API",
                    Version = "v1",
                    Description = "Approval-Finance",
                    TermsOfService = "None"
                });

                c.SwaggerDoc("General", new Info
                {
                    Title = "General API",
                    Version = "v1",
                    Description = "General",
                    TermsOfService = "None"
                });
                c.SwaggerDoc("ProjectManagement", new Info
                {
                    Title = "Project Management API",
                    Version = "v1",
                    Description = "Project Management",
                    TermsOfService = "None"
                });
                c.SwaggerDoc("Hris", new Info
                {
                    Title = "Hris API",
                    Version = "v1",
                    Description = "Hris",
                    TermsOfService = "None"
                });
                c.SwaggerDoc("Um", new Info
                {
                    Title = "User Management API",
                    Version = "v1",
                    Description = "User Management",
                    TermsOfService = "None"
                });
                c.SwaggerDoc("v1", new Info { Title = "Phoenix Web API", Version = "v1" });
                c.DocInclusionPredicate((_, api) => !string.IsNullOrWhiteSpace(api.GroupName));
                c.TagActionsBy(api => api.GroupName);
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
            });


            // DataTables.AspNet registration with default options.
            //services.RegisterDataTables(new DataTableModelBinder());
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<JwtAuthenticationAttribute>();
            services.UseServiceFinance();
            //-----------GN,PM.HR added----------------
            services.Configure<MsGraphConfig>(Configuration.GetSection("MsGraphConfig"));
            services.Configure<FileConfig>(Configuration.GetSection("FileConfig"));
            services.Configure<AppSettingsOption>(Configuration.GetSection("AppConfiguration"));
       
            services.Configure<Phoenix.Shared.Core.Areas.General.Dtos.Config.MsGraphSetting>(Configuration.GetSection("MsGraphSettings"));
            services.AddDbContext<EFContext>(options =>
                    //options.UseSqlServer(Configuration.GetConnectionString("AppDB"), ServiceLifetime.Transient)
                    //options.UseSqlServer(Configuration.GetConnectionString("AppDB")), ServiceLifetime.Scoped);
                    options.UseSqlServer(Configuration.GetConnectionString("baseConnectionString"),
                    op => op.EnableRetryOnFailure()));
            //           );

            services.AddDbContext<Phoenix.Data.DataContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("baseConnectionString"), op => op.EnableRetryOnFailure());
            });

            services.RegisterDataTables(ctx =>
                {
                    var appJson = ctx.ValueProvider.GetValue("appJson").FirstValue ?? "{}";
                    return JsonConvert.DeserializeObject<IDictionary<string, object>>(appJson);
                }, true);



            // services job
            services.AddSingleton<IHostedService, Services.JobScheduleServices>();

            services.AddSingleton<IdWorker, IdWorker>();
            services.AddSingleton<Id64Generator, Id64Generator>();
            services.AddScoped<EFContext, Phoenix.Shared.Core.Entities.AppDbContext>();

            ConfigServicesGeneral.Init(services);
            ConfigServicesHris.Init(services);
            ConfigServicesProjectManagement.Init(services);
            services.AddScoped(typeof(IEFRepository<,>), typeof(EFRepository<,>));
            services.AddScoped<INotificationCenter, NotificationCenter>();

            //--------------------------------------------
            //Registering DI services....
            services.RegisterPhoenixDependencies();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseCors("AllowAll");
            app.UseForwardedHeaders();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseStaticHttpContextMarvel();
            app.UseDeveloperExceptionPage();
           
            app.UseSignalR((options) =>
            {
                options.MapHub<NotificationHub>("/api/Hubs/Notification");
            });




            NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").Configuration.Reload();
            app.Use((context, next) =>
            {
                context.Request.Headers["Server-Type"] = AppConstants.ServerTypeAPI;
                var hubContext = context.RequestServices
                            .GetRequiredService<IHubContext<NotificationHub>>();
                return next.Invoke();
            });

            //app.UseHttpsRedirection();
            //app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            //app.UseHttpsRedirection();


            //app.UseSwagger(c => { c.PreSerializeFilters.Add((swagger, httpReq) => swagger.Host = httpReq.Host.Value); });
            //app.UseStaticFiles();
            app.UseSwagger();
            //app.UseStaticFiles();
            app.UseSwaggerUI(c =>
            {
                // c.InjectOnCompleteJavaScript("../swagger-bearer-auth.js");
                c.SwaggerEndpoint("/swagger/Core/swagger.json", "Core");
                c.SwaggerEndpoint("/swagger/Finance/swagger.json", "Finance");
                c.SwaggerEndpoint("/swagger/Finance-Report/swagger.json", "Finance-Report");
                c.SwaggerEndpoint("/swagger/General/swagger.json", "General");
                c.SwaggerEndpoint("/swagger/ProjectManagement/swagger.json", "ProjectManagement");
                c.SwaggerEndpoint("/swagger/Hris/swagger.json", "Hris");


                c.SwaggerEndpoint("/swagger/Approval-Finance/swagger.json", "Approval-Finance");
                //c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
            });
            //ApplicationLogging.ConfigureLogger(loggerFactory);
            //ApplicationLogging.LoggerFactory = loggerFactory;
            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}