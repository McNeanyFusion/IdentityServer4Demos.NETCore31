// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Services;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace IdentityServer
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IWebHostEnvironment environment)
        {
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // uncomment, if you want to add an MVC-based UI
            services.AddControllersWithViews();

            //services.AddCors(options =>
            //{
            //    options.AddPolicy("CorsPolicy",
            //        builder => builder.WithOrigins("https://localhost:44339")
            //        .AllowAnyMethod()
            //        .AllowAnyHeader());
            //});


            //var cors = new DefaultCorsPolicyService(null)
            //{
            //    AllowedOrigins = { "https://localhost:44339" }
            //};
            //services.AddSingleton<ICorsPolicyService>(cors);


            var builder = services.AddIdentityServer(opts =>
            {
                opts.UserInteraction.LoginUrl = "/Account/Login";
                opts.UserInteraction.LogoutUrl = "/Account/Logout";
            })
                //.AddSigningCredential(new X509Certificate2(@"C:\Users\smcneany\Downloads\asp-dot-net-core-oauth\03\demos\SocialNetwork\socialnetwork.pfx", "password"))
                .AddInMemoryIdentityResources(Config.Ids)
                .AddInMemoryApiResources(Config.Apis)
                .AddInMemoryClients(Config.Clients)
                .AddTestUsers(Config.Users().ToList());


            // not recommended for production - you need to store your key material somewhere secure
            builder.AddDeveloperSigningCredential();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // uncomment if you want to add MVC
            app.UseStaticFiles();
            app.UseRouting();



            app.UseCors("CorsPolicy");
            app.UseIdentityServer();


            // uncomment, if you want to add MVC
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
