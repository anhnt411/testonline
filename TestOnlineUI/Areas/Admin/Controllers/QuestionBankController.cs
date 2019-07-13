using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TestOnlineBase.Helper.PagingHelper;
using TestOnlineBusiness.Interface;
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

        [HttpPost]
        public async Task<IActionResult> AddQuestionGroup(QuestionGroupViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["error"] = "Có lỗi xảy ra";
                    return View();
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
                    return View();
                }

                TempData["success"] = "Thêm mới thành công";
                return View();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                TempData["error"] = "Có lỗi xảy ra";
                return View();
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