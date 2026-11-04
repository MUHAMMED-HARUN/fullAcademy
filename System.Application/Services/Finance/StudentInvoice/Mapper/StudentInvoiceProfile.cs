using AutoMapper;
using System.Application.Common;
using System.Application.Services.Finance.StudentInvoice.Command;
using System.Domain.Entities.Finance;

namespace System.Application.Services.Finance.StudentInvoice.Mapper
{
    public class StudentInvoiceProfile : Profile
    {
        public StudentInvoiceProfile()
        {
            CreateMap<NewStudentInvoiceCommand, clsStudentInvoice>()
                .ConstructUsing(src => new clsStudentInvoice(
                    clsUtil.ToGuid(src.StudentID),
                    clsUtil.ToGuid(src.FeeStructureID),
                    src.IssueDate,
                    src.DueDate,
                    src.OriginalAmount,
                    src.DiscountAmount,
                    src.Notes
                ))
                .ForAllMembers(opt => opt.Ignore());

            var updateMap = CreateMap<UpdateStudentInvoiceCommand, clsStudentInvoice>();
            updateMap.ForAllMembers(opt => opt.Ignore());
            updateMap.AfterMap((src, dest) =>
            {
                dest.SetDueDate(src.DueDate);
                dest.SetDiscountAmount(src.DiscountAmount);
                dest.SetStatus(src.Status);
                dest.SetNotes(src.Notes);
            });
        }
    }
}
