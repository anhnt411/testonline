using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestOnlineBase.Helper.PagingHelper;
using TestOnlineEntity.Model.Entity;
using TestOnlineModel.ViewModel.Admin;

namespace TestOnlineBusiness.Interface
{
    public interface ITestMemberDomain
    {
        Task<IEnumerable<TestMemberViewModel>> GetListMember(FilterModel filter, Guid unitId,string userId);
        Task<string> CreateMember(Member member, string userId);
        Task<bool> AddListMember(Guid unitId, IFormFile file,string userId, CancellationToken cancellationToken = default(CancellationToken));
    }
}
