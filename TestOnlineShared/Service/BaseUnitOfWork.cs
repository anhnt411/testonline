using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TestOnlineShared.Interface;

namespace TestOnlineShared.Service
{
    public class BaseUnitOfWork : IBaseUnitOfWork
    {
        #region Property
        private readonly DbContext _dbContext;
        #endregion

        #region Constructor
        public BaseUnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region Method
        public IDbContextTransaction BeginTransactionScope()
        {
            return _dbContext.Database.BeginTransaction();
        }

        public int Commit()
        {
            return _dbContext.SaveChanges();
        }

        public Task<int> CommitAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return _dbContext.SaveChangesAsync(cancellationToken);
        }
        #endregion

    }
}
