using System.Domain.Entities.BaseClass;
using System.Domain.Enums.Person;
using System.Domain.Exceptions;

namespace System.Domain.Entities
{
    public class clsStageInfo : clsBaseClass
    {
        public double From { get; private set; }
        public double To { get; private set; }
        public Guid StageID { get; private set; }
        public enUniteType UniteType { get; private set; }
        public enProgressType ProgressType { get; private set; }
        public Guid MatterID { get; private set; }

        public clsSession Matter { get; private set; }
        public clsStage Stage { get; private set; }
        protected clsStageInfo() { }

        public clsStageInfo(double from, double to, Guid stageID, enUniteType uniteType, enProgressType progressType, Guid matterID)
        {
            SetFrom(from);
            SetTo(to);
            SetStageID(stageID);
            SetUniteType(uniteType);
            SetProgressType(progressType);
            SetMatterID(matterID);
            BaseSetCreated("System");
        }

        public void SetFrom(double from)
        {
            if (from < 0)
                throw new DomainException(DomainExMsg.StageInfo_FromMustBePositive);
            From = from;
        }

        public void SetTo(double to)
        {
            if (to < 0)
                throw new DomainException(DomainExMsg.StageInfo_ToMustBePositive);
            if (to < From)
                throw new DomainException(DomainExMsg.StageInfo_ToMustBeGreaterThanFrom);
            To = to;
        }

        public void SetStageID(Guid stageID)
        {
            if (stageID == Guid.Empty)
            {
                throw new DomainException(DomainExMsg.StageInfo_StageIDRequired);
            }
            StageID = stageID;
        }

        public void SetUniteType(enUniteType uniteType)
        {
            UniteType = uniteType;
        }

        public void SetProgressType(enProgressType progressType)
        {
            ProgressType = progressType;
        }

        public void SetMatterID(Guid matterID)
        {
            if (matterID==Guid.Empty)
            {
                throw new DomainException(DomainExMsg.StageInfo_MatterIDRequired);
            }
            MatterID = matterID;
        }
    }
}