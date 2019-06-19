using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestOnlineModel.ViewModel.User
{
    public class ApplicationUserViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare(nameof(Password),ErrorMessage ="Password mismatch")]
        public string ConfirmPassWord { get; set; }
        public string FullName { get; set; }
        public string Token { get; set; }
        public string Image { get; set; }
    }
}
