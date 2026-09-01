export default class AddressMapper {
  static mapUpsertToApi(dataSchema) {
    return {
      AddressID: dataSchema.id,
      CountryID: dataSchema.countryID,
      CityID: dataSchema.cityID,
      DistrictID: dataSchema.districtID,
      NeighborhoodID: dataSchema.neighborhoodID,
      Notes: dataSchema.notes,
    };
  }
  static MapFromApiDTO(DataReq) {
    return {
      id: DataReq.AddressID,
      countryID: DataReq.CountryID,
      cityID: DataReq.CityID,
      districtID: DataReq.DistrictID,
      neighborhoodID: DataReq.NeighborhoodID,
      notes: DataReq.Notes,
    };
  }
  static mapListApiToAddress(AddressList) {
    if (!AddressList) return [];
    return AddressList.map((address) => {
      return {
        id: address.AddressID,
        countryID: address.CountryID,
        countryName: address.CountryName,
        cityName: address.CityName,
        cityID: address.CityID,
        districtID: address.DistrictID,
        districtName: address.DistrictName,
        neighborhoodID: address.NeighborhoodID,
        neighborhoodName: address.NeighborhoodName,
        notes: address.Notes,
      };
    });
  }
}
