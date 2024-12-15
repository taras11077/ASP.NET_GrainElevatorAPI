﻿using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.DTO.DTOs;

public class InvoiceRegisterDto
{
	[Required(ErrorMessage = "Id is required.")]
	[Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
	public int Id { get; set; }

	[MinLength(3, ErrorMessage = "RegisterNumber must be at least 3 characters long.")]
	[MaxLength(9, ErrorMessage = "RegisterNumber must be at least 9 characters long.")]
	public string RegisterNumber { get; set; }
	
	[DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
	[Range(typeof(DateTime), "1900-01-01", "2024-12-31", ErrorMessage = "ArrivalDate must be between 1900 and 2024.")]
	public DateTime ArrivalDate { get; set; }
	
	[Required(ErrorMessage = "WeedImpurityBase is required.")]
	[Range(0.0, 100.0, ErrorMessage = "WeedImpurityBase value must be between 0.0 and 100.0")]
	public double WeedImpurityBase { get; set; }

	[Required(ErrorMessage = "MoistureBase is required.")]
	[Range(0.0, 100.0, ErrorMessage = "MoistureBase value must be between 0.0 and 100.0")]
	public double MoistureBase { get; set; }
	
	[Range(1, int.MaxValue, ErrorMessage = "PhysicalWeightReg must be a positive number.")]
	public int? PhysicalWeightReg { get; set; }

	[Range(1, int.MaxValue, ErrorMessage = "ShrinkageReg must be a positive number.")]
	public int? ShrinkageReg { get; set; }

	[Range(1, int.MaxValue, ErrorMessage = "WasteReg must be a positive number.")]
	public int? WasteReg { get; set; }

	[Range(1, int.MaxValue, ErrorMessage = "AccWeightReg must be a positive number.")]
	public int? AccWeightReg { get; set; }

	[Range(1, double.MaxValue, ErrorMessage = "QuantitiesDryingReg must be a positive number.")]
	public double? QuantitiesDryingReg { get; set; }
	
	
	[Range(1, int.MaxValue, ErrorMessage = "SupplierId must be a positive number.")]
	public int SupplierId { get; set; }

	[Range(1, int.MaxValue, ErrorMessage = "ProductId must be a positive number.")]
	public int ProductId { get; set; }

	[Range(1, int.MaxValue, ErrorMessage = "CompletionReportId must be a positive number.")]
	public int? CompletionReportId { get; set; }
	
	public List<ProductionBatchDto> ProductionBatches { get; set; }
	
	public bool? IsFinalized { get; set; }
}