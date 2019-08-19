using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestOnlineModel.ViewModel.Admin
{
    public class QuestionViewModel
    {
        public int QuestionTypeKey { get; set; }

        public Guid QuestionGroupId { get; set; }

        [Required]
        public string Description { get; set; }

        public IEnumerable<AnswerViewModel> Answers { get; set; }
    }

    public class AnswerViewModel
    {
        [Required]
        public string AnswerName { get; set; }
        [Required]
        public string Description { get; set; }

        public int Sequence { get; set; }

        public bool IsCorrect { get; set; }
    }


}
