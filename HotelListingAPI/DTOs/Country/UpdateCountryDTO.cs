using System.ComponentModel.DataAnnotations;
using HotelListingAPI.DTOs.Hotel;

namespace HotelListingAPI.DTOs.Country;

public class UpdateCountryDTO : CreateCountryDTO
{
   public IList<CreateHotelDTO> Hotels { get; set; }
}