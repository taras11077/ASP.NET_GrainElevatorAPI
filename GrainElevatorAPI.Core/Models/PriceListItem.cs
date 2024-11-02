using System.ComponentModel.DataAnnotations;
using GrainElevatorAPI.Core.Interfaces.ModelInterfaces;
using GrainElevatorAPI.Core.Models.Base;

namespace GrainElevatorAPI.Core.Models;

public class PriceListItem : AuditableEntity, IPriceListItem
{
	[Required(ErrorMessage = "Id is required.")]
	[Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
	public int Id { get; set; }

	[Required(ErrorMessage = "OperationName is required.")]
	[MinLength(4, ErrorMessage = "OperationName must be at least 4 characters long.")]
	[MaxLength(20, ErrorMessage = "OperationName must be at least 20 characters long.")]
	public string OperationName { get; set; }

	[Range(0, double.MaxValue, ErrorMessage = "OperationPrice must be a positive number.")]
	public decimal OperationPrice { get; set; }
	

	[Range(1, int.MaxValue, ErrorMessage = "PriceListId must be a positive number.")]
	public int PriceListId { get; set; }

    public virtual PriceList PriceList { get; set; }
}

