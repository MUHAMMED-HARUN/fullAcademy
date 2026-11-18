using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.AcademicPlan.AcademicPlan.Command
{
    public class DeleteAcademicPlanCommand : ICommand, IRequest<TResult<string>>
    {
        public string AcademicPlanID { get; set; }
    }
}
