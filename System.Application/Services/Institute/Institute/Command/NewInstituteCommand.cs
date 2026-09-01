using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Address.Address.Command;

namespace System.Application.Services.Institute.Institute.Command
{
    public class NewInstituteCommand : ITransaction, IRequest<TResult<string>>
    {
        public string InstituteName { get; set; }
        public string InstituteNotes { get; set; }
        public NewAddressCommand Address { get; set; }
    }
}
