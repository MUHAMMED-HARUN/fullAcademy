namespace System.Application.Services.Person.Person.DTO
{
    public sealed class PersonForUpsertDto
    {
        public string PersonID { get; init; } = string.Empty;
        public string FirstName { get; init; } = string.Empty;
        public string FatherName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string MotherName { get; init; } = string.Empty;
        public string MotherLastName { get; init; } = string.Empty;
        public string NationalNumber { get; init; } = string.Empty;
        public bool Gendor { get; init; }
        public DateOnly BirthDate { get; init; }
        public string? PlaceOfBirthID { get; init; }
        public string? LivingPlaceID { get; init; }
        public PersonAddressForUpsertDto? Address { get; init; }
    }

    public sealed class PersonAddressForUpsertDto
    {
        public string AddressID { get; init; } = string.Empty;
        public string CountryID { get; init; } = string.Empty;
        public string CityID { get; init; } = string.Empty;
        public string DistrictID { get; init; } = string.Empty;
        public string NeighborhoodID { get; init; } = string.Empty;
        public string? Notes { get; init; }
    }
}
