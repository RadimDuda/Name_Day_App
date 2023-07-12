using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NameDayWorker.DbFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Reflection;

namespace playground_first2
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
            
        public void ConfigureServices(IServiceCollection services) {

            var connectionString = Configuration.GetConnectionString("NameDayDbConnection");

            services.AddDbContext<NameDayDbContext>(options =>
            {
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production") {
                    connectionString = Configuration.GetConnectionString("NameDayDbConnectionPrsoduction");
                    options.UseSqlServer(connectionString);
                }
                else {
                    options.UseSqlite(connectionString);
                }
            });

            services.AddControllers();

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Documentation", Version = "v1" });
                c.EnableAnnotations();

                var xmlDocFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlDocPath = Path.Combine(AppContext.BaseDirectory, xmlDocFile);
                c.IncludeXmlComments(xmlDocPath);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Documentation V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
