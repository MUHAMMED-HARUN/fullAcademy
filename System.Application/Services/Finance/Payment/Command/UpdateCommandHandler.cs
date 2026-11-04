using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Finance;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Enums.Finance;

namespace System.Application.Services.Finance.Payment.Command
{
    public class UpdatePaymentCommandHandler : IRequestHandler<UpdatePaymentCommand, TResult<string>>
    {
        private readonly IPaymentRepository _repo;
        private readonly IStudentInvoiceRepository _invoiceRepo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public UpdatePaymentCommandHandler(
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

        public async Task<TResult<string>> Handle(UpdatePaymentCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repo.GetEntityBy(request.PaymentID, enPaymentParamType.ID);
            if (entity == null)
                return TResult<string>.Fail(ApplicationExMsg.NotFound);

            var invoice = await _invoiceRepo.GetEntityBy(entity.StudentInvoiceID.ToString(), enStudentInvoiceParamType.ID);
            if (invoice == null)
                return TResult<string>.Fail(ApplicationExMsg.NotFound);

            var oldAmount = entity.Amount;
            if (request.Amount > oldAmount)
            {
                var extra = request.Amount - oldAmount;
                if (extra > invoice.RemainingAmount)
                    return TResult<string>.Fail(ApplicationExMsg.PaymentExceedsRemaining);
                invoice.ApplyPayment(extra);
            }
            else if (request.Amount < oldAmount)
            {
                invoice.ReversePayment(oldAmount - request.Amount);
            }

            _mapper.Map<UpdatePaymentCommand, Domain.Entities.Finance.clsPayment>(request, entity);
            entity.SetModified(_currentUserService.UserId.ToString());
            invoice.SetModified(_currentUserService.UserId.ToString());

            await _repo.Update(entity);
            await _invoiceRepo.Update(invoice);

            return TResult<string>.OK(string.Empty);
        }
    }
}
