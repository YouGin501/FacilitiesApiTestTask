namespace Domain.Models
{
	public class ProcessEquipmentType
	{
		public int Id { get; set; }
		public string Code { get; set; } = null!;
		public string Name { get; set; } = null!;
		public double Area { get; set; }

		public ICollection<Contract> Contracts { get; set; } = new List<Contract>();
	}
}
