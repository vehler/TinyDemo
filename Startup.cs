using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using TinyDemo.Common;
using TinyDemo.Common.Data;
using TinyDemo.Common.Models;

namespace TinyDemo
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            // Global API route prefix used for clarity and to avoid collitions
            // with mvc routes
            services.AddControllersWithViews(o => o.UseGlobalRoutePrefix("api"));

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(config => config.RootPath = "ClientApp/dist");

            services.AddDbContext<DataContext>(o => o.UseMySQL(Configuration.GetConnectionString("DefaultConnection")));

            services.AddCors();
            services.AddControllers().AddJsonOptions(o => o.JsonSerializerOptions.IgnoreNullValues = true);

            services.AddJWTScheme(Configuration);

            services.AddServices();

            services.AddHttpClient();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // TODO lock with domains / ip whitelist on production
            app.UseCors(policy => policy
                .SetIsOriginAllowed(origin => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });


            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
