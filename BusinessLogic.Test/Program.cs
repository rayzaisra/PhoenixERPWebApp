using System;
using BusinessLogic.Test.Core;
using CodeMarvel.Infrastructure.Logger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using CommonTool;
using DataAccess;
using DataAccess.PhoenixERP.General;
using DataAccess.PhoenixERP;
using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace BusinessLogic.Test
{
    class Program
    {

        static void Main(string[] args)
        {
            //Console.WriteLine("Start business logic test ...");
            //var appLogger = ApplicationLogging.GetCurrentClassLogger<Program>();
            //appLogger.LogInformation("Start business logic test ...");
            //var test = new BusinessLogic.TEST();
            
            //var auth = new BusinessLogic.Test.Core.Authentication(runner._logger);
            //auth.Run();

            //Load Application User

            int New = 0, Update = 0, Fail = 0;

            var cs = "Data Source=localhost;Initial Catalog=PhoenixDBUltimate;persist security info=True;user id=sa;password=P@ssw0rd123#;MultipleActiveResultSets=True;ConnectRetryCount=3;ConnectRetryInterval=3;Connection Timeout=9";
            //var cs = "Data Source=139.255.87.235,49172;Initial Catalog=PhoenixDBUltimate;persist security info=True;user id=dev-user;password=d3v-u53r;MultipleActiveResultSets=True;ConnectRetryCount=3;ConnectRetryInterval=3;Connection Timeout=9";
            using (var db = new PhoenixERPRepo(cs))
            {
                using (var scope = db.GetTransaction())
                {
                    Console.WriteLine(cs);

                    try
                    {
                        TextInfo ti = CultureInfo.CurrentCulture.TextInfo;

                        var qry = @" SELECT TABLE_CATALOG,TABLE_SCHEMA,TABLE_NAME,TABLE_TYPE
		                    FROM  INFORMATION_SCHEMA.TABLES
		                    WHERE TABLE_TYPE='BASE TABLE'
		                    ORDER BY TABLE_NAME ";

                        var data = db.Fetch<dynamic>(qry);
                        Console.WriteLine(db.LastCommand);

                        foreach (var i in data)
                        {
                            try
                            {
                                var isNew = false;
                                var id = GUIDHash.ConvertToMd5HashGUID(
                                    i.TABLE_SCHEMA + "." + i.TABLE_NAME).ToString();
                                application_entity record = 
                                    db.FirstOrDefault<application_entity>("WHERE id = @0", id);
                                Console.WriteLine(db.LastCommand);

                                if (record == null)
                                {
                                    isNew = true;
                                    record = new application_entity
                                    {
                                        id = id,
                                        created_by = "189f3ca3-83b0-41aa-8af6-9c0a346d087c",
                                        created_on = DateTime.Now,
                                        is_active = true,
                                        is_default = true,
                                        is_locked = true,
                                        owner_id = "189f3ca3-83b0-41aa-8af6-9c0a346d087c",
                                        organization_id = "9EE4834225E334380667DDCF2F60F6DA"
                                    };
                                }

                                record.entity_name = i.TABLE_SCHEMA + "." + i.TABLE_NAME;
                                record.display_name = ti.ToTitleCase(((string)i.TABLE_NAME).Replace("_", " "));

                                if (isNew)
                                {
                                    db.Insert(record);
                                    Console.WriteLine(db.LastCommand);
                                    New++;
                                }
                                else
                                {
                                    record.modified_by = record.created_by;
                                    record.modified_on = DateTime.Now;
                                    db.Update(record);
                                    Console.WriteLine(db.LastCommand);
                                    Update++;
                                }
                            }
                            catch (Exception ex)
                            {
                                Fail++;
                                Console.WriteLine(ex.Message);
                                continue;
                            }
                        }

                        scope.Complete();
                        Console.WriteLine("Failed = {0}", Fail);
                        Console.WriteLine("Done");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                }
            }

            Console.WriteLine("End of business logic test.");
            Console.Read();
        }


        private static IServiceProvider BuildDi()
        {
            var services = new ServiceCollection();

            //Runner is the custom class
            services.AddTransient<CodeMarvel.Infrastructure.Logger.Runner>();

            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddLogging((builder) => builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace));

            var serviceProvider = services.BuildServiceProvider();

            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            ApplicationLogging.ConfigureLogger(loggerFactory);
            ApplicationLogging.LoggerFactory = loggerFactory;
            ////configure NLog
            //loggerFactory.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
            //NLog.LogManager.LoadConfiguration("nlog.config");




            return serviceProvider;
        }

    }
}
