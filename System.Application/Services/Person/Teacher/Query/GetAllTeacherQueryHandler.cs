using Gridify;
using MediatR;
using System.Application.Abstractions.Person;
using System.Application.Common;
using System.Application.Services.Person.Student.DTO;
using System.Application.Services.Person.Teacher.DTO;

namespace System.Application.Services.Person.Teacher.Query
{
    class GetAllTeacherQueryHandler : IRequestHandler<GetAllTeacherQuery, TResult<Paging<TeacherInfo>>>
    {
        ITeacherRepository _TeacherRepository;
        public GetAllTeacherQueryHandler( ITeacherRepository TeacherRepository)
        {
            _TeacherRepository= TeacherRepository;
        }
        public async Task<TResult<Paging<TeacherInfo>>> Handle(GetAllTeacherQuery request, CancellationToken cancellationToken)
        {
            return  TResult<Paging<TeacherInfo>>.OK(await _TeacherRepository.GetAllTeacherFilter(request));
        }
    }
}
