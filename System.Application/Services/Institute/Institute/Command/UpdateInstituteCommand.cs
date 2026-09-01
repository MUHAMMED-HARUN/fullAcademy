

using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Address.Address.Command;

namespace System.Application.Services.Institute.Institute.Command
{
    public class UpdateInstituteCommand : ITransaction, IRequest<TResult<string>>
    {
        public string InstituteID { get; set; }
        public string InstituteName { get; set; }
        public string InstituteNotes { get; set; }
        public UpdateAddressCommand Address { get; set; }
    }
}
