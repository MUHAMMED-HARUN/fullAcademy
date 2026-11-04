using MediatR;
using System.Application.Abstractions.Person;
using System.Application.Common;

namespace System.Application.Services.Person.Teacher.Query
{
    public sealed class IsExistTeacherByQueryHandler : IRequestHandler<IsExistTeacherByQuery, TResult<bool>>
    {
        ITeacherRepository _TeacherRepository;
        public IsExistTeacherByQueryHandler(ITeacherRepository TeacherRepository)
        {
            _TeacherRepository = TeacherRepository;
        }
        public async Task<TResult<bool>> Handle(IsExistTeacherByQuery request, CancellationToken cancellationToken)
        {
            bool isExist = await _TeacherRepository.IsExistBy(request.Param, request.ParamType);
            return TResult<bool>.OK(isExist);

        }
    }
}
