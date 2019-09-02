using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TestOnlineEntity.Model.Entity
{
    [Table("ExamUsers")]
    public class ExamUser
    {
        [Key]
        public Guid Id { get; set; }

        public string MemberId { get; set; }

        public Guid ExamId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Updatedby { get; set; }

        public DateTime UpdatedDate { get; set; }

        public Guid ScheduleId { get; set; }

        public bool? IsAccess { get; set; }

        public bool? IsSubmit { get; set; }

        public bool IsActive { get; set; }

        public bool? IsPass { get; set; }
    }
}

