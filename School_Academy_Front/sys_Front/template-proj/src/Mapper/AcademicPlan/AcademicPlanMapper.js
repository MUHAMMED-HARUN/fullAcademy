import { normalizeGuid, toDateInput } from "@/Generic/FinanceEnums";
import { AcademicPlanScopeType } from "@/Generic/AcademicPlanEnums";

const scopeIdFields = {
  [AcademicPlanScopeType.Program]: "programID",
  [AcademicPlanScopeType.InstituteClass]: "instituteClassID",
  [AcademicPlanScopeType.Stage]: "stageID",
  [AcademicPlanScopeType.StageInfo]: "stageInfoID",
};

export default class AcademicPlanMapper {
  static mapCreateToApi(dataSchema) {
    const scopeType = Number(dataSchema.scopeType);
    const scopeField = scopeIdFields[scopeType];
    return {
      Name: dataSchema.name.trim(),
      ProviderInstituteID: dataSchema.providerInstituteID,
      ScopeType: scopeType,
      ProgramID: scopeField === "programID" ? dataSchema.programID : null,
      InstituteClassID:
        scopeField === "instituteClassID" ? dataSchema.instituteClassID : null,
      StageID: scopeField === "stageID" ? dataSchema.stageID : null,
      StageInfoID: scopeField === "stageInfoID" ? dataSchema.stageInfoID : null,
      EffectiveFrom: dataSchema.effectiveFrom,
      EffectiveTo: dataSchema.effectiveTo || null,
      Notes: dataSchema.notes || null,
    };
  }

  static mapUpdateToApi(dataSchema) {
    return {
      AcademicPlanID: dataSchema.academicPlanID,
      Name: dataSchema.name.trim(),
      EffectiveFrom: dataSchema.effectiveFrom,
      EffectiveTo: dataSchema.effectiveTo || null,
      Notes: dataSchema.notes || null,
    };
  }

  static MapFromApiDTO(DataReq) {
    if (!DataReq) return null;
    return {
      academicPlanID: DataReq.AcademicPlanID,
      name: DataReq.Name,
      providerInstituteID: normalizeGuid(DataReq.ProviderInstituteID),
      providerInstituteName: DataReq.ProviderInstituteName,
      scopeType: DataReq.ScopeType,
      scopeName: this.scopeNameFromDTO(DataReq),
      programID: normalizeGuid(DataReq.ProgramID),
      instituteClassID: normalizeGuid(DataReq.InstituteClassID),
      stageID: normalizeGuid(DataReq.StageID),
      stageInfoID: normalizeGuid(DataReq.StageInfoID),
      version: DataReq.Version,
      status: DataReq.Status,
      effectiveFrom: toDateInput(DataReq.EffectiveFrom),
      effectiveTo: toDateInput(DataReq.EffectiveTo),
      notes: DataReq.Notes,
      items: (DataReq.Items || []).map((item) => ({
        stageInfoID: item.StageInfoID,
        stageInfoName: item.StageInfoName,
        stageID: item.StageID,
        stageName: item.StageName,
      })),
    };
  }

  static mapLookupListToClass(dataList) {
    if (!dataList) return [];
    return dataList.map((item) => ({
      academicPlanID: item.AcademicPlanID,
      name: item.Name,
      displayName: `${item.Name} — ${item.ScopeName} (v${item.Version})`,
      providerInstituteName: item.ProviderInstituteName,
      scopeType: item.ScopeType,
      scopeName: item.ScopeName,
      version: item.Version,
      status: item.Status,
      effectiveFrom: toDateInput(item.EffectiveFrom),
      effectiveTo: toDateInput(item.EffectiveTo),
    }));
  }

  static scopeNameFromDTO(DataReq) {
    switch (Number(DataReq.ScopeType)) {
      case AcademicPlanScopeType.Program:
        return DataReq.ProgramName;
      case AcademicPlanScopeType.InstituteClass:
        return DataReq.InstituteClassName;
      case AcademicPlanScopeType.Stage:
        return DataReq.StageName;
      case AcademicPlanScopeType.StageInfo:
        return DataReq.StageInfoName;
      default:
        return "";
    }
  }
}
