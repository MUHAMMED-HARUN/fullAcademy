using AutoMapper;
using MediatR;
using System.Application.Abstractions.Assignment;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Enrollment;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;

namespace System.Application.Services.Assignment.AssignmentNomination.Command
{
    public class UpdateAssignmentNominationCommandHandler : IRequestHandler<UpdateAssignmentNominationCommand, TResult<string>>
    {
        private readonly IAssignmentNominationRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly System.Application.Abstractions.Enrollment.IStudentAcademicEnrollmentRepository _academicEnrollmentRepository;
        private readonly IMapper _mapper;

        public UpdateAssignmentNominationCommandHandler(IAssignmentNominationRepository repo, ICurrentUserService currentUserService, IStudentAcademicEnrollmentRepository academicEnrollmentRepository, IMapper mapper)
        {
            _repo = repo;
            _currentUserService = currentUserService;
            _academicEnrollmentRepository = academicEnrollmentRepository;
            _mapper = mapper;
        }

        public async Task<TResult<string>> Handle(UpdateAssignmentNominationCommand request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(request.AssignmentNominationID, out var nominationID) || nominationID == Guid.Empty ||
                !Guid.TryParse(request.StudentAcademicEnrollmentID, out var enrollmentID) || enrollmentID == Guid.Empty ||
                !Guid.TryParse(request.AssignmentScopeID, out var scopeID) || scopeID == Guid.Empty)
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            var entity = await _repo.GetEntityBy(nominationID);
            if (entity is null || entity.StudentAcademicEnrollmentID != enrollmentID)
                return TResult<string>.Fail(ApplicationExMsg.AssignmentNotFound);

            var enrollment = await _academicEnrollmentRepository.GetBy(enrollmentID);
            if (enrollment is null)
                return TResult<string>.Fail(ApplicationExMsg.StudentAcademicEnrollmentNotFound);
            if (enrollment.Status != System.Domain.Enums.Enrollment.enStudentAcademicEnrollmentStatus.Active ||
                request.Date < enrollment.ValidFrom ||
                (enrollment.ValidTo.HasValue && request.Date > enrollment.ValidTo.Value))
                return TResult<string>.Fail(ApplicationExMsg.StudentAcademicEnrollmentNotActive);
            if (!_currentUserService.IsAdmin && entity.NominationState != System.Domain.Enums.Enrollment.enAssignmentType.inProgress)
                return TResult<string>.Fail(ApplicationExMsg.AccessDinede);
            if (!await _repo.IsScopeIncludedInAcademicEnrollment(enrollmentID, scopeID))
                return TResult<string>.Fail(ApplicationExMsg.StudentAcademicEnrollmentStageInfoNotIncluded);

            _mapper.Map(request, entity);
            if (_currentUserService.UserId is not Guid userID)
                return TResult<string>.Fail(ApplicationExMsg.AccessDinede);
            entity.SetModified(userID.ToString());
            await _repo.Update(entity);
            return TResult<string>.OK(entity.Id.ToString());
        }
    }
}
