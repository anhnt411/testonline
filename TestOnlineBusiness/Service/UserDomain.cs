using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestOnlineBase;
using TestOnlineBase.Constant;
using TestOnlineBusiness.Interface;
using TestOnlineEntity.Interface;
using TestOnlineEntity.Model.ViewModel;
using TestOnlineModel.ViewModel;
using TestOnlineModel.ViewModel.User;

namespace TestOnlineBusiness.Service
{
    public class UserDomain : IUserDomain
    {
        private readonly ITestOnlienUnitOfWork _unitOfWork;
        private readonly ILogger<UserDomain> _logger;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationSettingViewModel _appSetting;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserDomain(ITestOnlienUnitOfWork unitOfWork, ILogger<UserDomain> logger, IOptions<ApplicationSettingViewModel> appSetting, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,IHttpContextAccessor contextAccessor)
        {
            this._unitOfWork = unitOfWork;
            this._logger = logger;
            this._appSetting = appSetting.Value;
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._httpContextAccessor = contextAccessor;


        }
        public async Task<string> CreateUserAsync(ApplicationUserViewModel userViewModel)
        {
            try
            {
                //var currentUser = GetCurrentUser();
                var user = new ApplicationUser()
                {
                    Email = userViewModel.Email,
                    UserName = userViewModel.UserName,
                    Address = userViewModel.Address,
                    PhoneNumber = userViewModel.PhoneNumber,                
                    Status = true
                };
                var result = await _userManager.CreateAsync(user, userViewModel.Password);                    
                await _userManager.AddToRoleAsync(user, Constant.Role.ADMIN);
                return user.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }

        }

        public async Task<string> Login(LoginViewModel viewModel)
        {
            try
            {
              
                var user = await _userManager.FindByNameAsync(viewModel.UserName);
                if (user != null && await _userManager.CheckPasswordAsync(user, viewModel.PassWord))
                {
                    //Get role assigned to the user
                    var role = await _userManager.GetRolesAsync(user);
                   

                      
                    IdentityOptions _options = new IdentityOptions();

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                        new Claim("UserID",user.Id.ToString()),
                        new Claim("UserName",user.UserName),    
                        new Claim("IsAdmin",role.Contains(Constant.Role.ADMIN).ToString()),
                        new Claim("IsUser",role.Contains(Constant.Role.NORMAL_USER).ToString()),
                        new Claim("IsSuperUser",role.Contains(Constant.Role.SUPER_USER).ToString()),
                        new Claim(_options.ClaimsIdentity.RoleClaimType,role.FirstOrDefault())
                        }),
                        Expires = DateTime.UtcNow.AddDays(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSetting.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                    var token = tokenHandler.WriteToken(securityToken);
                    return token;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public string GetUserName(CancellationToken cancellationToken = default(CancellationToken))
        {
            var identity = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;
            var userClaim = identity?.Claims.SingleOrDefault(x => x.Type.Equals("UserName"));

            if (userClaim == null)
                throw new HttpException(HttpStatusCode.Unauthorized, HttpStatusCode.Unauthorized.ToString());

            return userClaim.Value;
        }

        public string GetUserId(CancellationToken cancellationToken = default(CancellationToken))
        {
            var identity = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;
            var userClaim = identity?.Claims.SingleOrDefault(x => x.Type.Equals("UserID"));
            if(userClaim == null)
            {
                throw new HttpException(HttpStatusCode.Unauthorized, HttpStatusCode.Unauthorized.ToString());
            }
            return userClaim.Value;
        }

        public bool IsAdmin(CancellationToken cancellationToken = default(CancellationToken))
        {
            var identity = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;
            var userClaim = identity?.Claims.SingleOrDefault(x => x.Type.Equals("IsAdmin"));
            if (userClaim == null)
            {
                throw new HttpException(HttpStatusCode.Unauthorized, HttpStatusCode.Unauthorized.ToString());
            }
            return Convert.ToBoolean(userClaim.Value);
        }

        public bool IsUser(CancellationToken cancellationToken = default(CancellationToken))
        {
            var identity = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;
            var userClaim = identity?.Claims.SingleOrDefault(x => x.Type.Equals("IsUser"));
            if (userClaim == null)
            {
                throw new HttpException(HttpStatusCode.Unauthorized, HttpStatusCode.Unauthorized.ToString());
            }
            return Convert.ToBoolean(userClaim.Value);
        }

        public bool IsSuperUser(CancellationToken cancellationToken = default(CancellationToken))
        {
            var identity = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;
            var userClaim = identity?.Claims.SingleOrDefault(x => x.Type.Equals("IsSuperUser"));
            if (userClaim == null)
            {
                throw new HttpException(HttpStatusCode.Unauthorized, HttpStatusCode.Unauthorized.ToString());
            }
            return Convert.ToBoolean(userClaim.Value);
        }
    }
}
