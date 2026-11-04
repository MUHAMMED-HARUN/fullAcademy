using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Finance;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Enums.Finance;

namespace System.Application.Services.Finance.Payment.Command
{
    public class NewPaymentCommandHandler : IRequestHandler<NewPaymentCommand, TResult<string>>
    {
        private readonly IPaymentRepository _repo;
        private readonly IStudentInvoiceRepository _invoiceRepo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public NewPaymentCommandHandler(
            IPaymentRepository repo,
            IStudentInvoiceRepository invoiceRepo,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            _repo = repo;
            _invoiceRepo = invoiceRepo;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<TResult<string>> Handle(NewPaymentCommand request, CancellationToken cancellationToken)
        {
            var invoice = await _invoiceRepo.GetEntityBy(request.StudentInvoiceID, enStudentInvoiceParamType.ID);
            if (invoice == null)
                return TResult<string>.Fail(ApplicationExMsg.NotFound);
            if (invoice.Status == enInvoiceStatus.Cancelled)
                return TResult<string>.Fail(ApplicationExMsg.InvoiceCancelled);
            if (request.Amount > invoice.RemainingAmount)
                return TResult<string>.Fail(ApplicationExMsg.PaymentExceedsRemaining);

            Domain.Entities.Finance.clsPayment entity = _mapper.Map<NewPaymentCommand, Domain.Entities.Finance.clsPayment>(request);
            entity.SetCreated(_currentUserService.UserId.ToString());

            string entityID = await _repo.Add(entity);

            if (string.IsNullOrEmpty(entityID))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            // Update invoice remaining amount
            invoice.ApplyPayment(request.Amount);
            invoice.SetModified(_currentUserService.UserId.ToString());
            await _invoiceRepo.Update(invoice);

            return TResult<string>.OK(entityID);
        }
    }
}
