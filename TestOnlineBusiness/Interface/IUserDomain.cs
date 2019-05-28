using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestOnlineModel.ViewModel;
using TestOnlineModel.ViewModel.User;

namespace TestOnlineBusiness.Interface
{
    public interface IUserDomain
    {
        Task<string> CreateUserAsync(ApplicationUserViewModel userViewModel);
        Task<string> Login(LoginViewModel viewModel);
    }
}
