using System.ComponentModel.DataAnnotations;

namespace HotelListingAPI.DTOs;

public class CreateCountryDTO
{
    [Required]
    [StringLength(maximumLength: 50, ErrorMessage = "Country name is too long")]
    public string Name { get; set; }
    [Required]
    [StringLength(maximumLength: 2, ErrorMessage = "Short country name is too long")]
    public string ShortName { get; set; }
}