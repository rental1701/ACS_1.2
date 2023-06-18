using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.Model.DataContext
{
    internal class DataUserContext : DbContext
    {
        public DbSet<Person> Persons { get; set; } = null!;

        public DbSet<Division> Divisions { get; set; } = null!;
        public DbSet<Post> Posts { get; set; } = null!;
        public DbSet<Company> Companys { get; set; } = null !;
      //  public DbSet<LogData> AccessData { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false).Build();
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                 .HasOne(p => p.Division)
                 .WithMany(d => d.Persons)
                 .HasForeignKey(p => p.DivisionId);
            modelBuilder.Entity<Division>().HasMany(d => d.Persons);
        }

        public void GetFirstInputLastOutput()
        {

        }
    }
}
