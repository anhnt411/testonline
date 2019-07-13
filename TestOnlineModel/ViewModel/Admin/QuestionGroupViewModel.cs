using System;
using System.Collections.Generic;
using System.Text;

namespace TestOnlineModel.ViewModel.Admin
{
    public class QuestionGroupViewModel
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public int? NumberOfQuestion { get; set; }

        public string Description { get; set; }

        public Guid CategoryId { get; set; }

        public string CategoryName { get; set; }

        public long STT { get; set; }

        public int TotalRecord { get; set; }

        public string CreatedBy { get; set; }

        public string CreatedById { get; set; }

        public DateTime? CreatedDate { get; set; }

        


    }
}
