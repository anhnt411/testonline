using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestOnlineModel.ViewModel.Admin
{
    public class QuestionDetailViewModel
    {
        [Key]
        public Guid Id { get; set; }

        public Guid QuestionGroupId { get; set; }

        public int QuestionTypeKey { get; set; }

        public string Description { get; set; }

        public IEnumerable<AnswerDetailViewModel> Answers { get; set; }
    }

    public class AnswerDetailViewModel
    {
        public Guid AnswerId { get; set; }

        public string AnswerName { get; set; }

        public int? Sequence { get; set; }
        public string Content { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsCorrect { get; set; }
    }
}
