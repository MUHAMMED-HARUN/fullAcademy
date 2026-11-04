namespace System.Application.Services.Institute.InstituteClass.DTO
{
    public sealed class InstituteClassBulkResult
    {
        public int AddedCount { get; init; }
        public List<string> SkippedAlreadyLinkedClassIDs { get; init; } = new();
    }
}
