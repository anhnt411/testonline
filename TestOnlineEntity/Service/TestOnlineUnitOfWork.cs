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
using TestOnlineModel.ViewModel.Admin;
using TestOnlineShared.Interface;

namespace TestOnlineEntity.Service
{
    public class TestOnlineUnitOfWork : ITestOnlienUnitOfWork
    {
        private readonly DbContext _dbContext; 

        public TestOnlineUnitOfWork(DbContext dbContext,IRepository<TestCategory> testCategory,IRepository<TestUnit> testUnit
                                    ,IRepository<QuestionGroup> questionGroups,IRepository<Question> questions,IRepository<Exam> exams
                                    ,IRepository<Member> members,IRepository<Answer> answers ,IRepository<TestSchedule> testSchedules
                                    ,IRepository<ResultTest> resultTests,
                                     IRepository<TestCategoryViewModel> testCategoryViewModel,
                                     IRepository<TestUnitViewModel> testUnitViewModel,
                                     IRepository<TestMemberViewModel> testMemberViewModel,
                                     IRepository<QuestionGroupViewModel> questionGroupViewModel,
                                     IRepository<QuestionListViewModel> questionListViewModel,
                                     IRepository<QuestionContainerViewModel> questionContainerViewModel,
                                     IRepository<TestScheduleViewModel> testScheduleViewModel
            
                                    )
        {
            this._dbContext = dbContext;
            this.TestCategories = testCategory;
            this.TestUnits = testUnit;
            this.QuestionGroups = questionGroups;
            this.Questions = questions;
            this.Exams = exams;
            this.TestSchedules = testSchedules;
            this.Members = members;
            this.Answers = answers;
            this.ResultTests = resultTests;
            this.TestCategoryViewModels = testCategoryViewModel;
            this.TestUnitViewModels = testUnitViewModel;
            this.TestMemberViewModels = testMemberViewModel;
            this.QuestionGroupViewModels = questionGroupViewModel;
            this.QuestionListViewModels = questionListViewModel;
            this.QuestionContainerViewModels = questionContainerViewModel;
            this.TestScheduleViewModels = testScheduleViewModel;
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
        public virtual IRepository<TestCategoryViewModel> TestCategoryViewModels { get; }
        public virtual IRepository<TestUnitViewModel> TestUnitViewModels { get; }
        public virtual IRepository<TestMemberViewModel> TestMemberViewModels { get; }

        public virtual IRepository<QuestionGroupViewModel> QuestionGroupViewModels { get; }

        public virtual IRepository<QuestionListViewModel> QuestionListViewModels { get; }

        public virtual IRepository<QuestionContainerViewModel> QuestionContainerViewModels { get; }

        public virtual IRepository<TestScheduleViewModel> TestScheduleViewModels { get; }


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
