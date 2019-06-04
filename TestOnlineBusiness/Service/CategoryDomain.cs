using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestOnlineBusiness.Interface;
using TestOnlineEntity.Interface;
using TestOnlineEntity.Model.Entity;
using TestOnlineModel.ViewModel.Admin;

namespace TestOnlineBusiness.Service
{
    public class CategoryDomain : ICategoryDomain
    {
        private readonly ITestOnlienUnitOfWork _unitOfWork;
        private readonly ILogger<CategoryDomain> _logger;

        public CategoryDomain(ITestOnlienUnitOfWork unitOfWork,ILogger<CategoryDomain> logger)
        {
            this._unitOfWork = unitOfWork;
            this._logger = logger;
        }

        public async Task<bool> CreateCategory(TestCategoryViewModel viewModel, string userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var category = new TestCategory()
                {
                    Id = Guid.NewGuid(),
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    Image = viewModel.Image,
                    CreatedDate = DateTime.Now,
                    CreatedBy = userId,
                    Status = true
                };
                _unitOfWork.TestCategories.Insert(category);
                return await _unitOfWork.CommitAsync(cancellationToken) > 0;

            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteCategory(Guid categoryId, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                await _unitOfWork.TestCategories.Delete(categoryId);
                return await _unitOfWork.CommitAsync(cancellationToken) > 0;

            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<TestCategoryViewModel>> GetCategory()
        {
            try
            {
                var output = await _unitOfWork.TestCategories.Get(x => x.Status == true);
                var result = output.Select(x=> new TestCategoryViewModel() {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Image = x.Image

                });
                return result;

            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null; 
            }
        }

        public async Task<TestCategoryViewModel> GetCategoryDetail(Guid categoryId)
        {
            try
            {
                var output = await _unitOfWork.TestCategories.GetById(categoryId);
                return new TestCategoryViewModel()
                {
                    Id = output.Id,
                    Name = output.Name,
                    Description = output.Description,
                    Image = output.Image
                };
            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<bool> UpdateCategory(Guid categoryId,TestCategoryViewModel viewModel, string userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var category = await _unitOfWork.TestCategories.GetById(categoryId);
                category.Name = viewModel.Name;
                category.Description = viewModel.Description;
                category.Image = viewModel.Image;
                category.ModifiedBy = userId;
                category.ModifiedDate = DateTime.Now;
                _unitOfWork.TestCategories.Update(category);
                return await _unitOfWork.CommitAsync(cancellationToken ) > 0;

            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }
    }
}
