using Application.Interfaces;
using Domain.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ContractsController : ControllerBase
	{
		private readonly IContractService _contractService;
		private readonly ILogger<ContractsController> _logger;

		public ContractsController(IContractService contractService, ILogger<ContractsController> logger)
		{
			_contractService = contractService;
			_logger = logger;
		}

		/// <summary>
		/// Create a new equipment placement contract.
		/// </summary>
		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateContractDto dto)
		{
			await _contractService.CreateContractAsync(dto);
			return Ok(new { message = "Contract created." });
		}

		/// <summary>
		/// Get all equipment placement contracts.
		/// </summary>
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var result = await _contractService.GetContractsAsync();
			return Ok(result);
		}
	}
}