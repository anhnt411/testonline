using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestOnlineShared.Interface
{
    public interface IBaseUnitOfWork
    {
        #region Properties

        #endregion

        #region Methods

        /// <summary>
        ///     Save changes into database.
        /// </summary>
        /// <returns></returns>
        int Commit();

        /// <summary>
        ///     Save changes into database asynchronously.
        /// </summary>
        /// <returns></returns>
        Task<int> CommitAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        ///     Begin transaction scope.
        /// </summary>
        /// <returns></returns>
        IDbContextTransaction BeginTransactionScope();

        #endregion
    }
}
