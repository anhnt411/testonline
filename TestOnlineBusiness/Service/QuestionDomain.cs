using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using TestOnlineBase.Constant;
using TestOnlineBase.Helper.FileHelper;
using TestOnlineBase.Helper.PagingHelper;
using TestOnlineBusiness.Interface;
using TestOnlineEntity.Interface;
using TestOnlineEntity.Model.Entity;
using TestOnlineModel.ViewModel.Admin;

namespace TestOnlineBusiness.Service
{
    public class QuestionDomain : IQuestionDomain
    {
        private readonly ITestOnlienUnitOfWork _unitOfWork;
        private readonly ILogger<QuestionDomain> _logger;

        public QuestionDomain(ITestOnlienUnitOfWork unitOfWork, ILogger<QuestionDomain> logger)
        {
            this._unitOfWork = unitOfWork;
            this._logger = logger;
        }

        public async Task<bool> AddListQuestion(Guid questiongroupId, IFormFile file, string userId, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = false;
              
                    using (var stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream, cancellationToken);
                        using (var package = new ExcelPackage(stream))
                        {
                            ExcelWorksheet worksheet = package.Workbook.Worksheets["Sheet1"];

                            var rowCount = worksheet.Dimension.Rows;

                            var list = GetListRowQuestion(package);
                            var listimg = GetListImage(package);
                            var listQuestion = await GetListQuestion(list, listimg, package, questiongroupId);
                            result = await SaveListquestion(listQuestion, userId);

                        }


                    }




                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }

        }

        public async Task<bool> AddQuestion(QuestionViewModel viewModel, string userId, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!CheckQuestionValid(viewModel))
                {
                    return false;
                }
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var question = new Question()
                    {
                        Id = Guid.NewGuid(),
                        IsActive = true,
                        CreatedBy = userId,
                        CreatedDate = DateTime.Now,
                        UpdatedBy = userId,
                        UpdatedDate = DateTime.Now,
                        Description = viewModel.Description,
                        QuestionGroupId = viewModel.QuestionGroupId,
                        QuestionTypeKey = viewModel.QuestionTypeKey

                    };
                    _unitOfWork.Questions.Insert(question);
                    var result = await _unitOfWork.CommitAsync();

                    var listAnswer = viewModel.Answers;
                    foreach (var item in listAnswer)
                    {
                        var answer = new Answer()
                        {
                            Id = Guid.NewGuid(),
                            Sequence = item.Sequence,
                            Content = item.Description,
                            IsActive = true,
                            QuestionId = question.Id,
                            IsCorrect = item.IsCorrect,
                            CreatedBy = userId,
                            CreatedDate = DateTime.Now,
                            UpdatedBy = userId,
                            UpdatedDate = DateTime.Now
                        };
                        _unitOfWork.Answers.Insert(answer);
                        await _unitOfWork.CommitAsync();
                    }

                    scope.Complete();
                }
                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        private bool CheckQuestionValid(QuestionViewModel viewModel)
        {
            if (viewModel.QuestionTypeKey == 1)
            {
                var listAnswer = viewModel.Answers;
                var countIsCorrect = 0;
                foreach (var item in listAnswer)
                {
                    if (item.IsCorrect)
                    {
                        countIsCorrect++;
                    }
                }
                if (countIsCorrect == 0)
                {
                    return false;
                }
                if (countIsCorrect > 1)
                {
                    return false;
                }
                return true;
            }
            if (viewModel.QuestionTypeKey == 2)
            {
                var listAnswer = viewModel.Answers;
                var countIsCorrect = 0;
                foreach (var item in listAnswer)
                {
                    if (item.IsCorrect)
                    {
                        countIsCorrect++;
                    }
                }
                if (countIsCorrect == 0)
                {
                    return false;
                }
                if (countIsCorrect <= 1)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        private static List<int> GetListRowQuestion(ExcelPackage file)
        {
            ExcelWorksheet worksheet = file.Workbook.Worksheets["Sheet1"];
            var listRowQuestion = new List<int>();

            var rowCount = worksheet.Dimension.Rows;

            for (int row = 2; row <= rowCount; row++)
            {
                if (worksheet.Cells[row, 1].Value != null)
                {
                    listRowQuestion.Add(row);
                }


            }
            return listRowQuestion;
        }

        private List<ExcelPicture> GetListImage(ExcelPackage package)
        {
            var listImg = new List<ExcelPicture>();
            var count = package.Workbook.Worksheets[0].Drawings.Count;
            for (int i = 0; i < count; i++)
            {
                var img = package.Workbook.Worksheets[0].Drawings[i] as ExcelPicture;
                listImg.Add(img);
            }
            return listImg;
        }



     

        private async Task<List<QuestionViewModel>> GetListQuestion(List<int> list, List<ExcelPicture> listPicture, ExcelPackage file, Guid questionGroupId)
        {
            try
            {
                List<QuestionViewModel> listQUestion = new List<QuestionViewModel>();

                var total = file.Workbook.Worksheets[0].Dimension.Rows;
                var contentFile = file.Workbook.Worksheets[0];

                while (list.Count != 0)
                {
                    if (list.Count == 1)
                    {
                        int isCorrectCount = 0;

                        QuestionViewModel questionModel = new QuestionViewModel(
                          
                            );
                        List<AnswerViewModel> listAnswer = new List<AnswerViewModel>();
                        

                        for (int i = list[0]; i <= total; i++)
                        {

                            if (i == list[0])
                            {
                                var question = contentFile.Cells[i, 2].Value.ToString();
                                var description = $"<p>{question}</p>";
                                questionModel.Description = description;
                                questionModel.QuestionGroupId = questionGroupId;
                                questionModel.QuestionTypeKey = 1;
                                

                                if (listPicture.Exists(x => x.To.Row == (i - 1)))
                                {
                                    var picture = listPicture.Where(x => x.To.Row == (i - 1)).FirstOrDefault();
                                    if (picture != null)
                                    {
                                        var questionImgName = UploadImageFile.SaveImg(picture.Image);
                                        description += $"<img src = '/img/{questionImgName}' width = 400; height = 400;/>";
                                        questionModel.Description = description;
                                    }

                                }
                            }
                            else
                            {
                                var answer1 = new AnswerViewModel();
                                var answer = contentFile.Cells[i, 2].Value.ToString();
                                var answerDescription = $"<p>{answer}</p>";
                                answer1.Description = answerDescription;
                                if (listPicture.Exists(x => x.To.Row == (i - 1)))
                                {
                                    var picture = listPicture.Where(x => x.To.Row == (i - 1)).FirstOrDefault();
                                    if (picture != null)
                                    {
                                        var answerImgName = UploadImageFile.SaveImg(picture.Image);
                                        answerDescription += $"<img src = '/img/{answerImgName}' width = 400; height = 400;/>";
                                        answer1.Description = answerDescription;
                                    }


                                }
                                var iscorrect = (contentFile.Cells[i, 3].Value.ToString().ToLower() == "d") ? true : false;
                                answer1.IsCorrect = iscorrect;
                                listAnswer.Add(answer1);
                                if (iscorrect) isCorrectCount++;
                                



                            }





                        }
                        if (isCorrectCount < 1)
                        {
                            return null;
                        }
                        if (isCorrectCount > 1)
                        {
                            questionModel.QuestionTypeKey = 2;
                           
                        }
                        questionModel.Answers = listAnswer;
                        listQUestion.Add(questionModel);

                    }


                    else
                    {
                        int isCorrectCount = 0;

                        QuestionViewModel questionModel = new QuestionViewModel();
                        List<AnswerViewModel> listAnswer = new List<AnswerViewModel>();

                        for (int i = list[0]; i < list[1]; i++)
                        {

                            if (i == list[0])
                            {
                                var question = contentFile.Cells[i, 2].Value.ToString();
                                var description = $"<p>{question}</p>";
                                questionModel.Description = description;
                                questionModel.QuestionGroupId = questionGroupId;
                                questionModel.QuestionTypeKey = 1;

                                if (listPicture.Exists(x => x.To.Row == (i - 1)))
                                {
                                    var picture = listPicture.Where(x => x.To.Row == (i - 1)).FirstOrDefault();
                                    if (picture != null)
                                    {
                                        var questionImgName = UploadImageFile.SaveImg(picture.Image);
                                        description += $"<img src = '/img/{questionImgName}' width = 400; height = 400;/>";
                                        questionModel.Description = description;
                                    }

                                }
                            }
                            else
                            {
                                var answer1 = new AnswerViewModel();
                                var answer = contentFile.Cells[i, 2].Value.ToString();
                                var answerDescription = $"<p>{answer}</p>";
                                answer1.Description = answerDescription;
                                if (listPicture.Exists(x => x.To.Row == (i - 1)))
                                {
                                    var picture = listPicture.Where(x => x.To.Row == (i - 1)).FirstOrDefault();
                                    if (picture != null)
                                    {
                                        var answerImgName = UploadImageFile.SaveImg(picture.Image);
                                        answerDescription += $"<img src = '/img/{answerImgName}' width = 400; height = 400;/>";
                                        answer1.Description = answerDescription;
                                    }


                                }
                                var iscorrect = (contentFile.Cells[i, 3].Value.ToString().ToLower() == "d") ? true : false;
                                answer1.IsCorrect = iscorrect;
                                listAnswer.Add(answer1);
                                if (iscorrect) isCorrectCount++;
                              


                            }




                        }
                        if (isCorrectCount < 1)
                        {
                            return null;
                        }
                        if (isCorrectCount > 1)
                        {
                            questionModel.QuestionTypeKey = 2;
                            
                        }
                        questionModel.Answers = listAnswer;
                        listQUestion.Add(questionModel);



                    }
                    list.RemoveAt(0);

                }


                return listQUestion;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        private async Task<bool> SaveListquestion(List<QuestionViewModel> listquestion,string userId)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {

                    foreach (var item in listquestion)
                    {
                        if (!CheckQuestionValid(item))
                        {
                            return false;
                        }

                        
                        
                            var question = new Question()
                            {
                                Id = Guid.NewGuid(),
                                IsActive = true,
                                CreatedBy = userId,
                                CreatedDate = DateTime.Now,
                                UpdatedBy = userId,
                                UpdatedDate = DateTime.Now,
                                Description = item.Description,
                                QuestionGroupId = item.QuestionGroupId,
                                QuestionTypeKey = item.QuestionTypeKey

                            };
                            _unitOfWork.Questions.Insert(question);


                            var listAnswer = item.Answers;
                            int i = 0;
                            foreach (var item1 in listAnswer)
                            {

                            var answer = new Answer()
                            {
                                Id = Guid.NewGuid(),
                                Content = item1.Description,
                                IsActive = true,
                                QuestionId = question.Id,
                                IsCorrect = item1.IsCorrect,
                                CreatedBy = userId,
                                CreatedDate = DateTime.Now,
                                UpdatedBy = userId,
                                Sequence = i,
                                UpdatedDate = DateTime.Now
                                };
                            i++;
                                _unitOfWork.Answers.Insert(answer);

                            }
                            var result = await _unitOfWork.CommitAsync();
                            
                    
                         
                       

                    }
                    
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

        public async Task<IEnumerable<QuestionListViewModel>> GetListQuestion(FilterModel filter, Guid questionGroupId, string userId)
        {
            try
            {
                if (filter == null)
                {
                    filter = new FilterModel();
                }

                if (filter.Filter == null || filter.Filter.Count == 0)
                {
                    filter.Filter = new List<FilterTypeModel>() { new FilterTypeModel() { Field = Constant.Filter.QuestionFilterDefault, IsActive = true } };
                }

                if (filter.Sort == null || filter.Sort.Count == 0 || string.IsNullOrEmpty(filter.Sort[0].Field))
                {
                    filter.Sort = new List<SortTypeModel>
                    {
                         new SortTypeModel {Field = Constant.Filter.QuestionSortDefault, Asc =  false, IsActive = true}
                    };
                }

                var filterData = ApiUtils.ListToDataTable(filter.Filter);
                var sortData = ApiUtils.ListToDataTable(filter.Sort);

                var skip = filter.Skip ?? 0;
                var take = filter.Take ?? Constant.Filter.QuestionTakeDefault;
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
                    new SqlParameter {ParameterName = "@userId",Value = userId,DbType = DbType.String},
                    new SqlParameter {ParameterName = "@questionGroupId",Value = questionGroupId,DbType = DbType.Guid}
                };
                var source = await _unitOfWork.QuestionListViewModels.Get(Constant.StoreProcedure.GET_QUESTION_LIST, prams);
                return source;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<QuestionDetailViewModel> GetQuestionDetail(Guid questionId)
        {
            try
            {
                var question = await _unitOfWork.Questions.GetById(questionId);
                var answer = await _unitOfWork.Answers.Get(x => x.QuestionId == question.Id && x.IsActive == true);
                var result = new QuestionDetailViewModel()
                {
                    Id = questionId,
                    Description = question.Description,
                    QuestionGroupId = question.QuestionGroupId,
                    QuestionTypeKey = question.QuestionTypeKey,
                    Answers = answer.Select(x => new AnswerDetailViewModel()
                    {
                        AnswerId = x.Id,
                        Sequence = x.Sequence,
                        CreatedDate = x.CreatedDate,
                        Content = x.Content,
                        IsCorrect = x.IsCorrect
                    }).OrderBy(x => x.Sequence)
                };
                return result;

            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<bool> UpdateQuestion(Guid questionid, QuestionViewModel model,string userId, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!CheckQuestionValid(model))
                {
                    return false;
                }
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var question = await _unitOfWork.Questions.GetById(questionid);
                    var listanswer = await _unitOfWork.Answers.Get(x => x.QuestionId == questionid && x.IsActive == true);
                    foreach (var item in listanswer)
                    {
                        item.IsActive = false;
                        _unitOfWork.Answers.Update(item);
                    }
                    question.QuestionGroupId = model.QuestionGroupId;
                    question.QuestionTypeKey = model.QuestionTypeKey;
                    question.Description = model.Description;
                    question.UpdatedDate = DateTime.Now;
                    _unitOfWork.Questions.Update(question);

                    var listAnswer = model.Answers;
                    foreach (var item in listAnswer)
                    {
                        var answer = new Answer()
                        {
                            Id = Guid.NewGuid(),
                            Sequence = item.Sequence,
                            Content = item.Description,
                            IsActive = true,
                            QuestionId = question.Id,
                            IsCorrect = item.IsCorrect,
                            CreatedBy = userId,
                            CreatedDate = DateTime.Now,
                            UpdatedBy = userId,
                            UpdatedDate = DateTime.Now
                        };
                        _unitOfWork.Answers.Insert(answer);
                      
                    }
                     await _unitOfWork.CommitAsync();


                    scope.Complete();
                }
                return true;
               
            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteQuestion(Guid questionId, CancellationToken cancellationToken = default)
        {
            try
            {
                var question = await _unitOfWork.Questions.GetById(questionId);
                question.IsActive = false;
                 _unitOfWork.Questions.Update(question);
                return await _unitOfWork.CommitAsync() > 0;
            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        //public async Task<QuestionDetailViewModel> GetQuestionContainer(Guid questionId)
        //{
        //    try
        //    {
        //        SqlParameter[] prams =
        //      {
        //            new SqlParameter{ParameterName = "@questionId", Value = questionId , DbType = DbType.Guid}

        //        };



        //        var source = await _unitOfWork.QuestionContainerViewModels.Get(Constant.StoreProcedure.GET_QUESTION_CONTAINER2, prams);
        //        if (!source.Any())
        //        {
        //            return null;
        //        }


        //        QuestionDetailViewModel ck = new QuestionDetailViewModel();
        //        ck.Id = source.FirstOrDefault().Id;
        //        ck.QuestionGroupId = source.FirstOrDefault().QuestionGroupId;
        //        ck.QuestionTypeKey = source.FirstOrDefault().QuestionTypeKey;
        //        ck.Description = source.FirstOrDefault().Description;
        //        //ck.Answers = source.GroupBy(a => new { a.AnswerId, a.Content, a.IsCorrect }).Select(b => new AnswerDetailViewModel
        //        //{
        //        //    AnswerId = b.Key.AnswerId,
        //        //    Content = b.Key.Content,
        //        //    IsCorrect = b.Key.IsCorrect
        //        //});
        //        return ck;

        //    }
        //    catch(Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message);
        //        return null;
        //    }
        //}
    }
}
