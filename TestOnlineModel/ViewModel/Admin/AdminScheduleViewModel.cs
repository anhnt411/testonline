using System;
using System.Collections.Generic;
using System.Text;

namespace TestOnlineModel.ViewModel.Admin
{
    public class AdminScheduleViewModel
    {
        public Guid ScheduleId { get; set; }

        public int TotalMember { get; set; }

        public int TotalPass { get; set; }

        public int TotalAccess { get; set; }

        public int TotalNotAccess { get; set; }
    }
}
