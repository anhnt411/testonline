
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestOnlineBase.Constant;
using TestOnlineEntity.Model.ViewModel;

namespace TestOnlineEntity.Model.Context
{
    public class SeedData
    {
        public async static void InitilizeDatabase(IServiceProvider provider)
        {
            var context = provider.GetRequiredService<TestOnlineDbContext>();
            var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
            context.Database.EnsureCreated();

            var listRole = new List<string>() { Constant.Role.ADMIN,Constant.Role.NORMAL_USER,Constant.Role.SUPER_USER };
            foreach (var item in listRole)
            {
                var roleExit = await roleManager.RoleExistsAsync(item);
                if (!roleExit)
                {
                    await roleManager.CreateAsync(new IdentityRole(item));
                }
            }

            if (!context.Users.Any())
            {
                var user = new ApplicationUser()
                {
                    Email = "anh.nt.1196@gmail.com",
                    Address = "Ha Noi",
                    UserName = "tuananh411",
                    PhoneNumber = "0387516825",
                    Status = true                  
                };
                
                await userManager.CreateAsync(user, "Tanhlaai96");
                await userManager.AddToRoleAsync(user, Constant.Role.SUPER_USER);
            }
        }
    }
}
