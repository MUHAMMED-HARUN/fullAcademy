using MediatR;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Services.Institute.Class.DTO;

namespace System.Application.Services.Institute.Class.Query
{
    class GetAllClassQueryHandler : IRequestHandler<GetAllClassQuery, TResult<List<ClassInfo>>>
    {
        IClassRepository _repo;
        public GetAllClassQueryHandler( IClassRepository repo)
        {
            _repo= repo;
        }
        public async Task<TResult<List<ClassInfo>>> Handle(GetAllClassQuery request, CancellationToken cancellationToken)
        {
            return TResult<List<ClassInfo>>.OK(await _repo.GetList());
        }
    }
}
