using System;
using System.Domain.Entities.BaseClass;
using System.Domain.Enums.Person;
using System.Domain.Exceptions;

namespace System.Domain.Entities
{
    public class clsSession : clsBaseClass
    {
        public string Name { get; private set; }
        public string? Notes { get; private set; }
        public enSessionType Type { get; private set; }
        public Guid? SessionID { get; private set; }

        public clsSession Session { get; private set; }
        public ICollection<clsStageInfo> StageInfoMatters = new HashSet<clsStageInfo>();

        protected clsSession() { }

        public clsSession(string subjectName, string? notes,enSessionType type, Guid subjectID)
        {
            SetName(subjectName);
            SetNotes(notes);
            SetSessionType(type);
            SetSessionID(subjectID);
            BaseSetCreated("System"); // Default, will be updated by Service
        }

        public void SetName(string subjectName)
        {
            if (string.IsNullOrWhiteSpace(subjectName))
            {
                throw new DomainException(DomainExMsg.Subject_NameRequired);
            }
            Name = subjectName;
        }
        public void SetSessionType(enSessionType type)
        {
            Type= type;
        }
        public void SetNotes(string? notes)
        {
            Notes = notes;
        }
        public void SetSessionID(Guid sessionid)
        {
            if (Type == enSessionType.Matter || sessionid == Guid.Empty || sessionid == null)
                SessionID = null;
            else
                SessionID = sessionid;
        }
    }
}
