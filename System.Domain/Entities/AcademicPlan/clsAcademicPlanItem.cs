using System.Domain.Entities.BaseClass;
using System.Domain.Entities.Progress;
using System.Domain.Exceptions;

namespace System.Domain.Entities.AcademicPlan
{
    public class clsAcademicPlanItem : clsBaseClass
    {
        public Guid AcademicPlanID { get; private set; }
        public Guid StageInfoID { get; private set; }

        public clsAcademicPlan AcademicPlan { get; private set; }
        public clsStageInfo StageInfo { get; private set; }

        protected clsAcademicPlanItem() { }

        public clsAcademicPlanItem(Guid stageInfoID)
        {
            SetStageInfoID(stageInfoID);
            BaseSetCreated("System");
        }

        private void SetStageInfoID(Guid stageInfoID)
        {
            if (stageInfoID == Guid.Empty)
                throw new DomainException(DomainExMsg.AcademicPlanItem_StageInfoIDRequired);
            StageInfoID = stageInfoID;
        }
    }
}
