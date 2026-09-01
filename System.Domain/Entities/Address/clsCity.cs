
using System.Domain.Entities.BaseClass;
using System.Domain.Exceptions;

namespace System.Domain.Entities
{
    public class clsCity : clsBaseClass
    {
        public Guid CountryID { get; private set; }
        public string CityName { get; private set; }
        public string? CityCode { get; private set; }

        public clsCountry Country { get; private set; }

        protected clsCity() { }

        public clsCity(Guid countryID, string cityName, string? cityCode = null)
        {
            if (countryID == Guid.Empty)
                throw new DomainException(DomainExMsg.City_CountryID_Required);
            if (string.IsNullOrWhiteSpace(cityName))
                throw new DomainException(DomainExMsg.City_Name_Required);

            // SystemUseCase says: CityCode? (Nullable) but nothing about mandatory validation for it in conditions,
            // unlike CountryCode. So optional as per signature.
            
            CountryID = countryID;
            CityName = cityName;
            CityCode = cityCode;
            BaseSetCreated("System");
        }

        public void SetCountryID(Guid countryID)
        {
            if (countryID == Guid.Empty)
                throw new DomainException(DomainExMsg.City_CountryID_Required);
            CountryID = countryID;
        }

        public void SetCityName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException(DomainExMsg.City_Name_Required);
            CityName = name;
        }

        public void SetCityCode(string? code)
        {
             CityCode = code;
        }
    }
}
