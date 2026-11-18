namespace System.Domain.Policies.Assignment
{
    public sealed record AssignmentScoreOutcome(
        bool IsComplete,
        double? TotalScore,
        bool? IsPassed);
}
