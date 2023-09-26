using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace PhoenixWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {

            CreateWebHostBuilder(args).Build().Run();
            //CreateHostBuilder(args).Build().Run();
        }


        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                   
                })

                // Do not use Nlog DI 
                // If you want use Nlog Please config Startup.cs "ApplicationLogging"
                //ApplicationLogging.ConfigureLogger(loggerFactory);
                //ApplicationLogging.LoggerFactory = loggerFactory;
                //Finaly you will be Enable This Code
                //.ConfigureLogging((hostingContext, logging) =>
                //{
                //    logging.ClearProviders();
                //    //logging.AddConfiguration(hostingContet.Configuration.GetSection("Logging"));
                //    //logging.AddConsole();
                //    //logging.AddDebug();
                //})
                //.UseNLog() // Dependency Injection

                .UseStartup<Startup>();

       

    }
}
