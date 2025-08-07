using Domain.DTO;

namespace Application.Interfaces
{
	public interface IContractService
	{
		Task CreateContractAsync(CreateContractDto dto);
		Task<IEnumerable<ContractResultDto>> GetContractsAsync();
	}
}
