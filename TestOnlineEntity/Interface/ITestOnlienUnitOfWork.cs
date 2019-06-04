using System;
using System.Collections.Generic;
using System.Text;
using TestOnlineEntity.Model.Entity;
using TestOnlineShared.Interface;

namespace TestOnlineEntity.Interface
{
    public interface ITestOnlienUnitOfWork : IBaseUnitOfWork
    {
        IRepository<TestCategory> TestCategories { get; set; }

        IRepository<TestUnit> TestUnits { get; set; }

        IRepository<QuestionGroup> QuestionGroups { get; set; }

        IRepository<Member> Members { get; set; }

        IRepository<Answer> Answers { get; set; }

        IRepository<Exam> Exams { get; set; }

        IRepository<TestSchedule> TestSchedules { get; set; }

        IRepository<Question> Questions { get; set; }

        IRepository<ResultTest> ResultTests { get; set; }
    }
}
