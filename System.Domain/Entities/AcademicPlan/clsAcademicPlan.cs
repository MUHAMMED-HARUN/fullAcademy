using System.Domain.Entities.BaseClass;
using System.Domain.Entities.Institue;
using System.Domain.Entities.Progress;
using System.Domain.Enums.AcademicPlan;
using System.Domain.Exceptions;

namespace System.Domain.Entities.AcademicPlan
{
    public class clsAcademicPlan : clsBaseClass
    {
        public string Name { get; private set; }
        public Guid ProviderInstituteID { get; private set; }
        public enAcademicPlanScopeType ScopeType { get; private set; }
        public Guid? ProgramID { get; private set; }
        public Guid? InstituteClassID { get; private set; }
        public Guid? StageID { get; private set; }
        public Guid? StageInfoID { get; private set; }
        public int Version { get; private set; }
        public enAcademicPlanStatus Status { get; private set; }
        public DateOnly EffectiveFrom { get; private set; }
        public DateOnly? EffectiveTo { get; private set; }
        public string? Notes { get; private set; }

        public clsInstitute ProviderInstitute { get; private set; }
        public clsProgram Program { get; private set; }
        public clsInstituteClass InstituteClass { get; private set; }
        public clsStage Stage { get; private set; }
        public clsStageInfo StageInfo { get; private set; }
        public ICollection<clsAcademicPlanItem> Items { get; private set; } = new HashSet<clsAcademicPlanItem>();

        public Guid ScopeID => ScopeType switch
        {
            enAcademicPlanScopeType.Program => ProgramID ?? Guid.Empty,
            enAcademicPlanScopeType.InstituteClass => InstituteClassID ?? Guid.Empty,
            enAcademicPlanScopeType.Stage => StageID ?? Guid.Empty,
            enAcademicPlanScopeType.StageInfo => StageInfoID ?? Guid.Empty,
            _ => Guid.Empty
        };

        protected clsAcademicPlan() { }

        public clsAcademicPlan(
            string name,
            Guid providerInstituteID,
            enAcademicPlanScopeType scopeType,
            Guid? programID,
            Guid? instituteClassID,
            Guid? stageID,
            Guid? stageInfoID,
            DateOnly effectiveFrom,
            DateOnly? effectiveTo,
            string? notes,
            int version = 1)
        {
            SetName(name);
            SetProviderInstituteID(providerInstituteID);
            SetScope(scopeType, programID, instituteClassID, stageID, stageInfoID);
            SetEffectivePeriod(effectiveFrom, effectiveTo);
            SetNotes(notes);
            SetVersion(version);
            Status = enAcademicPlanStatus.Draft;
            BaseSetCreated("System");
        }

        public void SetDetails(string name, string? notes, DateOnly effectiveFrom, DateOnly? effectiveTo)
        {
            EnsureDraft();
            SetName(name);
            SetNotes(notes);
            SetEffectivePeriod(effectiveFrom, effectiveTo);
        }

        public void SetItems(IEnumerable<Guid> stageInfoIDs)
        {
            EnsureDraft();

            var ids = stageInfoIDs?.Distinct().ToList() ?? new List<Guid>();
            if (ids.Count == 0 || ids.Any(id => id == Guid.Empty))
                throw new DomainException(DomainExMsg.AcademicPlan_ItemsRequired);

            if (ScopeType == enAcademicPlanScopeType.StageInfo && (ids.Count != 1 || ids[0] != StageInfoID))
                throw new DomainException(DomainExMsg.AcademicPlan_StageInfoScopeSingleItem);

            foreach (var item in Items.Where(item => !ids.Contains(item.StageInfoID)).ToList())
                Items.Remove(item);

            var existingStageInfoIDs = Items.Select(item => item.StageInfoID).ToHashSet();
            foreach (var id in ids.Where(id => !existingStageInfoIDs.Contains(id)))
                Items.Add(new clsAcademicPlanItem(id));
        }

        public void Activate(string userId)
        {
            if (Status != enAcademicPlanStatus.Draft)
                throw new DomainException(DomainExMsg.AcademicPlan_ActivateOnlyDraft);

            if (Items.Count == 0)
                throw new DomainException(DomainExMsg.AcademicPlan_ItemsRequired);

            Status = enAcademicPlanStatus.Active;
            SetModified(userId);
        }

        public void Archive(string userId)
        {
            if (Status != enAcademicPlanStatus.Active)
                throw new DomainException(DomainExMsg.AcademicPlan_ArchiveOnlyActive);

            Status = enAcademicPlanStatus.Archived;
            SetModified(userId);
        }

        public clsAcademicPlan CreateNextVersion(string userId)
        {
            if (Status != enAcademicPlanStatus.Active)
                throw new DomainException(DomainExMsg.AcademicPlan_VersionOnlyFromActive);

            var next = new clsAcademicPlan(
                Name,
                ProviderInstituteID,
                ScopeType,
                ProgramID,
                InstituteClassID,
                StageID,
                StageInfoID,
                EffectiveFrom,
                EffectiveTo,
                Notes,
                Version + 1);
            next.SetCreated(userId);
            return next;
        }

        public override void SetDeleted(string userId)
        {
            if (Status != enAcademicPlanStatus.Draft)
                throw new DomainException(DomainExMsg.AcademicPlan_DeleteOnlyDraft);

            base.SetDeleted(userId);
        }

        private void EnsureDraft()
        {
            if (Status != enAcademicPlanStatus.Draft)
                throw new DomainException(DomainExMsg.AcademicPlan_NotDraft);
        }

        private void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException(DomainExMsg.AcademicPlan_NameRequired);
            Name = name;
        }

        private void SetProviderInstituteID(Guid providerInstituteID)
        {
            if (providerInstituteID == Guid.Empty)
                throw new DomainException(DomainExMsg.AcademicPlan_ProviderInstituteIDRequired);
            ProviderInstituteID = providerInstituteID;
        }

        private void SetScope(
            enAcademicPlanScopeType scopeType,
            Guid? programID,
            Guid? instituteClassID,
            Guid? stageID,
            Guid? stageInfoID)
        {
            var scopeID = scopeType switch
            {
                enAcademicPlanScopeType.Program => programID,
                enAcademicPlanScopeType.InstituteClass => instituteClassID,
                enAcademicPlanScopeType.Stage => stageID,
                enAcademicPlanScopeType.StageInfo => stageInfoID,
                _ => null
            };

            if (scopeID is not Guid id || id == Guid.Empty)
                throw new DomainException(DomainExMsg.AcademicPlan_ScopeMismatch);

            ScopeType = scopeType;
            ProgramID = scopeType == enAcademicPlanScopeType.Program ? programID : null;
            InstituteClassID = scopeType == enAcademicPlanScopeType.InstituteClass ? instituteClassID : null;
            StageID = scopeType == enAcademicPlanScopeType.Stage ? stageID : null;
            StageInfoID = scopeType == enAcademicPlanScopeType.StageInfo ? stageInfoID : null;
        }

        private void SetEffectivePeriod(DateOnly effectiveFrom, DateOnly? effectiveTo)
        {
            if (effectiveTo is DateOnly to && to < effectiveFrom)
                throw new DomainException(DomainExMsg.AcademicPlan_EffectiveToBeforeFrom);

            EffectiveFrom = effectiveFrom;
            EffectiveTo = effectiveTo;
        }

        private void SetVersion(int version)
        {
            if (version <= 0)
                throw new DomainException(DomainExMsg.AcademicPlan_VersionInvalid);
            Version = version;
        }

        private void SetNotes(string? notes)
        {
            Notes = notes;
        }
    }
}
