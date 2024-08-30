using GrainElevatorAPI.Core.Calculators;
using GrainElevatorAPI.Core.Calculators.Impl;
using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.Core.Models;

public class CompletionReport
{
	[Required(ErrorMessage = "Id is required.")]
	[Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
	public int Id { get; set; }

	[MinLength(3, ErrorMessage = "ReportNumber must be at least 3 characters long.")]
	[MaxLength(9, ErrorMessage = "ReportNumber must be at least 9 characters long.")]
	public int ReportNumber { get; set; }

	[DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
	[Range(typeof(DateTime), "1900-01-01", "2024-12-31", ErrorMessage = "ReportDate must be between 1900 and 2024.")]
	public DateTime ReportDate { get; set; }

	[Range(0, double.MaxValue, ErrorMessage = "QuantitiesDrying must be a positive number.")]
	public double? QuantitiesDrying { get; set; }

	[Range(0, double.MaxValue, ErrorMessage = "ReportPhysicalWeight must be a positive number.")]
	public double? ReportPhysicalWeight { get; set; }
	
    public bool? IsFinalized { get; set; }
    
    public virtual ICollection<Register> Registers { get; set; } = new List<Register>();
    public virtual ICollection<CompletionReportItem> CompletionReportItems { get; set; } = new List<CompletionReportItem>();

    
    [Range(1, int.MaxValue, ErrorMessage = "SupplierId must be a positive number.")]
    public int SupplierId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "ProductId must be a positive number.")]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "PriceListId must be a positive number.")]
    public int? PriceListId { get; set; }

    public virtual Supplier Supplier { get; set; }
    public virtual Product Product { get; set; }
    public virtual PriceList? PriceList { get; set; }
    
    
	[DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
	[Range(typeof(DateTime), "1900-01-01", "2024-12-31", ErrorMessage = "CreatedAt must be between 1900 and 2024.")]
	public DateTime CreatedAt { get; set; }

	[DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
	[Range(typeof(DateTime), "1900-01-01", "2024-12-31", ErrorMessage = "ModifiedAt must be between 1900 and 2024.")]
	public DateTime? ModifiedAt { get; set; }

	[DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
	[Range(typeof(DateTime), "1900-01-01", "2024-12-31", ErrorMessage = "RemovedAt must be between 1900 and 2024.")]
	public DateTime? RemovedAt { get; set; }

	[DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
	[Range(typeof(DateTime), "1900-01-01", "2024-12-31", ErrorMessage = "RestoredAt must be between 1900 and 2024.")]
	public DateTime? RestoredAt { get; set; }

	[Range(1, int.MaxValue, ErrorMessage = "CreatedById must be a positive number.")]
	public int CreatedById { get; set; }
	[Range(1, int.MaxValue, ErrorMessage = "ModifiedById must be a positive number.")]
	public int? ModifiedById { get; set; }
	[Range(1, int.MaxValue, ErrorMessage = "RemovedById must be a positive number.")]
	public int? RemovedById { get; set; }
	[Range(1, int.MaxValue, ErrorMessage = "RestoreById must be a positive number.")]
	public int? RestoreById { get; set; }
	
    public virtual Employee? CreatedBy { get; set; }
    public virtual Employee? ModifiedBy { get; set; }
    public virtual Employee? RemovedBy { get; set; }
    public virtual Employee? RestoreBy { get; set; }
}

