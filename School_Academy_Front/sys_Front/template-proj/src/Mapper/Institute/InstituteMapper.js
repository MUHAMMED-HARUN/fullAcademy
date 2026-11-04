import AddressMapper from "../Address/addressMapper";
export default class InstituteMapper {
  static mapUpsertToApi(dataSchema) {
    return {
      InstituteID: dataSchema.instituteID,
      InstituteName: dataSchema.name,
      InstituteNotes: dataSchema.note,
      Address: AddressMapper.mapUpsertToApi(dataSchema.address),
    };
  }

  static MapFromApiDTO(DataReq) {
    return {
      instituteID: DataReq.InstituteID,
      name: DataReq.InstituteName,
      note: DataReq.InstituteNote,
      address: DataReq.InstituteAddress,
    };
  }
  static mapListApiToInstitute(dataList) {
    if (!dataList) return [];
    return dataList.map((list) => {
      return {
        instituteID: list.InstituteID,
        name: list.InstituteName,
        note: list.InstituteNote,
        address: list.InstituteAddress,
      };
    });
  }

  static mapLookupPagingApiToInstitute(paging) {
    const data = paging?.Data ?? paging?.data ?? paging?.items ?? [];
    return {
      items: data.map((item) => ({
        instituteID: item.InstituteID ?? item.instituteID ?? "",
        name: item.Name ?? item.name ?? "",
      })),
      count: paging?.Count ?? paging?.count ?? 0,
    };
  }
}
