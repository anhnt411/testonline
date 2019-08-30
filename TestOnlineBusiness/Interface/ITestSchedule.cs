using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestOnlineBase.Helper.PagingHelper;
using TestOnlineEntity.Model.Entity;
using TestOnlineEntity.Model.ViewModel;
using TestOnlineModel.ViewModel.Admin;

namespace TestOnlineBusiness.Interface
{
    public interface ITestScheduleDomain
    {
        Task<bool> AddSchedule(TestScheduleViewModel viewModel, string userId, CancellationToken cancellationToken = default(CancellationToken));
        Task<IEnumerable<TestScheduleViewModel>> GetListSchedule(FilterModel filter, string userId);

        Task<TestSchedule> GetSchedule(Guid scheduleId);

        Task<IEnumerable<QuestionBankInfoViewModel>> GetListQuestionBankInfo(Guid categoryId);

        Task<bool> CreateExam(CreateExamViewModel viewModel,string userId);

        Task<bool> CreatedListMember(CreateListMemberViewModel viewModel, string userId);

        Task<bool> SendEmail(CreateListMemberViewModel viewModel, string userId);

        Task<IEnumerable<Exam>> GetListExam(Guid scheduleId);

        Task<IEnumerable<ApplicationUser>> GetListMemberSchedule(Guid scheduleId);

        Task<bool> DeleteMemberSchedule(DeleteMemberViewModel viewModle);
        Task<IEnumerable<ExamDetailViewModel>> GetListExamDetail(Guid examId, string userId);

        Task<IEnumerable<ExamDetailViewModel>> GetUserListExamDetail(Guid examId);

        Task<bool> UpdateAccessExam(Guid examId);

        Task<IEnumerable<UserScheduleViewModel>> GetListUserSchedule(FilterModel model, string userId);

        Task<bool> AddAnswerExamUser(UserAnswerViewModel viewModel, string userId);
    }
}
