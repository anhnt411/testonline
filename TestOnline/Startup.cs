using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using TestOnlineEntity.Interface;
using TestOnlineEntity.Model.Context;
using TestOnlineEntity.Service;
using TestOnlineShared.Interface; 
using TestOnlineShared.Service;
using TestOnlineModel.ViewModel.User;
using Microsoft.AspNetCore.Identity;
using TestOnlineEntity.Model.ViewModel;
using Serilog;
using Microsoft.AspNetCore.Http;
using TestOnlineBusiness.Service;
using TestOnlineBusiness.Interface;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace TestOnline
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApplicationSettingViewModel>(Configuration.GetSection("ApplicationSettings"));
            AddService(services);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

          

            services.AddIdentity<ApplicationUser, IdentityRole>()
      .AddEntityFrameworkStores<TestOnlineDbContext>()
      .AddDefaultTokenProviders();

            services.AddCors();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
            }
          );

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "TestOnline API",
                    Description = "TestOnline API V1"
                });
            });


          
            var key = Encoding.UTF8.GetBytes(Configuration["ApplicationSettings:JWT_Secret"].ToString());

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x => {
                x.RequireHttpsMetadata = false;
                x.SaveToken = false;
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,ILoggerFactory loggerFactory)
        {
            SeedData.InitilizeDatabase(app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope().ServiceProvider);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
            
            app.UseCors(builder =>
             builder.WithOrigins(Configuration["ApplicationSettings:Client_URL"].ToString())
             .AllowAnyHeader()
             .AllowAnyMethod()

             );
            app.UseHttpsRedirection();
            
            app.UseAuthentication();
            loggerFactory.AddSerilog();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TestOnlineAPI V1");
            });
        }

        private void AddService(IServiceCollection services)
        {
            services.AddDbContext<TestOnlineDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("sqlServerConnectionString")));
            services.TryAddScoped<DbContext, TestOnlineDbContext>();
            services.TryAddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.TryAddScoped<ITestOnlienUnitOfWork, TestOnlineUnitOfWork>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddScoped<IUserDomain, UserDomain>();
            
        }
    }
}
