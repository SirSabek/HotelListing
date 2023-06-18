using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HotelListingAPI.Data;
using HotelListingAPI.DTOs.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace HotelListingAPI.Services;

public class AuthManager : IAuthManager
{
    private readonly UserManager<APIUser> _userManager;
    private readonly IConfiguration _configuration;
    private APIUser _user;

    public AuthManager(UserManager<APIUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<bool> ValidateUser(LoginUserDTO userDTO)
    {
        _user = await _userManager.FindByNameAsync(userDTO.Email);
        return (_user != null && await _userManager.CheckPasswordAsync(_user, userDTO.Password));
    }

    public async Task<string> CreateToken()
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims();
        var token= GenerateTokenOptions(signingCredentials, claims);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    
    private SigningCredentials GetSigningCredentials()
    {
        var key = "2a5979f0 - 0d15 - 11ee - be56 - 0242ac120002"; //Environment.GetEnvironmentVariable("KEY");
        var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private async Task<List<Claim>> GetClaims()
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, _user.UserName)
        };

        var roles = await _userManager.GetRolesAsync(_user);

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }

    private SecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var token = new JwtSecurityToken(
                       issuer: jwtSettings.GetSection("Issuer").Value,
                       claims: claims,
                       expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings.GetSection("Lifetime").Value)),
                       signingCredentials: signingCredentials);
        return token;
    }

}