export default class AddressMapper {
  static mapUpsertToApi(dataSchema) {
    const payload = {
      CountryID: dataSchema.countryID,
      CityID: dataSchema.cityID,
      DistrictID: dataSchema.districtID,
      NeighborhoodID: dataSchema.neighborhoodID,
      Notes: dataSchema.notes ?? null,
    };
    if (dataSchema.id) payload.AddressID = dataSchema.id;
    return payload;
  }
  static MapFromApiDTO(DataReq) {
    if (!DataReq) return null;
    return {
      id: DataReq.AddressID ?? DataReq.id ?? "",
      countryID: DataReq.CountryID ?? DataReq.countryID ?? "",
      countryName: DataReq.CountryName ?? DataReq.countryName ?? "",
      cityID: DataReq.CityID ?? DataReq.cityID ?? "",
      cityName: DataReq.CityName ?? DataReq.cityName ?? "",
      districtID: DataReq.DistrictID ?? DataReq.districtID ?? "",
      districtName: DataReq.DistrictName ?? DataReq.districtName ?? "",
      neighborhoodID: DataReq.NeighborhoodID ?? DataReq.neighborhoodID ?? "",
      neighborhoodName:
        DataReq.NeighborhoodName ?? DataReq.neighborhoodName ?? "",
      notes: DataReq.Notes ?? DataReq.notes ?? "",
    };
  }
  static mapListApiToAddress(AddressList) {
    if (!AddressList) return [];
    return AddressList.map((address) => {
      return {
        ...AddressMapper.MapFromApiDTO(address),
      };
    });
  }
}
