using System;
using System.Collections.Generic;
using System.Text;

namespace TestOnlineModel.ViewModel.Admin
{
    public class ExamDetailViewModel
    {
        public Guid QuestionId { get; set; }

        public string QuestionName { get; set; }

        public IEnumerable<AnswerViewModel2> ListAnswer { get; set; }

    }

    public class AnswerViewModel2
    {
        public string AnswerDescript { get; set; }

        public bool IsCorrect { get; set; }

        public string AnswerSequence { get; set; }
    }
}
