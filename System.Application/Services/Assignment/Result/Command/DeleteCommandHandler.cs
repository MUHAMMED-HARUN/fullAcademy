using MediatR;
using System.Application.Abstractions.Assignment;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Enrollment;
using System.Application.Common;
using System.Application.Exceptions;

namespace System.Application.Services.Assignment.Result.Command
{
    public sealed class DeleteResultCommandHandler : IRequestHandler<DeleteResultCommand, TResult<string>>
    {
        private readonly IResultRepository _resultRepository;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IAssignmentNominationRepository _nominationRepository;
        private readonly IStudentAcademicEnrollmentRepository _academicEnrollmentRepository;
        private readonly IAssignmentScopeInfoRepository _scopeInfoRepository;
        private readonly ICurrentUserService _currentUser;

        public DeleteResultCommandHandler(
            IResultRepository resultRepository,
            IAssignmentRepository assignmentRepository,
            IAssignmentNominationRepository nominationRepository,
            IStudentAcademicEnrollmentRepository academicEnrollmentRepository,
            IAssignmentScopeInfoRepository scopeInfoRepository,
            ICurrentUserService currentUser)
        {
            _resultRepository = resultRepository;
            _assignmentRepository = assignmentRepository;
            _nominationRepository = nominationRepository;
            _academicEnrollmentRepository = academicEnrollmentRepository;
            _scopeInfoRepository = scopeInfoRepository;
            _currentUser = currentUser;
        }

        public async Task<TResult<string>> Handle(DeleteResultCommand request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(request.ResultID, out var resultID) || resultID == Guid.Empty)
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            var entity = await _resultRepository.GetEntityBy(resultID);
            if (entity is null)
                return TResult<string>.Fail(ApplicationExMsg.NotFound);

            var contextResult = await AssignmentResultScoringWorkflow.LoadContext(
                entity.AssignmentID,
                entity.AssignmentScopeInfoID,
                _assignmentRepository,
                _nominationRepository,
                _academicEnrollmentRepository,
                _scopeInfoRepository);
            if (!contextResult.IsSuccess)
                return TResult<string>.Fail(contextResult.Error);
            if (_currentUser.UserId is not Guid userID || userID == Guid.Empty)
                return TResult<string>.Fail(ApplicationExMsg.AccessDinede);

            entity.SetDeleted(userID.ToString());
            await _resultRepository.Update(entity);
            await AssignmentResultScoringWorkflow.Recalculate(
                contextResult.Value,
                _assignmentRepository,
                _resultRepository,
                userID,
                excludedResultID: entity.Id);

            return TResult<string>.OK(string.Empty);
        }
    }
}
