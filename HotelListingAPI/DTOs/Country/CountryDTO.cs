using System.ComponentModel.DataAnnotations;
using HotelListingAPI.DTOs.Hotel;

namespace HotelListingAPI.DTOs.Country;

public class CountryDTO : CreateCountryDTO
{
    public int Id { get; set; }
    public IList<HotelDTO> Hotels { get; set; }
}