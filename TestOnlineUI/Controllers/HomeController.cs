using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TestOnlineBase.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TestOnlineBusiness.Interface;
using TestOnlineEntity.Model.ViewModel;
using TestOnlineModel.ViewModel;
using TestOnlineModel.ViewModel.User;

namespace TestOnlineUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserDomain _userDomain;
        private UserManager<ApplicationUser> _userManager;
        private IEmailSender _sender;

        public HomeController(ILogger<UserController> logger, IUserDomain userDomain, UserManager<ApplicationUser> userManager, IEmailSender sender)
        {
            this._logger = logger;
            this._userDomain = userDomain;
            this._userManager = userManager;
            this._sender = sender;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RegisterAdmin()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterAdmin(ApplicationUserViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["error"] = "Xảy ra lỗi khi đăng ký";
                    return View();
                }
                var checkExit = await _userManager.FindByNameAsync(viewModel.UserName);
                if (checkExit != null)
                {
                    TempData["warning"] = "Tên tài khoản đã tồn tại";
                    return View();
                }
                var userId = await _userDomain.CreateUserAsync(viewModel);
                var user = await _userManager.FindByIdAsync(userId);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.Action("ConfirmEmail", "Home", new { UserId = user.Id, Code = code }, protocol: Request.Scheme);
                 await _sender.SendEmailAsync(user.Email, "TestOnline - Confirm Your Email", "Please confirm your e-mail by clicking this link: <a href=\"" + callbackUrl + "\">click here</a>");
                TempData["success"] = "Đăng ký thành công. Vui lòng kiểm tra email và xác thực tài khoản ";
                return View();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                TempData["error"] = "Xảy ra lỗi khi đăng ký";
                return View();
            }
        }

        [HttpGet]
        public IActionResult LoginAdmin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAdmin(LoginViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["error"] = "Sai tên tài khoản hoặc mật khẩu";
                    return View();
                }
                var output = await _userDomain.Login(viewModel);
                if (output == null)
                {
                    TempData["error"] = "Sai tên tài khoản hoặc mật khẩu";
                    return View();
                }
                if (output.Equals("Xac thuc"))
                {
                    TempData["warning"] = "Vui lòng xác thực email của bạn";
                    return View();
                }
                return RedirectToAction("Index", "Home",new { area = "Admin"});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                TempData["error"] = "Có lỗi xảy ra";
                return View();
            }
        }

        [HttpGet]
        public IActionResult LoginUser()
        {
            return View();
        }
       
     
        public async Task<IActionResult> ConfirmEmail(string userId,string code)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
                {
                    return RedirectToAction("LoginAdmin", "Home");

                }

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return RedirectToAction("LoginAdmin", "Home"); ;
                }

                if (user.EmailConfirmed)
                {
                    return RedirectToAction("Index","Home");
                }

                var result = await _userManager.ConfirmEmailAsync(user, code);

                if (result.Succeeded)
                {
                    
                    return RedirectToAction("Confirm", "Home", new { userId,code });

                }
                return RedirectToAction("Confirm", "Home", new { userId = "",code = "" });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }
        [HttpGet]
        public IActionResult Tearms()
        {
            return View();
        }

        public IActionResult Confirm(string userId, string code)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                return Redirect("http://localhost:4200/dangnhapquantri");

            }
            return View();
        }
    }
}     