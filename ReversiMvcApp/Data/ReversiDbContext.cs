using Microsoft.EntityFrameworkCore;
using ReversiMvcApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiMvcApp.Data
{
	public class ReversiDbContext : DbContext
	{
		public ReversiDbContext(DbContextOptions<ReversiDbContext> options) : base(options) { }
		public DbSet<Speler> Spelers { get; set; }
		public DbSet<ReversiMvcApp.Models.GebruikerRole> GebruikerRole { get; set; }
		//public DbSet<GebruikerRole> GebruikerRoles { get; set; }
	}
}
