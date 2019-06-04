using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TestOnlineEntity.Model.Entity
{
    [Table("Exams")]
    public class Exam
    {
        [Key]
        public Guid Id { get; set; }

        public int TotalQuestion {get;set;}

        public Guid TestScheduleId { get; set; }

        public int? PassCondition { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public bool IsActive { get; set; }
    }
}
