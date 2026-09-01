using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Domain.Enums.Person;
namespace System.Application.Services.Progress.StageInfo.Command
{
    public class UpdateStageInfoCommand : ICommand, IRequest<TResult<string>>
    {
        public string StageInfoID { get; set; }
        public double From { get; set; }
        public double To { get; set; }
        public string StageID { get; set; }
        public string MatterID { get; set; }
        public enUniteType UniteType { get; set; }
        public enProgressType ProgressType { get; set; }
    }
}
