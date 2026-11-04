using Gridify;
using Gridify.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Progress;
using System.Application.Common;
using System.Application.Services.Progress.Session.DTO;
using System.Application.Services.Progress.Session.Query;
using System.Domain.Entities.Progress;
using System.Domain.Enums.Person;
using System.Infrastructure.Extensions.Security;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Progress
{
    public class SessionService : ISessionRepository
    {
        private static readonly IGridifyMapper<SessionLookupInfo> SessionFilterMapper =
            new GridifyMapper<SessionLookupInfo>()
                .AddMap(nameof(SessionLookupInfo.SessionName), session => session.SessionName)
                .AddMap(nameof(SessionLookupInfo.SessionType), session => session.SessionType)
                .AddMap(nameof(SessionLookupInfo.SessionTypeName), session => session.SessionTypeName)
                .AddMap(nameof(SessionLookupInfo.ParentSessionName), session => session.ParentSessionName);

        private readonly AppDbContext _context;
        private readonly DbSet<clsSession> _set;
        private readonly ICurrentUserService _currentUser;

        private static string GetTypeName(enSessionType type)
        {
            switch (type)
            {
                case enSessionType.Subject:
                    return "موضوع";
                case enSessionType.Matter:
                    return "مادة";
            }
            return "موضوع";
        }
        public SessionService(AppDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _set = _context.Set<clsSession>();
            _currentUser = currentUser;
        }
        public async Task<string> Add(clsSession entity)
        {
          await _set.AddAsync(entity);
            return entity.Id.ToString();
        }

        public async Task<SessionInfo?> GetBy(string param, enSessionParamType paramType)
        {
            var query = _set.AsNoTracking().ApplySessionReadScope(_currentUser, _context);
            query = paramType == enSessionParamType.ID
                ? query.Where(session => session.Id == clsUtil.ToGuid(param))
                : query.Where(session => session.Name == param);

            return await ProjectSessionInfo(query).FirstOrDefaultAsync();
        }

        public async Task<clsSession?> GetEntityBy(string param, enSessionParamType paramType)
        {
            if (paramType == enSessionParamType.ID)
                return await _set.Where(x => x.Id == clsUtil.ToGuid(param))
                            .FirstOrDefaultAsync();

            return await _set.Where(x => x.Name == param).FirstOrDefaultAsync();
        }

        public async Task<List<SessionInfo>> GetList()
        {
            var query = _set.AsNoTracking().ApplySessionReadScope(_currentUser, _context);
            return await ProjectSessionInfo(query).ToListAsync();
        }

        public async Task<Paging<SessionLookupInfo>> GetListFilter(GetAllSessionFilterQuery request)
        {
            request.Page = Math.Max(1, request.Page);
            request.PageSize = Math.Clamp(request.PageSize, 1, 100);

            var query = _set.AsNoTracking().ApplySessionReadScope(_currentUser, _context);
            if (request.ParentOnly)
                query = query.Where(session => session.Type == enSessionType.Subject);

            var projected = query.Select(session => new SessionLookupInfo
            {
                SessionID = session.Id.ToString(),
                SessionName = session.Name,
                SessionType = (int)session.Type,
                SessionTypeName = session.Type == enSessionType.Subject ? "موضوع" : "مادة",
                ParentSessionID = session.SessionID == null ? null : session.SessionID.ToString(),
                ParentSessionName = session.Session == null ? null : session.Session.Name,
            });

            return await projected.GridifyAsync(request, SessionFilterMapper);
        }

        public async Task<bool> IsExistBy(string param, enSessionParamType paramType)
        {
            var query = _set.AsNoTracking().ApplySessionReadScope(_currentUser, _context);
            return paramType == enSessionParamType.ID
                ? await query.AnyAsync(session => session.Id == clsUtil.ToGuid(param))
                : await query.AnyAsync(session => session.Name == param);
        }

        public async Task<bool> IsExistNameByType(
            string name,
            enSessionType sessionType,
            Guid? excludedSessionID = null)
        {
            return await _set.AnyAsync(session =>
                session.Name == name &&
                session.Type == sessionType &&
                (excludedSessionID == null || session.Id != excludedSessionID.Value));
        }

        public async Task<bool> HasChildren(Guid sessionID)
        {
            return await _set.AnyAsync(session => session.SessionID == sessionID);
        }

        public Task<bool> Update(clsSession entity)
        {
            _set.Update(entity);
            return Task.FromResult(true);
        }

        private static IQueryable<SessionInfo> ProjectSessionInfo(IQueryable<clsSession> query)
        {
            return query.Select(session => new SessionInfo
            {
                SessionID = session.Id.ToString(),
                SessionName = session.Name,
                SessionNote = session.Notes,
                SessionTypeName = GetTypeName(session.Type),
                SessionType = session.Type,
                ParentSessionID = session.SessionID == null ? null : session.SessionID.ToString(),
                ParentSessionName = session.Session == null ? null : session.Session.Name,
            });
        }
    }
}
