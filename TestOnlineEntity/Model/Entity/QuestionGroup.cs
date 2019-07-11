using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TestOnlineEntity.Model.Entity
{
    [Table("QuestionGroups")]
    public class QuestionGroup
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }

        public Guid CategoryId { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public  DateTime? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public bool IsActive { get; set; }

        public string Description { get; set; }
    }
}
