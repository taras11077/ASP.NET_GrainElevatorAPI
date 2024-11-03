using GrainElevatorAPI.Core.Calculators;
using GrainElevatorAPI.Core.Calculators.Impl;
using System.ComponentModel.DataAnnotations;
using GrainElevatorAPI.Core.Interfaces.ModelInterfaces;
using GrainElevatorAPI.Core.Models.Base;

namespace GrainElevatorAPI.Core.Models;

public class CompletionReport : AuditableEntity, ICompletionReport
{
	[Required(ErrorMessage = "Id is required.")]
	[Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
	public int Id { get; set; }

	[MinLength(3, ErrorMessage = "ReportNumber must be at least 3 characters long.")]
	[MaxLength(9, ErrorMessage = "ReportNumber must be at least 9 characters long.")]
	public string ReportNumber { get; set; }

	[DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
	[Range(typeof(DateTime), "1900-01-01", "2024-12-31", ErrorMessage = "ReportDate must be between 1900 and 2024.")]
	public DateTime ReportDate { get; set; }

	
	public int? PhysicalWeightReport { get; set; }
	public int? QuantitiesDryingReport { get; set; }
	public int? ShrinkageReport { get; set; }
	public int? WasteReport { get; set; }
	public int? AccWeightReport { get; set; }

	
	
	
	[Range(0, double.MaxValue, ErrorMessage = "TotalCost must be a positive number.")]
	public decimal TotalCost { get; set; }
	
    public bool? IsFinalized { get; set; }
    
    public virtual ICollection<InvoiceRegister> Registers { get; set; } = new List<InvoiceRegister>();
    public virtual ICollection<CompletionReportOperation> CompletionReportOperations { get; set; } = new List<CompletionReportOperation>();

    
    [Range(1, int.MaxValue, ErrorMessage = "SupplierId must be a positive number.")]
    public int SupplierId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "ProductId must be a positive number.")]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "PriceListId must be a positive number.")]
    public int? PriceListId { get; set; }

    public virtual Supplier Supplier { get; set; }
    public virtual Product Product { get; set; }
    public virtual PriceList? PriceList { get; set; }
    
}

