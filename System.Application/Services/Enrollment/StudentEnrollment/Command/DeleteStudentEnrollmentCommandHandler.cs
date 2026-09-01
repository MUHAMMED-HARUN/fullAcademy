using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Enrollment;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;
using System.Domain.Enums.Enrollment;

namespace System.Application.Services.Enrollment.StudentEnrollment.Command
{
    public class DeleteStudentEnrollmentCommandHandler : IRequestHandler<DeleteStudentEnrollmentCommand, TResult<string>>
    {
        private readonly IStudentEnrollmentRepository _repo;
        private readonly ICurrentUserService _currentUserService;

        public DeleteStudentEnrollmentCommandHandler(IStudentEnrollmentRepository repo, ICurrentUserService currentUserService)
        {
            _repo = repo;
            _currentUserService = currentUserService;
        }

        public async Task<TResult<string>> Handle(DeleteStudentEnrollmentCommand request, CancellationToken cancellationToken)
        {
            clsStudentEnrollment entity = await _repo.GetEntityBy(request.EnrollmentID, enStudentEnrollmentType.ID);
            if (entity == null)
                return TResult<string>.Fail(ApplicationExMsg.StudentEnrollmentNotFound);

            entity.SetDeleted(_currentUserService.UserId.ToString());
            await _repo.Update(entity);

            return TResult<string>.OK(string.Empty);
        }
    }
}
