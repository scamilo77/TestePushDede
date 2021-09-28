using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TestAPI
{
    public class Program
    {
        public static string PathToContentRoot { get; set; }
        public static IConfigurationRoot Configuration { get; set; }
        public static void Main(string[] args)
        {
            var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
            PathToContentRoot = Path.GetDirectoryName(pathToExe);
            CreateHostBuilder(args).Start();
        }

        public static IHost CreateHostBuilder(string[] args)
        {
            var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";

            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>()
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .ConfigureAppConfiguration((hosting, config) =>
                        {
                            var environment = hosting.HostingEnvironment;

                            config
                                .AddEnvironmentVariables()
                                .SetBasePath(environment.ContentRootPath)
                                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
                        })
                        .UseUrls($"http://0.0.0.0:{port}")
                        .UseKestrel()
                        .ConfigureLogging(logging =>
                        {
                            logging.ClearProviders();
                            logging.AddDebug();
                            logging.AddConsole();
                        });
                })
                .UseWindowsService()
                .Build();
        }
    }
}
