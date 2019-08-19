using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TestOnlineEntity.Model.Entity
{
    [Table("Answers")]
    public class Answer
    {
        [Key]
        public Guid Id { get; set; }

        public Guid QuestionId { get; set; }

        public string Content { get; set; }

        public int? Sequence { get; set; }
        public bool IsCorrect { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public bool IsActive { get; set; }
    }
}
