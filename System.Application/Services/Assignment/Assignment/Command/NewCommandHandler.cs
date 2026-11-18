using MediatR;
using System.Application.Abstractions.Assignment;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Enrollment;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;
using System.Domain.Enums.Enrollment;

namespace System.Application.Services.Assignment.Assignment.Command
{
    public class NewAssignmentCommandHandler : IRequestHandler<NewAssignmentCommand, TResult<string>>
    {
        private readonly IAssignmentRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAssignmentNominationRepository _nominationRepository;
        private readonly IStudentAcademicEnrollmentRepository _academicEnrollmentRepository;

        public NewAssignmentCommandHandler(
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

        public async Task<TResult<string>> Handle(NewAssignmentCommand request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(request.AssignmentNominationID, out var nominationID) || nominationID == Guid.Empty ||
                !DateOnly.TryParse(request.ScheduledDate, out var scheduledDate))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            var nomination = await _nominationRepository.GetEntityBy(nominationID);
            if (nomination is null || nomination.NominationState != enAssignmentType.inProgress)
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            var academicEnrollment = await _academicEnrollmentRepository.GetBy(nomination.StudentAcademicEnrollmentID);
            if (academicEnrollment is null)
                return TResult<string>.Fail(ApplicationExMsg.StudentAcademicEnrollmentNotFound);
            if (academicEnrollment.Status != enStudentAcademicEnrollmentStatus.Active ||
                scheduledDate < academicEnrollment.ValidFrom ||
                (academicEnrollment.ValidTo.HasValue && scheduledDate > academicEnrollment.ValidTo.Value))
                return TResult<string>.Fail(ApplicationExMsg.StudentAcademicEnrollmentNotActive);
            if (!await _nominationRepository.IsScopeIncludedInAcademicEnrollment(
                    nomination.StudentAcademicEnrollmentID, nomination.AssignmentScopeID))
                return TResult<string>.Fail(ApplicationExMsg.StudentAcademicEnrollmentStageInfoNotIncluded);
            if (await _repo.HasAssignment(nominationID))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            if (_currentUserService.UserId is not Guid userID || userID == Guid.Empty)
                return TResult<string>.Fail(ApplicationExMsg.AccessDinede);

            var committeeID = _currentUserService.IsAdmin
                ? _currentUserService.EntityGuid ?? userID
                : _currentUserService.EntityGuid;
            if (committeeID is not Guid committeeGuid || committeeGuid == Guid.Empty)
                return TResult<string>.Fail(ApplicationExMsg.AccessDinede);

            var committeeType = _currentUserService.IsAdmin
                ? enCommitteeType.User
                : string.Equals(_currentUserService.EntityType, "Teacher", StringComparison.OrdinalIgnoreCase)
                    ? enCommitteeType.Teacher
                    : enCommitteeType.User;
            if (!_currentUserService.IsAdmin && committeeType != enCommitteeType.Teacher)
                return TResult<string>.Fail(ApplicationExMsg.AccessDinede);

            var entity = new clsAssignment(
                nominationID,
                committeeGuid,
                committeeType,
                scheduledDate,
                enAssignmentType.inProgress,
                null,
                null);
            entity.SetCreated(userID.ToString());

            string entityID = await _repo.Add(entity);

            if (string.IsNullOrEmpty(entityID))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);
          

            nomination.SetNominationState(enAssignmentType.Completed);
            nomination.SetModified(userID.ToString());
            await _nominationRepository.Update(nomination);

            return TResult<string>.OK(entityID);
        }
    }
}
