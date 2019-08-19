using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestOnlineBase.Constant;
using TestOnlineBase.Helper.PagingHelper;
using TestOnlineBusiness.Interface;
using TestOnlineEntity.Interface;
using TestOnlineEntity.Model.Entity;
using TestOnlineModel.ViewModel.Admin;

namespace TestOnlineBusiness.Service
{
    public class TestScheduleDomain : ITestScheduleDomain
    {

        private readonly ITestOnlienUnitOfWork _unitOfWork;
        private readonly ILogger<TestScheduleDomain> _logger;

        public TestScheduleDomain(ITestOnlienUnitOfWork unitOfWork, ILogger<TestScheduleDomain> logger)
        {
            this._unitOfWork = unitOfWork;
            this._logger = logger;

        }

        public async Task<bool> AddSchedule(TestScheduleViewModel viewModel, string userId, CancellationToken cancellationToken = default)
        {
            try
            {
                var listTestSchedule = await _unitOfWork.TestSchedules.GetOne(x => x.TestCategoryId == viewModel.CategoryId && x.IsActive == true && x.Name == viewModel.Name);
                if (listTestSchedule != null)
                {
                    return false;
                }
                var testSchedule = new TestSchedule()
                {
                    Id = Guid.NewGuid(),
                    AllowViewAnswer = viewModel.AllowViewAnswer,
                    IsActive = true,
                    IsOpen = true,
                    CreatedBy = userId,
                    UpdatedBy = userId,
                    UpdatedDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    TestTime = viewModel.Time,
                    StartDate = viewModel.StartDate.AddDays(1),
                    EndDate = viewModel.EndDate.AddDays(1),
                    TestCategoryId = viewModel.CategoryId,
                    Percentage = viewModel.Percentage,
                    TotalQuestion = viewModel.TotalQuestion
                    
                };
                _unitOfWork.TestSchedules.Insert(testSchedule);
                return await _unitOfWork.CommitAsync() > 0;
            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<TestScheduleViewModel>> GetListSchedule(FilterModel filter, string userId)
        {
            try
            {
                if (filter == null)
                {
                    filter = new FilterModel();
                }

                if (filter.Filter == null || filter.Filter.Count == 0)
                {
                    filter.Filter = new List<FilterTypeModel>() { new FilterTypeModel() { Field = Constant.Filter.ScheduleFilterDefault, IsActive = true } };
                }

                if (filter.Sort == null || filter.Sort.Count == 0 || string.IsNullOrEmpty(filter.Sort[0].Field))
                {
                    filter.Sort = new List<SortTypeModel>
                    {
                         new SortTypeModel {Field = Constant.Filter.ScheduleSortDefault, Asc =  false, IsActive = true}
                    };
                }

                var filterData = ApiUtils.ListToDataTable(filter.Filter);
                var sortData = ApiUtils.ListToDataTable(filter.Sort);

                var skip = filter.Skip ?? 0;
                var take = filter.Take ?? Constant.Filter.ScheduleTakeDefault;
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
                var source = await _unitOfWork.TestScheduleViewModels.Get(Constant.StoreProcedure.GET_TEST_SCHEDULE, prams);
                return source;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }
    }
}
