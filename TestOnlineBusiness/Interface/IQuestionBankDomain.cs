using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestOnlineBase.Helper.PagingHelper;
using TestOnlineModel.ViewModel.Admin;

namespace TestOnlineBusiness.Interface
{
    public interface IQuestionBankDomain
    {
        Task<bool> AddQuestionBank(QuestionGroupViewModel model,string userId);
        Task<IEnumerable<QuestionGroupViewModel>> GetListQuestionGroup(FilterModel filter, string userId);
    }
}
