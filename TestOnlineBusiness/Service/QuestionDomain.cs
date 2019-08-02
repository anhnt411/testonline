using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using TestOnlineBase.Helper.FileHelper;
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
                                    UpdatedDate = DateTime.Now
                                };
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
    }
}
