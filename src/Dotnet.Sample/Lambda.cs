using System;
using System.Threading.Tasks;
using Amazon.Lambda.ApplicationLoadBalancerEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.Json;
using Microsoft.AspNetCore.Hosting;

namespace Dotnet.Sample
{
    public class Lambda : Amazon.Lambda.AspNetCoreServer.ApplicationLoadBalancerFunction<Startup>
    {
        private static async Task Main(string[] args)
        {
            Func<ApplicationLoadBalancerRequest, ILambdaContext, Task<ApplicationLoadBalancerResponse>> func =
                new Lambda().FunctionHandlerAsync;
            using (var handlerWrapper = HandlerWrapper.GetHandlerWrapper(func, new JsonSerializer()))
            using (var bootstrap = new LambdaBootstrap(handlerWrapper))
            {
                await bootstrap.RunAsync();
            }
        }

        protected override void Init(IWebHostBuilder builder)
        {
            Program.ConfigureWebHostBuilder(builder);
        }
    }
}