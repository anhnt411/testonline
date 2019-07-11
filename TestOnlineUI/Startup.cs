using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TestOnlineBase.EmailHelper;
using TestOnlineBase.Helper;
using TestOnlineBusiness.Interface;
using TestOnlineBusiness.Service;
using TestOnlineEntity.Interface;
using TestOnlineEntity.Model.Context;
using TestOnlineEntity.Model.ViewModel;
using TestOnlineEntity.Service;
using TestOnlineModel.ViewModel.User;
using TestOnlineShared.Interface;
using TestOnlineShared.Service;
using Microsoft.Extensions.Logging;
using Serilog;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TestOnlineBase.Constant;
using System.Data.SqlClient;

namespace TestOnlineUI
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
            services.Configure<ApplicationSettingViewModel>(Configuration.GetSection("ApplicationSettings"));
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.Configure<PageSize>(Configuration.GetSection("PageSize"));
          
            AddService(services);
            services.AddIdentity<ApplicationUser,IdentityRole>(config =>
            {
                config.SignIn.RequireConfirmedEmail = true;
                config.User.RequireUniqueEmail = true;

            })
              .AddEntityFrameworkStores<TestOnlineDbContext>().AddDefaultTokenProviders();

            services.AddCors();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
             





            }
          );

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Home/LoginAdmin");
                options.Cookie.Name = "TestOnlne";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.LoginPath = new PathString("/Home/LoginAdmin");
                // ReturnUrlParameter requires 
                //using Microsoft.AspNetCore.Authentication.Cookies;
               // options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });


            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            SeedData.InitilizeDatabase(app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope().ServiceProvider);
            app.UseHttpsRedirection();
            app.UseStaticFiles();
          
          
        
            loggerFactory.AddSerilog();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                

                routes.MapRoute(
                        name: "areaRoute",
                        template: "{area:exists}/{controller=Home}/{action=index}/{id?}"
                     
                              );

              
                
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });

            });
            app.UseCookiePolicy();

        }

        private void AddService(IServiceCollection services)
        {
            services.AddDbContext<TestOnlineDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("sqlServerConnectionString")));
            services.AddScoped<DbContext, TestOnlineDbContext>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<ITestOnlienUnitOfWork, TestOnlineUnitOfWork>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUserDomain, UserDomain>();
            services.AddScoped<ICategoryDomain, CategoryDomain>();
            services.AddScoped<ITestUnitDomain, TestUnitDomain>();
            services.AddScoped<ITestMemberDomain, TestMemberDomain>();
            services.AddScoped<IQuestionBankDomain, QuestionBankDomain>();
            services.AddSingleton<TestOnlineBase.Helper.IEmailSender, EmailSender>();

        }
    }
}
