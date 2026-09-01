using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Person;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities.Person;
using System.Domain.Enums.Person;

namespace System.Application.Services.Person.Student.Command
{
    public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand, TResult<string>>
    {
        private readonly IStudentRepository _studentRepo;
        private readonly ICurrentUserService _currentUserService;
        public DeleteStudentCommandHandler(IStudentRepository studentRepository,ICurrentUserService currentUser)
        {
            _studentRepo = studentRepository;
            _currentUserService = currentUser;
        }
        public async Task<TResult<string>> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            clsStudent entity = await _studentRepo.GetEntityBy(request.StudentID, enStudentParamType.ID);
            if (entity == null)
                return TResult<string>.Fail(ApplicationExMsg.FailToFindStudent);

            entity.SetDeleted(_currentUserService.UserId.ToString());
           await _studentRepo.Update(entity);

           

            return TResult<string>.OK(string .Empty);
        }
    }
}
