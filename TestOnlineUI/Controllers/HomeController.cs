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
using System.Threading;
using Microsoft.AspNetCore.Http;
using TestOnlineBase.Helper.FileHelper;
using Newtonsoft.Json;
using System.IO;
using System.Drawing;

namespace TestOnlineUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserDomain _userDomain;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IEmailSender _sender;

        public HomeController(ILogger<UserController> logger, IUserDomain userDomain, UserManager<ApplicationUser> userManager, IEmailSender sender, SignInManager<ApplicationUser> signInManager)
        {
             this._logger = logger;
            this._userDomain = userDomain;
            this._userManager = userManager;
            this._sender = sender;
            this._signInManager = signInManager;
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
        public IActionResult LoginAdmin(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAdmin(LoginViewModel viewModel,string returnUrl)
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
                var user = (ApplicationUser)output;
                await _signInManager.SignInAsync(user, false);
                if (Url.IsLocalUrl(returnUrl))
                {
                     return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Home", new { area = "Admin"  });
             
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

        public IActionResult ResetPassword(string userId,string code)
        {
            try
            {
                if (code == null)
                {
                    return RedirectToAction("Index");
                }
                ViewBag.Code = code;
                return View();

            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewmodel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Có lỗi xảy ra";
                return View();
            }
            var user = await _userManager.FindByEmailAsync(viewmodel.Email);
            if(user == null)
            {
                TempData["error"] = "Email không đúng";
                return View();
            }
            var result = await _userManager.ResetPasswordAsync(user, viewmodel.Code, viewmodel.Password);
            if (result.Succeeded)
            {
                TempData["success"] = "Mật khẩu thay đổi thành công. Hãy đăng nhập";
                return View();
            }
            TempData["error"] = "Có lỗi xảy ra";
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

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return Json(new
                {
                    status = -2
                });
            }
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null)
            {
                TempData["error"] = "Email không tồn tại trong hệ thống";
                return  Json(new
                {
                    status = -1
                });
            }
            var check = await _userManager.IsEmailConfirmedAsync(user);
            if (!check )
            {
                return Json(new
                {
                    status = 0
                });
            }
            string code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action("ResetPassword", "Home", new { userId = user.Id, Code = code }, protocol: Request.Scheme);
            if (callbackUrl != null)
            {
                await _sender.SendEmailAsync(user.Email, "Reset Password", "Please reset your password by clicking this link: <a href=\"" + callbackUrl + "\">click here</a>");
            }
         

            return Json(new
            {
                status = 1
            });
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewmodel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Xảy ra lỗi";
                return Json(new
                {
                    status = -2
                });
            }
            var user = await _userManager.GetUserAsync(this.User);
            var result = await _userManager.ChangePasswordAsync(user, viewmodel.Password, viewmodel.NewPassword);
            if (result.Succeeded)
            {
                TempData["success"] = "Đổi mật khẩu thành công";
                return Json(new
                {

                    status = 1
                });
            }
            TempData["error"] = "Sai mật khẩu";
            return Json(new
            {

                status = 0
            });
        }

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("LoginAdmin","Home");
        }

        [HttpPost]
        public ActionResult UploadImage(IFormFile upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            if (upload.Length <= 0) return null;
            if (!upload.IsImage())
            {
                var NotImageMessage = "Vui lòng chọn 1 ảnh";
                dynamic NotImage = JsonConvert.DeserializeObject("{ 'uploaded': 0, 'error': { 'message': \"" + NotImageMessage + "\"}}");
                return Json(NotImage);
            }

            var fileName = upload.FileName;
             

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload_img/", fileName);
            if (System.IO.File.Exists(path))
            {
                var message = "Upload ảnh thành công";
                var urlimg = $"{"/upload_img/"}{fileName}";
                dynamic result = JsonConvert.DeserializeObject("{ 'uploaded': 1,'fileName': \"" + fileName + "\",'url': \"" + urlimg + "\", 'error': { 'message': \"" + message + "\"}}");
                return Json(result);
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                upload.CopyTo(stream);

            }

            var url = $"{"/upload_img/"}{fileName}";
            var successMessage = "Upload ảnh thành công";
            dynamic success = JsonConvert.DeserializeObject("{ 'uploaded': 1,'fileName': \"" + fileName + "\",'url': \"" + url + "\", 'error': { 'message': \"" + successMessage + "\"}}");
            return Json(success);
        }

        [HttpPost]
        public ActionResult UploadCKEditor(IFormFile upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            if (upload.Length <= 0) return null;
            if (!upload.IsImage())
            {
                var NotImageMessage = "Vui lòng chọn 1 ảnh";
                dynamic NotImage = JsonConvert.DeserializeObject("{ 'uploaded': 0, 'error': { 'message': \"" + NotImageMessage + "\"}}");
                return Json(NotImage);
            }

            var fileName = upload.FileName;


            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload_img/", fileName);
            if (System.IO.File.Exists(path))
            {
                var message = "Upload ảnh thành công";
                var urlimg = $"{"/upload_img/"}{fileName}";
                dynamic result = JsonConvert.DeserializeObject("{ 'uploaded': 1,'fileName': \"" + fileName + "\",'url': \"" + urlimg + "\", 'error': { 'message': \"" + message + "\"}}");
                return Json(result);
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                upload.CopyTo(stream);

            }

            var url = $"{"/upload_img/"}{fileName}";
            var successMessage = "Upload ảnh thành công";
            dynamic success = JsonConvert.DeserializeObject("{ 'uploaded': 1,'fileName': \"" + fileName + "\",'url': \"" + url + "\", 'error': { 'message': \"" + successMessage + "\"}}");
            return Json(success);
        }




    }
}     