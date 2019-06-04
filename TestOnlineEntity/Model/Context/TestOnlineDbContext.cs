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

        public DbSet<TestUnit> TestUnits { get; set; }

        public DbSet<Answer> Answers { get; set; }

        public DbSet<Exam> Exams { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<QuestionGroup> QuestionGroups { get; set; }

        public DbSet<Member> Members { get; set; }

        public DbSet<TestSchedule> TestSchedules { get; set; }

    }
}
