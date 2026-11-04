import { normalizeGuid } from "@/Generic/FinanceEnums";

export default class FeeStructureMapper {
  static mapUpsertToApi(dataSchema) {
    return {
      FeeStructureID: dataSchema.feeStructureID || undefined,
      Name: dataSchema.name,
      FeeType: Number(dataSchema.feeType),
      Amount: Number(dataSchema.amount),
      ProgramID: dataSchema.programID || null,
      ClassID: dataSchema.classID || null,
      StageID: dataSchema.stageID || null,
      StageInfoID: dataSchema.stageInfoID || null,
      Notes: dataSchema.notes || null,
    };
  }

  static MapFromApiDTO(DataReq) {
    if (!DataReq) return null;
    return {
      feeStructureID: DataReq.FeeStructureID,
      name: DataReq.Name,
      feeType: DataReq.FeeType,
      amount: DataReq.Amount,
      programID: normalizeGuid(DataReq.ProgramID),
      programName: DataReq.ProgramName,
      classID: normalizeGuid(DataReq.ClassID),
      className: DataReq.ClassName,
      stageID: normalizeGuid(DataReq.StageID),
      stageName: DataReq.StageName,
      stageInfoID: normalizeGuid(DataReq.StageInfoID),
      stageInfoName: DataReq.StageInfoName,
      notes: DataReq.Notes,
    };
  }

  static mapListApiToClass(dataList) {
    if (!dataList) return [];
    const items = Array.isArray(dataList) ? dataList : [];
    return items.map((item) => this.MapFromApiDTO(item));
  }
}
