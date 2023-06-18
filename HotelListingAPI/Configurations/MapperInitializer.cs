using AutoMapper;
using HotelListingAPI.Data;
using HotelListingAPI.DTOs.Country;
using HotelListingAPI.DTOs.Hotel;
using HotelListingAPI.DTOs.User;

namespace HotelListingAPI.Configurations;

public class MapperInitializer : Profile
{
    public MapperInitializer()
    {
        CreateMap<Country, CountryDTO>().ReverseMap();
        CreateMap<Country, CreateCountryDTO>().ReverseMap();
        CreateMap<Country, UpdateCountryDTO>().ReverseMap();
        CreateMap<Hotel, HotelDTO>().ReverseMap();
        CreateMap<Hotel, CreateHotelDTO>().ReverseMap();
        CreateMap<Hotel, UpdateHotelDTO>().ReverseMap();
        CreateMap<APIUser, UserDTO>().ReverseMap();
    }
}