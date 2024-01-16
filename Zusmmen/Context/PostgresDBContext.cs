using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Zusmmen.Models;

namespace Zusmmen;

public class ZusammenDbContext:DbContext
{
    public ZusammenDbContext(DbContextOptions<ZusammenDbContext> options)
        : base(options)
    {
    }    
    
    public DbSet<Rooms> Rooms { get; set; }
    public DbSet<Films> Films { get; set; }
}
