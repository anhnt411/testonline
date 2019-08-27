using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TestOnlineEntity.Model.Entity
{
    [Table("ExamDetails")]
    public class ExamDetail
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ExamId { get; set; }

        public Guid QuestionId { get; set; }

        public int QuestionSequence { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public bool IsActive { get; set; }
    }
}
