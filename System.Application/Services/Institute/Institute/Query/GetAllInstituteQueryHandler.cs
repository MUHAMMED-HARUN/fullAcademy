using MediatR;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Services.Institute.DTO;
using System.Application.Services.Institute.Institute.Query;

namespace System.Application.Services.Person.Person.Query
{
    class GetAllInstituteQueryHandler : IRequestHandler<GetAllInstituteQuery, TResult<List<InstituteInfo>>>
    {
        IInstituteRepository _repo;
        public GetAllInstituteQueryHandler( IInstituteRepository repo)
        {
            _repo= repo;
        }
        public async Task<TResult<List<InstituteInfo>>> Handle(GetAllInstituteQuery request, CancellationToken cancellationToken)
        {
            return TResult<List<InstituteInfo>>.OK(await _repo.GetList());
        }
    }
}
