using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TestOnlineBase.Helper.PagingHelper;
using TestOnlineBusiness.Interface;
using TestOnlineEntity.Model.ViewModel;
using TestOnlineModel.ViewModel.Admin;

namespace TestOnlineUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class QuestionController : Controller
    {
        private readonly IQuestionBankDomain _bank;
        private readonly IQuestionDomain _question;

        private readonly ILogger<QuestionController> _logger;
        private readonly IUserDomain _user;
        private UserManager<ApplicationUser> _userManager;

        public QuestionController(IQuestionBankDomain bank, IQuestionDomain question, ILogger<QuestionController> logger, IUserDomain userDomain, UserManager<ApplicationUser> userManager)
        {
            this._bank = bank;
            this._question = question;
            this._logger = logger;
            this._user = userDomain;
            this._userManager = userManager;
        }

        public async Task<IActionResult> Index(Guid questionGroupId)
        {
            var user = await _userManager.GetUserAsync(this.User);
            var questionGroup = await _bank.GetQuestionGroupDetail(questionGroupId);
            if (questionGroup == null)
            {
                return View("error.cshtml");
            }

            ViewBag.QuestionGroup = questionGroup;
            var result = await _bank.GetAll(user.Id);
            ViewBag.ListQuestionGroup = result.ToList();

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Add(Guid groupQuestionId)
        {
            var user = await _userManager.GetUserAsync(this.User);
            var questionGroup = await _bank.GetQuestionGroupDetail(groupQuestionId);
           
            if (questionGroup == null)
            {
                return View("error.cshtml");
            }

            ViewBag.QuestionGroup = questionGroup;
            var result = await _bank.GetAll(user.Id);
            ViewBag.ListQuestionGroup = result.ToList();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddListQuestion(Guid groupQuestionId)
        {
            var user = await _userManager.GetUserAsync(this.User);
            var questionGroup = await _bank.GetQuestionGroupDetail(groupQuestionId);
            if (questionGroup == null)
            {
                return View("error.cshtml");
            }

            ViewBag.QuestionGroup = questionGroup;
            var result = await _bank.GetAll(user.Id);
            ViewBag.ListQuestionGroup = result.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(QuestionViewModel question)
        {
            try
            {
                var user = await _userManager.GetUserAsync(this.User);
                var result = await _question.AddQuestion(question, user.Id);
                TempData["success"] = "Thêm mới câu hỏi thành công";
                if (result)
                {
                    return Json(new
                    {
                        status = 1
                    });
                }


                return Json(new
                {
                    status = 0
                });
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddListQuestion(Guid questionGroupId, IFormFile file)
        {
            try
            {
                var user = await _userManager.GetUserAsync(this.User);
                var result = await _question.AddListQuestion(questionGroupId, file, user.Id);

                var questionGroup = await _bank.GetQuestionGroupDetail(questionGroupId);
                if (questionGroup == null)
                {
                    return View("error.cshtml");
                }

                ViewBag.QuestionGroup = questionGroup;
                var result1 = await _bank.GetAll(user.Id);
                ViewBag.ListQuestionGroup = result1.ToList();
                if (result)
                {
                    TempData["success"] = "Thêm mới danh sách câu hỏi thành công";

                }
                else
                {
                    TempData["error"] = "Xảy ra lỗi";
                }
                return View();



            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetListQuestion(FilterModel model, Guid questionGroupId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(this.User);
                var questions = await _question.GetListQuestion(model, questionGroupId, user.Id);
                return PartialView(questions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return View("Error.cshtml");
            }

        }

        [HttpGet]
        public async Task<IActionResult> UpdateQuestion(Guid id)
        {
            try
            {

                {
                    var user = await _userManager.GetUserAsync(this.User);
                    var question = await _question.GetQuestionDetail(id);
                    var questiongroup = await _bank.GetQuestionGroupDetail(question.QuestionGroupId);
                    ViewBag.Question = question;
                    ViewBag.ListQuestionGroup = await _bank.GetAll(user.Id);
                    if (question == null)
                    {

                        return View("Error.cshtml");
                    }

                    return View(question);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuestion(Guid questionId, QuestionViewModel question)
        {
            try
            {
                var user = await _userManager.GetUserAsync(this.User);
                var output = await _question.UpdateQuestion(questionId, question, user.Id);
              
                if (output)
                {
                    return Json(new
                    {
                        status = 1
                    });
                }


                return Json(new
                {
                    status = 0
                });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _question.DeleteQuestion(id);
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
            catch(Exception ex)
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