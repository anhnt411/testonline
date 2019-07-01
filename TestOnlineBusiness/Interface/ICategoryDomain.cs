using System;
using System.Collections.Generic;
using TestOnlineModel.ViewModel.Admin;
using System.Threading.Tasks;
using System.Threading;
using TestOnlineBase.Helper.PagingHelper;
using TestOnlineEntity.Model.Entity;
using System.Linq.Expressions;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace TestOnlineBusiness.Interface
{
    public interface ICategoryDomain
    {
        Task<IEnumerable<TestCategoryViewModel>> GetCategory(FilterModel filter,string userId);
        Task<bool> UpdateCategory(Guid categoryId,TestCategoryViewModel viewModel, string userId,IFormFile file, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> CreateCategory(TestCategoryViewModel viewModel, string userId,IFormFile file, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> DeleteCategory(Guid categoryId, CancellationToken cancellationToken = default(CancellationToken));
        Task<TestCategoryViewModel> GetCategoryDetail(Guid categoryId);
         
    }
}
