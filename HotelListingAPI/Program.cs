using HotelListingAPI;
using HotelListingAPI.Configurations;
using HotelListingAPI.Data;
using HotelListingAPI.IRepository;
using HotelListingAPI.Repository;
using HotelListingAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//adding Serilog Configurations to the app
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.ClearProviders(); 
builder.Logging.AddSerilog(logger);

//adding cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

//adding dbcontext
builder.Services.AddDbContext<Context>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Configure Identity with Identity role and token provider
builder.Services.AddAuthentication("Identity.Application")
    .AddCookie("Identity.Application", options =>
    {
        // Configure the cookie options if needed
        options.Cookie.Name = "YourCookieName";
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        // Additional configuration if needed
    });

//configure versioning
builder.Services.ConfigureVersioning();

//builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();

//Configure JWT
builder.Services.ConfigureJWT(builder.Configuration);

//adding automapper
builder.Services.AddAutoMapper(typeof(MapperInitializer));  // MapperInitializer is a class in Configurations folder

//Registering all the services
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthManager, AuthManager>();


//adding newtonsoftjson
builder.Services.AddControllers(config =>
{
    config.CacheProfiles.Add("CacheDuration", new CacheProfile()
    {
        Duration = 180
    });
}).AddNewtonsoftJson(op =>
       op.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

//caching
builder.Services.AddResponseCaching();
builder.Services.ConfigureHttpCacheHeader();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureExceptionHandler();

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseResponseCaching();

app.UseHttpCacheHeaders();

app.MapControllers();

app.Run();
