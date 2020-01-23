using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using RecruitmentTask.Core;
using RecruitmentTask.Domain;

namespace RecruitmentTask.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IDocumentProcessor, DocumentProcessor>();

            services.AddMvc(opts => opts.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Documents manager API",
                    Description = "Aplikacja umożliwiająca wyszukiwanie dokumentów",
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName == "Development")
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseExceptionHandler(eh => eh.Run(async ctx =>
            {
                var exceptionHandlerPathFeature = ctx.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature.Error;

                var result = JsonConvert.SerializeObject(new { error = exception.Message });
                ctx.Response.ContentType = "application/json";

                await ctx.Response.WriteAsync(result);
            }));
            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "DocumentsManagerAPI V1");
            });

            AWS.Instance.AWSOptions = Configuration.GetAWSOptions();
            AWS.Instance.S3BucketName = Configuration["S3BucketName"];
            AWS.Instance.DynamoDbTableName = Configuration["DynamoDbTableName"];
        }
    }
}
