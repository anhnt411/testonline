using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestOnlineBase.Helper.PagingHelper;
using TestOnlineModel.ViewModel.Admin;

namespace TestOnlineBusiness.Interface
{
    public interface ITestUnitDomain
    {
        Task<IEnumerable<TestUnitViewModel>> GetUnit(FilterModel filter, string userId);
        Task<bool> UpdateUnit(Guid unitId, TestUnitViewModel viewModel, string userId,  CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> CreateUnit(TestUnitViewModel viewModel, string userId, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> DeleteUnit(Guid unitId, CancellationToken cancellationToken = default(CancellationToken));
        Task<TestUnitViewModel> GetUnitDetail(Guid? unitId);
        Task<IEnumerable<TestUnitViewModel>> GetAll(string userId);
    }
}
