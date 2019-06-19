using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TestOnlineBusiness.Interface;
using TestOnlineEntity.Model.ViewModel;
using TestOnlineModel.ViewModel.User;

namespace TestOnlineUI.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserDomain _userDomain;
        private UserManager<ApplicationUser> _userManager;
        private IEmailSender _sender;


        public UserController(ILogger<UserController> logger, IUserDomain userDomain, UserManager<ApplicationUser> userManager, IEmailSender sender)
        {
            this._logger = logger;
            this._userDomain = userDomain;
            this._userManager = userManager;
            this._sender = sender;

        }
       
    }
}