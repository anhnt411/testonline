using System;
using System.Collections.Generic;
using System.Text;

namespace TestOnlineModel.ViewModel.Admin
{
    public class TestScheduleViewModel
    {
        public Guid? Id { get; set; }
        public Guid CategoryId { get; set; }

        public string CategoryName { get; set; }
        public string Name { get; set; }

        public int Time { get; set; }

        public int TotalQuestion { get; set; }

        public int Percentage { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public string UserCreate { get; set; }

        public string OpenOrClose { get; set; }
        public bool? AllowViewAnswer { get; set; }
        public long STT { get; set; }

        public int TotalRecord { get; set; }
    }
}
