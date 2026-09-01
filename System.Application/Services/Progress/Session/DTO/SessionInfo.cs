
using System.Domain.Enums.Person;

namespace System.Application.Services.Institute.Program.DTO
{
    public class SessionInfo
    {
        public string SessionID { get; set; }
        public string SessionName { get; set; }
        public enSessionType SessionType { get; set; }
        public string SessionTypeName { get; set; }
        public string SessionNote { get; set; }
        public string ParentSessionID { get; set; }
        public string ParentSessionName { get; set; }

    }
}
