using AutoMapper;
using System.Application.Common;
using System.Application.Services.Progress.Session.Command;
using System.Domain.Entities;
using System.Domain.Entities.Progress;

namespace System.Application.Services.Progress.Session.Mapper
{
    public class SessionProfile : Profile
    {
        public SessionProfile()
        {
            CreateMap<NewSessionCommand, clsSession>()
                  .ConstructUsing(src => new clsSession(src.SessionName, src.SessionNote,src.SessionType,clsUtil.ToGuid( src.ParentSessionID))
                  );

            var UpdateMap = CreateMap<UpdateSessionCommand, clsSession>();
            UpdateMap.ForAllMembers(opt => opt.Ignore());
            UpdateMap
                .AfterMap((src, dest) =>
            {
                dest.SetID(clsUtil.ToGuid(src.SessionID));
                dest.SetName(src.SessionName);
                dest.SetNotes(src.SessionNote);
                dest.SetSessionType(src.SessionType);
                dest.SetSessionID(clsUtil.ToGuid(src.ParentSessionID));
            }
                );
        }
    }
}
