using Application.Interfaces;
using Domain.DTO;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
	public class ContractService : IContractService
	{
		private readonly AppDbContext _context;
		private readonly ILogger<ContractService> _logger;

		public ContractService(AppDbContext context, ILogger<ContractService> logger)
		{
			_context = context;
			_logger = logger;
		}

		public async Task CreateContractAsync(CreateContractDto dto)
		{
			var facility = await _context.ProductionFacilities
				.Include(f => f.Contracts)
				.ThenInclude(c => c.ProcessEquipmentType)
				.FirstOrDefaultAsync(f => f.Code == dto.FacilityCode);

			var equipment = await _context.ProcessEquipmentTypes
				.FirstOrDefaultAsync(e => e.Code == dto.EquipmentCode);

			if (facility == null || equipment == null)
				throw new ArgumentException("Invalid facility or equipment code.");

			double usedArea = facility.Contracts.Sum(c => c.Quantity * c.ProcessEquipmentType.Area);
			double requiredArea = dto.Quantity * equipment.Area;

			if (usedArea + requiredArea > facility.StandardArea)
				throw new InvalidOperationException("Not enough available area.");

			var contract = new Contract
			{
				ProductionFacilityId = facility.Id,
				ProcessEquipmentTypeId = equipment.Id,
				Quantity = dto.Quantity
			};

			_context.Contracts.Add(contract);
			await _context.SaveChangesAsync();

			_logger.LogInformation($"Contract created: Facility={dto.FacilityCode}, Equipment={dto.EquipmentCode}, Quantity={dto.Quantity}");
		}

		public async Task<IEnumerable<ContractResultDto>> GetContractsAsync()
		{
			var result = await _context.Contracts
				.Include(c => c.ProductionFacility)
				.Include(c => c.ProcessEquipmentType)
				.Select(c => new ContractResultDto(
					c.ProductionFacility.Name,
					c.ProcessEquipmentType.Name,
					c.Quantity
				))
				.ToListAsync();

			return result;
		}
	}
}
