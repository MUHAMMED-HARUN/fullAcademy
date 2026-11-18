export default class FeeStructureMapper {
  static mapUpsertToApi(dataSchema) {
    return {
      FeeStructureID: dataSchema.feeStructureID || undefined,
      Name: dataSchema.name,
      FeeType: Number(dataSchema.feeType),
      Amount: Number(dataSchema.amount),
      AcademicPlanID: dataSchema.academicPlanID,
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
      academicPlanID: DataReq.AcademicPlanID,
      academicPlanName: DataReq.AcademicPlanName,
      academicPlanVersion: DataReq.AcademicPlanVersion,
      providerInstituteName: DataReq.ProviderInstituteName,
      scopeName: DataReq.ScopeName,
      planStatus: DataReq.PlanStatus,
      hasInvoices: DataReq.HasInvoices,
      isUsed: DataReq.IsUsed ?? DataReq.HasInvoices,
      notes: DataReq.Notes,
    };
  }

  static mapListApiToClass(dataList) {
    if (!dataList) return [];
    const items = Array.isArray(dataList) ? dataList : [];
    return items.map((item) => this.MapFromApiDTO(item));
  }

  static mapPagingApiToClass(paging) {
    const source =
      paging?.Data ?? paging?.data ?? paging?.Items ?? paging?.items ?? [];
    return {
      items: this.mapListApiToClass(source),
      count:
        paging?.Count ??
        paging?.count ??
        paging?.TotalCount ??
        paging?.totalCount ??
        0,
    };
  }
}
