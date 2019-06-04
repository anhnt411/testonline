using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestOnlineModel.ViewModel.Admin
{
    public class TestCategoryViewModel
    {
        [Key]
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(300)]
        public string Name { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }
    }
}
