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

        public TestOnlineUnitOfWork(DbContext dbContext,IRepository<TestCategory> testCategory)
        {
            this._dbContext = dbContext;
            this.TestCategories = testCategory;
        }

        public virtual IRepository<TestCategory> TestCategories { get; set; }

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
