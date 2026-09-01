import TResult from "@/Generic/TResult";
import countryRepository from "@/repositories/Address/country_repository";
import { countrySchema } from "@/validations/Address/countrySchema";
import { validate1 } from "@/validations/validator";
class CountryService {
  async getAll() {
    return await countryRepository.getCountryList();
  }

  async getById(param, paramType = 1) {
    return await countryRepository.getCountryById(param, paramType);
  }

  async create(countryData) {
    const validation = validate1(countryData, countrySchema);
    if (!validation.isValid) {
      return { IsSuccess: false, Error: validation.errors };
    }
    const payload = {
      countryID: countryData.ID,
      countryName: countryData.Name,
      countryCode: countryData.Code,
    };
    return await countryRepository.addCountry(payload);
  }

  async update(countryData) {
    const validation = validate1(countryData, countrySchema);
    if (!validation.isValid) {
      return new TResult(null, validation.isValid, validation.errors);
    }
    const payload = {
      countryID: countryData.ID,
      countryName: countryData.Name,
      countryCode: countryData.Code,
    };
    return await countryRepository.updateCountry(payload);
  }
}

export default new CountryService();
