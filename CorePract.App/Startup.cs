using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CorePract.Services;
using CorePract.Messaging.Consuming;
using CorePract.Messaging.RabbitMQ;
using CorePract.Controllers.Validators;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CorePract.App
{
    public class Startup
    {
        private IssuesStorage _issuesStorage;
        private RmqRestServerIssuesConsumer _rmqConsumer;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(env.ContentRootPath)
                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                 .AddJsonFile($"appsettings{env.EnvironmentName}.json", optional: true)
                 .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton<IConfiguration>(Configuration);

            _issuesStorage =  new IssuesStorage();
            services.AddSingleton<IssuesStorage>(_issuesStorage);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IConfiguration config)
        {
            _rmqConsumer = new RmqRestServerIssuesConsumer(
                   _issuesStorage,
                   config["RabbitMq:user"],
                   config["RabbitMq:vHost"],
                   config["RabbitMq:password"],
                   config["RabbitMq:host"],
                   config["RabbitMq:queueProcessed"],
                   config["RabbitMq:exchange"],
                   config["RabbitMq:routingKeyProcessed"]);

            _rmqConsumer.Consume();

            app.UseMvc();      
        }
       

       

    }
}
