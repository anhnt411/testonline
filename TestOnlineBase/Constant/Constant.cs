﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TestOnlineBase.Constant
{
    public static class Constant
    {
        #region StoreProcedure
        public class StoreProcedure
        {

        }
        #endregion

        #region Message
        public static class Message
        {
            public const string PARAMETER_IS_NOT_VALID = "Parameter is not valid";
            public const string GET_DATA_SUCCESSFULLY = "Get data successfully";
            public const string SAVE_DATA_SUCCESSFULLY = "Save data successfully";
            public const string SERVER_ERROR = "InternalServerError";
            public const string NOTFOUND_RESULT = "NotFound";
            public const string FORBIDDENRESULT = "ForbiddenError";
            public const string FAILDPROCESSINGRESULT = "InternalServerError";
            public const string UNAUTHORIZED = "Authorize Error";
            public const string EMAIL_CONFIRM_SUCCESS = "Confirmed Success";
            public const string EMAIL_NOT_CONFIRM = "Please confirmed email";
            


        }

        public static class Role
        {
            public const string SUPER_USER = "SuperUSer";
            public const string ADMIN = "Admin";
            public const string NORMAL_USER = "User";
        }
        #endregion

    }
}
