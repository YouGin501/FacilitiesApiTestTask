using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<ProductionFacility> ProductionFacilities { get; set; }
		public DbSet<ProcessEquipmentType> ProcessEquipmentTypes { get; set; }
		public DbSet<Contract> Contracts { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<ProductionFacility>().HasIndex(p => p.Code).IsUnique();
			modelBuilder.Entity<ProcessEquipmentType>().HasIndex(p => p.Code).IsUnique();

			modelBuilder.Entity<ProductionFacility>().HasData(
				new ProductionFacility { Id = 1, Code = "FAC-001", Name = "Factory A", StandardArea = 500 },
				new ProductionFacility { Id = 2, Code = "FAC-002", Name = "Factory B", StandardArea = 1000 }
			);

			modelBuilder.Entity<ProcessEquipmentType>().HasData(
				new ProcessEquipmentType { Id = 1, Code = "EQ-001", Name = "Boiler", Area = 100 },
				new ProcessEquipmentType { Id = 2, Code = "EQ-002", Name = "Conveyor", Area = 200 }
			);

			modelBuilder.Entity<Contract>().HasData(
				new Contract
				{
					Id = 1,
					ProductionFacilityId = 1,
					ProcessEquipmentTypeId = 1,
					Quantity = 2
				}
			);
		}
	}
}
