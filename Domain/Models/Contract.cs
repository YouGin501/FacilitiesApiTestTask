namespace Domain.Models
{
	public class Contract
	{
		public int Id { get; set; }
		public int Quantity { get; set; }

		public int ProductionFacilityId { get; set; }
		public ProductionFacility ProductionFacility { get; set; } = null!;

		public int ProcessEquipmentTypeId { get; set; }
		public ProcessEquipmentType ProcessEquipmentType { get; set; } = null!;
	}
}
