using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using MediatR;
using Serilog;
using FluentValidation.AspNetCore;

using Core;

namespace Blog
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            // configure Serilog
            // (from https://github.com/serilog/serilog-extensions-logging)
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.ApplicationInsightsTraces(Configuration.GetSection("ApplicationInsights:InstrumentationKey").Value)
                .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services
                .AddMvc(options =>
                {
                })
                .AddFeatureFolders()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.AddMediatR(typeof(Startup));

            services.Configure<DocumentDBWrapperConfig>(
                docConfig => {
                    Configuration.GetSection("Storage:DocumentDBWrapperConfig").Bind(docConfig);
                    // retrieve AuthKey from elsewhere (I am using 'User Secrets' for dev)
                    docConfig.AuthKey = Configuration["DocumentDbAuthKey"];
                    });

            services.AddScoped<IDocumentDBWrapper<BlogEntry>, DocumentDBWrapper<BlogEntry>>();

            bool useDocumentDB = Configuration.GetSection("Storage").GetValue<bool>("UseDocumentDB");
            if (useDocumentDB)
            {
                services.AddScoped<IBlogEntryRepository, BlogEntryRepositoryDocumentDb>();
            }
            else
            {
                services.AddScoped<IBlogEntryRepository, BlogEntryRepositoryInMemory>();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IApplicationLifetime appLifetime)
        {
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();

            // replace default logging with Serilog
            loggerFactory.AddSerilog();
            appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
