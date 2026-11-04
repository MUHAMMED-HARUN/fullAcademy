using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Finance;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Enums.Finance;

namespace System.Application.Services.Finance.StudentInvoice.Command
{
    public class NewStudentInvoiceCommandHandler : IRequestHandler<NewStudentInvoiceCommand, TResult<string>>
    {
        private readonly IStudentInvoiceRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public NewStudentInvoiceCommandHandler(
            IStudentInvoiceRepository repo,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            _repo = repo;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<TResult<string>> Handle(NewStudentInvoiceCommand request, CancellationToken cancellationToken)
        {
            Domain.Entities.Finance.clsStudentInvoice entity = _mapper.Map<NewStudentInvoiceCommand, Domain.Entities.Finance.clsStudentInvoice>(request);
            entity.SetCreated(_currentUserService.UserId.ToString());

            string entityID = await _repo.Add(entity);

            if (string.IsNullOrEmpty(entityID))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            return TResult<string>.OK(entityID);
        }
    }
}
