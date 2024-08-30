﻿using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.Core.Models;

public class CompletionReportItem
{
	[Required(ErrorMessage = "Id is required.")]
	[Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
	public int Id { get; set; }

	[Required(ErrorMessage = "TitlTechnologicalOperatione is required.")]
	[MinLength(4, ErrorMessage = "TechnologicalOperation must be at least 4 characters long.")]
	[MaxLength(20, ErrorMessage = "TechnologicalOperation must be at least 20 characters long.")]
	public string TechnologicalOperation { get; set; }

	[Range(0, double.MaxValue, ErrorMessage = "Amount must be a positive number.")]
	public double Amount { get; set; }

	[Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
	public double Price { get; set; }

	[Range(0, double.MaxValue, ErrorMessage = "TotalCost must be a positive number.")]
	public double TotalCost { get; set; }
	
	
	[Range(1, int.MaxValue, ErrorMessage = "CompletionReportId must be a positive number.")]
	public int? CompletionReportId { get; set; }

	public virtual CompletionReport CompletionReport { get; set; }
	


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
