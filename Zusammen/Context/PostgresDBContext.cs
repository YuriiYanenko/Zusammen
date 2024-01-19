using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Zusammen.Models;

namespace Zusammen;

public class ZusammenDbContext:DbContext
{
    public ZusammenDbContext(DbContextOptions<ZusammenDbContext> options)
        : base(options)
    {
    }    
    
    //object of rooms table
    public DbSet<rooms> rooms { get; set; }
    //object of film table
    public DbSet<films> films { get; set; }
}
