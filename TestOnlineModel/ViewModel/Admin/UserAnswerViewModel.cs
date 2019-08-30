using System;
using System.Collections.Generic;
using System.Text;

namespace TestOnlineModel.ViewModel.Admin
{
    public class UserAnswerViewModel
    {
        public Guid ExamId { get; set; }
        public List<AnswerUserViewModel> ListAnswer { get; set; }
    }

    public class AnswerUserViewModel
    {
        public Guid QuestionId { get; set; }

        public Guid AnswerId { get; set; }
    }
}
