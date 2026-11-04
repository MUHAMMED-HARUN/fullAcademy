using AutoMapper;
using System.Application.Common;
using System.Application.Services.Finance.Payment.Command;
using System.Domain.Entities.Finance;

namespace System.Application.Services.Finance.Payment.Mapper
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<NewPaymentCommand, clsPayment>()
                .ConstructUsing(src => new clsPayment(
                    clsUtil.ToGuid(src.StudentInvoiceID),
                    src.Amount,
                    src.PaymentDate,
                    src.PaymentMethod,
                    src.ReferenceNumber,
                    src.Notes
                ))
                .ForAllMembers(opt => opt.Ignore());

            var updateMap = CreateMap<UpdatePaymentCommand, clsPayment>();
            updateMap.ForAllMembers(opt => opt.Ignore());
            updateMap.AfterMap((src, dest) =>
            {
                dest.SetAmount(src.Amount);
                dest.SetPaymentDate(src.PaymentDate);
                dest.SetPaymentMethod(src.PaymentMethod);
                dest.SetReferenceNumber(src.ReferenceNumber);
                dest.SetNotes(src.Notes);
            });
        }
    }
}
