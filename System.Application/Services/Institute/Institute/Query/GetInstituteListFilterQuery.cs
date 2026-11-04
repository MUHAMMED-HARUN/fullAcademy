using System.Application.Common.Models;
using System.Application.Services.Institute.DTO;

namespace System.Application.Services.Institute.Institute.Query
{
    public sealed class GetInstituteListFilterQuery : BaseFilterQuery<InstituteLookupInfo>
    {
        public string Search { get; set; } = string.Empty;
    }
}
