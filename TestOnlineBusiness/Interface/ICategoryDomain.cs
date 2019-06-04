using System;
using System.Collections.Generic;
using TestOnlineModel.ViewModel.Admin;
using System.Threading.Tasks;
using System.Threading;

namespace TestOnlineBusiness.Interface
{
    public interface ICategoryDomain
    {
        Task<IEnumerable<TestCategoryViewModel>> GetCategory();
        Task<bool> UpdateCategory(Guid categoryId,TestCategoryViewModel viewModel, string userId, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> CreateCategory(TestCategoryViewModel viewModel, string userId, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> DeleteCategory(Guid categoryId, CancellationToken cancellationToken = default(CancellationToken));
        Task<TestCategoryViewModel> GetCategoryDetail(Guid categoryId);
         
    }
}
