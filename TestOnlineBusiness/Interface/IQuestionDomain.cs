using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestOnlineModel.ViewModel.Admin;

namespace TestOnlineBusiness.Interface
{
    public interface IQuestionDomain
    {
        Task<bool> AddQuestion(QuestionViewModel viewModel,string userId, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> AddListQuestion(Guid questiongroupId, IFormFile file,string userId, CancellationToken cancellationToken = default(CancellationToken));
    }
}
