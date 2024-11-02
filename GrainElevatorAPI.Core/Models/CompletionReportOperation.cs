using System.ComponentModel.DataAnnotations;
using GrainElevatorAPI.Core.Interfaces.ModelInterfaces;
using GrainElevatorAPI.Core.Models.Base;

namespace GrainElevatorAPI.Core.Models;

public class CompletionReportOperation : AuditableEntity, ICompletionReportOperation
{
	[Required(ErrorMessage = "Id is required.")]
	[Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
	public int Id { get; set; }

	[Required(ErrorMessage = "OperationName is required.")]
	[MinLength(4, ErrorMessage = "OperationName must be at least 4 characters long.")]
	[MaxLength(20, ErrorMessage = "OperationName must be at least 20 characters long.")]
	public string OperationName  { get; set; }

	[Range(0, double.MaxValue, ErrorMessage = "Amount must be a positive number.")]
	public double Amount { get; set; }

	
	[Range(1, int.MaxValue, ErrorMessage = "CompletionReportId must be a positive number.")]
	public int? CompletionReportId { get; set; }

	public virtual CompletionReport CompletionReport { get; set; }
	
}

