using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestOnlineBase.Helper.PagingHelper;
using TestOnlineModel.ViewModel.Admin;

namespace TestOnlineBusiness.Interface
{
    public interface ITestScheduleDomain
    {
        Task<bool> AddSchedule(TestScheduleViewModel viewModel, string userId, CancellationToken cancellationToken = default(CancellationToken));
        Task<IEnumerable<TestScheduleViewModel>> GetListSchedule(FilterModel filter, string userId);

       
    }
}
