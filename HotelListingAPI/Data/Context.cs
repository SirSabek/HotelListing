using Microsoft.EntityFrameworkCore;

namespace HotelListingAPI.Data;

public class Context : DbContext
{
    public Context(DbContextOptions options) : base(options)
    {
        
    }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
}