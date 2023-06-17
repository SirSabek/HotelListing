using HotelListingAPI.Data;
using Microsoft.AspNetCore.Identity;

namespace HotelListingAPI;

public static class ServiceExtensions
{
    public static void ConfigureIdentity(this IServiceCollection services)
    {
        var builder = services.AddIdentityCore<APIUser>(q => q.User.RequireUniqueEmail = true);
        builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);
        builder.AddSignInManager<SignInManager<APIUser>>().AddEntityFrameworkStores<Context>().AddDefaultTokenProviders();
    }
}