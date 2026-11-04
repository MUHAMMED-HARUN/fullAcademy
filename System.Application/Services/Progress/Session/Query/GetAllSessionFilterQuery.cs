using System.Application.Common.Models;
using System.Application.Services.Progress.Session.DTO;

namespace System.Application.Services.Progress.Session.Query
{
    public sealed class GetAllSessionFilterQuery : BaseFilterQuery<SessionLookupInfo>
    {
        public bool ParentOnly { get; set; }

        public GetAllSessionFilterQuery()
        {
            Page = 1;
            PageSize = 10;
        }
    }
}
