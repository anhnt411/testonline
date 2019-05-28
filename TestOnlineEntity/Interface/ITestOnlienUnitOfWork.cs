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
    }
}
