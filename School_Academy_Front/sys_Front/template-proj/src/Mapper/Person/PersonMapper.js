import AddressMapper from "../Address/addressMapper";
export default class PersonMapper {
  static mapUpsertToApi(dataSchema) {
    return {
      PersonID: dataSchema.id,
      FirstName: dataSchema.firstName,
      FatherName: dataSchema.fatherName,
      LastName: dataSchema.lastName,
      MotherName: dataSchema.motherName,
      MotherLastName: dataSchema.motherLastName,
      NationalNumber: dataSchema.nationalNumber,
      Gendor: dataSchema.gender,
      BirthDate: dataSchema.birthDate,
      PlaceOfBirthID: dataSchema.placeOfBirthID,
      LivingPlaceID: dataSchema.livingPlaceID,
      Address: AddressMapper.mapUpsertToApi(dataSchema.address),
    };
  }

  static MapFromApiDTO(DataReq) {
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
      placeOfBirthID: DataReq.PlaceOfBirthID,
      placeOfBirth: DataReq.PlaceOfBirth,
      livingPlaceID: DataReq.LivingPlaceID,
      livingPlace: DataReq.LivingPlace,
    };
  }
  static mapListApiToPerson(PersonList) {
    if (!PersonList) return [];

    // التحقق مما إذا كانت القائمة مصفوفة مباشرة أم كائن يحتوي على items القادم من Gridify
    const items = Array.isArray(PersonList)
      ? PersonList
      : PersonList.items || PersonList.data || [];

    return items.map((person) => {
      return {
        id: person.PersonID,
        firstName: person.FirstName,
        fatherName: person.FatherName,
        lastName: person.LastName,
        motherName: person.MotherName,
        motherLastName: person.MotherLastName,
        nationalNumber: person.NationalNumber,
        gender: person.Gendor,
        birthDate: person.BirthDate,
        placeOfBirth: person.PlaceOfBirth,
        livingPlace: person.LivingPlace,
      };
    });
  }
}
