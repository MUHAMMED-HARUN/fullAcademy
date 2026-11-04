using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Finance;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Enums.Finance;

namespace System.Application.Services.Finance.StudentInvoice.Command
{
    public class UpdateStudentInvoiceCommandHandler : IRequestHandler<UpdateStudentInvoiceCommand, TResult<string>>
    {
        private readonly IStudentInvoiceRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public UpdateStudentInvoiceCommandHandler(
            IStudentInvoiceRepository repo,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            _repo = repo;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<TResult<string>> Handle(UpdateStudentInvoiceCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repo.GetEntityBy(request.StudentInvoiceID, enStudentInvoiceParamType.ID);
            if (entity == null)
                return TResult<string>.Fail(ApplicationExMsg.NotFound);

            _mapper.Map<UpdateStudentInvoiceCommand, Domain.Entities.Finance.clsStudentInvoice>(request, entity);
            entity.SetModified(_currentUserService.UserId.ToString());

            await _repo.Update(entity);

            return TResult<string>.OK(string.Empty);
        }
    }
}
