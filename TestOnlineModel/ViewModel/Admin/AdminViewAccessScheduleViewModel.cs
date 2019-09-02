using System;
using System.Collections.Generic;
using System.Text;

namespace TestOnlineModel.ViewModel.Admin
{
    public class AdminViewAccessScheduleViewModel
    {
        public string FullName { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public Guid ExamId { get; set; }

        public string MemberId { get; set; }

        public Guid UserExamId { get; set; }
    }
}
