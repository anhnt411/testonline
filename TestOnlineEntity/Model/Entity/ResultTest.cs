using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TestOnlineEntity.Model.Entity
{
    [Table("ResultTest")]
    public class ResultTest
    {
        [Key]
        public Guid Id { get; set; }

        public Guid MemberId { get; set; }

        public Guid ExamId { get; set; }

        public string TotalTime { get; set; }

        public int? Score { get; set; }

        public string CreatedBy { get; set; }
      
        public DateTime? CreatedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool IsActive { get; set; }
    }
}
