export default class DistrictMapper {
  static mapUpsertToApi(dataSchema) {
    return {
      DistrictID: dataSchema.id,
      DistrictName: dataSchema.name,
      CityID: dataSchema.cityID,
    };
  }
  static MapFromApiDTO(DataReq) {
    return {
      id: DataReq.DistrictID,
      name: DataReq.DistrictName,
      cityID: DataReq.CityID,
    };
  }
  static mapListApiToDistrict(DistrictList) {
    if (!DistrictList) return [];
    return DistrictList.map((district) => {
      return {
        id: district.ID,
        name: district.DistrictName,
        cityName: district.CityName,
        cityID: district.CityID,
        countryName: district.CountryName,
        countryID: district.CountryID,
      };
    });
  }
}
