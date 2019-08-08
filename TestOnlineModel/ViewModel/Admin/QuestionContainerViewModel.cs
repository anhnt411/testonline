using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestOnlineModel.ViewModel.Admin
{
    public class QuestionContainerViewModel
    {
        [Key]
        public Guid Id { get; set; }

        public Guid QuestionGroupId { get; set; }

        public Guid AnswerId { get; set; }

        //public string Content { get; set; }

        public int QuestionTypeKey { get; set; }

        public string Description { get; set; }

    

       // public bool IsCorrect { get; set; }
    }
}
