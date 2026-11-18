using AutoMapper;
using MediatR;
using System.Application.Abstractions.Assignment;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Enrollment;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;
using System.Domain.Enums.Enrollment;

namespace System.Application.Services.Assignment.Result.Command
{
    public sealed class NewResultCommandHandler : IRequestHandler<NewResultCommand, TResult<string>>
    {
        private readonly IResultRepository _resultRepository;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IAssignmentNominationRepository _nominationRepository;
        private readonly IStudentAcademicEnrollmentRepository _academicEnrollmentRepository;
        private readonly IAssignmentScopeInfoRepository _scopeInfoRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;

        public NewResultCommandHandler(
            IResultRepository resultRepository,
            IAssignmentRepository assignmentRepository,
            IAssignmentNominationRepository nominationRepository,
            IStudentAcademicEnrollmentRepository academicEnrollmentRepository,
            IAssignmentScopeInfoRepository scopeInfoRepository,
            ICurrentUserService currentUser,
            IMapper mapper)
        {
            _resultRepository = resultRepository;
            _assignmentRepository = assignmentRepository;
            _nominationRepository = nominationRepository;
            _academicEnrollmentRepository = academicEnrollmentRepository;
            _scopeInfoRepository = scopeInfoRepository;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<TResult<string>> Handle(NewResultCommand request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(request.AssignmentID, out var assignmentID) || assignmentID == Guid.Empty ||
                !Guid.TryParse(request.AssignmentScopeInfoID, out var scopeInfoID) || scopeInfoID == Guid.Empty)
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            var contextResult = await AssignmentResultScoringWorkflow.LoadContext(
                assignmentID,
                scopeInfoID,
                _assignmentRepository,
                _nominationRepository,
                _academicEnrollmentRepository,
                _scopeInfoRepository);
            if (!contextResult.IsSuccess)
                return TResult<string>.Fail(contextResult.Error);

            var context = contextResult.Value;
            if (context.Assignment.AssignmentState != enAssignmentType.inProgress ||
                context.AcademicEnrollment.Status != enStudentAcademicEnrollmentStatus.Active)
                return TResult<string>.Fail(ApplicationExMsg.StudentAcademicEnrollmentNotActive);
            if (!double.IsFinite(request.Score) || request.Score < 0 ||
                request.Score > context.SelectedScopeInfo.MaxScore)
                return TResult<string>.Fail(ApplicationExMsg.ResultScoreOutOfRange);
            if (await _resultRepository.HasResult(assignmentID, scopeInfoID))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);
            if (_currentUser.UserId is not Guid userID || userID == Guid.Empty)
                return TResult<string>.Fail(ApplicationExMsg.AccessDinede);

            var entity = _mapper.Map<NewResultCommand, clsResult>(request);
            entity.SetCreated(userID.ToString());
            var resultID = await _resultRepository.Add(entity);
            if (string.IsNullOrWhiteSpace(resultID))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            await AssignmentResultScoringWorkflow.Recalculate(
                context,
                _assignmentRepository,
                _resultRepository,
                userID,
                replacementResult: entity);

            return TResult<string>.OK(resultID);
        }
    }
}
