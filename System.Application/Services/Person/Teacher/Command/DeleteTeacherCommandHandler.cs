using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Person;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;
using System.Domain.Entities.Person;
using System.Domain.Enums.Person;

namespace System.Application.Services.Person.Teacher.Command
{
    public class DeleteTeacherCommandHandler : IRequestHandler<DeleteTeacherCommand, TResult<string>>
    {
        private readonly ITeacherRepository _TeacherRepo;
        private readonly ICurrentUserService _currentUserService;
        public DeleteTeacherCommandHandler(ITeacherRepository TeacherRepository,ICurrentUserService currentUser)
        {
            _TeacherRepo = TeacherRepository;
            _currentUserService = currentUser;
        }
        public async Task<TResult<string>> Handle(DeleteTeacherCommand request, CancellationToken cancellationToken)
        {
            clsTeacher entity = await _TeacherRepo.GetEntityBy(request.TeacherID, enTeacherParamType.ID);
            if (entity == null)
                return TResult<string>.Fail(ApplicationExMsg.FailToFindTeacher);

            entity.SetDeleted(_currentUserService.UserId.ToString());
           await _TeacherRepo.Update(entity);

           

            return TResult<string>.OK(string .Empty);
        }
    }
}
