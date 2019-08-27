using ATM.Simulates.API.Application.Queries;
using ATM.Simulates.API.Models;
using log4net;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Text;

namespace ATM.Simulates.API
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<ATMContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));
            services.AddSwaggerDocument();
            services.AddMediatR(typeof(Startup));
            services.AddScoped<IAccountQueries, AccountQueries>();

            var shareKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSecurityToken:Key"]));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.FromMinutes(5),
                    IssuerSigningKey = shareKey,
                    RequireSignedTokens = true,
                    ValidAudience = Configuration["JwtSecurityToken:Audience"],
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["JwtSecurityToken:Issuer"]
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ATMContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            var logRepository = LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly());
            log4net.Config.XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            DbInitializer.Initialize(context);
            app.UseSwagger(config =>
            {
                config.Path = "/swagger/v1/swagger.json";
            });
            app.UseSwaggerUi3();
            app.UseAuthentication();
           // app.UseExcuteMiddleware();
            app.UseMvc();


        }
    }
}
