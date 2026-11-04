
using System.Domain.Enums.Person;

namespace System.Application.Services.Progress.Session.DTO
{
    public class SessionInfo
    {
        public string SessionID { get; set; } = string.Empty;
        public string SessionName { get; set; } = string.Empty;
        public enSessionType SessionType { get; set; }
        public string SessionTypeName { get; set; } = string.Empty;
        public string? SessionNote { get; set; }
        public string? ParentSessionID { get; set; }
        public string? ParentSessionName { get; set; }

    }
}
