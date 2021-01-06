using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ProductCatalog.Data;
using ProductCatalog.Repositories;

namespace ProductCatalog
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<StoreDataContext>();
            services.AddSingleton<StoreDataContext>();
            services.AddTransient<ProductRepository, ProductRepository>();
            services.AddResponseCompression();
            services.AddControllers();
            services.AddSwaggerGen(x => {
                x.SwaggerDoc(
                    "v1",
                    new OpenApiInfo()
                    {
                        Title = "My API",
                        Version = "v1"
                    }
                );
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => 
            {
                endpoints.MapControllers();
            });

            app.UseResponseCompression();

            app.UseSwagger();
            app.UseSwaggerUI( x => {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
            });
        }
    }
}
