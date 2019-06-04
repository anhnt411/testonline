using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TestOnline.Object;
using TestOnlineBase.Constant;
using TestOnlineBase.Enum;
using TestOnlineBusiness.Interface;
using TestOnlineEntity.Model.ViewModel;
using TestOnlineModel.ViewModel;
using TestOnlineModel.ViewModel.User;

namespace TestOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : BaseApiController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserDomain _userDomain;
         private UserManager<ApplicationUser> _userManager;

        public UserController(ILogger<UserController> logger,IUserDomain userDomain,UserManager<ApplicationUser> userManager)
        {
            this._logger = logger;
            this._userDomain = userDomain;
            this._userManager = userManager;
        }

        
        [HttpPost("user")]           
        public async Task<IActionResult> CreateUser([FromBody]ApplicationUserViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }             
            
                var userId = await _userDomain.CreateUserAsync(viewModel);
                var result = new ResultObject()
                {
                    Message = Constant.Message.SAVE_DATA_SUCCESSFULLY,
                    StatusCode = Enums.StatusCode.Ok,
                    Result = userId
                };
                return Ok(result);

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return FailedProcessingErorrResult();
            }
        }
        [HttpPost("login")]
        
        public async Task<IActionResult> Login([FromBody] LoginViewModel viewModel)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var output = await _userDomain.Login(viewModel);
                if(output == null)
                {
                    return BadRequest();
                }
                var result = new ResultObject()
                {
                    Message = Constant.Message.GET_DATA_SUCCESSFULLY,
                    StatusCode = Enums.StatusCode.Accepted,
                    Result = output
                };
                return Ok(result);
            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return FailedProcessingErorrResult();
            }
        }


    }
}