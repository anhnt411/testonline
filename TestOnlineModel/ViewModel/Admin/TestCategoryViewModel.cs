using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestOnlineModel.ViewModel.Admin
{
    public class TestCategoryViewModel
    {
       
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(300)]
        public string Name { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }

        public string UpdatedBy { get; set; }

        public string UpdateById { get; set; }

        public DateTime UpdatedTime { get; set; }

        public int TotalRecord { get; set; }

        public long STT { get; set; }

        public bool? Status { get; set; }
    }
}
