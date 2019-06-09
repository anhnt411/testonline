using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebSockets.Internal;
using TestOnline.Object;
using TestOnlineBase.Constant;
using TestOnlineBase.Enum;

namespace TestOnline.Controllers
{
   
    public class BaseApiController : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult InternalServerErrorResult(object className, string methodName, string info = null, string errorMessage = null)
        {
            var resultObject = new ResultObject()
            {
                Message = Constant.Message.SERVER_ERROR,
                Result = null,
                StatusCode = Enums.StatusCode.Error
            };
            return Ok(resultObject);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult DataValidationError(string message)
        {
            var resultObject = new ResultObject()
            {
                Message = message,
                Result = null,
                StatusCode = Enums.StatusCode.Error
            };
            return Ok(resultObject);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult NotFoundErrorResult()
        {
            var resultObject = new ResultObject()
            {
                Message = Constant.Message.NOTFOUND_RESULT,
                Result = null,
                StatusCode = Enums.StatusCode.Error
            };
            return Ok(resultObject);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult ForbiddenErrorResult()
        {
            var resultObject = new ResultObject()
            {
                Message = Constant.Message.FORBIDDENRESULT,
                Result = null,
                StatusCode = Enums.StatusCode.Forbidden
            };
            return Ok(resultObject);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult FailedProcessingErorrResult()
        {
            var resultObject = new ResultObject()
            {
                Message = Constant.Message.FAILDPROCESSINGRESULT,
                Result = null,
                StatusCode = Enums.StatusCode.Error
            };
            return Ok(resultObject);
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult AuthorizedErorrResult()
        {
            var resultObject = new ResultObject()
            {
                Message = Constant.Message.UNAUTHORIZED,
                Result = null,
                StatusCode = Enums.StatusCode.Unauthorized
            };
            return BadRequest(resultObject);
            //return Ok(resultObject);
        }


    }
}