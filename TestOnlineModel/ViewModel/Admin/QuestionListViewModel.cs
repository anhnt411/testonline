using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestOnlineModel.ViewModel.Admin
{
    public class QuestionListViewModel
    {
        [Key]
        public Guid Id { get; set; }

        public long STT { get; set; }

        public string Description { get; set; }

        public string QuestionType { get; set;}

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public int TotalRecord { get; set; }

    }
}
