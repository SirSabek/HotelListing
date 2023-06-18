using HotelListingAPI.DTOs.User;

namespace HotelListingAPI.Services;

public interface IAuthManager
{
    Task<bool> ValidateUser(LoginUserDTO userDTO);
    Task<string> CreateToken();
}