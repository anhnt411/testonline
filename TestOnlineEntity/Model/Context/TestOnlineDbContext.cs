using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TestOnlineEntity.Model.Entity;
using TestOnlineEntity.Model.ViewModel;

namespace TestOnlineEntity.Model.Context
{
    public class TestOnlineDbContext : IdentityDbContext<ApplicationUser>
    {
        public TestOnlineDbContext(DbContextOptions<TestOnlineDbContext> options):base(options)
        {

        }

        public DbSet<TestCategory> TestCategories { get; set; }

    }
}
