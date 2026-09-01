using System.Domain.Entities.BaseClass;
using System.Domain.Exceptions;

namespace System.Domain.Entities
{
    public class clsAssignmentScopeinfo : clsBaseClass
    {
        public short MaxScore { get; private set; }
        public short MinPassScore { get; private set; }
        public float Weight { get; private set; }
        public Guid StageInfoID { get; private set; }
        public Guid AssignmentScopeID { get; private set; }
  

        public clsStageInfo StageInfo { get; private set; }
        public clsAssignmentScope AssignmentScope { get; private set; }
        protected clsAssignmentScopeinfo() { }  

        public clsAssignmentScopeinfo(short maxScore, short minPassScore, float weight, Guid stageInfoID, Guid assignmentScopeID)
        {
            SetMaxScore(maxScore);
            SetMinPassScore(minPassScore);
            SetWeight(weight);
            SetStageInfoID(stageInfoID);
            SetAssignmentScopeID(assignmentScopeID);
            BaseSetCreated("System");
        }

        public void SetMaxScore(short maxScore)
        {
            if (maxScore < 0)
                throw new DomainException(DomainExMsg.AssignmentScopeInfo_MaxScoreInvalid);
            MaxScore = maxScore;
        }

        public void SetMinPassScore(short minPassScore)
        {
            if (minPassScore < 0)
                throw new DomainException(DomainExMsg.AssignmentScopeInfo_MinPassScoreInvalid);
            if (minPassScore > MaxScore)
                throw new DomainException(DomainExMsg.AssignmentScopeInfo_MinPassScoreInvalid);
            MinPassScore = minPassScore;
        }

        public void SetWeight(float weight)
        {
            if (weight < 0)
                throw new DomainException(DomainExMsg.AssignmentScopeInfo_WeightInvalid);
            Weight = weight;
        }

        public void SetStageInfoID(Guid stageInfoID)
        {
            if (stageInfoID == Guid.Empty)
            {
                throw new DomainException(DomainExMsg.AssignmentScopeInfo_StageInfoIDRequired);
            }
            StageInfoID = stageInfoID;
        }

        public void SetAssignmentScopeID(Guid assignmentScopeID)
        {
            if (assignmentScopeID == Guid.Empty)
            {
                throw new DomainException(DomainExMsg.AssignmentScopeInfo_AssignmentScopeIDRequired);
            }
            AssignmentScopeID = assignmentScopeID;
        }
    }
}