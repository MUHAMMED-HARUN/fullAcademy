using System.Domain.Entities.BaseClass;
using System.Domain.Exceptions;

namespace System.Domain.Entities
{
    public class clsCountry : clsBaseClass
    {
        public string CountryName { get; private set; }
        public string CountryCode { get; private set; }

        protected clsCountry() { }

        public clsCountry(string countryName, string countryCode)
        {
            if (string.IsNullOrWhiteSpace(countryName))
                throw new DomainException(DomainExMsg.Country_Name_Required);
            if (string.IsNullOrWhiteSpace(countryCode))
                throw new DomainException(DomainExMsg.Country_Code_Required);

            CountryName = countryName;
            CountryCode = countryCode;
            BaseSetCreated("System");
        }

        public void SetCountryName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException(DomainExMsg.Country_Name_Required);
            CountryName = name;
        }

        public void SetCountryCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new DomainException(DomainExMsg.Country_Code_Required);
            CountryCode = code;
        }
    }
}
