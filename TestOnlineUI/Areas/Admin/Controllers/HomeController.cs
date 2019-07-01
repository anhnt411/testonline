using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestOnlineBase.Helper.FileHelper;
using TestOnlineEntity.Model.ViewModel;
using TestOnlineModel.ViewModel.User;

namespace TestOnlineUI.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    [Area("Admin")]
    public class HomeController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _hostingEnvironment;

        public HomeController(UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment)
        {
            this._userManager = userManager;
            this._hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
       public async Task<IActionResult> UpdateInfoUser()
        {
            var user = await _userManager.GetUserAsync(this.User);
            if (user == null)
            {
                TempData["error"] = "Đã xảy ra lỗi";
                return View();
            }
            var userInfo = new UserInfoViewModel()
            {
                UserName = user.UserName,
                Email = user.Email,
                FullName = user.FullName,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                Image = user.Image
            };
            return View(userInfo);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateInfoUser(UserInfoViewModel viewModel,IFormFile file)
        {
            var user = await _userManager.GetUserAsync(this.User);
            var userInfo = new UserInfoViewModel()
            {
                UserName = user.UserName,
                Email = user.Email,
                FullName = user.FullName,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                Image = user.Image
            };
            string imageName = null;

            if (!ModelState.IsValid) {
                TempData["error"] = "Xảy ra lỗi";
                return View(userInfo);
            }
            if (file != null)
            {
                imageName = UploadImageFile.UploadImage(file);
            }
            
          
            user.FullName = viewModel.FullName;
            user.PhoneNumber = viewModel.PhoneNumber;
            user.Address = viewModel.Address;
            if (imageName != null)
            {
                user.Image = imageName;
            }
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["success"] = "Cập nhật thông tin thành công";
                var newUserInfo = new UserInfoViewModel()
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    FullName = user.FullName,
                    Address = user.Address,
                    PhoneNumber = user.PhoneNumber,
                    Image = user.Image
                };
                return View(newUserInfo);
            }
            TempData["error"] = "Có lỗi xảy ra";
            return View(userInfo);

        }
    }
}