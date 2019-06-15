using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestOnlineEntity.Model.ViewModel;
using TestOnlineModel.ViewModel;
using TestOnlineModel.ViewModel.User;

namespace TestOnlineBusiness.Interface
{
    public interface IUserDomain
    {
        Task<string> CreateUserAsync(ApplicationUserViewModel userViewModel);
        Task<object> Login(LoginViewModel viewModel);
        string GetUserName(CancellationToken cancellationToken = default(CancellationToken));   
        string GetUserId(CancellationToken cancellationToken = default(CancellationToken));
        Task<ApplicationUser> GetUserInfo(string userId);
        Task<bool> ChangePassword ( string userId,ChangePasswordViewmodel viewModel);
        Task<bool> UpdateUser(string userId,ApplicationUserViewModel viewModel, CancellationToken cancellationToken = default(CancellationToken));
        bool IsAdmin(CancellationToken cancellationToken = default(CancellationToken));
        bool IsUser(CancellationToken cancellationToken = default(CancellationToken));
        bool IsSuperUser(CancellationToken cancellationToken = default(CancellationToken));


    }
}
