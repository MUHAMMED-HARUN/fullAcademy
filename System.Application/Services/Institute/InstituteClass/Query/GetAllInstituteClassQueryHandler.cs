using MediatR;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Services.Institute.Institute.Query;
using System.Application.Services.Institute.Program.DTO;

namespace System.Application.Services.Institute.InstituteClass.Query
{
    class GetAllInstituteClassQueryHandler : IRequestHandler<GetAllInstituteClassQuery, TResult<List<InstituteClassInfo>>>
    {
        IInstituteClassRepository _repo;
        public GetAllInstituteClassQueryHandler(IInstituteClassRepository repo)
        {
            _repo= repo;
        }
        public async Task<TResult<List<InstituteClassInfo>>> Handle(GetAllInstituteClassQuery request, CancellationToken cancellationToken)
        {
            return TResult<List<InstituteClassInfo>>.OK(await _repo.GetList());
        }
    }
}
