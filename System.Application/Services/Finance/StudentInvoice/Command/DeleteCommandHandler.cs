using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Finance;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Enums.Finance;

namespace System.Application.Services.Finance.StudentInvoice.Command
{
    public class DeleteStudentInvoiceCommandHandler : IRequestHandler<DeleteStudentInvoiceCommand, TResult<string>>
    {
        private readonly IStudentInvoiceRepository _repo;
        private readonly ICurrentUserService _currentUserService;

        public DeleteStudentInvoiceCommandHandler(
            IStudentInvoiceRepository repo,
            ICurrentUserService currentUserService)
        {
            _repo = repo;
            _currentUserService = currentUserService;
        }

        public async Task<TResult<string>> Handle(DeleteStudentInvoiceCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repo.GetEntityBy(request.StudentInvoiceID, enStudentInvoiceParamType.ID);
            if (entity == null)
                return TResult<string>.Fail(ApplicationExMsg.NotFound);

            entity.SetDeleted(_currentUserService.UserId.ToString());
            await _repo.Update(entity);

            return TResult<string>.OK(string.Empty);
        }
    }
}
