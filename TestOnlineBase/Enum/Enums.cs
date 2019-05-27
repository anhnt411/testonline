using System;
using System.Collections.Generic;
using System.Text;

namespace TestOnlineBase.Enum
{
    public class Enums
    {
        public enum StatusCode
        {
            Ok = 201,
            Accepted = 202,
            Unauthorized = 401,
            Forbidden = 403,
            Error = 500,
            GatewayTimeout = 503,
            TokenInValid = 190
        }
    }
}
