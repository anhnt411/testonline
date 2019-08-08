using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestOnlineBase.Helper.PagingHelper;
using TestOnlineEntity.Model.Entity;
using TestOnlineModel.ViewModel.Admin;

namespace TestOnlineBusiness.Interface
{
    public interface IQuestionDomain
    {
        Task<IEnumerable<QuestionListViewModel>> GetListQuestion(FilterModel filter, Guid questionGroupId, string userId);

        //Task<QuestionDetailViewModel> GetQuestionContainer(Guid questionId);

        Task<QuestionDetailViewModel> GetQuestionDetail(Guid questionId);
        Task<bool> AddQuestion(QuestionViewModel viewModel,string userId, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> AddListQuestion(Guid questiongroupId, IFormFile file,string userId, CancellationToken cancellationToken = default(CancellationToken));
    }
}
