using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TestOnlineEntity.Model.Entity
{
    [Table("Questions")]
    public class Question
    {
        [Key]
        public Guid Id { get; set; }

        public int QuestionTypeKey { get; set; }

        public Guid QuestionGroupId { get; set; }

      
        [Required]
        public string Description { get; set; }

     

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public bool IsActive { get; set; }
    }
}
