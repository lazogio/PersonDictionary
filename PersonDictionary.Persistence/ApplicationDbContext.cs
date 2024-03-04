using Microsoft.EntityFrameworkCore;
using PersonDictionary.Domain.Entities;
using PersonDictionary.Persistence.Configurations;

namespace PersonDictionary.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<PersonRelation> PersonRelations { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }
        public DbSet<City> Cities { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PersonConfiguration());
            modelBuilder.ApplyConfiguration(new PersonRelationConfiguration());
            modelBuilder.ApplyConfiguration(new PhoneNumberConfiguration());
            modelBuilder.ApplyConfiguration(new CityConfiguration());
        }
    }
}