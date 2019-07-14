using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TestOnlineBase.Helper.PagingHelper;
using TestOnlineBusiness.Interface;
using TestOnlineEntity.Model.Entity;
using TestOnlineEntity.Model.ViewModel;
using TestOnlineModel.ViewModel.Admin;

namespace TestOnlineUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class QuestionBankController : Controller
    {

        private readonly IQuestionBankDomain _questionBank;
        private readonly ICategoryDomain _category;
        private readonly ILogger<QuestionBankController> _logger;
        private readonly IUserDomain _user;
        private UserManager<ApplicationUser> _userManager;

        public QuestionBankController(IQuestionBankDomain questionBank, ICategoryDomain category, ILogger<QuestionBankController> logger, IUserDomain user, UserManager<ApplicationUser> userManager)
        {
            this._questionBank = questionBank;
            this._category = category;
            this._logger = logger;
            this._user = user;
            this._userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            await SetViewBag();
            return View();
        }
        

        public async Task<IActionResult> AddQuestionGroup(Guid categoryId)
        {
            try
            {
                ViewBag.CategoryId = categoryId;
                await SetViewBag();
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return View("Error.cshtml");
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateQuestionGroup(Guid questionGroupId)
        {
            try
            {
                var questionGroup = await _questionBank.GetQuestionGroupDetail(questionGroupId);
                await SetViewBag();
                return View(questionGroup);

            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return View("error.cshtml");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuestionGroup(QuestionGroup model)
        {
            try
            {
                var user = await _userManager.GetUserAsync(this.User);
                if(user == null)
                {
                    return View("error.cshtml");
                }
                var result = await _questionBank.UpdateQuestionBank(model, user.Id);
                if (!result)
                {
                   
                    TempData["errorgroupupdate"] = "Nhóm câu hỏi này đã tồn tại";
                    return RedirectToAction("UpdateQuestionGroup",new { questionGroupId = model.Id});
                }
              
                TempData["updategroupsuccess"] = "Cập nhật thành công";
                return RedirectToAction("Index");




            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await SetViewBag();
                TempData["error"] = "Có lỗi xảy ra";
                return View(model.Id);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddQuestionGroup(QuestionGroupViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["error"] = "Có lỗi xảy ra";
                    await SetViewBag();
                    return View(model.CategoryId);
                }
                 var user = await _userManager.GetUserAsync(this.User);
                if(user == null)
                {
                    return View("error.cshtml");
                }
                var output = await _questionBank.AddQuestionBank(model, user.Id);

                if (!output)
                {
                    TempData["error"] = "Có lỗi xảy ra";
                    await SetViewBag();
                    return View(model.CategoryId);
                }

                TempData["success"] = "Thêm mới thành công";
                await SetViewBag();
                return View(model.CategoryId);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                TempData["error"] = "Có lỗi xảy ra";
                return View();
            }
        }

        public async Task<IActionResult> DeleteGroup(Guid questionGroupId)
        {
            try
            {
                var result = await _questionBank.DeleteQuestionBank(questionGroupId);
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

        [HttpPost]
        public async Task<IActionResult> GetListQuestionGroup(FilterModel model)
        {
            try
            {
                var user = await _userManager.GetUserAsync(this.User);
                var questionGroup = await _questionBank.GetListQuestionGroup(model, user.Id);
                return PartialView(questionGroup);
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
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return;
            }
           
        }
    }
}