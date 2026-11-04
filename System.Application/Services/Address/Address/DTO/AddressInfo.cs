

namespace System.Application.Services.Address.Address.DTO
{
    public class AddressInfo
    {
        public string AddressID { get; set; } = string.Empty;
        public string CountryID { get; set; } = string.Empty;
        public string CountryName { get; set; } = string.Empty;
        public string CityID { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
        public string DistrictID { get; set; } = string.Empty;
        public string DistrictName { get; set; } = string.Empty;
        public string NeighborhoodID { get; set; } = string.Empty;
        public string NeighborhoodName { get; set; } = string.Empty;
        public string? Notes { get; set; }

    }
}
