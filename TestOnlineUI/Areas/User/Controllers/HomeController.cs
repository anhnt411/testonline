using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TestOnlineBase.Helper.FileHelper;
using TestOnlineBase.Helper.PagingHelper;
using TestOnlineBusiness.Interface;
using TestOnlineEntity.Model.ViewModel;
using TestOnlineModel.ViewModel.Admin;
using TestOnlineModel.ViewModel.User;

namespace TestOnlineUI.Areas.User.Controllers
{
    [Authorize(Roles = "User")]
    [Area("User")]
    public class HomeController : Controller
    {

        private UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<HomeController> _logger;
        private readonly ITestScheduleDomain _schedule;

        public HomeController(UserManager<ApplicationUser> userManager, ITestScheduleDomain schedule, ILogger<HomeController> logger, IHostingEnvironment hostingEnvironment)
        {
            this._userManager = userManager;
            this._hostingEnvironment = hostingEnvironment;
            this._logger = logger;
            this._schedule = schedule;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UserViewExamDetail(Guid userexamId)
        {
            var user = await _userManager.GetUserAsync(this.User);
            var result = await _schedule.UpdateAccessExam(userexamId);
            var examDetails = await _schedule.GetUserListExamDetail(userexamId);
            return View(examDetails);
        }
        [HttpPost]
        public async Task<IActionResult> UserViewExamDetail(UserAnswerViewModel viewModel)
        {
            try
            {
                var user = await _userManager.GetUserAsync(this.User);
                var result = await _schedule.AddAnswerExamUser(viewModel, user.Id);
                return Json(new
                {
                    status = 1
                });
            } catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Json(new
                {
                    status = 0
                });
            }
        }
        [HttpGet]
        public async Task<IActionResult> ReviewUserExam(Guid examId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(this.User);
                var result = await _schedule.ReviewUserExamDetail(examId, user.Id);
                int correctQuestion = 0;
                foreach (var item in result)
                {
                    if(item.QuestionTrue == false || item.QuestionTrue == null)
                    {
                        correctQuestion++;
                    }
                }
                ViewBag.CorrectQuestion = result.Count() - correctQuestion;
                return View(result);
            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return View("error.cshtml");
            }
        }
        [HttpPost]
        public async Task<IActionResult> GetListUserSchedule(FilterModel model)
        {
            try
            {
                var user = await _userManager.GetUserAsync(this.User);
                var list = await _schedule.GetListUserSchedule(model, user.Id);
                return PartialView(list);
            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return View("error.cshtml");
            }
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
        public async Task<IActionResult> UpdateInfoUser(UserInfoViewModel viewModel, IFormFile file)
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

            if (!ModelState.IsValid)
            {
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