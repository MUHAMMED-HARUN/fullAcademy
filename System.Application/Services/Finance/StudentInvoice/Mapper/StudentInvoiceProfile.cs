using AutoMapper;
using System.Application.Services.Finance.StudentInvoice.Command;
using System.Domain.Entities.Finance;

namespace System.Application.Services.Finance.StudentInvoice.Mapper
{
    public class StudentInvoiceProfile : Profile
    {
        public StudentInvoiceProfile()
        {
            var updateMap = CreateMap<UpdateStudentInvoiceCommand, clsStudentInvoice>();
            updateMap.ForAllMembers(opt => opt.Ignore());
            updateMap.AfterMap((src, dest) =>
            {
                dest.SetDiscountAmount(src.DiscountAmount);
                dest.SetNotes(src.Notes);
            });
        }
    }
}
