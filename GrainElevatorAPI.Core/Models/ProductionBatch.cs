using System.ComponentModel.DataAnnotations;
using GrainElevatorAPI.Core.Calculators.Impl;

namespace GrainElevatorAPI.Core.Models;

public class ProductionBatch
{
	[Required(ErrorMessage = "Id is required.")]
	[Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
	public int Id { get; set; }

	[Required(ErrorMessage = "WeedImpurityBase is required.")]
	[Range(0.0, 100.0, ErrorMessage = "WeedImpurityBase value must be between 0.0 and 100.0")]
	public double WeedImpurityBase { get; set; }

	[Required(ErrorMessage = "MoistureBase is required.")]
	[Range(0.0, 100.0, ErrorMessage = "MoistureBase value must be between 0.0 and 100.0")]
	public double MoistureBase { get; set; }

	[Range(1, int.MaxValue, ErrorMessage = "Waste must be a positive number.")]
	public int? Waste { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Shrinkage must be a positive number.")]
	public int? Shrinkage { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "AccountWeight must be a positive number.")]
	public int? AccountWeight { get; set; }



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



	[Required(ErrorMessage = "LaboratoryCardId is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "LaboratoryCardId must be a positive number.")]
	public int LaboratoryCardId { get; set; }

	[Range(1, int.MaxValue, ErrorMessage = "RegisterId must be a positive number.")]
	public int? RegisterId { get; set; }
	
    public virtual LaboratoryCard LaboratoryCard { get; set; }
    public virtual Register? Register { get; set; }

    public virtual Employee CreatedBy { get; set; }
    public virtual Employee? ModifiedBy { get; set; }
    public virtual Employee? RemovedBy { get; set; }
    public virtual Employee? RestoreBy { get; set; }
}

