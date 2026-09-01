using Gridify;
using MediatR;
using System.Application.Abstractions.Person;
using System.Application.Common;
using System.Application.Services.Person.Student.DTO;

namespace System.Application.Services.Person.Student.Query
{
    class GetAllStudentQueryHandler : IRequestHandler<GetAllStudentQuery, TResult<Paging<StudentInfo>>>
    {
        IStudentRepository _studentRepository;
        public GetAllStudentQueryHandler( IStudentRepository studentRepository)
        {
            _studentRepository= studentRepository;
        }
        public async Task<TResult<Paging<StudentInfo>>> Handle(GetAllStudentQuery request, CancellationToken cancellationToken)
        {
            return  TResult < Paging < StudentInfo >>.OK(await _studentRepository.GetAllStudentFilter(request));
        }
    }
}
