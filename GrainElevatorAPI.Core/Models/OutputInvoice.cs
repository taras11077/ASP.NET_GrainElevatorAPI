using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.Core.Models;

public class OutputInvoice
{
	[Required(ErrorMessage = "Id is required.")]
	[Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
	public int Id { get; set; }

	[MinLength(3, ErrorMessage = "InvoiceNumber must be at least 3 characters long.")]
	[MaxLength(9, ErrorMessage = "InvoiceNumber must be at least 9 characters long.")]
	public string InvoiceNumber { get; set; }

	[DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
	[Range(typeof(DateTime), "1900-01-01", "2024-12-31", ErrorMessage = "ShipmentDate must be between 1900 and 2024.")]
	public DateTime ShipmentDate { get; set; }


	[MinLength(2, ErrorMessage = "VehicleNumber must be at least 3 characters long.")]
	[MaxLength(10, ErrorMessage = "VehicleNumber must be at least 9 characters long.")]
	public string? VehicleNumber { get; set; }

	[MinLength(2, ErrorMessage = "ProductCategory must be at least 2 characters long.")]
	[MaxLength(20, ErrorMessage = "ProductCategory must be at least 20 characters long.")]
	public string ProductCategory { get; set; }

	[Required(ErrorMessage = "ProductWeight is required.")]
	[Range(1, int.MaxValue, ErrorMessage = "ProductWeight must be a positive number.")]
	public int ProductWeight { get; set; }
	
	

	[Range(1, int.MaxValue, ErrorMessage = "SupplierId must be a positive number.")]
	public int SupplierId { get; set; }

	[Range(1, int.MaxValue, ErrorMessage = "ProductId must be a positive number.")]
	public int ProductId { get; set; }

	[Range(1, int.MaxValue, ErrorMessage = "DepotItemId must be a positive number.")]
	public int DepotItemId { get; set; }
	
	public virtual Supplier Supplier { get; set; }
	public virtual Product Product { get; set; }
	public virtual DepotItem DepotItem { get; set; }
	
	

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

	
	public virtual Employee CreatedBy { get; set; }
	public virtual Employee? ModifiedBy { get; set; }
	public virtual Employee? RemovedBy { get; set; }
	public virtual Employee? RestoreBy { get; set; }
}