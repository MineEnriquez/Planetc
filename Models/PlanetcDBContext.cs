using Microsoft.EntityFrameworkCore;

namespace Planetc.Models
{
  public class PlanetcDBContext : DbContext
  {        
    // base() calls the parent class' constructor passing the "options" parameter along
    public PlanetcDBContext(DbContextOptions options) : base(options)
    {

    }
    public DbSet<User> Users { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Like> Likes { get; set; }

  }
}
