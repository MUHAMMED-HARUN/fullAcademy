using MediatR;
using System.Application.Abstractions.Person;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Person.Teacher.DTO;

namespace System.Application.Services.Person.Teacher.Query
{
    public sealed class GetTeacherByQueryHandler : IRequestHandler<GetTeacherByQuery, TResult<TeacherInfo>>
    {
        ITeacherRepository _TeacherRepository;
        public GetTeacherByQueryHandler( ITeacherRepository TeacherRepository)
        {
            _TeacherRepository= TeacherRepository;
        }
        public async Task<TResult<TeacherInfo>> Handle(GetTeacherByQuery request, CancellationToken cancellationToken)
        {
            var teacherInfo = await _TeacherRepository.GetBy(request.Param, request.ParamType);
            if (teacherInfo == null)
            return TResult<TeacherInfo>.Fail(ApplicationExMsg.FailToFindTeacher);

            return TResult<TeacherInfo>.OK(teacherInfo);

        }
    }
}
