using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestOnlineModel.ViewModel
{
   public class LoginViewModel
    {
        [Required(ErrorMessage = "Please input UserName")]
        [MaxLength(200)]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please input Password")]
        [MaxLength(400)]
        public string PassWord { get; set; }
    }
}
