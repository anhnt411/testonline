using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using TestOnlineBase.Constant;
using TestOnlineBase.Helper.PagingHelper;
using TestOnlineBusiness.Interface;
using TestOnlineEntity.Interface;
using TestOnlineEntity.Model.Entity;
using TestOnlineModel.ViewModel.Admin;

namespace TestOnlineBusiness.Service
{
    public class QuestionBankDomain:IQuestionBankDomain
    {
        private readonly ITestOnlienUnitOfWork _unitOfWork;
        private readonly ILogger<QuestionBankDomain> _logger;

        public QuestionBankDomain(ITestOnlienUnitOfWork unitOfWork, ILogger<QuestionBankDomain> logger)
        {
            this._unitOfWork = unitOfWork;
            this._logger = logger;

        }

        public async Task<bool> AddQuestionBank(QuestionGroupViewModel model,string userId)
        {
            try
            {


                var listQuestionBank = await _unitOfWork.QuestionGroups.GetOne(x => x.CategoryId == model.CategoryId && x.CreatedBy == userId && x.IsActive == true && x.Name == model.Name);


                if (listQuestionBank != null)
                {
                    return false;
                }

                var questionGroup = new QuestionGroup()
                {
                    Id = new Guid(),
                    CategoryId = model.CategoryId,
                    Name = model.Name,
                    IsActive = true,
                    Description = model.Description,
                    CreatedBy = userId,
                    CreatedDate = DateTime.Now

                };

                _unitOfWork.QuestionGroups.Insert(questionGroup);
                return await _unitOfWork.CommitAsync() > 0;          
            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteQuestionBank(Guid questionGroupId)
        {
            try
            {
                var questionGroup = await _unitOfWork.QuestionGroups.GetById(questionGroupId);
                if(questionGroup == null)
                {
                    return false;
                }
                questionGroup.IsActive = false;
                _unitOfWork.QuestionGroups.Update(questionGroup);
                return await _unitOfWork.CommitAsync() > 0;
            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public  async Task<IEnumerable<QuestionGroupViewModel>> GetListQuestionGroup(FilterModel filter, string userId)
        {
            try
            {
                if (filter == null)
                {
                    filter = new FilterModel();
                }

                if (filter.Filter == null || filter.Filter.Count == 0)
                {
                    filter.Filter = new List<FilterTypeModel>() { new FilterTypeModel() { Field = Constant.Filter.QuestionGroupFilterDefault, IsActive = true } };
                }

                if (filter.Sort == null || filter.Sort.Count == 0 || string.IsNullOrEmpty(filter.Sort[0].Field))
                {
                    filter.Sort = new List<SortTypeModel>
                    {
                         new SortTypeModel {Field = Constant.Filter.QuestionGroupSortDefault, Asc =  false, IsActive = true}
                    };
                }

                var filterData = ApiUtils.ListToDataTable(filter.Filter);
                var sortData = ApiUtils.ListToDataTable(filter.Sort);

                var skip = filter.Skip ?? 0;
                var take = filter.Take ?? Constant.Filter.QuestionGroupTakeDefault;
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
                var source = await _unitOfWork.QuestionGroupViewModels.Get(Constant.StoreProcedure.GET_QUESTION_GROUP_LIST, prams);
                return source;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<QuestionGroup> GetQuestionGroupDetail(Guid questionGroupId)
        {
            try
            {
                var detail = await _unitOfWork.QuestionGroups.GetById(questionGroupId);
                return detail;

            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<bool> UpdateQuestionBank(QuestionGroup model, string userId)
        {
            try
            {
                var questionGroup = await _unitOfWork.QuestionGroups.GetById(model.Id);
                if(questionGroup.Name == model.Name)
                {
                    questionGroup.Description = model.Description;
                    questionGroup.UpdatedBy = userId;
                    questionGroup.UpdatedDate = DateTime.Now;
                    _unitOfWork.QuestionGroups.Update(questionGroup);
                    return await _unitOfWork.CommitAsync() > 0;
                }
                var listQuestionBank = await _unitOfWork.QuestionGroups.GetOne(x => x.CategoryId == model.CategoryId && x.CreatedBy == userId && x.IsActive == true && x.Name == model.Name);


                if (listQuestionBank != null)
                {
                    return false;
                }
              
                questionGroup.Name = model.Name;
                questionGroup.Description = model.Description;
                questionGroup.UpdatedBy = userId;
                questionGroup.UpdatedDate = DateTime.Now;

                _unitOfWork.QuestionGroups.Update(questionGroup);

                return await _unitOfWork.CommitAsync() > 0;

            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }
    }
}
