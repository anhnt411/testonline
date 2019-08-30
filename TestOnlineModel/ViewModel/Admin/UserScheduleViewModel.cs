using System;
using System.Collections.Generic;
using System.Text;

namespace TestOnlineModel.ViewModel.Admin
{
    public class UserScheduleViewModel
    {
        public Guid Id { get; set; }

        public Guid ScheduleId { get; set; }

        public Guid ExamId { get; set; }

        public string UserId { get; set;}

        public string FullName { get; set; }

        public string ScheduleName { get; set; }
        
        public string CategoryName { get; set; }

        public int TotalTime { get; set; }

        public string Status { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool? IsAccess { get; set; }

        public bool? IsSubmit { get; set; }
        public long STT { get; set; }

        public int TotalRecord { get; set; }
    }
}
