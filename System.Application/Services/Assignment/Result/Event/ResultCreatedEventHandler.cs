using MediatR;
using System.Application.Abstractions.Assignment;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;
using System.Domain.Enums.Enrollment;

namespace System.Application.Services.Assignment.Assignment.EventHandlers
{
    public class ResultCreatedEventHandler : INotificationHandler<ResultCreatedEvent>
    {
        private readonly IAssignmentRepository _assigmentRepo;
        private readonly IAssignmentScopeInfoRepository _scopeInfoRepo;
        private readonly IResultRepository _resultRepo;


        public ResultCreatedEventHandler(IAssignmentRepository assignment,
            IAssignmentScopeInfoRepository assignmentScopeInfo,
            IResultRepository result)
        {
            _assigmentRepo =assignment;
            _scopeInfoRepo =assignmentScopeInfo;
            _resultRepo =result;
        }
        public async Task Handle(ResultCreatedEvent result, CancellationToken cancellationToken)
        {
            var ScopeInfos = await _scopeInfoRepo.GetList(result.AssignmentScopeInfoID);

            if (ScopeInfos != null)
            {
                var scopeInfoResults = await _resultRepo.GetList(
                    ScopeInfos.Select(x => x.Key).ToArray());

                if (!scopeInfoResults.Any(r => r.AssignmentScopeInfoID == result.AssignmentScopeInfoID))
                {
                    scopeInfoResults.Add(new clsResult(result.AssigmentID, result.AssignmentScopeInfoID, result.Score, null));
                }
                if (scopeInfoResults != null )
                {
                    decimal totalWeightedScore = 0;
                    decimal totalWeights = 0;

                    foreach (var res in scopeInfoResults)
                    {
                        if (ScopeInfos.TryGetValue(res.AssignmentScopeInfoID, out var scopeInfo))
                        {
                            if (scopeInfo.MaxScore > 0 && scopeInfo.Weight > 0)
                            {
                                decimal ratio = (decimal)res.Score / (decimal)scopeInfo.MaxScore;
                                totalWeightedScore += ratio * (decimal)scopeInfo.Weight;
                                totalWeights += (decimal)scopeInfo.Weight;
                            }
                        }
                    }

                    decimal finalPercentage = totalWeights > 0 ? (totalWeightedScore / totalWeights) * 100 : 0;

                    bool isPass = finalPercentage >= 50.0m;

                    var assignment = await _assigmentRepo.GetEntityBy(result.AssigmentID);
                    if (assignment != null)
                    {
                        assignment.SetTotalScore((double)finalPercentage);
                        assignment.SetIsPassed(isPass);
                        assignment.SetAssignmentState(enAssignmentType.Completed);

                    }

                }

            }
        }
    }
}