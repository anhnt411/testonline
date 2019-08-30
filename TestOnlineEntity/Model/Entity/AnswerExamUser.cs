﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TestOnlineEntity.Model.Entity
{
    [Table("AnswerExamUsers")]
    public class AnswerExamUser
    {
        [Key]
        public Guid Id { get; set; }
            
        public Guid ExamId { get; set;}

        public Guid QuestionId { get; set; }

        public Guid AnswerId { get; set; }
        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public bool IsActive { get; set; }
    }
}
