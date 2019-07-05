using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TestOnlineBase.Constant;
using TestOnlineBase.Helper.PagingHelper;
using TestOnlineBusiness.Interface;
using TestOnlineEntity.Interface;
using TestOnlineEntity.Model.Entity;
using TestOnlineEntity.Model.ViewModel;
using TestOnlineModel.ViewModel.Admin;

namespace TestOnlineUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TestCategoryController : Controller
    {
        private readonly ICategoryDomain _category;
        private readonly ILogger<TestCategoryController> _logger;
     
        private readonly IUserDomain _user;
        private readonly PageSize _pageSize;
        private UserManager<ApplicationUser> _userManager;
        public TestCategoryController(ICategoryDomain category, ILogger<TestCategoryController> logger, IUserDomain user,IOptions<PageSize> pageSize,UserManager<ApplicationUser> userManager, ITestOnlienUnitOfWork unitOfWork)
        {
            this._category = category;
            this._logger = logger;
            this._user = user;
            this._pageSize = pageSize.Value;
            this._userManager = userManager;
          
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetListCategory(FilterModel model)
        {
            try
            {
                var user = await _userManager.GetUserAsync(this.User);
                var categoires = await _category.GetCategory(model,user.Id);

                return PartialView(categoires);
            }catch(Exception ex)
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
        public async Task<IActionResult> Update( Guid categoryId)
        {
            try
            {
                var category = await _category.GetCategoryDetail(categoryId);
                if (category == null)
                {
                   
                    return View("Error.cshtml");
                }
               
                return View(category);
            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return View("Error.cshtml");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(TestCategoryViewModel viewmodel, IFormFile file)
        {
            try
            {
                
                if (!ModelState.IsValid)
                {
                    TempData["error"] = "Có lỗi xảy ra";

                    return RedirectToAction("Update", new { categoryId = viewmodel.Id });
                }
                var user = await _userManager.GetUserAsync(this.User);
                var result = await _category.UpdateCategory(viewmodel.Id,viewmodel, user.Id, file);
                if (!result)
                {
                    TempData["error"] = "Có lỗi xảy ra";
                    return RedirectToAction("Index");
                }
                TempData["updatesuccess"] = "Cập nhật chuyên mục thành công";

                 return RedirectToAction("Index");
            
            }
            catch (Exception ex)
            {
                
                _logger.LogError(ex, ex.Message);
                TempData["error"] = "Có lỗi xảy ra";
                return RedirectToAction("Index"); 
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(TestCategoryViewModel viewmodel,IFormFile file)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["error"] = "Có lỗi xảy ra";
                    return View();
                }
                var user = await _userManager.GetUserAsync(this.User);
                var result = await _category.CreateCategory(viewmodel, user.Id, file);
                if (!result)
                {
                    TempData["error"] = "Có lỗi xảy ra.Thử lại với tên chuyên mục khác";
                    return View();
                }
                TempData["success"] = "Đã thêm mới chuyên mục";
                return View();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                TempData["error"] = "Có lỗi xảy ra";
                return View();
            }
           
        }
       
        public async Task<IActionResult> DeleteCategory(Guid categoryId)
        {
            try
            {
                var result = await _category.DeleteCategory(categoryId);
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
            }catch(Exception ex)
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