using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using TestOnlineBusiness.Interface;
using TestOnlineEntity.Model.Entity;
using TestOnlineEntity.Model.ViewModel;

namespace TestOnlineUI.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    [Area("Admin")]
    public class TestMemberController : Controller
    {
        private readonly ITestMemberDomain _member;
        private readonly ITestUnitDomain _unit;
        private readonly ILogger<TestMemberController> _logger;
        private readonly IUserDomain _user;
        private UserManager<ApplicationUser> _userManager;

        public TestMemberController(ITestUnitDomain unit,ITestMemberDomain member, ILogger<TestMemberController> logger, IUserDomain user, UserManager<ApplicationUser> userManager)
        {
            this._member = member;
            this._logger = logger;
            this._user = user;
            this._userManager = userManager;
            this._unit = unit;

        }

        public async Task<IActionResult> Index(Guid unitId)
        {
            var unit = await _unit.GetUnitDetail(unitId);  
            if(unit == null)
            {
                return View("error.cshtml");
            }
            
            ViewBag.Unit = unit;
            return View();
        }

        public async Task<IActionResult> Add(Guid unitId)
        {
            var unit = await _unit.GetUnitDetail(unitId);
            if (unit == null)
            {
                return View("error.cshtml");
            }
           
            ViewBag.Unit = unit;
             
            ViewBag.ListUnit = await _unit.GetAll();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Member member)
        {
            try
            {
                var user = await _userManager.GetUserAsync(this.User);
                if (!ModelState.IsValid)
                {
                    await SetViewBag(member.TestUnitId);
                    return View();
                }
                var result = await _member.CreateMember(member, user.Id);
                if (result == null)
                {
                    TempData["error"] = "Có lỗi xảy ra";
                    await SetViewBag(member.TestUnitId);
                    return View();
                }
                TempData["success"] = "Thêm mới thành viên thành công";
                await SetViewBag(member.TestUnitId);
                return View();

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                TempData["error"] = "Có lỗi xảy ra";
                await SetViewBag(member.TestUnitId);
                return View();
            }
           
        }


        public async Task<IActionResult> AddListMember(Guid unitId)
        {
            var unit = await _unit.GetUnitDetail(unitId);
            if (unit == null)
            {
                return View("error.cshtml");
            }

            ViewBag.Unit = unit;

            ViewBag.ListUnit = await _unit.GetAll();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddListMember(Guid unitId,IFormFile file)
        {
            try
            {
                if (file == null)
                {
                    TempData["error"] = "Có lỗi xảy ra";
                    await SetViewBag(unitId);
                    return View();
                }
                if(!Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    TempData["error"] = "File không hợp lệ";
                    await SetViewBag(unitId);
                    return View();
                }
                var user = await _userManager.GetUserAsync(this.User);
                var result = await _member.AddListMember(unitId, file, user.Id);
                if (result) { }

                return View();

            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                TempData["error"] = "Có lỗi xảy ra";
                await SetViewBag(unitId);
                return View();
            }
        }

        public async Task SetViewBag(Guid? unitId)
        {
            var unit = await _unit.GetUnitDetail(unitId);
           
            ViewBag.Unit = unit;

            ViewBag.ListUnit = await _unit.GetAll();
        }
    }
}