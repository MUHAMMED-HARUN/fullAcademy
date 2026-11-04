export default class DistrictMapper {
  static mapUpsertToApi(dataSchema) {
    const payload = {
      DistrictName: dataSchema.name.trim(),
      CityID: dataSchema.cityID,
    };
    if (dataSchema.id) payload.DistrictID = dataSchema.id;
    return payload;
  }
  static MapFromApiDTO(DataReq) {
    if (!DataReq) return null;
    return {
      id: DataReq.ID ?? DataReq.DistrictID ?? DataReq.id ?? "",
      name: DataReq.DistrictName ?? DataReq.name ?? "",
      cityID: DataReq.CityID ?? DataReq.cityID ?? "",
      cityName: DataReq.CityName ?? DataReq.cityName ?? "",
      countryID: DataReq.CountryID ?? DataReq.countryID ?? "",
      countryName: DataReq.CountryName ?? DataReq.countryName ?? "",
      displayName: [
        DataReq.DistrictName ?? DataReq.name,
        DataReq.CityName ?? DataReq.cityName,
        DataReq.CountryName ?? DataReq.countryName,
      ]
        .filter(Boolean)
        .join(" — "),
    };
  }
  static mapListApiToDistrict(DistrictList) {
    if (!DistrictList) return [];
    return DistrictList.map((district) => this.MapFromApiDTO(district));
  }

  static mapPagingApiToDistrict(paging) {
    const data = paging?.Data ?? paging?.data ?? paging?.items ?? [];
    return {
      items: this.mapListApiToDistrict(data),
      count: paging?.Count ?? paging?.count ?? 0,
    };
  }
}
