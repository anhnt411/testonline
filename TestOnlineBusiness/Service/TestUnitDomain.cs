using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TestOnlineBase.Constant;
using TestOnlineBase.Helper.PagingHelper;
using TestOnlineBusiness.Interface;
using TestOnlineEntity.Interface;
using TestOnlineEntity.Model.Entity;
using TestOnlineModel.ViewModel.Admin;

namespace TestOnlineBusiness.Service
{
    public class TestUnitDomain : ITestUnitDomain
    {
        private readonly ITestOnlienUnitOfWork _unitOfWork;
        private readonly ILogger<TestUnitDomain> _logger;

        public TestUnitDomain(ITestOnlienUnitOfWork unitOfWork, ILogger<TestUnitDomain> logger)
        {
            this._unitOfWork = unitOfWork;
            this._logger = logger;

        }

        public async Task<bool> CreateUnit(TestUnitViewModel viewModel, string userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var check = await _unitOfWork.TestUnits.CheckExist(x => x.CreatedBy == userId && x.Name == viewModel.Name);
                if (check)
                {
                    return false;
                }
                var unit = new TestUnit()
                {
                    Id = Guid.NewGuid(),
                    Name = viewModel.Name,
                    Address = viewModel.Address,
                    PhoneNumber = viewModel.PhoneNumber,
                    CreatedDate = DateTime.Now,
                    CreatedBy = userId,
                     UpdatedDate= DateTime.Now,
                    UpdatedBy = userId,
                    IsActive = true
                };
               
                _unitOfWork.TestUnits.Insert(unit);
                
                return await _unitOfWork.CommitAsync(cancellationToken) > 0;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteUnit(Guid unitId, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var unit = await _unitOfWork.TestUnits.GetById(unitId);
                unit.IsActive = false;
                _unitOfWork.TestUnits.Update(unit);

                return await _unitOfWork.CommitAsync(cancellationToken) > 0;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<TestUnitViewModel>> GetAll(string userId)
        {
           var output  = await _unitOfWork.TestUnits.Get(x => x.IsActive == true && x.CreatedBy == userId) ;
            var unitList = output.Select(x => new TestUnitViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Address = x.Address,
                Status = x.IsActive
            });
            return unitList;
        }

        public async Task<IEnumerable<TestUnitViewModel>> GetUnit(FilterModel filter, string userId)
        {
            try
            {
                if (filter == null)
                {
                    filter = new FilterModel();
                }

                if (filter.Filter == null || filter.Filter.Count == 0)
                {
                    filter.Filter = new List<FilterTypeModel>() { new FilterTypeModel() { Field = Constant.Filter.UnitFilterDefault, IsActive = true } };
                }

                if (filter.Sort == null || filter.Sort.Count == 0 || string.IsNullOrEmpty(filter.Sort[0].Field))
                {
                    filter.Sort = new List<SortTypeModel>
                    {
                         new SortTypeModel {Field = Constant.Filter.UnitSortDefault, Asc =  false, IsActive = true}
                    };
                }

                var filterData = ApiUtils.ListToDataTable(filter.Filter);
                var sortData = ApiUtils.ListToDataTable(filter.Sort);

                var skip = filter.Skip ?? 0;
                var take = filter.Take ?? Constant.Filter.CategoryTakeDefault;
                var isExport = filter.IsExport ?? false;
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
                var source = await _unitOfWork.TestUnitViewModels.Get(Constant.StoreProcedure.GET_UNITS_LIST, prams);
                return source;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<TestUnitViewModel> GetUnitDetail(Guid? unitId)
        {
            try
            {
                var output = await _unitOfWork.TestUnits.GetById(unitId);
                return new TestUnitViewModel()
                {
                    Id = output.Id,
                    Name = output.Name,
                    Address = output.Address,
                    PhoneNumber = output.PhoneNumber,
                    CreatedDate = output.CreatedDate,
                    CreatedBy = output.CreatedBy
                   
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<bool> UpdateUnit(Guid unitId, TestUnitViewModel viewModel, string userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var check = await _unitOfWork.TestUnits.CheckExist(x => x.CreatedBy == userId && x.Name == viewModel.Name);
                if (check)
                {
                    return false;
                }
                var unit = await _unitOfWork.TestUnits.GetById(unitId);
                unit.Name = viewModel.Name;
                unit.PhoneNumber = viewModel.PhoneNumber;
              

                unit.UpdatedBy = userId;
                unit.UpdatedDate = DateTime.Now;
                _unitOfWork.TestUnits.Update(unit);
                return await _unitOfWork.CommitAsync(cancellationToken) > 0;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }
    }
}
