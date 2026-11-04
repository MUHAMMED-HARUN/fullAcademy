using System.Domain.Entities.BaseClass;
using System.Domain.Entities.Progress;
using System.Domain.Entities.Progress;
using System.Domain.Enums.Finance;
using System.Domain.Exceptions;

namespace System.Domain.Entities.Finance
{
    public class clsFeeStructure : clsBaseClass
    {
        public string Name { get; private set; }
        public enFeeType FeeType { get; private set; }
        public decimal Amount { get; private set; }
        public Guid? ProgramID { get; private set; }
        public Guid? ClassID { get; private set; }
        public Guid? StageID { get; private set; }
        public Guid? StageInfoID { get; private set; }
        public string? Notes { get; private set; }

        // Navigation properties
        public clsProgram Program { get; private set; }
        public clsClass Class { get; private set; }
        public clsStage Stage { get; private set; }
        public clsStageInfo StageInfo { get; private set; }
        public ICollection<clsStudentInvoice> StudentInvoices { get; private set; } = new HashSet<clsStudentInvoice>();

        protected clsFeeStructure() { }

        public clsFeeStructure(
            string name,
            enFeeType feeType,
            decimal amount,
            Guid? programID,
            Guid? classID,
            Guid? stageID,
            Guid? stageInfoID,
            string? notes)
        {
            SetName(name);
            SetFeeType(feeType);
            SetAmount(amount);
            SetProgramID(programID);
            SetClassID(classID);
            SetStageID(stageID);
            SetStageInfoID(stageInfoID);
            SetNotes(notes);
            BaseSetCreated("System");
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException(DomainExMsg.FeeStructure_NameRequired);
            Name = name;
        }

        public void SetFeeType(enFeeType feeType)
        {
            FeeType = feeType;
        }

        public void SetAmount(decimal amount)
        {
            if (amount < 0)
                throw new DomainException(DomainExMsg.FeeStructure_AmountInvalid);
            Amount = amount;
        }

        public void SetProgramID(Guid? programID)
        {
            ProgramID = programID == Guid.Empty ? null : programID;
        }

        public void SetClassID(Guid? classID)
        {
            ClassID = classID == Guid.Empty ? null : classID;
        }

        public void SetStageID(Guid? stageID)
        {
            StageID = stageID == Guid.Empty ? null : stageID;
        }

        public void SetStageInfoID(Guid? stageInfoID)
        {
            StageInfoID = stageInfoID == Guid.Empty ? null : stageInfoID;
        }

        public void SetNotes(string? notes)
        {
            Notes = notes;
        }
    }
}
