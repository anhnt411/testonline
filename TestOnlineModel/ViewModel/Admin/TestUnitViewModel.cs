using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestOnlineModel.ViewModel.Admin
{
   public class TestUnitViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(300)]
        public string Name { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int TotalRecord { get; set; }

        public long STT { get; set; }

        public bool? Status { get; set; }

        public string CreatedById { get; set; }
    }
}
