using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PruebaTecnica.Models
{
	public class ModelsContext : DbContext
	{
		private readonly IConfiguration _configuration;

		public ModelsContext(DbContextOptions<ModelsContext> options, IConfiguration configuration)
			: base(options)
		{
			_configuration = configuration;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			// Configure the PostgreSQL connection string
			optionsBuilder.UseNpgsql(_configuration.GetConnectionString("ExampleConnection"));
		}

		public DbSet<User> Users { get; set; }
	}
}

