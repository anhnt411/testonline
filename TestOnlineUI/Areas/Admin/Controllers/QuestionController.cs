﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        public QuestionController(IQuestionBankDomain bank, IQuestionDomain question,ILogger<QuestionController> logger,IUserDomain userDomain, UserManager<ApplicationUser> userManager)
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
                var result = await _question.AddQuestion(question,user.Id);
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
                }); ;
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
                var result = await _question.AddListQuestion(questionGroupId, file);
                return null;
            }catch(Exception ex)
            {
                return null;
            }
        }
    }
}