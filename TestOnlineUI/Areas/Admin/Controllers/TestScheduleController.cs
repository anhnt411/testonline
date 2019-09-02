using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
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
        private readonly ITestUnitDomain _unit;
        private readonly ITestMemberDomain _member;
        public TestScheduleController(ITestScheduleDomain schedule,ITestMemberDomain member,ICategoryDomain category,ITestUnitDomain unit, ILogger<TestScheduleController> logger, IUserDomain user, UserManager<ApplicationUser> userManager)
        {
            this._schedule = schedule;
            this._logger = logger;
            this._user = user;
            this._userManager = userManager;
            this._category = category;
            this._unit = unit;
            this._member = member;
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
        [HttpGet]
        public async Task<IActionResult> CreateExam(Guid scheduleId)
        {
            try
            {
                var schedule = await _schedule.GetSchedule(scheduleId);
                ViewBag.Schedule = schedule;
                var listInfo = await _schedule.GetListQuestionBankInfo(schedule.TestCategoryId);
                ViewBag.QuestionBankInfo = listInfo;
                return View();
            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateListMember(Guid scheduleId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(this.User);
                var schedule = await _schedule.GetSchedule(scheduleId);
                ViewBag.Schedule = schedule;
                var listUnit = await _unit.GetAll(user.Id);
                ViewBag.listUnit = listUnit;
                ViewBag.User = user;
                return View();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return View("error.cshtml");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetListMember(Guid unitId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(this.User);
                var filterModel = new FilterModel()
                {
                    Filter = null,
                    Sort = null,
                    IsExport = true
                };
                var listMember = await _member.GetListMember(filterModel, unitId, user.Id);
                return PartialView(listMember);
            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return View("error.cshtml");
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateExam(CreateExamViewModel viewModel)
        {
            try
            {
                var user = await _userManager.GetUserAsync(this.User);
                var result = await _schedule.CreateExam(viewModel, user.Id);
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
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Json(new {
                    status = 0
                });


            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateListMember(CreateListMemberViewModel viewModel)
        {
            try
            {
                var user = await _userManager.GetUserAsync(this.User);
                var result = await _schedule.CreatedListMember(viewModel, user.Id);
                if (result)
                {
                    TempData["success"] = "Thêm danh sách thí sinh  thành công";
                    return Json(new
                    {
                        status = 1
                    });
                }
                TempData["error"] = "Có lỗi xảy ra";
                return Json(new
                {
                  
                status = 0
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                TempData["error"] = "Có lỗi xảy ra";
                return Json(new
                {
                    status = 0
                });


            }
        }
        [HttpGet]
        public async Task<IActionResult> ViewExam(Guid scheduleId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(this.User);
                var listExam = await _schedule.GetListExam(scheduleId);
                var schedule = await _schedule.GetSchedule(scheduleId);
                ViewBag.Schedule = schedule;
                ViewBag.User = user;
                return View(listExam);
            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return View("error.cshtml");
            }
        }
        [HttpPost]
        public async Task<IActionResult> SendEmail(CreateListMemberViewModel viewModel)
        {
            try
            {
                var user = await _userManager.GetUserAsync(this.User);
                var result = await _schedule.SendEmail(viewModel, user.Id);
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
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Json(new
                {
                    status = 0
                }); ;
            }
        }

        [HttpGet]
        public async Task<IActionResult> ViewExamDetail(Guid examId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(this.User);
                var examDetails = await _schedule.GetListExamDetail(examId, user.Id);
                return View(examDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return View("error.cshtml");
            }
        }
       

        [HttpGet]
        public async Task<IActionResult> ViewListMemberDetail(Guid scheduleId)
        {
            try
            {
                try
                {
                 
                    var listMemberBySchedule = await _schedule.GetListMemberSchedule(scheduleId);
                    var schedule = await _schedule.GetSchedule(scheduleId);
                    ViewBag.Schedule = schedule;
                  
                    return View(listMemberBySchedule);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    return View("error.cshtml");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return View("error.cshtml");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMember(DeleteMemberViewModel viewModel)
        {
            try
            {
                var user = await _userManager.GetUserAsync(this.User);
                var result = await _schedule.DeleteMemberSchedule(viewModel);
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
            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Json(new
                {
                    status = 0
                });
            }
        }

        public async Task<IActionResult> DeleteSchedule(Guid scheduleId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(this.User);
                var result = await _schedule.DeleteSchedule(scheduleId);
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
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Json(new
                {
                    status = 0
                });
            }
        }

        public async Task<IActionResult> AdminViewSchedule(Guid scheduleId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(this.User);
                var result = await _schedule.GetAdminViewModel(scheduleId, user.Id);
                return View(result);
            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return View("error.cshtml");
            }
        }

        [HttpGet]
        public async Task<IActionResult> AdminViewListMember(Guid scheduleId)
        {
            try
            {
                var listMemberBySchedule = await _schedule.GetListMemberSchedule(scheduleId);
                var schedule = await _schedule.GetSchedule(scheduleId);
                ViewBag.Schedule = schedule;

                return View(listMemberBySchedule);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return View("error.cshtml");
            }
        }

        [HttpGet]
        public async Task<IActionResult> AdminViewListMemberAccess(Guid scheduleId)
        {
            try
            {
                var listMemberBySchedule = await _schedule.GetListMemberScheduleAccess(scheduleId);
                var schedule = await _schedule.GetSchedule(scheduleId);
                ViewBag.Schedule = schedule;

                return View(listMemberBySchedule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return View("error.cshtml");
            }
        }
        [HttpGet]
        public async Task<IActionResult> AdminViewListMemberNotAccess(Guid scheduleId)
        {
            try
            {
                var listMemberBySchedule = await _schedule.GetListMemberScheduleNotAccess(scheduleId);
                var schedule = await _schedule.GetSchedule(scheduleId);
                ViewBag.Schedule = schedule;

                return View(listMemberBySchedule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return View("error.cshtml");
            }
        }

        public async Task<IActionResult>  AdminViewListMemberPass(Guid scheduleId)
        {
            try
            {
                var listMemberBySchedule = await _schedule.GetListMemberSchedulePass(scheduleId);
                var schedule = await _schedule.GetSchedule(scheduleId);
                ViewBag.Schedule = schedule;

                return View(listMemberBySchedule);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return View("error.cshtml");
            }
        }

        [HttpGet]
        public async Task<IActionResult> AdminReviewUserExam(Guid examId,string memberId)
        {
            try
            {
                
                var result = await _schedule.ReviewUserExamDetail(examId, memberId);
                int correctQuestion = 0;
                foreach (var item in result)
                {
                    if (item.QuestionTrue == false || item.QuestionTrue == null)
                    {
                        correctQuestion++;
                    }
                }
                ViewBag.CorrectQuestion = result.Count() - correctQuestion;
                return View(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return View("error.cshtml");
            }
        }

        // [HttpPost]
        //public async Task<IActionResult> ExportToExcel(FilterExport viewModel)
        //{
        //    try
        //    {
        //        var user = await _userManager.GetUserAsync(this.User);
        //        var list = await _schedule.ExportToExcel(viewModel.ScheduleId, user.Id, viewModel.Key);
        //        var list1 = new List<dynamic>();
        //        int i = 1;
        //        foreach (var item in list)
        //        {
        //            list1.Add(new
        //            {
        //                STT = i,
        //                FullName = item.FullName,
        //                Email = item.Email,
        //                PhoneNumber = item.PhoneNumber,
        //                Address = item.Address
        //            });
        //            i++;
        //        }

        //        var stream = new MemoryStream();
        //        byte[] fileContents;

        //        using (var package = new ExcelPackage(stream))
        //        {
        //            var workSheet = package.Workbook.Worksheets.Add("Sheet1");
        //            workSheet.Cells.LoadFromCollection(list, true);
        //            fileContents = package.GetAsByteArray();
        //        }


        //        string excelName = $"UserList-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

        //        return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        //    }
        //    catch(Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message);
        //        return Json(new
        //        {
        //            status = 0
        //        }); 
        //    }
        //}


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