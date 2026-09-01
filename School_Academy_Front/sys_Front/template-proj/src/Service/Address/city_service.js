import TResult from "@/Generic/TResult";
import CityMapper from "@/Mapper/Address/cityMapper";
import city_repositories from "@/repositories/Address/city_repositories";
import { citySchema } from "@/validations/Address/citySchema";
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

  async getById(param) {
    return await city_repositories.getById(param);
  }

  async create(cityData) {
    const validation = validate1(cityData, citySchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    console.log("data req", cityData);

    const payload = CityMapper.mapUpsertCityToApi(cityData);
    return await city_repositories.add(payload);
  }

  async update(cityData) {
    const validation = validate1(cityData, citySchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }

    const payload = CityMapper.mapUpsertCityToApi(cityData);
    return await city_repositories.update(payload);
  }
}

export default new CityService();
