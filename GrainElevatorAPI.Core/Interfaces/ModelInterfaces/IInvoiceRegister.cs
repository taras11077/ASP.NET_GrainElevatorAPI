using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ModelInterfaces;

public interface IInvoiceRegister
{
	int Id { get; set; }
	string RegisterNumber { get; set; }
	DateTime ArrivalDate { get; set; }
	double WeedImpurityBase { get; set; }
	double MoistureBase { get; set; }
	
	ICollection<ProductionBatch> ProductionBatches { get; set; }
	
	int? PhysicalWeightReg { get; set; }
	int? ShrinkageReg { get; set; }
	int? WasteReg { get; set; }
	int? AccWeightReg { get; set; }
	double? QuantitiesDryingReg { get; set; }
	
	int SupplierId { get; set; }
	int ProductId { get; set; }
	int? CompletionReportId { get; set; }
}