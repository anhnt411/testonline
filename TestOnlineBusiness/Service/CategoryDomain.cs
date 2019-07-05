using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestOnlineBase.Constant;
using TestOnlineBase.Helper.FileHelper;
using TestOnlineBase.Helper.PagingHelper;
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
        private readonly PageSize _pageSize;

        public CategoryDomain(ITestOnlienUnitOfWork unitOfWork,ILogger<CategoryDomain> logger)
        {
            this._unitOfWork = unitOfWork;
            this._logger = logger;
            
        }

        public async Task<bool> CreateCategory(TestCategoryViewModel viewModel, string userId,IFormFile file, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var exits = await _unitOfWork.TestCategories.CheckExist(x => x.CreatedBy == userId && x.Name == viewModel.Name);
                if (exits)
                {
                    return false;
                }
                string imageName = null;
                if(file == null)
                {
                    imageName = "result.png";
                }
                else
                {
                    imageName = UploadImageFile.UploadImage(file);
                }
                var category = new TestCategory()
                {
                    Id = Guid.NewGuid(),
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    Image = imageName,
                    CreatedDate = DateTime.Now,
                    CreatedBy = userId,
                    ModifiedDate = DateTime.Now,
                    ModifiedBy = userId,
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
                var category = await _unitOfWork.TestCategories.GetById(categoryId);
                category.Status = false;
                 _unitOfWork.TestCategories.Update(category);
                
                return await _unitOfWork.CommitAsync(cancellationToken) > 0;

            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<TestCategoryViewModel>> GetCategory(FilterModel filter,string userId)
        {
            try
            {
                if (filter == null)
                {
                    filter = new FilterModel();
                }

                if (filter.Filter == null || filter.Filter.Count == 0)
                {
                    filter.Filter = new List<FilterTypeModel>() { new FilterTypeModel() { Field = Constant.Filter.CategoryFilterDefault, IsActive = true } };
                }

                if (filter.Sort == null || filter.Sort.Count == 0 || string.IsNullOrEmpty(filter.Sort[0].Field))
                {
                    filter.Sort = new List<SortTypeModel>
                    {
                         new SortTypeModel {Field = Constant.Filter.CategorySortDefault, Asc =  false, IsActive = true}
                    };
                }

                var filterData = ApiUtils.ListToDataTable(filter.Filter);
                var sortData = ApiUtils.ListToDataTable(filter.Sort);

                var skip = filter.Skip ?? 0;
                var take = filter.Take ?? Constant.Filter.CategoryTakeDefault;
                var isExport = filter.IsExport??false;
                if (!string.IsNullOrEmpty(filter.MultipeFilter))
                {
                    filterData = null;
                }
                SqlParameter[] prams =
                {
                    new SqlParameter{ParameterName = "@filter", Value = filterData , SqlDbType = SqlDbType.Structured,TypeName = "dbo.FilterType"},
                    new SqlParameter {ParameterName = "@sort",Value = sortData, SqlDbType = SqlDbType.Structured,TypeName = "dbo.SortType"},
                    new SqlParameter {ParameterName = "@skip",Value = skip ,DbType = DbType.Int32},
                    new SqlParameter {ParameterName = "@take",Value = take,DbType = DbType.Int32},
                    new SqlParameter {ParameterName = "@multipeFilter",Value = filter.MultipeFilter as Object ?? DBNull.Value,DbType = DbType.String },
                    new SqlParameter {ParameterName = "@isExport",Value = isExport,DbType = DbType.Boolean},
                    new SqlParameter {ParameterName = "@userId",Value = userId,DbType = DbType.String}

                };
                var source = await _unitOfWork.TestCategoryViewModels.Get(Constant.StoreProcedure.GET_CATEGORIES_LIST,prams);
                return source;

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

        public async Task<bool> UpdateCategory(Guid categoryId,TestCategoryViewModel viewModel, string userId,IFormFile file, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                string imageName;
                var exits = await _unitOfWork.TestCategories.CheckExist(x => x.CreatedBy == userId && x.Name == viewModel.Name);
                if (exits)
                {
                    return false;
                }
                var category = await _unitOfWork.TestCategories.GetById(categoryId);
                category.Name = viewModel.Name;
                category.Description = viewModel.Description;
                if (file != null)
                {
                    imageName = UploadImageFile.UploadImage(file);
                    category.Image = imageName;
                }
             
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
