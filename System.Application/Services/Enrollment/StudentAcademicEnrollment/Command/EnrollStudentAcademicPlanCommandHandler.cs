using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Enrollment;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Enrollment.StudentAcademicEnrollment.Command;
using System.Domain.Entities.Enrollment;

namespace System.Application.Services.Enrollment.StudentAcademicEnrollment.Command
{
    public sealed class EnrollStudentAcademicPlanCommandHandler : IRequestHandler<EnrollStudentAcademicPlanCommand, TResult<string>>
    {
        private readonly IStudentAcademicEnrollmentRepository _repository;
        private readonly ICurrentUserService _currentUser;

        public EnrollStudentAcademicPlanCommandHandler(
            IStudentAcademicEnrollmentRepository repository,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<TResult<string>> Handle(EnrollStudentAcademicPlanCommand request, CancellationToken cancellationToken)
        {
            var studentID = clsUtil.ToGuid(request.StudentID);
            var academicPlanID = clsUtil.ToGuid(request.AcademicPlanID);
            if (!_currentUser.IsAdmin &&
                (!string.Equals(_currentUser.EntityType, "Student", StringComparison.OrdinalIgnoreCase) ||
                 _currentUser.EntityGuid != studentID))
                return TResult<string>.Fail("لا يمكنك إنشاء اشتراك أكاديمي لهذا الطالب");
            if (!await _repository.IsStudentExist(studentID))
                return TResult<string>.Fail(ApplicationExMsg.StudentAcademicEnrollmentStudentNotFound);

            var plan = await _repository.GetActivePlanForEnrollment(academicPlanID);
            if (plan == null)
                return TResult<string>.Fail(ApplicationExMsg.StudentAcademicEnrollmentPlanUnavailable);
            if (request.EnrollmentDate < plan.EffectiveFrom ||
                (plan.EffectiveTo.HasValue && request.EnrollmentDate > plan.EffectiveTo) ||
                request.ValidFrom < plan.EffectiveFrom || request.ValidFrom > request.EnrollmentDate ||
                (plan.EffectiveTo.HasValue && request.ValidTo.HasValue && request.ValidTo > plan.EffectiveTo))
                return TResult<string>.Fail(ApplicationExMsg.StudentAcademicEnrollmentOutsidePlanValidity);
            if (await _repository.HasActiveEnrollment(studentID, academicPlanID))
                return TResult<string>.Fail(ApplicationExMsg.StudentAcademicEnrollmentAlreadyExists);
            if (_currentUser.UserId is not Guid userID)
                return TResult<string>.Fail("المستخدم الحالي غير صالح");

            var snapshots = plan.Items.Select(item => new clsStudentAcademicEnrollmentItem(
                clsUtil.ToGuid(item.StageInfoID), item.From, item.To,
                clsUtil.ToGuid(item.StageID), item.StageName,
                clsUtil.ToGuid(item.MatterID), item.MatterName,
                item.UniteType, item.ProgressType, item.UniteTypeName, item.ProgressTypeName)).ToList();
            var enrollment = new clsStudentAcademicEnrollment(
                studentID, academicPlanID, plan.Version, request.EnrollmentDate,
                request.ValidFrom, request.ValidTo ?? plan.EffectiveTo, snapshots);
            enrollment.SetCreated(userID.ToString());
            foreach (var item in enrollment.Items)
                item.SetCreated(userID.ToString());

            var id = await _repository.Add(enrollment);
            return string.IsNullOrWhiteSpace(id)
                ? TResult<string>.Fail(ApplicationExMsg.FailAdd)
                : TResult<string>.OK(id);
        }
    }
}
