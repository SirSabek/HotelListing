using System.ComponentModel.DataAnnotations;

namespace HotelListingAPI.DTOs;

public class CountryDTO : CreateCountryDTO
{
    public int Id { get; set; }
    public IList<HotelDTO> Hotels { get; set; }
}