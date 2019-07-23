using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using TestOnlineBase.Constant;
using TestOnlineBase.Helper;
using TestOnlineBase.Helper.PagingHelper;
using TestOnlineBase.Helper.RandomHelper;
using TestOnlineBusiness.Interface;
using TestOnlineEntity.Interface;
using TestOnlineEntity.Model.Entity;
using TestOnlineEntity.Model.ViewModel;
using TestOnlineModel.ViewModel.Admin;

namespace TestOnlineBusiness.Service
{
    public class TestMemberDomain : ITestMemberDomain
    {
        private readonly ITestOnlienUnitOfWork _unitOfWork;
        private readonly ILogger<TestMemberDomain> _logger;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IEmailSender _sender;

        public TestMemberDomain(ITestOnlienUnitOfWork unitOfWork, ILogger<TestMemberDomain> logger, UserManager<ApplicationUser> userManager,
               SignInManager<ApplicationUser> signInManager, IEmailSender sender)
        {
            this._unitOfWork = unitOfWork;
            this._logger = logger;
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._sender = sender;
        }

        public async Task<bool> AddListMember(Guid unitId, IFormFile file, string userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var listUser = new List<ApplicationUser>();
                    using (var stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream, cancellationToken);
                        using (var package = new ExcelPackage(stream))
                        {
                            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                            var rowCount = worksheet.Dimension.Rows;

                            for (int row = 2; row <= rowCount; row++)
                            {
                                var password = RandomHelper.RandomPassword();
                                DateTime? date = DateTime.Now;
                                var valueDate = (worksheet.Cells[row, 5].Value == null) ? null : (worksheet.Cells[row, 5].Value);
                                if ( valueDate != null)
                                {
                                    date = Convert.ToDateTime(valueDate);
                                }
                                

                                var user = new ApplicationUser()
                                {
                                    Email = worksheet.Cells[row, 3].Value.ToString().Trim(),
                                    CreatedBy = userId,
                                    UnitId = unitId,
                                    FullName = worksheet.Cells[row, 2].Value.ToString().Trim(),
                                    PhoneNumber = (worksheet.Cells[row, 4].Value == null) ? null : (worksheet.Cells[row, 4].Value.ToString().Trim()),
                                    DateOfBirth = date,
                                    Status = true,
                                    EmailConfirmed = true,
                                    Address = (worksheet.Cells[row, 6].Value == null) ? null :( worksheet.Cells[row, 6].Value.ToString().Trim()),
                                    UserName = RandomHelper.RandomUserName(),
                                    MemberPass = password
                                };
                                var result = await _userManager.CreateAsync(user, password);
                                await _userManager.AddToRoleAsync(user, Constant.Role.NORMAL_USER);
                            }
                        }
                        scope.Complete();
                        
                    }
                    return true;
                }
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                
                return false;
            }

        }

        public async Task<string> CreateMember(Member member, string userId)
        {
            try
            {
                var password = RandomHelper.RandomPassword();
                var user = new ApplicationUser()
                {
                    Email = member.Email,
                    CreatedBy = userId,
                    UnitId = member.TestUnitId,
                    FullName = member.Name,
                    PhoneNumber = member.Phone,
                    DateOfBirth = member.DateOfBirth,
                 
                    Status = true,
                    EmailConfirmed = true,
                    Address = member.Address,
                    UserName = RandomHelper.RandomUserName(),
                    MemberPass = password
                };
                var result = await _userManager.CreateAsync(user, password);
                await _userManager.AddToRoleAsync(user, Constant.Role.NORMAL_USER);
                return user.Id;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<bool> DeleteMember(string memberId, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var member = await _userManager.FindByIdAsync(memberId);
                member.Status = false;
                var result = await _userManager.UpdateAsync(member);

                return result.Succeeded;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<TestMemberViewModel>> GetListMember(FilterModel filter, Guid unitId,string userId)
        {
            try
            {
                if (filter == null)
                {
                    filter = new FilterModel();
                }

                if (filter.Filter == null || filter.Filter.Count == 0)
                {
                    filter.Filter = new List<FilterTypeModel>() { new FilterTypeModel() { Field = Constant.Filter.MemberFilterDefault, IsActive = true } };
                }

                if (filter.Sort == null || filter.Sort.Count == 0 || string.IsNullOrEmpty(filter.Sort[0].Field))
                {
                    filter.Sort = new List<SortTypeModel>
                    {
                         new SortTypeModel {Field = Constant.Filter.MemberSortDefault, Asc =  false, IsActive = true}
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
                    new SqlParameter {ParameterName = "@userId",Value = userId,DbType = DbType.String},
                    new SqlParameter {ParameterName = "@unitId",Value = unitId,DbType = DbType.Guid}
                };
                var source = await _unitOfWork.TestMemberViewModels.Get(Constant.StoreProcedure.GET_MEMBER_LIST, prams);
                return source;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<TestMemberViewModel> GetMemberDetail(string memberId)
        {
            try
            {
                var member = await _userManager.FindByIdAsync(memberId);
                if(member == null)
                {
                    return null;
                }
                return new TestMemberViewModel() {
                    Id = member.Id,
                    FullName = member.FullName,
                    Address = member.Address,
                    DateOfBirth = member.DateOfBirth,
                    Email = member.Email,
                    PhoneNumber = member.PhoneNumber,
                    Status = member.Status,
                    UnitId = member.UnitId
                };

            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<bool> UpdateMember(string memberId, Member model)
        {
            try
            {
        
                var member = await _userManager.FindByIdAsync(memberId);
                member.UnitId = model.TestUnitId;
                member.FullName = model.Name;
                member.Address = model.Address;
                member.DateOfBirth = model.DateOfBirth;
                member.PhoneNumber = model.Phone;
                var a =  await _userManager.UpdateAsync(member);
                if (a.Succeeded)
                {
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        private bool IsDateTime(string txtDate)
        {
            DateTime tempDate;
            return DateTime.TryParse(txtDate, out tempDate);
        }
    }
}
