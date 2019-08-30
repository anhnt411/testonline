using System;
using System.Collections.Generic;
using System.Text;

namespace TestOnlineBase.Constant
{
    public static class Constant
    {
        #region StoreProcedure
        public class StoreProcedure
        {
            public const string GET_CATEGORIES_LIST = "sp_GetTestCategory";
            public const string GET_UNITS_LIST = "sp_GetTestUnit";
            public const string GET_MEMBER_LIST = "sp_GetTestMember";
            public const string GET_QUESTION_GROUP_LIST = "sp_GetQuestionGroup";
            public const string GET_QUESTION_LIST = "sp_GetQuestionList";
            public const string GET_QUESTION_CONTAINER = "sp_GetQuestionDetail";
            public const string GET_QUESTION_CONTAINER2 = "sp_GetQuestionDetail2";
            public const string GET_TEST_SCHEDULE = "sp_GetListSchedule";
            public const string GET_USER_SCHEDULE = "sp_GetListUserSchedule";

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

        public static class Filter
        {
            public const string CategoryFilterDefault = "TestCategoryName";
            public const string CategorySortDefault = "UpdatedDate";
            public const int CategoryTakeDefault = 5;

            public const string UnitFilterDefault = "TestUnitName";
            public const string UnitSortDefault = "CreatedDate";
            public const int UnitTakeDefault = 5;

            public const string MemberFilterDefault = "TestUnitName";
            public const string MemberSortDefault = "FullName";
            public const int MemberTakeDefault = 5;

            public const string QuestionGroupFilterDefault = "QuestionGroupName";
            public const string QuestionGroupSortDefault = "CreatedDate";
            public const int QuestionGroupTakeDefault = 5;

            public const string QuestionFilterDefault = "Description";
            public const string QuestionSortDefault = "CreatedDate";
            public const int QuestionTakeDefault = 5;

            public const string ScheduleFilterDefault = "Name";
            public const string ScheduleSortDefault = "CreatedDate";
            public const int ScheduleTakeDefault = 5;
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
