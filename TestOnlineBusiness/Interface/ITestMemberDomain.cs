using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestOnlineEntity.Model.Entity;

namespace TestOnlineBusiness.Interface
{
    public interface ITestMemberDomain
    {
        Task<string> CreateMember(Member member, string userId);
        Task<bool> AddListMember(Guid unitId, IFormFile file,string userId, CancellationToken cancellationToken = default(CancellationToken));
    }
}
