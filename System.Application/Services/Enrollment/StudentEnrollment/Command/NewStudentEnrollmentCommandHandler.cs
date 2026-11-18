using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Enrollment;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Enrollment.StudentEnrollment.Command;
using System.Domain.Entities;

namespace System.Application.Services.Enrollment.StudentEnrollment.Command
{
    public class NewStudentEnrollmentCommandHandler : IRequestHandler<NewStudentEnrollmentCommand, TResult<string>>
    {
        private readonly IStudentEnrollmentRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        public NewStudentEnrollmentCommandHandler(IStudentEnrollmentRepository repo, ICurrentUserService currentUser)
        {
            _repo = repo;
            _currentUserService = currentUser;
        }

        public async Task<TResult<string>> Handle(NewStudentEnrollmentCommand request, CancellationToken cancellationToken)
        {
            var academicEnrollmentID = clsUtil.ToGuid(request.StudentAcademicEnrollmentID);
            var groupID = clsUtil.ToGuid(request.GroupID);
            var enrollmentContext = await _repo.GetEligibleAcademicEnrollmentForGroup(academicEnrollmentID, groupID, request.Date);
            if (enrollmentContext == null)
                return TResult<string>.Fail(ApplicationExMsg.StudentAcademicEnrollmentGroupNotEligible);

            if (await _repo.IsAcademicEnrollmentAlreadyInGroup(academicEnrollmentID, groupID))
                return TResult<string>.Fail(ApplicationExMsg.StudentAlreadyEnrolledInGroup);

            if (_currentUserService.UserId is not Guid userID)
                return TResult<string>.Fail("المستخدم الحالي غير صالح");

            var entity = new clsStudentEnrollment(
                clsUtil.ToGuid(enrollmentContext.StudentID),
                groupID,
                academicEnrollmentID,
                request.Date);
            entity.SetCreated(userID.ToString());

            string entityID = await _repo.Add(entity);

            if (string.IsNullOrEmpty(entityID))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            return TResult<string>.OK(entityID);
        }
    }
}
