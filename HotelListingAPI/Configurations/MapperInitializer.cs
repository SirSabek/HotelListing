using AutoMapper;
using HotelListingAPI.Data;
using HotelListingAPI.DTOs;

namespace HotelListingAPI.Configurations;

public class MapperInitializer : Profile
{
    public MapperInitializer()
    {
        CreateMap<Country, CountryDTO>().ReverseMap();
        CreateMap<Country, CreateCountryDTO>().ReverseMap();
        CreateMap<Hotel, HotelDTO>().ReverseMap();
        CreateMap<Hotel, CreateHotelDTO>().ReverseMap();
        CreateMap<APIUser, UserDTO>().ReverseMap();
    }
}