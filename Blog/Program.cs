using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Blog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseAzureAppServices() // ASP.NET Core does not by default feed logs to Azure App Service - https://shellmonger.com/2017/02/16/running-asp-net-core-applications-in-azure-app-service/
                .UseApplicationInsights()
                .UseStartup<Startup>()
                .Build();
        }
    }
}
