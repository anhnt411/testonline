using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using TestOnlineBase.Constant;
using TestOnlineBase.Helper;
using TestOnlineBase.Helper.RandomHelper;
using TestOnlineBusiness.Interface;
using TestOnlineEntity.Interface;
using TestOnlineEntity.Model.Entity;
using TestOnlineEntity.Model.ViewModel;

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
                                DateTime? date = new DateTime();
                                var valueDate = worksheet.Cells[row, 5].Value.ToString().Trim();
                                if (IsDateTime(valueDate))
                                {
                                    date = Convert.ToDateTime(valueDate);
                                }
                                date = DateTime.MinValue;
                                var user = new ApplicationUser()
                                {
                                    Email = worksheet.Cells[row, 3].Value.ToString().Trim(),
                                    CreatedBy = userId,
                                    UnitId = unitId,
                                    FullName = worksheet.Cells[row, 2].Value.ToString().Trim(),
                                    PhoneNumber = worksheet.Cells[row, 4].Value.ToString().Trim(),
                                    DateOfBirth = date,
                                    Status = true,
                                    EmailConfirmed = true,
                                    Address = worksheet.Cells[row, 6].Value.ToString().Trim(),
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

        private bool IsDateTime(string txtDate)
        {
            DateTime tempDate;
            return DateTime.TryParse(txtDate, out tempDate);
        }
    }
}
