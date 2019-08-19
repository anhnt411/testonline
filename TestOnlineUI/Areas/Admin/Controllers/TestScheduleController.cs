using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TestOnlineBase.Helper.PagingHelper;
using TestOnlineBusiness.Interface;
using TestOnlineEntity.Model.ViewModel;
using TestOnlineModel.ViewModel.Admin;

namespace TestOnlineUI.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class TestScheduleController : Controller
    {
        private readonly ITestScheduleDomain _schedule;
        private readonly ILogger<TestScheduleController> _logger;
        private readonly IUserDomain _user;
        private readonly ICategoryDomain _category;
        private UserManager<ApplicationUser> _userManager;
        public TestScheduleController(ITestScheduleDomain schedule,ICategoryDomain category, ILogger<TestScheduleController> logger, IUserDomain user, UserManager<ApplicationUser> userManager)
        {
            this._schedule = schedule;
            this._logger = logger;
            this._user = user;
            this._userManager = userManager;
            this._category = category;

        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await SetViewBag();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddSchedule(Guid categoryId)
        {

            ViewBag.CategoryId = categoryId;
            await SetViewBag();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSchedule(TestScheduleViewModel viewModel)
        {
            try
            {
                var user = await _userManager.GetUserAsync(this.User);
                var output = await _schedule.AddSchedule(viewModel, user.Id);
                if (output)
                {
                    TempData["success"] = "Thêm mới thành công";
                    await SetViewBag();
                    return View(viewModel.CategoryId);
                }
                TempData["error"] = "Có lỗi xảy ra";
                await SetViewBag();
                return View(viewModel.CategoryId);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return View("error.cshtml");
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetListTestSchedule(FilterModel model)
        {
            try
            {
                var user = await _userManager.GetUserAsync(this.User);
                var testschedules = await _schedule.GetListSchedule(model, user.Id);
                return PartialView(testschedules);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return View("Error.cshtml");
            }

        }
        public async Task SetViewBag()
        {
            try
            {
                var user = await _userManager.GetUserAsync(this.User);
                var listCategory = await _category.GetAllCategory(user.Id);

                ViewBag.ListCategory = listCategory;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return;
            }

        }

    }
}