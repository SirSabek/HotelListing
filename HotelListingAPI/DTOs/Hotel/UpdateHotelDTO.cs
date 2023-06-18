using System.ComponentModel.DataAnnotations;

namespace HotelListingAPI.DTOs.Hotel;

public class UpdateHotelDTO
{
    [Required]
    [StringLength(maximumLength: 150, ErrorMessage = "Hotel Name Is Too Long")]
    public string Name { get; set; } = null!;
    [Required]
    [StringLength(maximumLength: 250, ErrorMessage = "Address Is Too Long")]
    public string Address { get; set; } = null!;
    [Required]
    [Range(1, 5)]
    public double Rating { get; set; }
    [Required]
    public int CountryId { get; set; }
}