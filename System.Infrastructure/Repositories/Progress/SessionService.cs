

using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Services.Institute.Program.DTO;
using System.Application.Services.Person.Person.DTO;
using System.Domain.Entities;
using System.Domain.Enums.Person;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Progress
{
    public class SessionService : ISessionRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<clsSession> _set;
        private  static string getType(enSessionType type)
        {
            switch(type)
            {
                case enSessionType.Subject:
                    return "موضوع";
                case enSessionType.Matter:
                    return "مادة";
            }
            return "موضوع";
        }
        public SessionService(AppDbContext context)
        {
            _context = context;
            _set = _context.Set<clsSession>();
        }
        public async Task<string> Add(clsSession entity)
        {
          await _set.AddAsync(entity);
            return entity.Id.ToString();
        }

        public async Task<SessionInfo> GetBy(string param, enSessionParamType paramType)
        {
            if (paramType == enSessionParamType.ID)

                return await _set.Where(x => x.Id == clsUtil.ToGuid(param)).
                             Select(x => new SessionInfo
                             {
                               SessionID = x.Id.ToString(),
                               SessionName = x.Name,
                               SessionNote= x.Notes,
                               SessionType  = x.Type,
                               SessionTypeName  = getType(x.Type),
                                 ParentSessionID =x.SessionID.ToString(),
                               ParentSessionName=x.Session.Name
                             }
                            ).FirstOrDefaultAsync();

            else
    return await _set.Where(x => x.Name == param).
                             Select(x => new SessionInfo
                             {
                                 SessionID = x.Id.ToString(),
                                 SessionName = x.Name,
                                 SessionNote = x.Notes,
                                 SessionType = x.Type,
                                 SessionTypeName = getType(x.Type),
                                 ParentSessionID = x.SessionID.ToString(),
                                 ParentSessionName = x.Session.Name
                             }
                            ).FirstOrDefaultAsync();
        }

        public async Task<clsSession> GetEntityBy(string param, enSessionParamType paramType)
        {
            if (paramType == enSessionParamType.ID)

                return await _set.Where(x => x.Id == clsUtil.ToGuid(param))
                            .FirstOrDefaultAsync();

            else
                return await _set.Where(x => x.Name == param).
                                         FirstOrDefaultAsync();
        }

        public async Task<List<SessionInfo>> GetList()
        {
            return await _set.Select(x => new SessionInfo
            {
                SessionID = x.Id.ToString(),
                SessionName = x.Name,
                SessionNote = x.Notes,
                SessionTypeName = getType(x.Type),
                SessionType = x.Type,
                ParentSessionID = x.SessionID.ToString(),
                ParentSessionName = x.Session.Name
            }).ToListAsync();
        }

        public async Task<bool> IsExistBy(string param, enSessionParamType paramType)
        {
            if (paramType == enSessionParamType.ID)

                return await _set.AnyAsync(x => x.Id == clsUtil.ToGuid(param))
                             ;

            else
                return await _set.AnyAsync(x => x.Name == param)
                                        ;
        }

        public async Task<bool> Update(clsSession entity)
        {
            _set.Update(entity);
            return true;
        }
    }
}
