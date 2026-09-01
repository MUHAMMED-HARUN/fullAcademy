using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Domain.Enums.Person;
namespace System.Application.Services.Progress.StageInfo.Command
{
    public class NewStageInfoCommand : ICommand, IRequest<TResult<string>>
    {
        public double From { get; set; }
        public double To { get; set; }
        public string StageID { get; set; }
        public string MatterID { get; set; }
        public enUniteType UniteType { get; set; }
        public enProgressType ProgressType { get; set; }
    }
}
