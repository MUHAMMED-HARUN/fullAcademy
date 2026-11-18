using System.Domain.Entities;

namespace System.Domain.Policies.Assignment
{
    /// <summary>
    /// Calculates the weighted percentage after every active scope item has exactly one result.
    /// Passing is determined by each scope item's configured MinPassScore; the weighted percentage is informational.
    /// </summary>
    public static class AssignmentScoringPolicy
    {
        public static AssignmentScoreOutcome Calculate(
            IEnumerable<clsAssignmentScopeinfo> assignmentScopeInfos,
            IEnumerable<clsResult> results)
        {
            var scopeInfos = assignmentScopeInfos
                .Where(info => info.IsActive && info.DeletedAt is null)
                .ToList();
            var activeResults = results
                .Where(result => result.IsActive && result.DeletedAt is null)
                .ToList();

            if (scopeInfos.Count == 0 ||
                activeResults.GroupBy(result => result.AssignmentScopeInfoID).Any(group => group.Count() != 1) ||
                scopeInfos.Any(info => activeResults.All(result => result.AssignmentScopeInfoID != info.Id)) ||
                activeResults.Any(result => scopeInfos.All(info => info.Id != result.AssignmentScopeInfoID)))
            {
                return new AssignmentScoreOutcome(false, null, null);
            }

            decimal weightedScore = 0m;
            decimal totalWeight = 0m;
            var everyItemPassed = true;

            foreach (var info in scopeInfos)
            {
                var result = activeResults.Single(item => item.AssignmentScopeInfoID == info.Id);
                everyItemPassed &= result.Score >= info.MinPassScore;

                var weight = (decimal)info.Weight;
                totalWeight += weight;
                if (weight > 0 && info.MaxScore > 0)
                    weightedScore += (decimal)result.Score / info.MaxScore * weight;
            }

            var totalScore = totalWeight > 0 ? weightedScore / totalWeight * 100m : 0m;
            return new AssignmentScoreOutcome(
                true,
                (double)totalScore,
                everyItemPassed);
        }
    }
}
