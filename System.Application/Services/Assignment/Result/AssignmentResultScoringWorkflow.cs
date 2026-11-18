using System.Application.Abstractions.Assignment;
using System.Application.Abstractions.Enrollment;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Enrollment.StudentAcademicEnrollment.DTO;
using System.Domain.Entities;
using System.Domain.Entities.Enrollment;
using System.Domain.Enums.Enrollment;
using System.Domain.Policies.Assignment;

namespace System.Application.Services.Assignment.Result
{
    internal sealed record AssignmentResultContext(
        clsAssignment Assignment,
        clsAssignmentNomination Nomination,
        StudentAcademicEnrollmentInfo AcademicEnrollment,
        Dictionary<Guid, clsAssignmentScopeinfo> ScopeInfos,
        clsAssignmentScopeinfo SelectedScopeInfo);

    internal static class AssignmentResultScoringWorkflow
    {
        public static async Task<TResult<AssignmentResultContext>> LoadContext(
            Guid assignmentID,
            Guid assignmentScopeInfoID,
            IAssignmentRepository assignmentRepository,
            IAssignmentNominationRepository nominationRepository,
            IStudentAcademicEnrollmentRepository academicEnrollmentRepository,
            IAssignmentScopeInfoRepository scopeInfoRepository)
        {
            var assignment = await assignmentRepository.GetEntityBy(assignmentID);
            if (assignment is null || assignment.AssignmentState == enAssignmentType.Canceled)
                return TResult<AssignmentResultContext>.Fail(ApplicationExMsg.AssignmentNotFound);

            var nomination = await nominationRepository.GetEntityBy(assignment.AssignmentNominationID);
            if (nomination is null)
                return TResult<AssignmentResultContext>.Fail(ApplicationExMsg.NotFound);

            var enrollment = await academicEnrollmentRepository.GetBy(nomination.StudentAcademicEnrollmentID);
            if (enrollment is null)
                return TResult<AssignmentResultContext>.Fail(ApplicationExMsg.StudentAcademicEnrollmentNotFound);
            if (assignment.ScheduledDate < enrollment.ValidFrom ||
                (enrollment.ValidTo.HasValue && assignment.ScheduledDate > enrollment.ValidTo.Value))
                return TResult<AssignmentResultContext>.Fail(ApplicationExMsg.StudentAcademicEnrollmentNotActive);

            if (!await nominationRepository.IsScopeIncludedInAcademicEnrollment(
                    studentAcademicEnrollmentID: nomination.StudentAcademicEnrollmentID,
                    assignmentScopeID: nomination.AssignmentScopeID))
                return TResult<AssignmentResultContext>.Fail(ApplicationExMsg.StudentAcademicEnrollmentStageInfoNotIncluded);

            var scopeInfos = await scopeInfoRepository.GetList(assignmentScopeInfoID);
            if (!scopeInfos.TryGetValue(assignmentScopeInfoID, out var selectedScopeInfo) ||
                selectedScopeInfo.AssignmentScopeID != nomination.AssignmentScopeID ||
                !enrollment.Items.Any(item => clsUtil.ToGuid( item.StageInfoID )== selectedScopeInfo.StageInfoID))
                return TResult<AssignmentResultContext>.Fail(ApplicationExMsg.StudentAcademicEnrollmentStageInfoNotIncluded);

            return TResult<AssignmentResultContext>.OK(
                new AssignmentResultContext(assignment, nomination, enrollment, scopeInfos, selectedScopeInfo));
        }

        public static async Task Recalculate(
            AssignmentResultContext context,
            IAssignmentRepository assignmentRepository,
            IResultRepository resultRepository,
            Guid userID,
            Guid? excludedResultID = null,
            clsResult? replacementResult = null)
        {
            var results = await resultRepository.GetListByAssignmentID(context.Assignment.Id);
            if (excludedResultID.HasValue)
                results.RemoveAll(result => result.Id == excludedResultID.Value);
            if (replacementResult is not null)
                results.Add(replacementResult);

            var outcome = AssignmentScoringPolicy.Calculate(context.ScopeInfos.Values, results);
            context.Assignment.SetTotalScore(outcome.TotalScore);
            context.Assignment.SetIsPassed(outcome.IsPassed);
            context.Assignment.SetAssignmentState(
                outcome.IsComplete ? enAssignmentType.Completed : enAssignmentType.inProgress);
            context.Assignment.SetModified(userID.ToString());
            await assignmentRepository.Update(context.Assignment);
        }
    }
}
