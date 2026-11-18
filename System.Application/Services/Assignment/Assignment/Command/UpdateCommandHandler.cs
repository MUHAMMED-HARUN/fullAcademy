using MediatR;
using System.Application.Abstractions.Assignment;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Enrollment;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;

namespace System.Application.Services.Assignment.Assignment.Command
{
    public class UpdateAssignmentCommandHandler : IRequestHandler<UpdateAssignmentCommand, TResult<string>>
    {
        private readonly IAssignmentRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAssignmentNominationRepository _nominationRepository;
        private readonly IStudentAcademicEnrollmentRepository _academicEnrollmentRepository;

        public UpdateAssignmentCommandHandler(
            IAssignmentRepository repo,
            IAssignmentNominationRepository nominationRepository,
            IStudentAcademicEnrollmentRepository academicEnrollmentRepository,
            ICurrentUserService currentUserService)
        {
            _repo = repo;
            _nominationRepository = nominationRepository;
            _academicEnrollmentRepository = academicEnrollmentRepository;
            _currentUserService = currentUserService;
        }

        public async Task<TResult<string>> Handle(UpdateAssignmentCommand request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(request.AssignmentID, out var assignmentID) || assignmentID == Guid.Empty ||
                !DateOnly.TryParse(request.ScheduledDate, out var scheduledDate))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            var entity = await _repo.GetEntityBy(assignmentID);
            if (entity is null)
                return TResult<string>.Fail(ApplicationExMsg.AssignmentNotFound);
            if (entity.AssignmentState != System.Domain.Enums.Enrollment.enAssignmentType.inProgress &&
                !_currentUserService.IsAdmin)
                return TResult<string>.Fail(ApplicationExMsg.AccessDinede);

            var nomination = await _nominationRepository.GetEntityBy(entity.AssignmentNominationID);
            if (nomination is null)
                return TResult<string>.Fail(ApplicationExMsg.NotFound);
            var academicEnrollment = await _academicEnrollmentRepository.GetBy(nomination.StudentAcademicEnrollmentID);
            if (academicEnrollment is null)
                return TResult<string>.Fail(ApplicationExMsg.StudentAcademicEnrollmentNotFound);
            if (academicEnrollment.Status != System.Domain.Enums.Enrollment.enStudentAcademicEnrollmentStatus.Active ||
                scheduledDate < academicEnrollment.ValidFrom ||
                (academicEnrollment.ValidTo.HasValue && scheduledDate > academicEnrollment.ValidTo.Value))
                return TResult<string>.Fail(ApplicationExMsg.StudentAcademicEnrollmentNotActive);
            if (!await _nominationRepository.IsScopeIncludedInAcademicEnrollment(
                    nomination.StudentAcademicEnrollmentID, nomination.AssignmentScopeID))
                return TResult<string>.Fail(ApplicationExMsg.StudentAcademicEnrollmentStageInfoNotIncluded);

            if (_currentUserService.UserId is not Guid userID || userID == Guid.Empty)
                return TResult<string>.Fail(ApplicationExMsg.AccessDinede);

            entity.SetScheduledDate(scheduledDate);
            entity.SetModified(userID.ToString());
            await _repo.Update(entity);
            return TResult<string>.OK(entity.Id.ToString());
        }
    }
}
