using MediatR;
using System.Application.Abstractions.Assignment;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Enrollment;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;
using System.Domain.Enums.Enrollment;
using System.Domain.Enums.Person;

namespace System.Application.Services.Assignment.AssignmentNomination.Command
{
    public class NewAssignmentNominationCommandHandler : IRequestHandler<NewAssignmentNominationCommand, TResult<string>>
    {
        private readonly IAssignmentNominationRepository     _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStudentAcademicEnrollmentRepository _academicEnrollmentRepository;
        private readonly IAssignmentScopeRepository _assignmentScopeRepository;

        public NewAssignmentNominationCommandHandler(
            IAssignmentNominationRepository repo,
            IStudentAcademicEnrollmentRepository academicEnrollmentRepository,
            IAssignmentScopeRepository assignmentScopeRepository,
            ICurrentUserService currentUserService)
        {
            _repo = repo;
            _academicEnrollmentRepository = academicEnrollmentRepository;
            _assignmentScopeRepository = assignmentScopeRepository;
            _currentUserService = currentUserService;
        }

        public async Task<TResult<string>> Handle(NewAssignmentNominationCommand request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(request.StudentAcademicEnrollmentID, out var academicEnrollmentID) || academicEnrollmentID == Guid.Empty ||
                !Guid.TryParse(request.AssignmentScopeID, out var assignmentScopeID) || assignmentScopeID == Guid.Empty ||
                !DateOnly.TryParse(request.Date, out var nominationDate))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            var enrollment = await _academicEnrollmentRepository.GetBy(academicEnrollmentID);
            if (enrollment is null)
                return TResult<string>.Fail(ApplicationExMsg.StudentAcademicEnrollmentNotFound);
            if (enrollment.Status != enStudentAcademicEnrollmentStatus.Active ||
                nominationDate < enrollment.ValidFrom ||
                (enrollment.ValidTo.HasValue && nominationDate > enrollment.ValidTo.Value))
                return TResult<string>.Fail(ApplicationExMsg.StudentAcademicEnrollmentNotActive);

            bool hasNomination = await _repo.HasNomination(academicEnrollmentID, assignmentScopeID);
            if (hasNomination)
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            var availableScopes = await _assignmentScopeRepository.GetCanNominate(academicEnrollmentID.ToString());
            if (!availableScopes.Any(scope => Guid.TryParse(scope.AssignmentScopeID, out var scopeID) && scopeID == assignmentScopeID))
                return TResult<string>.Fail(ApplicationExMsg.AssignmentNominationScopeNotEligible);

            if (_currentUserService.UserId is not Guid userID || userID == Guid.Empty)
                return TResult<string>.Fail(ApplicationExMsg.AccessDinede);

            var nominatedByEntityID = _currentUserService.IsAdmin
                ? _currentUserService.EntityGuid ?? userID
                : _currentUserService.EntityGuid;
            if (nominatedByEntityID is not Guid actorID || actorID == Guid.Empty)
                return TResult<string>.Fail(ApplicationExMsg.AccessDinede);

            var nominatedByType = _currentUserService.IsAdmin
                ? enUserType.Admin
                : string.Equals(_currentUserService.EntityType, "Teacher", StringComparison.OrdinalIgnoreCase)
                    ? enUserType.Teacher
                    : enUserType.Admin;
            if (!_currentUserService.IsAdmin && nominatedByType != enUserType.Teacher)
                return TResult<string>.Fail(ApplicationExMsg.AccessDinede);

            var entity = new clsAssignmentNomination(
                academicEnrollmentID,
                assignmentScopeID,
                nominationDate,
                actorID,
                nominatedByType,
                enAssignmentType.inProgress);
            entity.SetCreated(userID.ToString());

            string entityID = await _repo.Add(entity);

            if (string.IsNullOrEmpty(entityID))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            return TResult<string>.OK(entityID);
        }
    }
}
