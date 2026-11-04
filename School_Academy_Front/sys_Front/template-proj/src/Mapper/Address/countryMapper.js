export default class CountryMapper {
  static mapUpsertToApi(country) {
    const payload = {
      CountryName: country.name.trim(),
      CountryCode: country.code.trim(),
    };

    if (country.id) payload.CountryID = country.id;
    return payload;
  }

  static mapFromApiDTO(country) {
    if (!country) return null;
    return {
      id: country.ID ?? country.Id ?? country.id ?? "",
      name: country.Name ?? country.name ?? "",
      code: country.Code ?? country.code ?? "",
    };
  }

  static mapListApiToCountry(countries) {
    if (!Array.isArray(countries)) return [];
    return countries.map((country) => this.mapFromApiDTO(country));
  }

  static mapPagingApiToCountry(paging) {
    const data = paging?.Data ?? paging?.data ?? paging?.items ?? [];
    return {
      items: this.mapListApiToCountry(data),
      count: paging?.Count ?? paging?.count ?? 0,
    };
  }
}
