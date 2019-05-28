using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestOnlineBusiness.Interface;
using TestOnlineEntity.Interface;
using TestOnlineModel.ViewModel;
using TestOnlineModel.ViewModel.User;

namespace TestOnlineBusiness.Service
{
    public class UserDomain : IUserDomain
    {
        private readonly ITestOnlienUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public UserDomain(ITestOnlienUnitOfWork unitOfWork,ILogger logger )
        {
            this._unitOfWork = unitOfWork;
            this._logger = logger;
        }
        public Task<string> CreateUserAsync(ApplicationUserViewModel userViewModel)
        {
            throw new NotImplementedException();
        }

        public Task<string> Login(LoginViewModel viewModel)
        {
            throw new NotImplementedException();
        }
    }
}
