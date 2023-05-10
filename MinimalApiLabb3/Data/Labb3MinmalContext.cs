using Microsoft.EntityFrameworkCore;
using static MinimalApiLabb3.Program;

namespace MinimalApiLabb3.Data
{
    public class Labb3MinmalContext : DbContext
    {
        public Labb3MinmalContext()
        {
        }

        public Labb3MinmalContext(DbContextOptions<Labb3MinmalContext> options) : base(options)
        {
            
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Interest> Interests { get; set; }
        public DbSet<Link> Links { get; set; }
    }
}
