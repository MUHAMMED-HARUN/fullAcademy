using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.AcademicPlan.AcademicPlan.Command
{
    public class ActivateAcademicPlanCommand : ICommand, IRequest<TResult<string>>
    {
        public string AcademicPlanID { get; set; }
    }
}
