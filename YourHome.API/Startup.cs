using System;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YourHome.API.Configuration;
using YourHome.Core.Abstract;
using YourHome.Core.Services;
using YourHome.Core.Utils;
using YourHome.Infrastructure;
using YourHome.Infrastructure.Repositories;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                             .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());
            services.AddElasticsearch(Configuration);
            services.AddAutoMapper();
            

            services.AddScoped<IOfferRepository, OfferRepository>();
            services.AddScoped<IGeoCodeProvider, BingGeoCodeProvider>();
            services.AddScoped<IOfferService, OfferService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IEmailSender, GmailEmailSender>();
            services.AddScoped<IImagePathBuilder, ImagePathBuilder>(i => new ImagePathBuilder(Configuration["ImagesFolderPath"]));
            services.AddScoped<IImageSaver, ImageSaver>(i => new ImageSaver(Configuration["ImagesFolderPath"]));

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.Options.StartupTimeout = new TimeSpan(0, 0, 120);
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
