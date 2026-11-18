using AutoMapper;
using MediatR;
using System.Application.Abstractions.Assignment;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Enrollment;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;

namespace System.Application.Services.Assignment.Result.Command
{
    public sealed class UpdateResultCommandHandler : IRequestHandler<UpdateResultCommand, TResult<string>>
    {
        private readonly IResultRepository _resultRepository;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IAssignmentNominationRepository _nominationRepository;
        private readonly IStudentAcademicEnrollmentRepository _academicEnrollmentRepository;
        private readonly IAssignmentScopeInfoRepository _scopeInfoRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;

        public UpdateResultCommandHandler(
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

        public async Task<TResult<string>> Handle(UpdateResultCommand request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(request.ResultID, out var resultID) || resultID == Guid.Empty ||
                !Guid.TryParse(request.AssignmentID, out var assignmentID) || assignmentID == Guid.Empty ||
                !Guid.TryParse(request.AssignmentScopeInfoID, out var scopeInfoID) || scopeInfoID == Guid.Empty)
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            var entity = await _resultRepository.GetEntityBy(resultID);
            if (entity is null || entity.AssignmentID != assignmentID || entity.AssignmentScopeInfoID != scopeInfoID)
                return TResult<string>.Fail(ApplicationExMsg.NotFound);

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
            if (!double.IsFinite(request.Score) || request.Score < 0 ||
                request.Score > context.SelectedScopeInfo.MaxScore)
                return TResult<string>.Fail(ApplicationExMsg.ResultScoreOutOfRange);
            if (_currentUser.UserId is not Guid userID || userID == Guid.Empty)
                return TResult<string>.Fail(ApplicationExMsg.AccessDinede);

            // Keep the result's assignment and scope-info identity immutable during editing.
            _mapper.Map(request, entity);
            entity.SetModified(userID.ToString());
            await _resultRepository.Update(entity);
            await AssignmentResultScoringWorkflow.Recalculate(
                context,
                _assignmentRepository,
                _resultRepository,
                userID,
                excludedResultID: entity.Id,
                replacementResult: entity);

            return TResult<string>.OK(entity.Id.ToString());
        }
    }
}
