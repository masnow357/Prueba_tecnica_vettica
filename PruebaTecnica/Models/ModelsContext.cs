using System;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PruebaTecnica.Models
{
	public class ModelsContext : DbContext
	{
		private readonly IConfiguration _configuration;

		public ModelsContext(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			// Configure the PostgreSQL connection string
			optionsBuilder.UseNpgsql(_configuration.GetConnectionString("ExampleConnection"));
		}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleID);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<DocumentType> DocumentType { get; set; }
        public DbSet<Role> Role { get; set; }

    }
}
