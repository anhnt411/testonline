using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestOnlineModel.ViewModel.User
{
    public class ChangePasswordViewmodel
    {
        [Required]
        public string Password { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        [Compare(nameof(NewPassword), ErrorMessage = "Password mismatch")]
        public string ConfirmNewPassword { get; set; }
    }
}
