using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TestOnlineEntity.Model.Entity;

namespace TestOnlineEntity.Model.Context
{
    public class TestOnlineDbContext : DbContext
    {
        public TestOnlineDbContext(DbContextOptions<TestOnlineDbContext> options):base(options)
        {

        }

        public DbSet<TestCategory> TestCategories { get; set; }

    }
}
