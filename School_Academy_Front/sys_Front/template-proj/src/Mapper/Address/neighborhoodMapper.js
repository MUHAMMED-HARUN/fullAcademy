export default class NeighborhoodMapper {
  static mapUpsertToApi(dataSchema) {
    const payload = {
      NeighborhoodName: dataSchema.name.trim(),
      DistrictID: dataSchema.districtID,
    };
    if (dataSchema.id) payload.NeighborhoodID = dataSchema.id;
    return payload;
  }
  static MapFromApiDTO(DataReq) {
    if (!DataReq) return null;
    return {
      id: DataReq.NeighborhoodID ?? DataReq.ID ?? DataReq.id ?? "",
      name: DataReq.NeighborhoodName ?? DataReq.name ?? "",
      districtID: DataReq.DistrictID ?? DataReq.districtID ?? "",
      districtName: DataReq.DistrictName ?? DataReq.districtName ?? "",
      cityID: DataReq.CityID ?? DataReq.cityID ?? "",
      cityName: DataReq.CityName ?? DataReq.cityName ?? "",
      countryID: DataReq.CountryID ?? DataReq.countryID ?? "",
      countryName: DataReq.CountryName ?? DataReq.countryName ?? "",
      displayName: [
        DataReq.NeighborhoodName ?? DataReq.name,
        DataReq.DistrictName ?? DataReq.districtName,
        DataReq.CityName ?? DataReq.cityName,
        DataReq.CountryName ?? DataReq.countryName,
      ]
        .filter(Boolean)
        .join(" — "),
    };
  }
  static mapListApiToNeighborhood(NeighborhoodList) {
    if (!NeighborhoodList) return [];
    return NeighborhoodList.map((neighborhood) =>
      this.MapFromApiDTO(neighborhood)
    );
  }

  static mapPagingApiToNeighborhood(paging) {
    const data = paging?.Data ?? paging?.data ?? paging?.items ?? [];
    return {
      items: this.mapListApiToNeighborhood(data),
      count: paging?.Count ?? paging?.count ?? 0,
    };
  }
}
