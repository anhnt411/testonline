﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using TestOnlineBusiness.Interface;
using TestOnlineEntity.Interface;
using TestOnlineEntity.Model.Entity;
using TestOnlineModel.ViewModel.Admin;

namespace TestOnlineBusiness.Service
{
    public class QuestionDomain:IQuestionDomain
    {
        private readonly ITestOnlienUnitOfWork _unitOfWork;
        private readonly ILogger<QuestionDomain> _logger;

        public QuestionDomain(ITestOnlienUnitOfWork unitOfWork,ILogger<QuestionDomain> logger)
        {
            this._unitOfWork = unitOfWork;
            this._logger = logger;
        }

        public async Task<bool> AddQuestion(QuestionViewModel viewModel,string userId, CancellationToken cancellationToken = default)
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
                   var result =  await _unitOfWork.CommitAsync();

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
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        private bool CheckQuestionValid(QuestionViewModel viewModel)
        {
            if(viewModel.QuestionTypeKey == 1)
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
                if(countIsCorrect == 0)
                {
                    return false;
                }
                if(countIsCorrect > 1)
                {
                    return false;
                }
                return true;
            }
            if(viewModel.QuestionTypeKey == 2)
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
    }
}
