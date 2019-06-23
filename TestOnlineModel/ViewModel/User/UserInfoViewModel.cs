using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TestOnlineBase.Helper.FileHelper;

namespace TestOnlineModel.ViewModel.User
{
    public class UserInfoViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string FullName { get; set; }
        public string Image { get; set; }
      
    }
}
