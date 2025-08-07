namespace Domain.Models
{
	public class ProductionFacility
	{
		public int Id { get; set; }
		public string Code { get; set; } = null!;
		public string Name { get; set; } = null!;
		public double StandardArea { get; set; }

		public ICollection<Contract> Contracts { get; set; } = new List<Contract>();
	}
}
