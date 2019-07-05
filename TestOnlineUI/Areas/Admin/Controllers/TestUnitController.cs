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
using TestOnlineEntity.Interface;
using TestOnlineEntity.Model.ViewModel;
using TestOnlineModel.ViewModel.Admin;

namespace TestOnlineUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TestUnitController : Controller
    {
        private readonly ITestUnitDomain _unit;
        private readonly ILogger<TestUnitController> _logger;
        private readonly IUserDomain _user;  
        private UserManager<ApplicationUser> _userManager;
        public TestUnitController(ITestUnitDomain unit, ILogger<TestUnitController> logger, IUserDomain user, UserManager<ApplicationUser> userManager)
        {
            this._unit = unit;
            this._logger = logger;
            this._user = user;       
            this._userManager = userManager;

        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetListUnit(FilterModel model)
        {
            try
            {
                var user = await _userManager.GetUserAsync(this.User);
                var units = await _unit.GetUnit(model, user.Id);

                return PartialView(units);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return View("Error.cshtml");
            }

        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid unitId)
        {
            try
            {
                var unit = await _unit.GetUnitDetail(unitId);
                if (unit == null)
                {

                    return View("Error.cshtml");
                }

                return View(unit);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return View("Error.cshtml");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(TestUnitViewModel viewmodel)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    TempData["error"] = "Có lỗi xảy ra";

                    return RedirectToAction("Update", new { unitId = viewmodel.Id });
                }
                var user = await _userManager.GetUserAsync(this.User);
                var result = await _unit.UpdateUnit(viewmodel.Id, viewmodel, user.Id);
                if (!result)
                {
                    TempData["error"] = "Có lỗi xảy ra. Thử lại với tên đơn vị khác";
                    return RedirectToAction("Update", new { unitId = viewmodel.Id });
                }
                TempData["updateunitsuccess"] = "Cập nhật đơn vị thành công";

                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
                TempData["error"] = "Có lỗi xảy ra, thử lại với tên đơn vị khác";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(TestUnitViewModel viewmodel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["error"] = "Có lỗi xảy ra";
                    return View();
                }
                var user = await _userManager.GetUserAsync(this.User);
                var result = await _unit.CreateUnit(viewmodel, user.Id);
                if (!result)
                {
                    TempData["error"] = "Có lỗi xảy ra.Thử lại với tên đơn vị khác";
                    return View();
                }
                TempData["success"] = "Đã thêm mới đơn vị";
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                TempData["error"] = "Có lỗi xảy ra";
                return View();
            }

        }

        public async Task<IActionResult> DeleteUnit(Guid unitId)
        {
            try
            {
                var result = await _unit.DeleteUnit(unitId);
                if (!result)
                {
                    return Json(new
                    {
                        status = 0
                    });
                }
                return Json(new
                {
                    status = 1
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Json(new
                {
                    status = 0
                });
            }
        }
    }
}