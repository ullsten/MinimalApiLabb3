using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using static MinimalApiLabb3.Models.InterestModel;
using static MinimalApiLabb3.Models.LinkModel;
using static MinimalApiLabb3.Models.PersonModel;
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder
            .UseSqlServer("Server=ULLSTENLENOVO; Database=Lab3MinimalV1;Trusted_Connection=true; MultipleActiveResultSets=true; Encrypt=True; TrustServerCertificate=True;");
    }
}
