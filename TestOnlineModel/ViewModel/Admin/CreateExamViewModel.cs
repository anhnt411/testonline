using System;
using System.Collections.Generic;
using System.Text;

namespace TestOnlineModel.ViewModel.Admin
{
    public class CreateExamViewModel
    {
        public Guid ScheduleId { get; set; }
        public int TotalExam { get; set; }

        public IEnumerable<QuestionGroupList> QuestionGroupList { get; set; }
    }

    public class QuestionGroupList
    {
        public Guid QuestionGroupId { get; set; }
        public int TotalQuestion { get; set; }
    }
}
