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
using TestOnlineBase.Helper;
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
        private IEmailSender _sender;


        public UserController(ILogger<UserController> logger, IUserDomain userDomain, UserManager<ApplicationUser> userManager, IEmailSender sender)
        {
            this._logger = logger;
            this._userDomain = userDomain;
            this._userManager = userManager;
            this._sender = sender;

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
                var checkExit = await _userManager.FindByNameAsync(viewModel.UserName);
                if (checkExit != null)
                {
                    return Ok(new ResultObject()
                    {
                        Message = "Tài khoản đã tồn tại",
                        StatusCode = Enums.StatusCode.Forbidden,
                        Result = viewModel.UserName
                    });
                }
                var userId = await _userDomain.CreateUserAsync(viewModel);
                var user = await _userManager.FindByIdAsync(userId);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.Action("ConfirmEmail", "User", new { UserId = user.Id, Code = code }, protocol: HttpContext.Request.Scheme);
                await _sender.SendEmailAsync(user.Email, "TestOnline - Confirm Your Email", "Please confirm your e-mail by clicking this link: <a href=\"" + callbackUrl + "\">click here</a>");
                var result = new ResultObject()
                {
                    Message = Constant.Message.SAVE_DATA_SUCCESSFULLY,
                    StatusCode = Enums.StatusCode.Ok,
                    Result = userId
                };
                return Ok(result);

            }
            catch (Exception ex)
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
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var output = await _userDomain.Login(viewModel);
                if (output == null)
                {
                    return BadRequest();
                }
                if (output.Equals("Xac thuc"))
                {
                    var kq = new ResultObject()
                    {
                        Message = Constant.Message.EMAIL_NOT_CONFIRM,
                        StatusCode = Enums.StatusCode.Unauthorized,
                        Result = output
                    };
                    return Ok(kq);
                }
                var result = new ResultObject()
                {
                    Message = Constant.Message.GET_DATA_SUCCESSFULLY,
                    StatusCode = Enums.StatusCode.Accepted,
                    Result = output

                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return FailedProcessingErorrResult();
            }
        }

        [HttpGet("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
                {
                    ModelState.AddModelError("", "User Id and Code are required");
                    return BadRequest(ModelState);

                }

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return new JsonResult("ERROR");
                }

                if (user.EmailConfirmed)
                {
                    return Redirect("http://localhost:4200/dangky");
                }

                var result = await _userManager.ConfirmEmailAsync(user, code);

                if (result.Succeeded)
                {

                    return RedirectToAction("EmailConfirmed", "Notifications", new { userId, code });

                }
                else
                {
                    List<string> errors = new List<string>();
                    foreach (var error in result.Errors)
                    {
                        errors.Add(error.ToString());
                    }
                    return new JsonResult(errors);
                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        [HttpGet("userInfo/{userName}")]
        public async Task<IActionResult> GetUserInfo(string userName)
        {
            try
            {
                var user = await _userDomain.GetUserInfo(userName);
                if(user == null)
                {
                    return NotFoundErrorResult();
                }
                var output = new ResultObject()
                {
                    Message = Constant.Message.GET_DATA_SUCCESSFULLY,
                    StatusCode = Enums.StatusCode.Ok,
                    Result = user
                };
                return Ok(user);
            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return FailedProcessingErorrResult();
            }
        }

        [Authorize]
        [HttpPut("update/{userId}")]
        public async Task<IActionResult> UpdateUser([FromBody] ApplicationUserViewModel viewModel,[FromRoute] string userId)
        {
            try
            {
                var output = await _userDomain.UpdateUser(userId, viewModel);
                if (!output)
                {
                    return FailedProcessingErorrResult();
                }
                return Ok(new ResultObject()
                {
                    Message = Constant.Message.SAVE_DATA_SUCCESSFULLY,
                    StatusCode = Enums.StatusCode.Ok,
                    Result = output
                });
            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return FailedProcessingErorrResult();
            }
        }

        [HttpPut("ChangePassword/{userId}")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(string userId,ChangePasswordViewmodel viewModel)
        {
            try
            {
                var result = await _userDomain.ChangePassword(userId, viewModel);
                if (!result)
                {
                    return FailedProcessingErorrResult();
                }
                return Ok(new ResultObject()
                {
                    Message = Constant.Message.SAVE_DATA_SUCCESSFULLY,
                    StatusCode = Enums.StatusCode.Ok,
                    Result = result
                });

            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return FailedProcessingErorrResult();
            }
        }


    }
}