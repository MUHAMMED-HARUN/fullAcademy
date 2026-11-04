import TResult from "@/Generic/TResult";
import CountryMapper from "@/Mapper/Address/countryMapper";
import countryRepository from "@/repositories/Address/country_repository";
import {
  NewCountrySchema,
  UpdateCountrySchema,
} from "@/validations/Address/countrySchema";
import { validate1 } from "@/validations/validator";
class CountryService {
  async getAll() {
    return await countryRepository.getCountryOptions();
  }

  async getPage(filter) {
    const result = await countryRepository.getCountryList(filter);
    if (!result.success) return result;

    return new TResult(
      CountryMapper.mapPagingApiToCountry(result.data),
      result.success,
      result.errors
    );
  }

  async getById(param, paramType = 1) {
    const result = await countryRepository.getCountryById(param, paramType);
    if (!result.success) return result;

    return new TResult(
      CountryMapper.mapFromApiDTO(result.data),
      result.success,
      result.errors
    );
  }

  async create(countryData) {
    const validation = validate1(countryData, NewCountrySchema);
    if (!validation.isValid) {
      return new TResult(null, false, validation.errors);
    }
    const payload = CountryMapper.mapUpsertToApi(countryData);
    return await countryRepository.addCountry(payload);
  }

  async update(countryData) {
    const validation = validate1(countryData, UpdateCountrySchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = CountryMapper.mapUpsertToApi(countryData);
    return await countryRepository.updateCountry(payload);
  }
}

export default new CountryService();
