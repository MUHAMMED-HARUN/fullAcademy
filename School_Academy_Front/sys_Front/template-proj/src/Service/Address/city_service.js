import TResult from "@/Generic/TResult";
import CityMapper from "@/Mapper/Address/cityMapper";
import city_repositories from "@/repositories/Address/city_repositories";
import {
  CityLookupSchema,
  NewCitySchema,
  UpdateCitySchema,
} from "@/validations/Address/citySchema";
import { validate1 } from "@/validations/validator";
class CityService {
  async getAll() {
    const rees = await city_repositories.getList();
    var Cities;
    if (rees.success) {
      Cities = CityMapper.mapListApiToCity(rees.data);
      return new TResult(Cities, rees.success, rees.errors);
    }
    return new TResult(null, false, rees.errors);
  }

  async getPage(filter) {
    const result = await city_repositories.getCityList(filter);
    if (!result.success) return result;

    return new TResult(
      CityMapper.mapPagingApiToCity(result.data),
      result.success,
      result.errors
    );
  }

  async searchCities(criteria) {
    const validation = validate1(criteria, CityLookupSchema);
    if (!validation.isValid) {
      return new TResult(null, false, validation.errors);
    }

    const value = criteria.value?.trim() || "";
    const filters = [`CountryID=${criteria.countryID}`];
    if (value) {
      filters.unshift(`Name=*${value}`);
    }

    return await this.getPage({
      Page: 1,
      PageSize: 5,
      Filter: filters.join(","),
    });
  }

  async getById(param, paramType = 1) {
    const result = await city_repositories.getById(param, paramType);
    if (!result.success) return result;

    return new TResult(
      CityMapper.MapFromApiDTO(result.data),
      result.success,
      result.errors
    );
  }

  async create(cityData) {
    const validation = validate1(cityData, NewCitySchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = CityMapper.mapUpsertCityToApi(cityData);
    return await city_repositories.add(payload);
  }

  async update(cityData) {
    const validation = validate1(cityData, UpdateCitySchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }

    const payload = CityMapper.mapUpsertCityToApi(cityData);
    return await city_repositories.update(payload);
  }
}

export default new CityService();
