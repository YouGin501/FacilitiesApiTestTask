using Api.Controllers;
using Application.Interfaces;
using Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace FacilitiesApi.Tests
{
	public class ContractsControllerTests
	{
		private readonly Mock<IContractService> _mockService;
		private readonly ContractsController _controller;

		public ContractsControllerTests()
		{
			_mockService = new Mock<IContractService>();
			var logger = new Mock<ILogger<ContractsController>>();
			_controller = new ContractsController(_mockService.Object, logger.Object);
		}

		[Fact]
		public async Task Create_ShouldReturnOk_WhenContractIsCreated()
		{
			// Arrange
			var dto = new CreateContractDto("FAC-001", "EQ-001", 3);

			_mockService.Setup(s => s.CreateContractAsync(dto)).Returns(Task.CompletedTask);

			// Act
			var result = await _controller.Create(dto);

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal(200, okResult.StatusCode);
			_mockService.Verify(s => s.CreateContractAsync(dto), Times.Once);
		}

		[Fact]
		public async Task Create_ShouldReturnBadRequest_WhenServiceThrowsArgumentException()
		{
			// Arrange
			var dto = new CreateContractDto("INVALID", "EQ-001", 5);

			_mockService
				.Setup(s => s.CreateContractAsync(dto))
				.ThrowsAsync(new ArgumentException("Invalid codes"));

			// Act
			var result = await Record.ExceptionAsync(() => _controller.Create(dto));

			// Assert
			Assert.IsType<ArgumentException>(result);
		}

		[Fact]
		public async Task GetAll_ShouldReturnOk_WithContracts()
		{
			// Arrange
			var contracts = new List<ContractResultDto>
			{
				new ContractResultDto("Factory A", "Pump", 5),
				new ContractResultDto("Factory B", "Conveyor", 2)
			};

			_mockService.Setup(s => s.GetContractsAsync()).ReturnsAsync(contracts);

			// Act
			var result = await _controller.GetAll();

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal(200, okResult.StatusCode);
			var returnedContracts = Assert.IsAssignableFrom<IEnumerable<ContractResultDto>>(okResult.Value);
			Assert.Equal(2, returnedContracts.Count());
		}
	}
}
