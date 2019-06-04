using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace TestOnlineBase
{
    public class HttpException:Exception
    {
        public HttpStatusCode HttpStatus { get; }


        public HttpException()
        {
            HttpStatus = HttpStatusCode.InternalServerError;
        }

        public HttpException(string message, Exception innerException) : base(message, innerException)
        {
            HttpStatus = HttpStatusCode.InternalServerError;
        }

        public HttpException(HttpStatusCode httpStatus, string message) : base(message)
        {
            HttpStatus = httpStatus;
        }
    }
}
