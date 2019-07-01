using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TestOnlineEntity.Model.ViewModel;

namespace TestOnlineUI.Component
{
    public class HeaderComponent : ViewComponent
    {
        private UserManager<ApplicationUser> _userManager;
        public HeaderComponent(UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var currentUser = await _userManager.GetUserAsync(this.UserClaimsPrincipal);

            return  View(currentUser);
        }

    }
}
