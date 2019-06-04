using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TestOnline.Object;
using TestOnlineBase.Constant;
using TestOnlineBase.Enum;
using TestOnlineBusiness.Interface;
using TestOnlineModel.ViewModel.Admin;

namespace TestOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TestCategoryController :BaseApiController
    {
        private readonly ICategoryDomain _category;
        private readonly ILogger<TestCategoryController> _logger;
        private readonly IUserDomain _user;
        public TestCategoryController(ICategoryDomain category,ILogger<TestCategoryController> logger,IUserDomain user)
        {
            this._category = category;
            this._logger = logger;
            this._user = user;
        }

        [HttpGet("categories")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var output = await _category.GetCategory();
                var result = new ResultObject()
                {
                    Message = Constant.Message.GET_DATA_SUCCESSFULLY,
                    StatusCode = Enums.StatusCode.Ok,
                    Result = output
                };
                return Ok(result);

            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        [HttpGet("category/{id}")]
        public async Task<IActionResult> CategoryDetail(Guid categoryId)
        {
            try
            {
                var category = await _category.GetCategoryDetail(categoryId);
                var result = new ResultObject()
                {
                    Message = Constant.Message.GET_DATA_SUCCESSFULLY,
                    StatusCode = Enums.StatusCode.Ok,
                    Result = category
                };
                return Ok(category);

            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return FailedProcessingErorrResult();
            }
        }

        [HttpPost("category")]
        public async Task<IActionResult> CreateCategory([FromBody] TestCategoryViewModel viewModel)
        {
            try
            {
                var userId = _user.GetUserId();
                if(userId==null || _user.IsUser())
                {
                    return Unauthorized();
                }
                var output = await _category.CreateCategory(viewModel, userId);
                var result = new ResultObject()
                {
                    Message = Constant.Message.SAVE_DATA_SUCCESSFULLY,
                    StatusCode = Enums.StatusCode.Ok,
                    Result = output
                };
                return Ok(result);

            }catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return FailedProcessingErorrResult();

            }
        }

        [HttpPut("category/{categoryId}")]
        public async Task<IActionResult> UpdaetCategory([FromBody] TestCategoryViewModel viewModel,[FromRoute] Guid categoryId)
        {
            try
            {
                var userId = _user.GetUserId();
                if (userId == null || !_user.IsAdmin())
                {
                    return Unauthorized();
                }
                var output = await _category.UpdateCategory(categoryId,viewModel,userId);
                var result = new ResultObject()
                {
                    Message = Constant.Message.SAVE_DATA_SUCCESSFULLY,
                    StatusCode = Enums.StatusCode.Ok,
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

    }
}