using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TestOnlineEntity.Interface;
using TestOnlineEntity.Model.Entity;
using TestOnlineShared.Interface;

namespace TestOnlineEntity.Service
{
    public class TestOnlineUnitOfWork : ITestOnlienUnitOfWork
    {
        private readonly DbContext _dbContext; 

        public TestOnlineUnitOfWork(DbContext dbContext,IRepository<TestCategory> testCategory,IRepository<TestUnit> testUnit
                                    ,IRepository<QuestionGroup> questionGroups,IRepository<Question> questions,IRepository<Exam> exams
                                    ,IRepository<Member> members,IRepository<Answer> answers ,IRepository<TestSchedule> testSchedules
                                    ,IRepository<ResultTest> resultTests
                                    )
        {
            this._dbContext = dbContext;
            this.TestCategories = testCategory;
            this.TestUnits = TestUnits;
            this.QuestionGroups = questionGroups;
            this.Questions = questions;
            this.Exams = exams;
            this.TestSchedules = testSchedules;
            this.Members = members;
            this.Answers = answers;
            this.ResultTests = resultTests;
        }

        public virtual IRepository<TestCategory> TestCategories { get; set; }       
        public virtual IRepository<TestUnit> TestUnits { get; set; }     
        public virtual IRepository<QuestionGroup> QuestionGroups { get; set; }   
        public virtual IRepository<Member> Members { get; set; }    
        public virtual IRepository<Answer> Answers { get; set; }  
        public virtual IRepository<Exam> Exams { get; set; }     
        public virtual IRepository<TestSchedule> TestSchedules { get; set; }  
        public virtual IRepository<Question> Questions { get; set; }
        public virtual IRepository<ResultTest> ResultTests { get; set; }
 
        public IDbContextTransaction BeginTransactionScope()
        {
            return _dbContext.Database.BeginTransaction();
        }

        public int Commit()
        {
            return _dbContext.SaveChanges();
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public IDbContextTransaction BeginTransactionScope(IsolationLevel isolationLevel)
        {
            return _dbContext.Database.BeginTransaction(isolationLevel);
        }
    }
}
