﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TestOnlineEntity.Model.Entity;
using TestOnlineEntity.Model.ViewModel;
using TestOnlineModel.ViewModel.Admin;

namespace TestOnlineEntity.Model.Context
{
    public class TestOnlineDbContext : IdentityDbContext<ApplicationUser>
    {
        public TestOnlineDbContext(DbContextOptions<TestOnlineDbContext> options):base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.Query<TestCategoryViewModel>();
        }

        public DbSet<TestCategory> TestCategories { get; set; }

        public DbSet<TestUnit> TestUnits { get; set; }

        public DbSet<Answer> Answers { get; set; }

        public DbSet<Exam> Exams { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<QuestionGroup> QuestionGroups { get; set; }

        public DbSet<Member> Members { get; set; }

        public DbSet<TestSchedule> TestSchedules { get; set; }

        public DbSet<ExamDetail> ExamDetails { get; set; }

        public DbSet<ScheduleUser> ScheduleUsers { get; set; }

        public DbSet<ExamUser> ExamUsers { get; set; }

        public DbSet<AnswerExamUser> AnswerExamUsers { get; set; }

        public DbSet<TestCategoryViewModel> TestCategoryViewModels { get; }

        public DbSet<TestUnitViewModel> TestUnitViewModels { get; }

        public DbSet<TestMemberViewModel> TestMemberViewModels { get; }

        public DbSet<QuestionGroupViewModel> QuestionGroupViewModels { get; }

        public DbSet<QuestionListViewModel> QuestionListViewModels { get; }

        public DbSet<TestScheduleViewModel> TestScheduleViewModels { get; }
        public DbSet<QuestionContainerViewModel> QuestionContainerViewModel { get; }

        public DbSet<QuestionBankInfoViewModel> QuestionBankInfoViewModels { get; }

        public DbSet<UserScheduleViewModel> UserScheduleViewModels { get; }
    }

    
}
