export default class CityMapper {
  static mapUpsertCityToApi(dataSchema) {
    const payload = {
      CityName: dataSchema.name.trim(),
      CityCode: dataSchema.code?.trim() || null,
      CountryID: dataSchema.countryID,
    };
    if (dataSchema.id) payload.CityID = dataSchema.id;
    return payload;
  }
  static MapFromApiDTO(DataReq) {
    if (!DataReq) return null;
    return {
      id: DataReq.ID ?? DataReq.CityID ?? DataReq.id ?? "",
      name: DataReq.Name ?? DataReq.CityName ?? DataReq.name ?? "",
      code: DataReq.Code ?? DataReq.CityCode ?? DataReq.code ?? "",
      countryID: DataReq.CountryID,
      countryName: DataReq.CountryName ?? DataReq.countryName ?? "",
      displayName: [
        DataReq.Name ?? DataReq.CityName ?? DataReq.name,
        DataReq.CountryName ?? DataReq.countryName,
      ]
        .filter(Boolean)
        .join(" — "),
    };
  }
  static mapListApiToCity(cityList) {
    if (!cityList) return [];
    return cityList.map((city) => this.MapFromApiDTO(city));
  }

  static mapPagingApiToCity(paging) {
    const data = paging?.Data ?? paging?.data ?? paging?.items ?? [];
    return {
      items: this.mapListApiToCity(data),
      count: paging?.Count ?? paging?.count ?? 0,
    };
  }
}
