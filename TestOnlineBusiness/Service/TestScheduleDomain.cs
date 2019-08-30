using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using TestOnlineBase.Constant;
using TestOnlineBase.Helper;
using TestOnlineBase.Helper.PagingHelper;
using TestOnlineBase.Helper.RandomHelper;
using TestOnlineBusiness.Interface;
using TestOnlineEntity.Interface;
using TestOnlineEntity.Model.Entity;
using TestOnlineEntity.Model.ViewModel;
using TestOnlineModel.ViewModel.Admin;

namespace TestOnlineBusiness.Service
{
    public class TestScheduleDomain : ITestScheduleDomain
    {

        private readonly ITestOnlienUnitOfWork _unitOfWork;
        private readonly ILogger<TestScheduleDomain> _logger;
        private UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _sender;
        private readonly IHostingEnvironment _env;

        public TestScheduleDomain(ITestOnlienUnitOfWork unitOfWork, IHostingEnvironment env, IEmailSender sender, ILogger<TestScheduleDomain> logger, UserManager<ApplicationUser> userManager)
        {
            this._unitOfWork = unitOfWork;
            this._logger = logger;
            this._userManager = userManager;
            this._sender = sender;
            this._env = env;
           
        }

        public async Task<bool> AddSchedule(TestScheduleViewModel viewModel, string userId, CancellationToken cancellationToken = default)
        {
            try
            {
                var listTestSchedule = await _unitOfWork.TestSchedules.GetOne(x => x.TestCategoryId == viewModel.CategoryId && x.IsActive == true && x.Name == viewModel.Name);
                if (listTestSchedule != null)
                {
                    return false;
                }
                var testSchedule = new TestSchedule()
                {
                    Id = Guid.NewGuid(),
                    AllowViewAnswer = viewModel.AllowViewAnswer,
                    IsActive = true,
                    IsOpen = true,
                    CreatedBy = userId,
                    UpdatedBy = userId,
                    UpdatedDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    TestTime = viewModel.Time,
                    StartDate = viewModel.StartDate.AddDays(1),
                    EndDate = viewModel.EndDate.AddDays(1),
                    TestCategoryId = viewModel.CategoryId,
                    Percentage = viewModel.Percentage,
                    TotalQuestion = viewModel.TotalQuestion
                    
                };
                _unitOfWork.TestSchedules.Insert(testSchedule);
                return await _unitOfWork.CommitAsync() > 0;
            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public async Task<bool> CreatedListMember(CreateListMemberViewModel viewModel, string userId)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    foreach (var item in viewModel.ListMember)
                    {
                        var temp = new ScheduleUser()
                        {
                            Id = Guid.NewGuid(),
                            MemberId = item,
                            CreatedBy = userId,
                            CreatedDate = DateTime.Now,
                            Updatedby = userId,
                            UpdatedDate = DateTime.Now,
                            IsActive = true,
                            TestScheduleId = viewModel.ScheduleId
                        };
                        _unitOfWork.ScheduleUsers.Insert(temp);
                    }
                    await _unitOfWork.CommitAsync();
                    scope.Complete();
                }
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public async Task<bool> CreateExam(CreateExamViewModel viewModel,string userId)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var ran = new Random();
                    for(int i = 0; i < viewModel.TotalExam; i++)
                    {
                        List<Question> listQuestion = new List<Question>();
                        var temp = new Exam()
                        {
                            Id = Guid.NewGuid(),
                            TestScheduleId = viewModel.ScheduleId,
                            IsActive = true,
                            CreatedBy = userId,
                            CreatedDate = DateTime.Now,
                            UpdatedBy = userId,
                            UpdatedDate = DateTime.Now
                        };

                        _unitOfWork.Exams.Insert(temp);

                        foreach (var item in viewModel.QuestionGroupList)
                        {
                            var list1 = (await _unitOfWork.Questions.Get(x => x.QuestionGroupId == item.QuestionGroupId && x.IsActive == true)).ToList();
                            
                            for(int i1 = 0; i1 < item.TotalQuestion; i1++)
                            {
                                var index = ran.Next(list1.Count());
                                var randItem = list1[index];
                                listQuestion.Add(randItem);
                                list1.RemoveAt(index);
                            }
                            

                        }
                        var list2 = RandomQuestion<Question>.Randomize(listQuestion);
                        for (int i2 = 0; i2 < list2.Count; i2++)
                        {
                            var examDetail = new ExamDetail()
                            {
                                Id = Guid.NewGuid(),
                                ExamId = temp.Id,
                                IsActive = true,
                                CreatedBy = userId,
                                CreatedDate = DateTime.Now,
                                UpdatedBy = userId,
                                UpdatedDate = DateTime.Now,
                                QuestionId = list2[i2].Id,
                                QuestionSequence = i2
                            };
                            _unitOfWork.ExamDetails.Insert(examDetail);
                        }
                      var result =  await _unitOfWork.CommitAsync();
                      
                     
                    }
                    scope.Complete();
                }
                return true;
            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<Exam>> GetListExam(Guid scheduleId)
        {
            try
            {
                var listExam = await _unitOfWork.Exams.Get(x => x.TestScheduleId == scheduleId && x.IsActive == true);
                return listExam;

            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<IEnumerable<ExamDetailViewModel>> GetListExamDetail(Guid examId, string userId)
        {
            try
            {
                List<ExamDetailViewModel> list = new List<ExamDetailViewModel>();
                var examDetails = (await _unitOfWork.ExamDetails.Get(x => x.IsActive == true && x.CreatedBy == userId && x.ExamId == examId)).Select(x => new {
                    QuestionId = x.QuestionId,
                    QuestionSequence = x.QuestionSequence,
                    
                }).OrderBy(x=>x.QuestionSequence).ToList();

                

                foreach (var item in examDetails)
                {
                    var listAnswer = (await _unitOfWork.Answers.Get(x => x.IsActive == true && x.QuestionId == item.QuestionId)).Select(x => new AnswerViewModel2
                    {
                        AnswerId = x.Id,
                        AnswerDescript = x.Content,
                        IsCorrect = x.IsCorrect,
                        AnswerSequence = Number2String(x.Sequence+1, true)
                    }).OrderBy(x => x.AnswerSequence).ToList();
                    ExamDetailViewModel temp = new ExamDetailViewModel()
                    {
                        QuestionId = item.QuestionId,
                        QuestionName = (await _unitOfWork.Questions.GetById(item.QuestionId)).Description,
                        ListAnswer = listAnswer
                    };
                    list.Add(temp);
                }


                return list;

            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null; 
            }
        }

        public async Task<IEnumerable<ApplicationUser>> GetListMemberSchedule(Guid scheduleId)
        {
            try
            {
                var listScheduleUser = (await _unitOfWork.ScheduleUsers.Get(x => x.TestScheduleId == scheduleId && x.IsActive == true)).
                    Select(x=>new {
                        MemberId = x.MemberId
                    }
                    );
                List<ApplicationUser> list = new List<ApplicationUser>();
                foreach (var item in listScheduleUser)
                {
                    var user = await _userManager.FindByIdAsync(item.MemberId);
                    list.Add(user);
                }
                return list;
            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<IEnumerable<QuestionBankInfoViewModel>> GetListQuestionBankInfo(Guid categoryId)
        {
            try
            {
                SqlParameter[] prams =
                {
                        new SqlParameter {ParameterName = "@categoryId",Value = categoryId ,DbType = DbType.Guid}
                };
                var result = await _unitOfWork.QuestionBankInfoViewModels.Get("sp_GetQuestionBankInfo", prams);
                return result;
            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<IEnumerable<TestScheduleViewModel>> GetListSchedule(FilterModel filter, string userId)
        {
            try
            {
                if (filter == null)
                {
                    filter = new FilterModel();
                }

                if (filter.Filter == null || filter.Filter.Count == 0)
                {
                    filter.Filter = new List<FilterTypeModel>() { new FilterTypeModel() { Field = Constant.Filter.ScheduleFilterDefault, IsActive = true } };
                }

                if (filter.Sort == null || filter.Sort.Count == 0 || string.IsNullOrEmpty(filter.Sort[0].Field))
                {
                    filter.Sort = new List<SortTypeModel>
                    {
                         new SortTypeModel {Field = Constant.Filter.ScheduleSortDefault, Asc =  false, IsActive = true}
                    };
                }

                var filterData = ApiUtils.ListToDataTable(filter.Filter);
                var sortData = ApiUtils.ListToDataTable(filter.Sort);

                var skip = filter.Skip ?? 0;
                var take = filter.Take ?? Constant.Filter.ScheduleTakeDefault;
                var isExport = filter.IsExport ?? false;
                if (!string.IsNullOrEmpty(filter.MultipeFilter))
                {
                    filterData = null;
                }
                SqlParameter[] prams =
                {
                    new SqlParameter{ParameterName = "@filter", Value = filterData , SqlDbType = SqlDbType.Structured,TypeName = "dbo.FilterType"},
                    new SqlParameter {ParameterName = "@sort",Value = sortData, SqlDbType = SqlDbType.Structured,TypeName = "dbo.SortType"},
                    new SqlParameter {ParameterName = "@skip",Value = skip ,DbType = DbType.Int32},
                    new SqlParameter {ParameterName = "@take",Value = take,DbType = DbType.Int32},
                    new SqlParameter {ParameterName = "@multipeFilter",Value = filter.MultipeFilter as Object ?? DBNull.Value,DbType = DbType.String },
                    new SqlParameter {ParameterName = "@isExport",Value = isExport,DbType = DbType.Boolean},
                    new SqlParameter {ParameterName = "@userId",Value = userId,DbType = DbType.String}

                };
                var source = await _unitOfWork.TestScheduleViewModels.Get(Constant.StoreProcedure.GET_TEST_SCHEDULE, prams);
                return source;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<TestSchedule> GetSchedule(Guid scheduleId)
        {
            try
            {
                var testschedule = await _unitOfWork.TestSchedules.GetById(scheduleId);
                return testschedule;
            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<bool> SendEmail(CreateListMemberViewModel viewModel, string userId)
        {
            try
            {
                var rand = new Random();
                
                var listExam = (await _unitOfWork.Exams.Get(x => x.TestScheduleId == viewModel.ScheduleId && x.IsActive == true && x.CreatedBy == userId)).
                    Select(x=> new {
                        ExamId = x.Id
                    }).ToList();
                using(var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    foreach (var item in viewModel.ListMember)
                    {
                        var index = rand.Next(listExam.Count());
                        var randItem = listExam[index];
                        var userExam = new ExamUser()
                        {
                            Id = Guid.NewGuid(),
                            ExamId = randItem.ExamId,
                            ScheduleId = viewModel.ScheduleId,
                            MemberId = item,
                            IsActive = true,
                            CreatedBy = userId,
                            CreatedDate = DateTime.Now,
                            Updatedby = userId,
                            UpdatedDate = DateTime.Now
                         

                        };
                        _unitOfWork.ExamUsers.Insert(userExam);
                    }
                    await _unitOfWork.CommitAsync();
                    scope.Complete();
                }

                var schedule = await _unitOfWork.TestSchedules.GetById(viewModel.ScheduleId);
                var path = _env.WebRootPath+"\\Template\\InfoExamTemplate.html";
                foreach (var item in viewModel.ListMember)
                {
                    var member = await _userManager.FindByIdAsync(item);
                   
                    string message = System.IO.File.ReadAllText(path);
                    message = message.Replace("{{ScheduleName}}", schedule.Name);
                    message = message.Replace("{{TotalTime}}", schedule.TestTime.ToString());
                    message = message.Replace("{{StartDate}}", schedule.StartDate.ToString("dd/MM/yyyy"));
                    message = message.Replace("{{EndDate}}", schedule.StartDate.ToString("dd/MM/yyyy"));
                    message = message.Replace("{{UserName}}", member.UserName);
                    message = message.Replace("{{PassWord}}", member.MemberPass);

                    await _sender.SendEmailAsync(member.Email, "TestOnline - Thư mời tham dự kì thi ", message);
                }
                return true;
            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteMemberSchedule(DeleteMemberViewModel viewModel)
        {
            try
            {
                var item = await _unitOfWork.ScheduleUsers.GetOne(x => x.TestScheduleId == viewModel.ScheduleId && x.MemberId == viewModel.MemberId);
                item.IsActive = false;
                _unitOfWork.ScheduleUsers.Update(item);
                return await _unitOfWork.CommitAsync() > 0;
            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        private string Number2String(int? number, bool isCaps)

        {

            Char c = (Char)((isCaps ? 65 : 97) + (number - 1));

            return c.ToString();

        }

        public async Task<IEnumerable<UserScheduleViewModel>> GetListUserSchedule(FilterModel filter, string userId)
        {
            try
            {
                if (filter == null)
                {
                    filter = new FilterModel();
                }

                if (filter.Filter == null || filter.Filter.Count == 0)
                {
                    filter.Filter = new List<FilterTypeModel>() { new FilterTypeModel() { Field = Constant.Filter.ScheduleFilterDefault, IsActive = true } };
                }

                if (filter.Sort == null || filter.Sort.Count == 0 || string.IsNullOrEmpty(filter.Sort[0].Field))
                {
                    filter.Sort = new List<SortTypeModel>
                    {
                         new SortTypeModel {Field = Constant.Filter.ScheduleSortDefault, Asc =  false, IsActive = true}
                    };
                }

                var filterData = ApiUtils.ListToDataTable(filter.Filter);
                var sortData = ApiUtils.ListToDataTable(filter.Sort);

                var skip = filter.Skip ?? 0;
                var take = filter.Take ?? Constant.Filter.ScheduleTakeDefault;
                var isExport = filter.IsExport ?? false;
               
                SqlParameter[] prams =
                {
                    new SqlParameter{ParameterName = "@filter", Value = filterData , SqlDbType = SqlDbType.Structured,TypeName = "dbo.FilterType"},
                    new SqlParameter {ParameterName = "@sort",Value = sortData, SqlDbType = SqlDbType.Structured,TypeName = "dbo.SortType"},
                    new SqlParameter {ParameterName = "@skip",Value = skip ,DbType = DbType.Int32},
                    new SqlParameter {ParameterName = "@take",Value = take,DbType = DbType.Int32},
                    new SqlParameter {ParameterName = "@multipeFilter",Value = filter.MultipeFilter as Object ?? DBNull.Value,DbType = DbType.String },
                    new SqlParameter {ParameterName = "@isExport",Value = isExport,DbType = DbType.Boolean},
                    new SqlParameter {ParameterName = "@userId",Value = userId,DbType = DbType.String}

                };
                var source = await _unitOfWork.UserscheduleViewModels.Get(Constant.StoreProcedure.GET_USER_SCHEDULE, prams);
                return source;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<IEnumerable<ExamDetailViewModel>> GetUserListExamDetail(Guid examId)
        {
            try
            {
                List<ExamDetailViewModel> list = new List<ExamDetailViewModel>();
                var userexam = await _unitOfWork.ExamUsers.GetById(examId);
                var examDetails = (await _unitOfWork.ExamDetails.Get(x => x.IsActive == true && x.ExamId == userexam.ExamId)).Select(x => new {
                    QuestionId = x.QuestionId,
                    QuestionSequence = x.QuestionSequence,
                    ExamId = x.ExamId

                }).OrderBy(x => x.QuestionSequence).ToList();



                foreach (var item in examDetails)
                {
                    var listAnswer = (await _unitOfWork.Answers.Get(x => x.IsActive == true && x.QuestionId == item.QuestionId)).Select(x => new AnswerViewModel2
                    {
                        AnswerId = x.Id,
                        AnswerDescript = x.Content,
                        IsCorrect = x.IsCorrect,
                        AnswerSequence = Number2String(x.Sequence + 1, true)
                    }).OrderBy(x => x.AnswerSequence).ToList();
                    ExamDetailViewModel temp = new ExamDetailViewModel()
                    {
                        ExamId = item.ExamId,
                        QuestionId = item.QuestionId,
                        QuestionName = (await _unitOfWork.Questions.GetById(item.QuestionId)).Description,
                        QuestionTypeKey = (await _unitOfWork.Questions.GetById(item.QuestionId)).QuestionTypeKey,
                        ListAnswer = listAnswer
                    };
                    list.Add(temp);
                }


                return list;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<bool> UpdateAccessExam(Guid examId)
        {
            try
            {
                var exam = await _unitOfWork.ExamUsers.GetById(examId);
                //var exam = await _unitOfWork.ExamUsers.GetOne(x => x.ExamId == examId && x.MemberId == userId);
                exam.IsAccess = true;
                _unitOfWork.ExamUsers.Update(exam);
                return await _unitOfWork.CommitAsync() > 0;
            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public async Task<bool> AddAnswerExamUser(UserAnswerViewModel viewModel, string userId)
        {
            try
            {
                var userExam = await _unitOfWork.ExamUsers.GetOne(x => x.ExamId == viewModel.ExamId && x.MemberId == userId);
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    userExam.IsSubmit = true;
                    _unitOfWork.ExamUsers.Update(userExam);

                    foreach (var item in viewModel.ListAnswer)
                    {
                        var answerExamUser = new AnswerExamUser()
                        {
                            Id = Guid.NewGuid(),
                            ExamId = viewModel.ExamId,
                            AnswerId = item.AnswerId,
                            QuestionId = item.QuestionId,
                            IsActive = true,
                            CreatedBy = userId,
                            CreatedDate = DateTime.Now,
                            UpdatedBy = userId,
                            UpdatedDate = DateTime.Now
                        };
                        _unitOfWork.AnswerExamUsers.Insert(answerExamUser);
                    }
                    await _unitOfWork.CommitAsync();
                    scope.Complete();
                    
                }


                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }
    }
}
