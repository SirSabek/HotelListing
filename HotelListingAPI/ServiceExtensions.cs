using System.Text;
using HotelListingAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace HotelListingAPI;

public static class ServiceExtensions
{
    public static void ConfigureIdentity(this IServiceCollection services)
    {
        var builder = services.AddIdentityCore<APIUser>(q => q.User.RequireUniqueEmail = true);
        builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);
        builder.AddEntityFrameworkStores<Context>().AddDefaultTokenProviders();
    }

    public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("Jwt");
        var key = "2a5979f0 - 0d15 - 11ee - be56 - 0242ac120002"; //Environment.GetEnvironmentVariable("HOTEL-KEY");

        services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.GetSection("Issuer").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };
            });

    }

    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(error =>
        {
            error.Run(async context =>
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    Log.Error($"Something went wrong in {contextFeature.Error}");

                    await context.Response.WriteAsync(new Error
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = "Internal server Error, please try again latter"
                    }.ToString());
                }
            });
        });
    }

    public static void ConfigureVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(option =>
        {
            option.ReportApiVersions = true;
            option.AssumeDefaultVersionWhenUnspecified = true;
            option.DefaultApiVersion = new ApiVersion(1, 0);
            option.ApiVersionReader = new HeaderApiVersionReader("api-version");
        });
    }
}
