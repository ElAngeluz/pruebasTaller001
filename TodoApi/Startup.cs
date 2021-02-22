using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using TodoApi.Repositorio;

namespace TodoApi
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
            services.AddDbContext<TodoContext>(opt =>
                                               opt.UseInMemoryDatabase("TodoList"));
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ToDo API",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Shayne Boyer",
                        Email = string.Empty,
                        Url = new Uri("https://twitter.com/spboyer"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });
            });

            _ = services.AddScoped<IPrueba, Prueba>();
            _ = services.AddTransient<IPrueba, Prueba>();
            _ = services.AddSingleton<IPrueba, Prueba>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                _ = app.UseDeveloperExceptionPage();
            }

            _ = app.UseHttpsRedirection();
            _ = app.UseRouting();
            _ = app.UseSwagger();
            _ = app.UseSwagger(c =>
              {
                  c.SerializeAsV2 = true;
              });
            _ = app.UseSwaggerUI(c =>
              {
                  c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                  c.RoutePrefix = string.Empty;
              });

            _ = app.UseAuthorization();

            _ = app.UseEndpoints(endpoints =>
              {
                  endpoints.MapControllers();
              });
        }
    }
}
