using MediatR;
using System.Application.Abstractions.Person;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Address.City.Query;

namespace System.Application.Services.Person.Teacher.Query
{
    class IsExistTeacherByQueryHandler : IRequestHandler<IsExistTeacherByQuery, TResult<bool>>
    {
        ITeacherRepository _TeacherRepository;
        public IsExistTeacherByQueryHandler(ITeacherRepository TeacherRepository)
        {
            _TeacherRepository= _TeacherRepository;
        }
        public async Task<TResult<bool>> Handle(IsExistTeacherByQuery request, CancellationToken cancellationToken)
        {
            bool isExist = await _TeacherRepository.IsExistBy(request.Param, request.ParamType);
            if (!isExist) 
            return TResult<bool>.Fail(ApplicationExMsg.FailToFindTeacher);

            return TResult<bool>.OK(isExist);

        }
    }
}
