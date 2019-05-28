using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TestOnlineEntity.Model.Entity
{
    [Table(nameof(TestCategory))]
    public class TestCategory
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is not empty")]
        [MaxLength(200,ErrorMessage = "Allow 200 character")]
        public string Name { get; set; }

        public string Image { get; set; }

        [Required]
        public string CreatedBy { get; set; }
        [Required]
        public DateTime? CreatedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool Status { get; set; }


    }
}
