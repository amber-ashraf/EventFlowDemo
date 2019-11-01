using Autofac;
using Autofac.Extensions.DependencyInjection;
using EventFlow;
using EventFlow.AspNetCore.Extensions;
using EventFlow.AspNetCore.Middlewares;
using EventFlow.Autofac.Extensions;
using EventFlow.Configuration;
using EventFlow.Elasticsearch.Extensions;
using EventFlow.Extensions;
using WSiteEventFlow.Core.Aggregates.Entities;
using WSiteEventFlow.Core.Aggregates.Locator;
using WSiteEventFlow.Core.Aggregates.Queries;
using WSiteEventFlow.ElasticSearch.QueryHandler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Nest;
using System;
using WSiteEventFlow.ElasticSearch.ReadModels;


namespace WSiteEventFlow.Read
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IEventFlowOptions Options { get; set; }
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "Read Api Eventflow Demo - API", Version = "v1" });
                
                x.DescribeAllEnumsAsStrings();
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            string elasticSearchUrl = "http://127.0.0.1:9200/";// Environment.GetEnvironmentVariable("ELASTICSEARCHURL");
            ContainerBuilder containerBuilder = new ContainerBuilder();
            Uri node = new Uri(elasticSearchUrl);
            ConnectionSettings settings = new ConnectionSettings(node);

            settings.DisableDirectStreaming();

            ElasticClient elasticClient = new ElasticClient(settings);

              Options = EventFlowOptions.New
                .UseAutofacContainerBuilder(containerBuilder)
                .AddDefaults(typeof(Employee).Assembly)
                .ConfigureElasticsearch(() => elasticClient)
                .RegisterServices(sr => sr.Register<IScopedContext, ScopedContext>(Lifetime.Scoped))
                .RegisterServices(sr => sr.RegisterType(typeof(EmployeeLocator)))
                .UseElasticsearchReadModel<EmployeeReadModel, EmployeeLocator>()
                .AddQueryHandlers(typeof(ElasticSearchEmployeeGetQueryHandler), typeof(ElasticSearchEmployeeGetAllQueryHandler)) 
                .Configure(c => c.IsAsynchronousSubscribersEnabled = true)
                .AddAspNetCoreMetadataProviders();

            containerBuilder.Populate(services);
            var container = containerBuilder.Build();
             
          
            return new AutofacServiceProvider(container);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
           
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "Read API Version 1");

            });

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseHttpsRedirection();
            app.UseMiddleware<CommandPublishMiddleware>();
            app.UseMvcWithDefaultRoute();
           
        }
    }
}
