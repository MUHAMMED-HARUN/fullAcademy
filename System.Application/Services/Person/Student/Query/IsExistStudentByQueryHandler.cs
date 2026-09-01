using MediatR;
using System.Application.Abstractions.Person;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Address.City.Query;

namespace System.Application.Services.Person.Student.Query
{
    class IsExistStudentByQueryHandler : IRequestHandler<IsExistStudentByQuery, TResult<bool>>
    {
        IStudentRepository _studentRepository;
        public IsExistStudentByQueryHandler(IStudentRepository studentRepository)
        {
            _studentRepository= _studentRepository;
        }
        public async Task<TResult<bool>> Handle(IsExistStudentByQuery request, CancellationToken cancellationToken)
        {
            bool isExist = await _studentRepository.IsExistBy(request.Param, request.ParamType);
            if (!isExist) 
            return TResult<bool>.Fail(ApplicationExMsg.FailToFindStudent);

            return TResult<bool>.OK(isExist);

        }
    }
}
