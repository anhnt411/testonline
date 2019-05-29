using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TestOnlineEntity.Model.ViewModel
{
    public class ApplicationUser:IdentityUser
    {
        public string CreatedBy { get; set; }
        [Column(TypeName = "nvarchar(150)")]
        public string FullName { get; set; }
        public string Address { get; set; }
        public bool Status { get; set; }
    }
}
