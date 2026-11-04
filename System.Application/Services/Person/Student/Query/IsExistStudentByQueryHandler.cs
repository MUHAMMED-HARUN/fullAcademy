using MediatR;
using System.Application.Abstractions.Person;
using System.Application.Common;

namespace System.Application.Services.Person.Student.Query
{
    public sealed class IsExistStudentByQueryHandler : IRequestHandler<IsExistStudentByQuery, TResult<bool>>
    {
        IStudentRepository _studentRepository;
        public IsExistStudentByQueryHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }
        public async Task<TResult<bool>> Handle(IsExistStudentByQuery request, CancellationToken cancellationToken)
        {
            bool isExist = await _studentRepository.IsExistBy(request.Param, request.ParamType);
            return TResult<bool>.OK(isExist);

        }
    }
}
