import AddressMapper from "../Address/addressMapper";

const toDateOnly = (value) => {
  if (value instanceof Date && !Number.isNaN(value.getTime())) {
    const year = value.getFullYear();
    const month = String(value.getMonth() + 1).padStart(2, "0");
    const day = String(value.getDate()).padStart(2, "0");
    return `${year}-${month}-${day}`;
  }

  return typeof value === "string" ? value.slice(0, 10) : value;
};

export default class PersonMapper {
  static mapUpsertToApi(dataSchema) {
    const address = AddressMapper.mapUpsertToApi(dataSchema.address);
    const payload = {
      FirstName: dataSchema.firstName,
      FatherName: dataSchema.fatherName,
      LastName: dataSchema.lastName,
      MotherName: dataSchema.motherName,
      MotherLastName: dataSchema.motherLastName,
      NationalNumber: dataSchema.nationalNumber,
      Gendor: dataSchema.gender,
      BirthDate: toDateOnly(dataSchema.birthDate),
      PlaceOfBirthID: dataSchema.placeOfBirthID,
      Address: address,
    };

    if (dataSchema.id) {
      payload.PersonID = dataSchema.id;
    } else {
      delete address.AddressID;
    }

    return payload;
  }

  static MapFromApiDTO(DataReq) {
    if (!DataReq) return null;
    return {
      id: DataReq.PersonID,
      firstName: DataReq.FirstName,
      fatherName: DataReq.FatherName,
      lastName: DataReq.LastName,
      motherName: DataReq.MotherName,
      motherLastName: DataReq.MotherLastName,
      nationalNumber: DataReq.NationalNumber,
      displayName: [DataReq.FirstName, DataReq.FatherName, DataReq.LastName]
        .filter(Boolean)
        .join(" "),
      gender: DataReq.Gendor,
      birthDate: DataReq.BirthDate,
      placeOfBirth: DataReq.PlaceOfBirth,
      livingPlace: DataReq.LivingPlace,
    };
  }

  static MapForUpsertFromApiDTO(DataReq) {
    if (!DataReq) return null;

    const address = DataReq.Address
      ? AddressMapper.MapFromApiDTO(DataReq.Address)
      : {
          id: "",
          countryID: "",
          cityID: "",
          districtID: "",
          neighborhoodID: "",
          notes: "",
        };

    return {
      id: DataReq.PersonID,
      firstName: DataReq.FirstName,
      fatherName: DataReq.FatherName,
      lastName: DataReq.LastName,
      motherName: DataReq.MotherName,
      motherLastName: DataReq.MotherLastName,
      nationalNumber: DataReq.NationalNumber,
      gender: DataReq.Gendor,
      birthDate: DataReq.BirthDate,
      placeOfBirthID: DataReq.PlaceOfBirthID || "",
      livingPlaceID: DataReq.LivingPlaceID || "",
      address,
    };
  }

  static mapListApiToPerson(PersonList) {
    if (!PersonList) return [];

    // التحقق مما إذا كانت القائمة مصفوفة مباشرة أم كائن يحتوي على items القادم من Gridify
    const items = Array.isArray(PersonList)
      ? PersonList
      : PersonList.items || PersonList.data || [];

    return items.map((person) => PersonMapper.MapFromApiDTO(person));
  }
}
