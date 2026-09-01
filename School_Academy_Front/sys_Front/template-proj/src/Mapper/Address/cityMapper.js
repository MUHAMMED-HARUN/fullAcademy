export default class CityMapper {
  static mapUpsertCityToApi(dataSchema) {
    return {
      CityID: dataSchema.id,
      CityName: dataSchema.name,
      CityCode: dataSchema.code,
      CountryID: dataSchema.countryID,
    };
  }
  static MapFromApiDTO(DataReq) {
    return {
      cityID: DataReq.ID,
      cityName: DataReq.Name,
      cityCode: DataReq.Code,
      countryID: DataReq.CountryID,
    };
  }
  static mapListApiToCity(cityList) {
    if (!cityList) return [];
    return cityList.map((city) => {
      return {
        id: city.ID,
        name: city.Name,
        code: city.Code,
        countryID: city.CountryID,
        countryName: city.CountryName,
      };
    });
  }
}
