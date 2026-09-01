using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Institute.Group.Command
{
    public class UpdateGroupCommand : ICommand, IRequest<TResult<string>>
    {
        public string GroupID { get; set; }
        public string GroupName { get; set; }
        public string Code { get; set; }
        public string InstituteClassID { get; set; }
    }
}
