using Bhbk.Lib.Hosting.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Diagnostics;
using System.IO;

namespace Bhbk.WebApi.Sample
{
    public class Program
    {
        public static IHostBuilder CreateIISHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(opt =>
            {
                opt.CaptureStartupErrors(true);
                opt.ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                });
                opt.UseIISIntegration();
                opt.UseStartup<Startup>();
            });

        public static IHostBuilder CreateKestrelHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(opt =>
            {
                opt.CaptureStartupErrors(true);
                opt.ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                });
                opt.UseKestrel(options =>
                {
                    options.ConfigureEndpoints();
                });
                opt.UseUrls();
                opt.UseStartup<Startup>();
            });

        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .WriteTo.File(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "appdebug.log", retainedFileCountLimit: 7)
                .CreateLogger();

            var process = Process.GetCurrentProcess();

            if (process.ProcessName.ToLower().Contains("iis"))
                CreateIISHostBuilder(args).Build().Run();

            else
                CreateKestrelHostBuilder(args).Build().Run();
        }
    }
}
