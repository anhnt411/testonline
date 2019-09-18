using System;
using System.Collections.Generic;
using System.Text;

namespace TestOnlineModel.ViewModel.Admin
{
    public class ExamDetailViewModel
    {
        public Guid? ExamId { get; set; }
        public Guid QuestionId { get; set; }

        public string QuestionName { get; set; }

        public int? QuestionTypeKey { get; set; }

        public bool? QuestionTrue { get; set; }

        public DateTime? StartTime { get; set; }

        public Guid? ScheduleId { get; set; }

        public bool? IsAccess { get; set; }

        public bool? IsSubmit { get; set; }

        public IEnumerable<AnswerViewModel2> ListAnswer { get; set; }

    }

    public class AnswerViewModel2
    {
        public Guid AnswerId { get; set; }
        public string AnswerDescript { get; set; }

        public bool? IsCorrect { get; set; }

        public string AnswerSequence { get; set; }

        public bool? IsUserAnswer { get; set; }
    }
}
