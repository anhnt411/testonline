using System;
using System.Collections.Generic;
using System.Text;

namespace TestOnlineModel.ViewModel.Admin
{
    public class CreateListMemberViewModel
    {
        public Guid ScheduleId { get; set; }

        public IEnumerable<string> ListMember { get; set; }
    }
}
