using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using AppOps;
using Microsoft.AspNetCore;
using Serilog;
using Serilog.Exceptions;

namespace Dotnet.Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return ConfigureWebHostBuilder(WebHost.CreateDefaultBuilder(args));
        }

        public static IWebHostBuilder ConfigureWebHostBuilder(IWebHostBuilder builder) =>
                    builder
                        .UseStartup<Startup>()
                        .AssignAppInfo((appInfo) =>
                        {
                            appInfo.GitCommitId = ThisAssembly.GitCommitId;
                            appInfo.Name = ThisAssembly.AssemblyName;
                        })
                        .UseSerilog((hostingContext, loggerConfiguration) =>
                        {
                            loggerConfiguration
                                .Enrich.FromLogContext()
                                .Enrich.WithProperty("Commit", ThisAssembly.GitCommitId)
                                .Enrich.WithExceptionDetails()
                                .ReadFrom.Configuration(hostingContext.Configuration);
                        });
    }
}
