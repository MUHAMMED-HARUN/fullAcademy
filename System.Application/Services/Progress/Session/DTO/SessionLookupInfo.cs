namespace System.Application.Services.Progress.Session.DTO
{
    public sealed class SessionLookupInfo
    {
        public string SessionID { get; set; } = string.Empty;
        public string SessionName { get; set; } = string.Empty;
        public int SessionType { get; set; }
        public string SessionTypeName { get; set; } = string.Empty;
        public string? ParentSessionID { get; set; }
        public string? ParentSessionName { get; set; }
    }
}
