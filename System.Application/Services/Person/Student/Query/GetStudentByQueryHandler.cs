using MediatR;
using System.Application.Abstractions.Person;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Person.Student.DTO;

namespace System.Application.Services.Person.Student.Query
{
    class GetStudentByQueryHandler : IRequestHandler<GetStudentByQuery, TResult<StudentInfo>>
    {
        IStudentRepository _studentRepository;
        public GetStudentByQueryHandler( IStudentRepository studentRepository)
        {
            _studentRepository= studentRepository;
        }
        public async Task<TResult<StudentInfo>> Handle(GetStudentByQuery request, CancellationToken cancellationToken)
        {
            StudentInfo studentInfo= await _studentRepository.GetBy(request.Param, request.ParamType);
            if (studentInfo == null) 
            return TResult<StudentInfo>.Fail(ApplicationExMsg.FailToFindStudent);

            return TResult<StudentInfo>.OK(studentInfo);

        }
    }
}
