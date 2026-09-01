using Gridify;
using MediatR;
using System.Application.Abstractions.Institute;
using System.Application.Abstractions.Person;
using System.Application.Common;
using System.Application.Services.Institute.Group.DTO;
using System.Application.Services.Institute.Group.Query;
using System.Domain.Entities;

namespace System.Application.Services.Institute.Group.Query
{
    class GetGroupsByTeacherIDQueryHandler : IRequestHandler<GetGroupsByTeacherIDQuery, TResult<List<clsGroup>>>
    {
        IGroupRepository _GroupRepository;
        public GetGroupsByTeacherIDQueryHandler(IGroupRepository Repository)
        {
            _GroupRepository = Repository;
        }
        public async Task<TResult<List<clsGroup>>> Handle(GetGroupsByTeacherIDQuery request, CancellationToken cancellationToken)
        {
            return TResult<List<clsGroup>>.OK(await _GroupRepository.GetGroupByTeacherID(request.TeacherID));

        }
    }
}
